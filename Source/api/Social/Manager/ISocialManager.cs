using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Xbox.Services.System;

namespace Microsoft.Xbox.Services.Social.Manager
{
    public interface ISocialManager
    {
        IList<XboxLiveUser> LocalUsers { get; }

        Task AddLocalUser(XboxLiveUser user, SocialManagerExtraDetailLevel extraDetailLevel);

        void RemoveLocalUser(XboxLiveUser user);

        XboxSocialUserGroup CreateSocialUserGroupFromList(XboxLiveUser user, List<ulong> userIds);

        XboxSocialUserGroup CreateSocialUserGroupFromFilters(XboxLiveUser user, PresenceFilter presenceFilter, RelationshipFilter relationshipFilter, uint titleId);

        IList<SocialEvent> DoWork();
        
    }
}