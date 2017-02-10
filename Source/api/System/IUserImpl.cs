using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Xbox.Services.System
{
    internal interface IUserImpl
    {
        bool IsSignedIn { get; }
        XboxLiveUser User { get; }

        string XboxUserId { get; }
        string Gamertag { get; }
        string AgeGroup { get; }
        string Privileges { get; }
        string WebAccountId { get; }
        AuthConfig AuthConfig { get; }

        Task<SignInResult> SignInImpl(bool showUI, bool forceRefresh);
        Task<TokenAndSignatureResult> GetTokenAndSignatureAsync(string httpMethod, string url, string headers, string requestBodyString);
        Task<TokenAndSignatureResult> InternalGetTokenAndSignatureAsync(string httpMethod, string url, string headers, byte[] body, bool promptForCredentialsIfNeeded, bool forceRefresh);
    }
}
