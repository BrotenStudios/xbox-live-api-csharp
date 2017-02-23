// Copyright (c) Microsoft Corporation
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// 
namespace Microsoft.Xbox.Services
{
    using global::System;
    using global::System.Threading.Tasks;

    using Microsoft.Xbox.Services.System;

    public partial class XboxLiveUser
    {
        public XboxLiveUser()
        {
            this.userImpl = new UserImpl(SignInCompleted, SignOutCompleted);
        }

        public Task<SignInResult> SignInAsync()
        {
            return this.SignInAsync(null);
        }

        public Task<SignInResult> SignInAsync(Windows.UI.Core.CoreDispatcher dispatcher)
        {
            XboxLiveContextSettings.Dispatcher = dispatcher;
            return this.userImpl.SignInImpl(true, false);
        }

        public Task<SignInResult> SignInSilentlyAsync()
        {
            return this.SignInSilentlyAsync(null);
        }

        public Task<SignInResult> SignInSilentlyAsync(Windows.UI.Core.CoreDispatcher dispatcher)
        {
            XboxLiveContextSettings.Dispatcher = dispatcher;
            return this.userImpl.SignInImpl(false, false);
        }

        public Task<SignInResult> SwitchAccountAsync()
        {
            return this.SwitchAccountAsync(null);
        }

        public Task<SignInResult> SwitchAccountAsync(Windows.UI.Core.CoreDispatcher dispatcher)
        {
            XboxLiveContextSettings.Dispatcher = dispatcher;
            throw new NotImplementedException();
            //return this.userImpl.SwitchAccountAsync();
        }

        public Task<TokenAndSignatureResult> GetTokenAndSignatureAsync(string httpMethod, string url, string headers)
        {
            return this.userImpl.GetTokenAndSignatureAsync(httpMethod, url, headers, null);
        }

        public Task<TokenAndSignatureResult> GetTokenAndSignatureAsync(string httpMethod, string url, string headers, string body)
        {
            return this.userImpl.GetTokenAndSignatureAsync(httpMethod, url, headers, body);
        }

        public Task<TokenAndSignatureResult> GetTokenAndSignatureArrayAsync(string httpMethod, string url, string headers, byte[] body)
        {
            return this.userImpl.InternalGetTokenAndSignatureAsync(httpMethod, url, headers, body, false, false);
        }

        public Task RefreshToken()
        {
            return this.userImpl.InternalGetTokenAndSignatureAsync("GET", this.userImpl.AuthConfig.XboxLiveEndpoint, null, null, false, true).ContinueWith((taskAndSignatureResultTask) =>
            {
                if (taskAndSignatureResultTask.Exception != null)
                {
                    throw taskAndSignatureResultTask.Exception;
                }
            });
        }
    }
}