using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Microsoft.Xbox.Services.Social.Manager
{
    public class XboxSocialUserGroup
    {

        public RelationshipFilter RelationshipFilterOfGroup
        {
            get;
            private set;
        }

        public PresenceFilter PresenceFilterOfGroup
        {
            get;
            private set;
        }

        public Microsoft.Xbox.Services.System.XboxLiveUser LocalUser
        {
            get;
            private set;
        }

        public IList<string> UsersTrackedBySocialUserGroup
        {
            get;
            private set;
        }

        public SocialUserGroupType SocialUserGroupType
        {
            get;
            private set;
        }

        public IList<XboxSocialUser> Users
        {
            get;
            private set;
        }


        public IList<XboxSocialUser> GetUsersFromXboxUserIds(string[] xboxUserIds)
        {
            throw new NotImplementedException();
        }

    }
}
