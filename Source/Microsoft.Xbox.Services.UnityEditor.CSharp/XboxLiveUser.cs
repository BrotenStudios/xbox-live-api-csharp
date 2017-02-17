// -----------------------------------------------------------------------
//  <copyright file="XboxLiveUser.cs" company="Microsoft">
//      Copyright (c) Microsoft. All rights reserved.
//      Licensed under the MIT license. See LICENSE file in the project root for full license information.
//  </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.Xbox.Services
{
    using global::System;
    using global::System.Text;
    using global::System.Threading.Tasks;
    using Microsoft.Xbox.Services.System;

    public partial class XboxLiveUser
    {
        private UserImpl mockUserImpl;
        public XboxLiveUser()
        {
            userImpl = new UserImpl();
            mockUserImpl = (UserImpl)userImpl;
        }

        public Task<SignInResult> SignInAsync()
        {
            if (XboxLiveContext.UseMockServices)
            {
                this.mockUserImpl.IsSignedIn = true;
                this.mockUserImpl.Gamertag = "2 dev 7727";
                this.mockUserImpl.XboxUserId = "123456789";
                return Task.FromResult(new SignInResult(SignInStatus.Success));
            }

            throw new NotImplementedException();
        }

        public Task<SignInResult> SignInSilentlyAsync()
        {
            if (XboxLiveContext.UseMockServices)
            {
                this.mockUserImpl.IsSignedIn = true;
                this.mockUserImpl.Gamertag = "2 dev 7727";
                this.mockUserImpl.XboxUserId = "123456789";
                return Task.FromResult(new SignInResult(SignInStatus.Success));
            }

            throw new NotImplementedException();
        }

        public Task<SignInResult> SwitchAccountAsync()
        {
            if (XboxLiveContext.UseMockServices)
            {
                this.mockUserImpl.IsSignedIn = true;
                this.mockUserImpl.Gamertag = "2 dev 7727";
                this.mockUserImpl.XboxUserId = "123456789";
                return Task.FromResult(new SignInResult(SignInStatus.Success));
            }

            throw new NotImplementedException();
        }

        public Task<TokenAndSignatureResult> GetTokenAndSignatureAsync(string httpMethod, string url, string headers)
        {
            return this.GetTokenAndSignatureAsync(httpMethod, url, headers, string.Empty);
        }

        public Task<TokenAndSignatureResult> GetTokenAndSignatureArrayAsync(string httpMethod, string url, string headers, byte[] body)
        {
            string bodyString = Encoding.UTF8.GetString(body);

            return this.GetTokenAndSignatureAsync(httpMethod, url, headers, bodyString);
        }

        public Task<TokenAndSignatureResult> GetTokenAndSignatureAsync(string httpMethod, string url, string headers, string body)
        {
            if (XboxLiveContext.UseMockServices)
            {
                return Task.FromResult(new TokenAndSignatureResult
                {
                    Gamertag = this.Gamertag,
                    XboxUserId = this.XboxUserId,
                    XboxUserHash = "Foo",
                    Token = "Bar",
                    Signature = "==",
                });
            }

            throw new NotImplementedException();
        }
        
    }
}