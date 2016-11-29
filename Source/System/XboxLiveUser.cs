using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Microsoft.Xbox.Services.System
{
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
            throw new NotImplementedException();
        }

        public Task<SignInResult> SignInSilentlyAsync(IntPtr coreDispatcher)
        {
            throw new NotImplementedException();
        }

        public Task<SignInResult> SwitchAccountAsync(IntPtr coreDispatcher)
        {
            throw new NotImplementedException();
        }

        public Task<GetTokenAndSignatureResult> GetTokenAndSignatureAsync(string httpMethod, string url, string headers, string body)
        {
            throw new NotImplementedException();
        }

        public Task<GetTokenAndSignatureResult> GetTokenAndSignatureAsync(string httpMethod, string url, string headers)
        {
            throw new NotImplementedException();
        }

        public Task<GetTokenAndSignatureResult> GetTokenAndSignatureArrayAsync(string httpMethod, string url, string headers, byte[] requestBodyArray)
        {
            throw new NotImplementedException();
        }

    }
}
