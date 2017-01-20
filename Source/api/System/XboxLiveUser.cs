using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

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
            if (XboxLiveContext.UseMockData)
            {
                IsSignedIn = true;
                Gamertag = "2 dev 7727";
                return Task.FromResult(new SignInResult(SignInStatus.Success));
            }

            global::System.Threading.ManualResetEvent waitEvent = new global::System.Threading.ManualResetEvent(false);
            SignInResult signInResult = null;
            SignIn(coreDispatcher, true, (userHandle, result) =>
            {
                signInResult = new SignInResult((SignInStatus)result.Status);
                waitEvent.Set();
            });

            return Task.Run(() =>
            {
                waitEvent.WaitOne();
                return signInResult;
            });
        }

        public Task<SignInResult> SignInSilentlyAsync(IntPtr coreDispatcher)
        {
            if (XboxLiveContext.UseMockData)
            {
                this.IsSignedIn = true;
                Gamertag = "2 dev 7727";
                return Task.FromResult(new SignInResult(SignInStatus.Success));
            }

            throw new NotImplementedException();
        }

        public Task<SignInResult> SwitchAccountAsync(IntPtr coreDispatcher)
        {
            if (XboxLiveContext.UseMockData)
            {
                this.IsSignedIn = true;
                Gamertag = "2 dev 7727";
                return Task.FromResult(new SignInResult(SignInStatus.Success));
            }

            throw new NotImplementedException();
        }

        public Task<GetTokenAndSignatureResult> GetTokenAndSignatureAsync(string httpMethod, string url, string headers, string body)
        {
            global::System.Threading.ManualResetEvent waitEvent = new global::System.Threading.ManualResetEvent(false);
            GetTokenAndSignatureResult result = null;
            Int32 xblErrorCode = 0;
            GetTokenAndSignature(httpMethod, url, headers, body, (errorCode, xblResult) =>
            {
                xblErrorCode = errorCode;
                result = new GetTokenAndSignatureResult()
                {
                    WebAccountId = xblResult.WebAccountId,
                    Privileges = xblResult.Privileges,
                    AgeGroup = xblResult.AgeGroup,
                    XboxUserHash = xblResult.XboxUserHash,
                    Gamertag = xblResult.Gamertag,
                    XboxUserId = xblResult.XboxUserId,
                    Signature = xblResult.Signature,
                    Token = xblResult.Token,
                    Reserved = xblResult.Reserved
                };
                waitEvent.Set();
            });

            return Task.Run(() =>
            {
                waitEvent.WaitOne();
                if (xblErrorCode != 0)
                {
                    //something went wrong
                }
                return result;
            });
        }

        public Task<GetTokenAndSignatureResult> GetTokenAndSignatureAsync(string httpMethod, string url, string headers)
        {
            return GetTokenAndSignatureAsync(httpMethod, url, headers, string.Empty);
        }

        public Task<GetTokenAndSignatureResult> GetTokenAndSignatureAsync(string httpMethod, string url, string headers, byte[] body)
        {
            string bodyString = Encoding.UTF8.GetString(body);

            return GetTokenAndSignatureAsync(httpMethod, url, headers, bodyString);
        }

        [StructLayout(LayoutKind.Sequential, Pack = 8)]
        struct NativeSignInResult
        {
            public Int32 Status;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 8)]
        struct NativeTokenAndSignatureResult
        {
            public string Token;
            public string Signature;
            public string XboxUserId;
            public string Gamertag;
            public string XboxUserHash;
            public string AgeGroup;
            public string Privileges;
            public string WebAccountId;
            public string Reserved;
        }

        delegate void SignInCompletedCallback(Int32 xblErrorCode, NativeSignInResult Result);
        [DllImport("Microsoft.Xbox.Services.140.Sidecar.AuthDll", CallingConvention=CallingConvention.Cdecl)]
        static extern void SignIn(IntPtr coreDispatcher, bool showUI, SignInCompletedCallback onCompleted);

        delegate void GetTokenAndSignatureCallback(Int32 xblErrorCode, NativeTokenAndSignatureResult result);
        [DllImport("Microsoft.Xbox.Services.140.Sidecar.AuthDll", CallingConvention = CallingConvention.Cdecl)]
        static extern void GetTokenAndSignature(string httpMethod, string url, string headers, string body, GetTokenAndSignatureCallback callback);
    }
}
