// -----------------------------------------------------------------------
//  <copyright file="XboxLiveUser.cs" company="Microsoft">
//      Copyright (c) Microsoft. All rights reserved.
//      Licensed under the MIT license. See LICENSE file in the project root for full license information.
//  </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.Xbox.Services.System
{
    using global::System;
    using global::System.Text;
    using global::System.Threading.Tasks;

    public partial class XboxLiveUser
    {
        public Task<SignInResult> SignInAsync()
        {
            if (XboxLiveContext.UseMockData)
            {
                this.IsSignedIn = true;
                this.Gamertag = "2 dev 7727";
                this.XboxUserId = "123456789";
                return Task.FromResult(new SignInResult(SignInStatus.Success));
            }

            throw new NotImplementedException();
        }

        public Task<SignInResult> SignInSilentlyAsync()
        {
            if (XboxLiveContext.UseMockData)
            {
                this.IsSignedIn = true;
                this.Gamertag = "2 dev 7727";
                this.XboxUserId = "123456789";
                return Task.FromResult(new SignInResult(SignInStatus.Success));
            }

            throw new NotImplementedException();
        }

        public Task<SignInResult> SwitchAccountAsync()
        {
            if (XboxLiveContext.UseMockData)
            {
                this.IsSignedIn = true;
                this.Gamertag = "2 dev 7727";
                this.XboxUserId = "123456789";
                return Task.FromResult(new SignInResult(SignInStatus.Success));
            }

            throw new NotImplementedException();
        }

        public Task<GetTokenAndSignatureResult> GetTokenAndSignatureAsync(string httpMethod, string url, string headers)
        {
            return this.GetTokenAndSignatureAsync(httpMethod, url, headers, string.Empty);
        }

        public Task<GetTokenAndSignatureResult> GetTokenAndSignatureAsync(string httpMethod, string url, string headers, byte[] body)
        {
            string bodyString = Encoding.UTF8.GetString(body);

            return this.GetTokenAndSignatureAsync(httpMethod, url, headers, bodyString);
        }

        public Task<GetTokenAndSignatureResult> GetTokenAndSignatureAsync(string httpMethod, string url, string headers, string body)
        {
            if (XboxLiveContext.UseMockData)
            {
                return Task.FromResult(new GetTokenAndSignatureResult
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