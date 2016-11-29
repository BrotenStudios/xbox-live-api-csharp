using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Microsoft.Xbox.Services.Social.Manager
{
    public class SocialManager
    {

        public IList<Microsoft.Xbox.Services.System.XboxLiveUser> LocalUsers
        {
            get;
            private set;
        }

        public static SocialManager SingletonInstance
        {
            get;
            private set;
        }


        public void AddLocalUser(Microsoft.Xbox.Services.System.XboxLiveUser user, SocialManagerExtraDetailLevel extraDetailLevel)
        {
            throw new NotImplementedException();
        }

        public void RemoveLocalUser(Microsoft.Xbox.Services.System.XboxLiveUser user)
        {
            throw new NotImplementedException();
        }

        public IList<SocialEvent> DoWork()
        {
            throw new NotImplementedException();
        }

        public XboxSocialUserGroup CreateSocialUserGroupFromFilters(Microsoft.Xbox.Services.System.XboxLiveUser user, PresenceFilter presenceFilter, RelationshipFilter relationshipFilter)
        {
            throw new NotImplementedException();
        }

        public XboxSocialUserGroup CreateSocialUserGroupFromList(Microsoft.Xbox.Services.System.XboxLiveUser user, string[] xboxUserIdList)
        {
            throw new NotImplementedException();
        }

        public void DestroySocialUserGroup(XboxSocialUserGroup xboxSocialUserGroup)
        {
            throw new NotImplementedException();
        }

        public void UpdateSocialUserGroup(XboxSocialUserGroup socialGroup, string[] users)
        {
            throw new NotImplementedException();
        }

    }
}
