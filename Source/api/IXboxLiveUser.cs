using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Microsoft.Xbox.Services
{
    interface IXboxLiveUser
    {
        string WebAccountId
        {
            get;
        }

        bool IsSignedIn
        {
            get;
        }

        string Privileges
        {
            get;
        }

        string AgeGroup
        {
            get;
        }

        string Gamertag
        {
            get;
        }

        string XboxUserId
        {
            get;
        }

        Task<SignInResult> SignInAsync();

        Task<SignInResult> SignInSilentlyAsync();

        Task<SignInResult> SwitchAccountAsync();

        Task<GetTokenAndSignatureResult> GetTokenAndSignatureAsync(string httpMethod, string url, string headers);

        Task<GetTokenAndSignatureResult> GetTokenAndSignatureAsync(string httpMethod, string url, string headers, string body);

        Task<GetTokenAndSignatureResult> GetTokenAndSignatureAsync(string httpMethod, string url, string headers, byte[] body);
    }
}
