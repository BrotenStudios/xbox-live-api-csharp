using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Threading.Tasks;
using Microsoft.Xbox.Services.System;
namespace Microsoft.Xbox.Services
{
    using global::System.Text;
    using Windows.Foundation;
    using Windows.Security.Authentication.Web.Core;
    using Windows.Security.Credentials;
    using Windows.System;

    public partial class XboxLiveUser
    {
        private readonly string signInHttpMethod = "GET";
        private readonly string signInUri = "https://xboxlive.com";

        public XboxLiveUser()
        {
            this.userImpl = new UserImpl(SignInCompleted, SignOutCompleted);
        }

        User WindowsSystemUser
        {
            get;
        }

        private static bool? isSupported = null;
        //private TaskCompletionSource<SignInResult> signInCompletionSource = null;

        public Task<SignInResult> SignInAsync(Windows.UI.Core.CoreDispatcher coreDispatcherObj)
        {
            XboxLiveContextSettings.Dispatcher = coreDispatcherObj;
            return this.userImpl.SignInImpl(true, false);
        }

        public Task<SignInResult> SignInSilentlyAsync(Windows.UI.Core.CoreDispatcher coreDispatcherObj)
        {
            XboxLiveContextSettings.Dispatcher = coreDispatcherObj;
            return this.userImpl.SignInImpl(false, false);
        }

        public Task<SignInResult> SwitchAccountAsync(Windows.UI.Core.CoreDispatcher coreDispatcherObj)
        {
            XboxLiveContextSettings.Dispatcher = coreDispatcherObj;
            throw new NotImplementedException();
            //return this.userImpl.SwitchAccountAsync();
        }

        public Task<SignInResult> SwitchAccountAsync()
        {
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
            return this.userImpl.InternalGetTokenAndSignatureAsync("GET", userImpl.AuthConfig.XboxLiveEndpoint, null, null, false, true).ContinueWith((taskAndSignatureResultTask) =>
            {
                if(taskAndSignatureResultTask.Exception != null)
                {
                    throw taskAndSignatureResultTask.Exception;
                }
            });
        }
    }
}
