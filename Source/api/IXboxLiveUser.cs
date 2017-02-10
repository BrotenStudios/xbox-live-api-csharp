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

#if WINDOWS_UWP
        Task<SignInResult> SignInAsync(Windows.UI.Core.CoreDispatcher coreDispatcher);

        Task<SignInResult> SignInSilentlyAsync(Windows.UI.Core.CoreDispatcher coreDispatcher);

        Task<SignInResult> SwitchAccountAsync(Windows.UI.Core.CoreDispatcher coreDispatcher);
#else
        Task<SignInResult> SignInAsync();

        Task<SignInResult> SignInSilentlyAsync();
#endif
        Task<SignInResult> SwitchAccountAsync();

        Task<TokenAndSignatureResult> GetTokenAndSignatureAsync(string httpMethod, string url, string headers);

        Task<TokenAndSignatureResult> GetTokenAndSignatureAsync(string httpMethod, string url, string headers, string body);

        Task<TokenAndSignatureResult> GetTokenAndSignatureArrayAsync(string httpMethod, string url, string headers, byte[] body);
    }
}
