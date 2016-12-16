using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Microsoft.Xbox.Services.System
{
    using global::System.Text;

    public class XboxLiveUser
    {
        public XboxLiveUser(XboxLiveUser systemUser) {
        }
        public XboxLiveUser() {
        }

        public XboxLiveUser WindowsSystemUser
        {
            get;
            private set;
        }

        public string WebAccountId
        {
            get;
            private set;
        }

        public bool IsSignedIn
        {
            get;
            private set;
        }

        public string Privileges
        {
            get;
            private set;
        }

        public string AgeGroup
        {
            get;
            private set;
        }

        public string Gamertag
        {
            get;
            private set;
        }

        public string XboxUserId
        {
            get;
            private set;
        }


        public static event EventHandler<SignOutCompletedEventArgs> SignOutCompleted;


        public Task<SignInResult> SignInAsync(IntPtr coreDispatcher)
        {
            this.IsSignedIn = true;
            this.Gamertag = "Veleek";
            return Task.FromResult(new SignInResult { Status = SignInStatus.Success });
        }

        public Task<SignInResult> SignInSilentlyAsync(IntPtr coreDispatcher)
        {
            this.IsSignedIn = true;
            return Task.FromResult(new SignInResult { Status = SignInStatus.Success });
        }

        public Task<SignInResult> SwitchAccountAsync(IntPtr coreDispatcher)
        {
            this.IsSignedIn = true;
            return Task.FromResult(new SignInResult { Status = SignInStatus.Success });
        }

        public Task<GetTokenAndSignatureResult> GetTokenAndSignatureAsync(string httpMethod, string url, string headers)
        {
            return this.GetTokenAndSignatureAsync(httpMethod, url, headers, (byte[])null);
        }

        public Task<GetTokenAndSignatureResult> GetTokenAndSignatureAsync(string httpMethod, string url, string headers, string body)
        {
            return this.GetTokenAndSignatureAsync(httpMethod, url, headers, Encoding.UTF8.GetBytes(body));
        }

        public Task<GetTokenAndSignatureResult> GetTokenAndSignatureAsync(string httpMethod, string url, string headers, byte[] body)
        {
            return Task.FromResult(
                new GetTokenAndSignatureResult
                {
                    Gamertag = this.Gamertag,
                    AgeGroup = this.AgeGroup,
                    Privileges = this.Privileges,
                    XboxUserId = this.XboxUserId,
                    WebAccountId = this.WebAccountId
                });
        }
    }
}
