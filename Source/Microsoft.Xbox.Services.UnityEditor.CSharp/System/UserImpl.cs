// Copyright (c) Microsoft Corporation
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Xbox.Services.System
{
    class UserImpl : IUserImpl
    {
        public bool IsSignedIn { get; set; }
        public XboxLiveUser User { get; set; }

        public string XboxUserId { get; set; }
        public string Gamertag { get; set; }
        public string AgeGroup { get; set; }
        public string Privileges { get; set; }
        public string WebAccountId { get; set; }
        public AuthConfig AuthConfig { get; set; }

        public Task<SignInResult> SignInImpl(bool showUI, bool forceRefresh)
        {
            throw new NotImplementedException();
        }
        public Task<TokenAndSignatureResult> GetTokenAndSignatureAsync(string httpMethod, string url, string headers, string requestBodyString)
        {
            throw new NotImplementedException();
        }
        public Task<TokenAndSignatureResult> InternalGetTokenAndSignatureAsync(string httpMethod, string url, string headers, byte[] body, bool promptForCredentialsIfNeeded, bool forceRefresh)
        {
            throw new NotImplementedException();
        }
    }
}
