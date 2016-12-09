using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Microsoft.Xbox.Services.Social
{
    public class XboxSocialRelationship
    {

        public IList<string> SocialNetworks
        {
            get;
            private set;
        }

        public bool IsFollowingCaller
        {
            get;
            private set;
        }

        public bool IsFavorite
        {
            get;
            private set;
        }

        public string XboxUserId
        {
            get;
            private set;
        }

    }
}
