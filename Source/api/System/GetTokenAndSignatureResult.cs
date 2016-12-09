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

        public string XboxUserHash
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

        public string Signature
        {
            get;
            private set;
        }

        public string Token
        {
            get;
            private set;
        }

    }
}
