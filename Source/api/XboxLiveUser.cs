// Copyright (c) Microsoft Corporation
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// 
namespace Microsoft.Xbox.Services
{
    using global::System;
    using global::System.Text;
    using global::System.Threading.Tasks;

    using Microsoft.Xbox.Services.System;

    public partial class XboxLiveUser : IXboxLiveUser
    {
        private readonly IUserImpl userImpl;

        public static event EventHandler<SignInCompletedEventArgs> SignInCompleted;
        public static event EventHandler<SignOutCompletedEventArgs> SignOutCompleted;

        public string WebAccountId
        {
            get
            {
                return this.userImpl.WebAccountId;
            }
        }

        public bool IsSignedIn
        {
            get
            {
                return this.userImpl.IsSignedIn;
            }
        }

        public string Privileges
        {
            get
            {
                return this.userImpl.Privileges;
            }
        }

        public string XboxUserId
        {
            get
            {
                return this.userImpl.XboxUserId;
            }
        }

        public string AgeGroup
        {
            get
            {
                return this.userImpl.AgeGroup;
            }
        }

        public string Gamertag
        {
            get
            {
                return this.userImpl.Gamertag;
            }
        }

        public Task<SignInResult> SignInAsync()
        {
            return this.userImpl.SignInImpl(true, false).ContinueWith(signInTask =>
            {
                OnSignInCompleted(this);

                return signInTask.Result;
            });
        }

        public Task<SignInResult> SignInSilentlyAsync()
        {
            return this.userImpl.SignInImpl(false, false);
        }

        public Task<SignInResult> SwitchAccountAsync()
        {
            return this.userImpl.SignInImpl(false, true);
        }

        public Task<TokenAndSignatureResult> GetTokenAndSignatureAsync(string httpMethod, string url, string headers)
        {
            return this.GetTokenAndSignatureArrayAsync(httpMethod, url, headers, null);
        }

        public Task<TokenAndSignatureResult> GetTokenAndSignatureAsync(string httpMethod, string url, string headers, string body)
        {
            return this.GetTokenAndSignatureArrayAsync(httpMethod, url, headers, body == null ? null : Encoding.UTF8.GetBytes(body));
        }

        public Task<TokenAndSignatureResult> GetTokenAndSignatureArrayAsync(string httpMethod, string url, string headers, byte[] body)
        {
            return this.userImpl.InternalGetTokenAndSignatureAsync(httpMethod, url, headers, body, false, false);
        }

        private static void OnSignInCompleted(IXboxLiveUser user)
        {
            var handler = SignInCompleted;
            if (handler != null) handler(null, new SignInCompletedEventArgs(user));
        }

        private static void OnSignOutCompleted(IXboxLiveUser user)
        {
            var handler = SignOutCompleted;
            if (handler != null) handler(null, new SignOutCompletedEventArgs(user));
        }
    }
}