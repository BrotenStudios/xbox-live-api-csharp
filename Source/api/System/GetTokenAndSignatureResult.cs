using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Microsoft.Xbox.Services.System
{
    public class GetTokenAndSignatureResult
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

    }
}
