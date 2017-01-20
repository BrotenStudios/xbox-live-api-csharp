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
        public XboxLiveUser(SignInCompletedCallback signInCallback, GetTokenAndSignatureCallback tokenCallback) {
            m_SignInCallback = signInCallback;
            m_TokenCallback = tokenCallback;
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
        
        public Task<SignInResult> SignInAsync()
        {
            if (XboxLiveContext.UseMockData)
            {
                IsSignedIn = true;
                Gamertag = "2 dev 7727";
                return Task.FromResult(new SignInResult(SignInStatus.Success));
            }

            global::System.Threading.ManualResetEvent waitEvent = new global::System.Threading.ManualResetEvent(false);
            int context = global::System.Threading.Interlocked.Increment(ref m_SignInRequestId);
            SignInRequest req = new SignInRequest();
            req.Result = new SignInResult(SignInStatus.UserInteractionRequired);
            req.User = this;
            req.Event = waitEvent;
            m_SigninDictionary.Add(context, req);
            SignIn(true, context, m_SignInCallback);

            return Task.Run(() =>
            {
                waitEvent.WaitOne();
                return req.Result;
            });
        }

        public Task<SignInResult> SignInSilentlyAsync()
        {
            if (XboxLiveContext.UseMockData)
            {
                this.IsSignedIn = true;
                Gamertag = "2 dev 7727";
                return Task.FromResult(new SignInResult(SignInStatus.Success));
            }

            throw new NotImplementedException();
        }

        public Task<SignInResult> SwitchAccountAsync()
        {
            if (XboxLiveContext.UseMockData)
            {
                this.IsSignedIn = true;
                Gamertag = "2 dev 7727";
                return Task.FromResult(new SignInResult(SignInStatus.Success));
            }

            throw new NotImplementedException();
        }

        public static void SignInRequestComplete(int context, int errorCode, NativeSignInResult result)
        {
            if (m_SigninDictionary.ContainsKey(context))
            {
                var request = m_SigninDictionary[context];
                var user = request.User;

                if (errorCode == 0 && result.Status == (int)SignInStatus.Success)
                {
                    user.Gamertag = result.Gamertag;
                    user.XboxUserId = result.XboxUserId;
                    user.AgeGroup = result.AgeGroup;
                    user.Privileges = result.Privileges;
                    user.WebAccountId = result.WebAccountId;

                    user.IsSignedIn = true;
                }
                else
                {
                    user.IsSignedIn = false;
                }

                request.Result = new SignInResult((SignInStatus)result.Status);
                request.Event.Set();

                m_SigninDictionary.Remove(context);
            }
        }

        public static void TokenRequestComplete(int context, int errorCode, NativeTokenAndSignatureResult xblResult)
        {
            if (m_TokenDictionary.ContainsKey(context))
            {
                var request = m_TokenDictionary[context];

                GetTokenAndSignatureResult result = new GetTokenAndSignatureResult()
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

                request.Result = result;
                request.Event.Set();
                m_TokenDictionary.Remove(context);
            }
        }

        public Task<GetTokenAndSignatureResult> GetTokenAndSignatureAsync(string httpMethod, string url, string headers, string body)
        {
            int context = global::System.Threading.Interlocked.Increment(ref m_TokenRequestId);
            global::System.Threading.ManualResetEvent waitEvent = new global::System.Threading.ManualResetEvent(false);
            TokenRequest req = new TokenRequest();
            req.Event = waitEvent;
            req.Result = new GetTokenAndSignatureResult();
            m_TokenDictionary.Add(context, req);

            Int32 xblErrorCode = 0;
            GetTokenAndSignature(httpMethod, url, headers, body, context, m_TokenCallback);

            return Task.Run(() =>
            {
                waitEvent.WaitOne();
                if (xblErrorCode != 0)
                {
                    //something went wrong
                }
                
                return req.Result;
            });
        }

        private static int m_TokenRequestId = 0;
        private static int m_SignInRequestId = 0;
        private static Dictionary<int, TokenRequest> m_TokenDictionary = new Dictionary<int, TokenRequest>();
        private static Dictionary<int, SignInRequest> m_SigninDictionary = new Dictionary<int, SignInRequest>();
        public static event EventHandler<SignOutCompletedEventArgs> SignOutCompleted;
        private SignInCompletedCallback m_SignInCallback;
        private GetTokenAndSignatureCallback m_TokenCallback;

        public Task<GetTokenAndSignatureResult> GetTokenAndSignatureAsync(string httpMethod, string url, string headers)
        {
            return GetTokenAndSignatureAsync(httpMethod, url, headers, string.Empty);
        }

        public Task<GetTokenAndSignatureResult> GetTokenAndSignatureAsync(string httpMethod, string url, string headers, byte[] body)
        {
            string bodyString = Encoding.UTF8.GetString(body);

            return GetTokenAndSignatureAsync(httpMethod, url, headers, bodyString);
        }

        private class TokenRequest
        {
            public global::System.Threading.ManualResetEvent Event;
            public GetTokenAndSignatureResult Result;
        }

        private class SignInRequest
        {
            public global::System.Threading.ManualResetEvent Event;
            public SignInResult Result;
            public XboxLiveUser User;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 8)]
        public struct NativeSignInResult
        {
            public Int32 Status;
            [MarshalAs(UnmanagedType.LPWStr)]
            public string XboxUserId;
            [MarshalAs(UnmanagedType.LPWStr)]
            public string Gamertag;
            [MarshalAs(UnmanagedType.LPWStr)]
            public string AgeGroup;
            [MarshalAs(UnmanagedType.LPWStr)]
            public string Privileges;
            [MarshalAs(UnmanagedType.LPWStr)]
            public string WebAccountId;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 8)]
        public struct NativeTokenAndSignatureResult
        {
            [MarshalAs(UnmanagedType.LPWStr)]
            public string Token;
            [MarshalAs(UnmanagedType.LPWStr)]
            public string Signature;
            [MarshalAs(UnmanagedType.LPWStr)]
            public string XboxUserId;
            [MarshalAs(UnmanagedType.LPWStr)]
            public string Gamertag;
            [MarshalAs(UnmanagedType.LPWStr)]
            public string XboxUserHash;
            [MarshalAs(UnmanagedType.LPWStr)]
            public string AgeGroup;
            [MarshalAs(UnmanagedType.LPWStr)]
            public string Privileges;
            [MarshalAs(UnmanagedType.LPWStr)]
            public string WebAccountId;
            [MarshalAs(UnmanagedType.LPWStr)]
            public string Reserved;
        }

        public delegate void SignInCompletedCallback(Int32 context, Int32 xblErrorCode, NativeSignInResult result);
        public delegate void GetTokenAndSignatureCallback(Int32 context, Int32 xblErrorCode, NativeTokenAndSignatureResult result);

        [DllImport("Microsoft.Xbox.Services.140.Sidecar.AuthDll", CallingConvention=CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
        static extern void SignIn(bool showUI, Int32 context, SignInCompletedCallback onCompleted);


        [DllImport("Microsoft.Xbox.Services.140.Sidecar.AuthDll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
        static extern void GetTokenAndSignature(string httpMethod, string url, string headers, string body, Int32 context, GetTokenAndSignatureCallback callback);
    }
}
