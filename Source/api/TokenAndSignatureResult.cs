using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
#if WINDOWS_UWP
using Windows.Security.Authentication.Web.Core;
#endif

namespace Microsoft.Xbox.Services
{
    public class TokenAndSignatureResult
    {

        public string WebAccountId
        {
            get;
            internal set;
        }

        public string Privileges
        {
            get;
            internal set;
        }

        public string AgeGroup
        {
            get;
            internal set;
        }

        public string XboxUserHash
        {
            get;
            internal set;
        }

        public string Gamertag
        {
            get;
            internal set;
        }

        public string XboxUserId
        {
            get;
            internal set;
        }

        public string Signature
        {
            get;
            internal set;
        }

        public string Token
        {
            get;
            internal set;
        }

        internal string Reserved
        {
            get;
            set;
        }
#if WINDOWS_UWP
        internal WebTokenRequestResult TokenRequestResult
        {
            get;
            set;
        }
#endif
    }
}
