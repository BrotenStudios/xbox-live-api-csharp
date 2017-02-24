// Copyright (c) Microsoft Corporation
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Xbox.Services.System
{
    using Windows.Foundation;
    using Windows.Security.Authentication.Web.Core;
    using Windows.Security.Credentials;
    using Windows.System;
    using Windows.System.Threading;
    using Windows.UI.Core;

    using global::System;
    using global::System.Linq;
    using global::System.Text;
    using global::System.Threading.Tasks;

    internal class UserImpl : IUserImpl
    {
        private static bool? isSupported;
        private WebAccountProvider provider;
        private readonly object userImplLock = new object();

        public bool IsSignedIn { get; private set; }
        public XboxLiveUser User { get; set; }

        public string XboxUserId { get; private set; }
        public string Gamertag { get; private set; }
        public string AgeGroup { get; private set; }
        public string Privileges { get; private set; }
        public string WebAccountId { get; private set; }
        public AuthConfig AuthConfig { get; private set; }

        private XboxLiveAppConfiguration appConfig;

        private readonly EventHandler<SignInCompletedEventArgs> signInCompleted;
        private readonly EventHandler<SignOutCompletedEventArgs> signOutCompleted;
        private ThreadPoolTimer threadPoolTimer;

        public UserImpl(EventHandler<SignInCompletedEventArgs> signInCompleted, EventHandler<SignOutCompletedEventArgs> signOutCompleted)
        {
            this.signInCompleted = signInCompleted;
            this.signOutCompleted = signOutCompleted;
            this.appConfig = XboxLiveAppConfiguration.Instance;

            this.AuthConfig = new AuthConfig
            {
                Sandbox = this.appConfig.Sandbox,
                EnvrionmentPrefix = this.appConfig.EnvironmentPrefix,
                Envrionment = this.appConfig.Environment,
                UseCompactTicket = this.appConfig.UseFirstPartyToken
            };
        }

        public Task<SignInResult> SignInImpl(bool showUI, bool forceRefresh)
        {
            var signInTask = this.InitializeProvider().ContinueWith((task) =>
            {
                var tokenAndSigResult = this.InternalGetTokenAndSignatureHelper(
                    "GET", this.AuthConfig.XboxLiveEndpoint,
                    "",
                    null,
                    showUI,
                    false
                );

                if (tokenAndSigResult != null && tokenAndSigResult.XboxUserId != null && tokenAndSigResult.XboxUserId.Length != 0)
                {
                    if (string.IsNullOrEmpty(tokenAndSigResult.Token))
                    {
                        var xboxUserId = tokenAndSigResult.XboxUserId;
                        // todo: set presence
                    }

                    this.UserSignedIn(tokenAndSigResult.XboxUserId, tokenAndSigResult.Gamertag, tokenAndSigResult.AgeGroup,
                        tokenAndSigResult.Privileges, tokenAndSigResult.WebAccountId);

                    return new SignInResult(SignInStatus.Success);
                }

                return this.ConvertWebTokenRequestStatus(tokenAndSigResult.TokenRequestResult);
            });

            return signInTask;
        }

        private Task InitializeProvider()
        {
            if (this.provider != null)
            {
                return Task.FromResult<object>(null);
            }

            TaskCompletionSource<object> taskCompletion = new TaskCompletionSource<object>();

            if (!XboxLiveContextSettings.Dispatcher.HasThreadAccess)
            {
                // We're not on the UI thread, so we'll use the dispatcher to make our call.
                IAsyncAction uiTask = XboxLiveContextSettings.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => this.InitializeProvider(taskCompletion));
            }
            else
            {
                // Otherwise just go ahead and make the call on this thread.
                this.InitializeProvider(taskCompletion);
            }

            return taskCompletion.Task;
        }

        private void InitializeProvider(TaskCompletionSource<object> completionSource)
        {
            IAsyncOperation<WebAccountProvider> providerTask = WebAuthenticationCoreManager.FindAccountProviderAsync("https://xsts.auth.xboxlive.com");
            providerTask.Completed = (webaccountProviderResult, state) => { this.FindAccountCompleted(webaccountProviderResult, state, completionSource); };
        }

        private void FindAccountCompleted(IAsyncOperation<WebAccountProvider> asyncInfo, AsyncStatus asyncStatus, TaskCompletionSource<object> completionSource)
        {
            this.provider = asyncInfo.GetResults();
            if (this.provider == null)
            {
                completionSource.SetException(new Exception("XBL IDP is not found")); // todo: make xbox live exception
            }

            completionSource.SetResult(null);
        }

        private bool IsMultiUserApplication()
        {
            if (isSupported == null)
            {
                try
                {
                    bool APIExist = Windows.Foundation.Metadata.ApiInformation.IsMethodPresent("Windows.System.UserPicker", "IsSupported");
                    isSupported = (APIExist && UserPicker.IsSupported()) ? true : false;
                }
                catch (Exception)
                {
                    isSupported = false;
                }
            }
            return isSupported == true;
        }

        public Task<TokenAndSignatureResult> InternalGetTokenAndSignatureAsync(string httpMethod, string url, string headers, byte[] body, bool promptForCredentialsIfNeeded, bool forceRefresh)
        {
            return Task.Factory.StartNew(() =>
            {
                var result = this.InternalGetTokenAndSignatureHelper(httpMethod, url, headers, body, promptForCredentialsIfNeeded, forceRefresh);
                if (result.TokenRequestResult != null && result.TokenRequestResult.ResponseStatus == WebTokenRequestStatus.UserInteractionRequired)
                {
                    if (this.AuthConfig.XboxLiveEndpoint != null && url == this.AuthConfig.XboxLiveEndpoint && this.IsSignedIn)
                    {
                        this.UserSignedOut();
                    }
                    else if (url != this.AuthConfig.XboxLiveEndpoint)
                    {
                        // todo: throw error
                    }
                }

                return result;
            });
        }

        private TokenAndSignatureResult InternalGetTokenAndSignatureHelper(string httpMethod, string url, string headers, byte[] body, bool promptForCredentialsIfNeeded, bool forceRefresh)
        {
            if (this.provider == null)
            {
                throw new Exception("Xbox Live identity provider is not initialized");
            }

            var request = new WebTokenRequest(this.provider);
            request.Properties.Add("HttpMethod", httpMethod);
            request.Properties.Add("Url", url);
            if (!string.IsNullOrEmpty(headers))
            {
                request.Properties.Add("RequestHeaders", headers);
            }
            if (forceRefresh)
            {
                request.Properties.Add("ForceRefresh", "true");
            }

            if (body != null && body.Length > 0)
            {
                request.Properties.Add("RequestBody", Encoding.UTF8.GetString(body));
            }

            request.Properties.Add("Target", this.AuthConfig.RPSTicketService);
            request.Properties.Add("Policy", this.AuthConfig.RPSTicketPolicy);
            if (promptForCredentialsIfNeeded)
            {
                var uiSettings = XboxLiveAppConfiguration.Instance.AppSignInUISettings;

                string pfn = Windows.ApplicationModel.Package.Current.Id.FamilyName;
                request.Properties.Add("PackageFamilyName", pfn);

                if (uiSettings.Enabled())
                {
                    if (uiSettings.BackgroundHexColor.Length != 0)
                    {
                        request.Properties.Add("PreferredColor", uiSettings.BackgroundHexColor);
                    }

                    if (uiSettings.TitleCategory == SignInUIGameCategory.Casual)
                    {
                        request.Properties.Add("CasualGame", "");
                    }

                    if (uiSettings.BackgroundImage.Length != 0)
                    {
                        request.Properties.Add("TitleUpsellImage", uiSettings.BackgroundImage);
                    }

                    var featureCount = uiSettings.EmphasisFeatures.Count;
                    if (featureCount > 0)
                    {
                        featureCount = Math.Min(3, featureCount);
                        string bullets = "";
                        foreach (var feature in uiSettings.EmphasisFeatures)
                        {
                            bullets += feature.ToString() + ",";
                        }

                        request.Properties.Add("TitleUpsellFeatures", bullets);
                    }
                }
            }

            TokenAndSignatureResult tokenAndSignatureReturnResult = null;
            var tokenResult = this.RequestTokenFromIDP(XboxLiveContextSettings.Dispatcher, promptForCredentialsIfNeeded, request);
            try
            {
                tokenAndSignatureReturnResult = this.ConvertWebTokenRequestResult(tokenResult);
                if (tokenAndSignatureReturnResult != null && this.IsSignedIn && tokenAndSignatureReturnResult.XboxUserId != this.XboxUserId)
                {
                    this.UserSignedOut();
                    throw new Exception("User has switched"); // todo: auth_user_switched
                }
            }
            catch (Exception e)
            {
                // log
            }

            return tokenAndSignatureReturnResult;
        }

        public Task<SignInResult> SwitchAccountAsync()
        {
            throw new NotImplementedException();
        }

        private WebTokenRequestResult RequestTokenFromIDP(CoreDispatcher coreDispatcher, bool promptForCredentialsIfNeeded, WebTokenRequest request)
        {
            WebTokenRequestResult tokenResult = null;
            if (coreDispatcher != null && promptForCredentialsIfNeeded)
            {
                TaskCompletionSource<object> completionSource = new TaskCompletionSource<object>();
                var requestTokenTask = coreDispatcher.RunAsync(CoreDispatcherPriority.Normal,
                    () =>
                    {
                        WebAuthenticationCoreManager.RequestTokenAsync(request).Completed = (info, status) =>
                        {
                            try
                            {
                                tokenResult = info.GetResults();
                                completionSource.SetResult(null);
                            }
                            catch (Exception e)
                            {
                                completionSource.SetException(e);
                            }
                        };
                    });

                completionSource.Task.Wait();
                if (completionSource.Task.Exception != null)
                {
                    throw completionSource.Task.Exception;
                }
            }
            else
            {
                IAsyncOperation<WebTokenRequestResult> getTokenTask;
                TaskCompletionSource<WebTokenRequestResult> webTokenRequestSource = new TaskCompletionSource<WebTokenRequestResult>();
                if (promptForCredentialsIfNeeded)
                {
                    getTokenTask = WebAuthenticationCoreManager.RequestTokenAsync(request);
                }
                else
                {
                    getTokenTask = WebAuthenticationCoreManager.GetTokenSilentlyAsync(request);
                }

                getTokenTask.Completed += (tokenTask, status) => webTokenRequestSource.SetResult(tokenTask.GetResults());

                tokenResult = webTokenRequestSource.Task.Result;
            }

            return tokenResult;
        }

        private TokenAndSignatureResult ConvertWebTokenRequestResult(WebTokenRequestResult tokenResult)
        {
            var tokenResponseStatus = tokenResult.ResponseStatus;

            if (tokenResponseStatus == WebTokenRequestStatus.Success)
            {
                if (tokenResult.ResponseData == null || tokenResult.ResponseData.Count == 0)
                {
                    throw new Exception("Invalid idp token response");
                }

                WebTokenResponse response = tokenResult.ResponseData.ElementAt(0);

                string xboxUserId = response.Properties["XboxUserId"];
                string gamertag = response.Properties["Gamertag"];
                string ageGroup = response.Properties["AgeGroup"];
                string environment = response.Properties["Environment"];
                string sandbox = response.Properties["Sandbox"];
                string webAccountId = response.WebAccount.Id;
                string token = response.Token;

                string signature = null;
                if (response.Properties.ContainsKey("Signature"))
                {
                    signature = response.Properties["Signature"];
                }

                string privilege = null;
                if (response.Properties.ContainsKey("Privileges"))
                {
                    privilege = response.Properties["Privileges"];
                }

                if (environment.ToLower() == "prod")
                {
                    environment = null;
                }

                var appConfig = XboxLiveAppConfiguration.Instance;
                appConfig.Sandbox = sandbox;
                appConfig.Environment = environment;

                return new TokenAndSignatureResult
                {
                    WebAccountId = webAccountId,
                    Privileges = privilege,
                    AgeGroup = ageGroup,
                    Gamertag = gamertag,
                    XboxUserId = xboxUserId,
                    Signature = signature,
                    Token = token,
                    TokenRequestResult = tokenResult
                };
            }
            else if (tokenResponseStatus == WebTokenRequestStatus.AccountSwitch)
            {
                this.UserSignedOut(); // todo: throw?
            }
            else if (tokenResponseStatus == WebTokenRequestStatus.ProviderError)
            {
                // todo: log error
            }

            return new TokenAndSignatureResult()
            {
                TokenRequestResult = tokenResult
            };
        }

        private void UserSignedIn(string xboxUserId, string gamertag, string ageGroup, string privileges, string webAccountId)
        {
            lock (this.userImplLock)
            {
                this.XboxUserId = xboxUserId;
                this.Gamertag = gamertag;
                this.AgeGroup = ageGroup;
                this.Privileges = privileges;
                this.WebAccountId = webAccountId;

                this.IsSignedIn = true;
                if (this.signInCompleted != null)
                {
                    this.signInCompleted(null, new SignInCompletedEventArgs(this.User));
                }
            }

            if (!this.IsMultiUserApplication())
            {
                TimeSpan delay = new TimeSpan(0, 0, 10);
                this.threadPoolTimer = ThreadPoolTimer.CreatePeriodicTimer(new TimerElapsedHandler((source) => { this.CheckUserSignedOut(); }),
                    delay
                );
            }
            else
            {
                // todo: implement MUA solution
            }
        }

        private void UserSignedOut()
        {
            bool isSignedIn = false;
            lock (this.userImplLock)
            {
                isSignedIn = this.IsSignedIn;
                this.IsSignedIn = false;
            }

            if (isSignedIn)
            {
                if (this.signInCompleted != null)
                {
                    this.signOutCompleted(this.User, new SignOutCompletedEventArgs(this.User));
                }
            }

            lock (this.userImplLock)
            {
                if (!isSignedIn)
                {
                    this.XboxUserId = null;
                    this.Gamertag = null;
                    this.AgeGroup = null;
                    this.Privileges = null;
                    this.WebAccountId = null;
                }
            }
        }

        private void CheckUserSignedOut()
        {
            try
            {
                if (this.IsSignedIn)
                {
                    var signedInAccount = WebAuthenticationCoreManager.FindAccountAsync(this.provider, this.WebAccountId);
                    if (signedInAccount == null)
                    {
                        this.UserSignedOut();
                    }
                }
            }
            catch (Exception)
            {
                this.UserSignedOut();
            }
        }

        private SignInResult ConvertWebTokenRequestStatus(WebTokenRequestResult tokenResult)
        {
            if (tokenResult.ResponseStatus == WebTokenRequestStatus.UserCancel)
            {
                return new SignInResult(SignInStatus.UserCancel);
            }
            else
            {
                return new SignInResult(SignInStatus.UserInteractionRequired);
            }
        }
    }
}