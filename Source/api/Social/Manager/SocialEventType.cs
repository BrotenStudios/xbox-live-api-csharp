using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Microsoft.Xbox.Services.Social.Manager
{
    public enum SocialEventType : int
    {
        UsersAddedToSocialGraph = 0,
        UsersRemovedFromSocialGraph = 1,
        PresenceChanged = 2,
        ProfilesChanged = 3,
        SocialRelationshipsChanged = 4,
        LocalUserAdded = 5,
        LocalUserRemoved = 6,
        SocialUserGroupLoaded = 7,
        SocialUserGroupUpdated = 8,
    }

}
