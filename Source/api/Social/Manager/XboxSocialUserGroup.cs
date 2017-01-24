// -----------------------------------------------------------------------
//  <copyright file="XboxSocialUserGroup.cs" company="Microsoft">
//      Copyright (c) Microsoft. All rights reserved.
//      Licensed under the MIT license. See LICENSE file in the project root for full license information.
//  </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.Xbox.Services.Social.Manager
{
    using global::System;
    using global::System.Collections.Generic;
    using global::System.Linq;

    using Microsoft.Xbox.Services.Presence;
    using Microsoft.Xbox.Services.System;

    public class XboxSocialUserGroup : IEnumerable<XboxSocialUser>
    {
        private const int MaxUsersFromList = 100;

        private readonly List<ulong> userIds;
        private readonly HashSet<XboxSocialUser> users = new HashSet<XboxSocialUser>(new XboxSocialUserIdEqualityComparer());

        private XboxSocialUserGroup(XboxLiveUser localUser, SocialUserGroupType type)
        {
            this.LocalUser = localUser;
            this.SocialUserGroupType = type;
        }

        internal XboxSocialUserGroup(XboxLiveUser localUser, ICollection<ulong> userIds)
            : this(localUser, SocialUserGroupType.UserListType)
        {
            if (userIds == null) throw new ArgumentNullException("userIds");
            if (userIds.Count == 0) throw new ArgumentException("You must provide at least on user id to create a group.", "userIds");
            if (userIds.Count > MaxUsersFromList) throw new ArgumentException(string.Format("You cannot provide more than {0} user ides to create a group.", MaxUsersFromList), "userIds");

            this.userIds = userIds.ToList();
            // This list will be populated as user info becomes available.
            this.users = new HashSet<XboxSocialUser>();
        }

        internal XboxSocialUserGroup(XboxLiveUser localUser, PresenceFilter presenceFilter, RelationshipFilter relationshipFilter, uint titleId)
            : this(localUser, SocialUserGroupType.FilterType)
        {
            this.PresenceFilter = presenceFilter;
            this.RelationshipFilter = relationshipFilter;
            this.TitleId = titleId;
        }

        public XboxLiveUser LocalUser { get; private set; }

        public SocialUserGroupType SocialUserGroupType { get; private set; }

        public PresenceFilter PresenceFilter { get; private set; }

        public RelationshipFilter RelationshipFilter { get; private set; }

        public uint TitleId { get; set; }

        public ICollection<XboxSocialUser> Users
        {
            get
            {
                return this.users;
            }
        }

        public XboxSocialUser GetUser(ulong userId)
        {
            return this.users.FirstOrDefault(u => u.XboxUserId == userId);
        }

        public IEnumerator<XboxSocialUser> GetEnumerator()
        {
            return this.userIds.Select(this.GetUser).GetEnumerator();
        }

        global::System.Collections.IEnumerator global::System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        private void AddUser(XboxSocialUser user)
        {
            // Add their user id to our tracking list if we don't already have it.
            // TODO: can we replace this list with a hashset as well?  Would that be any better?
            if (!this.userIds.Contains(user.XboxUserId))
            {
                this.userIds.Add(user.XboxUserId);
            }

            // We need to remove first because we want to replace the user that was originally 
            // there if the one we're getting has updated information.
            this.users.Remove(user);
            this.users.Add(user);
        }

        private void RemoveUser(XboxSocialUser user)
        {
            this.userIds.Remove(user.XboxUserId);
            this.users.Remove(user);
        }

        internal void UpdateView(Dictionary<ulong, XboxSocialUser> userSnapshot, List<SocialEvent> events)
        {
            switch (this.SocialUserGroupType)
            {
                case SocialUserGroupType.FilterType:
                    this.FilterList(userSnapshot, events);
                    break;
                case SocialUserGroupType.UserListType:
                    foreach (ulong userId in this.userIds)
                    {
                        XboxSocialUser user;
                        if (userSnapshot.TryGetValue(userId, out user))
                        {
                            this.AddUser(user);
                        }
                    }
                    break;
            }
        }

        internal void InitializeFilterList(IEnumerable<XboxSocialUser> users)
        {
            foreach (XboxSocialUser user in users)
            {
                if (!this.CheckRelationshipFilter(user, this.RelationshipFilter)) continue;
                if (this.CheckPresenceFilter(user, this.PresenceFilter))
                {
                    this.AddUser(user);
                }
            }
        }

        private void FilterList(IDictionary<ulong, XboxSocialUser> users, IEnumerable<SocialEvent> events)
        {
            foreach (SocialEvent socialEvent in events)
            {
                switch (socialEvent.EventType)
                {
                    case SocialEventType.PresenceChanged:
                    case SocialEventType.ProfilesChanged:
                    case SocialEventType.SocialRelationshipsChanged:
                        foreach (ulong userId in socialEvent.UsersAffected)
                        {
                            XboxSocialUser user = users[userId];

                            if (this.CheckRelationshipFilter(user, this.RelationshipFilter))
                            {
                                if (this.CheckPresenceFilter(user, this.PresenceFilter))
                                {
                                    this.RemoveUser(user);
                                }
                                else
                                {
                                    this.AddUser(user);
                                }
                            }
                        }
                        break;
                    case SocialEventType.UsersAddedToSocialGraph:
                        foreach (ulong userId in socialEvent.UsersAffected)
                        {
                            XboxSocialUser user = users[userId];

                            if (this.CheckRelationshipFilter(user, this.RelationshipFilter) && this.CheckPresenceFilter(user, this.PresenceFilter))
                            {
                                this.AddUser(user);
                            }
                        }
                        break;
                    case SocialEventType.UsersRemovedFromSocialGraph:
                        foreach (ulong userId in socialEvent.UsersAffected)
                        {
                            XboxSocialUser user = users[userId];
                            this.RemoveUser(user);
                        }
                        break;
                    default:
                        continue;
                }
            }
        }

        private bool CheckPresenceFilter(XboxSocialUser user, PresenceFilter presenceFilter)
        {
            switch (presenceFilter)
            {
                case PresenceFilter.All:
                    return true;
                case PresenceFilter.AllOffline:
                    return user.PresenceRecord.UserState == UserPresenceState.Offline;
                case PresenceFilter.AllOnline:
                    return user.PresenceRecord.UserState == UserPresenceState.Online;
                case PresenceFilter.AllTitle:
                    return user.TitleHistory.HasUserPlayed;
                case PresenceFilter.TitleOffline:
                    return user.PresenceRecord.UserState == UserPresenceState.Offline && user.TitleHistory.HasUserPlayed;
                case PresenceFilter.TitleOnline:
                    return user.PresenceRecord.IsUserPlayingTitle(this.TitleId);
                default:
                    return false;
            }
        }

        private bool CheckRelationshipFilter(XboxSocialUser user, RelationshipFilter relationshipFilter)
        {
            switch (relationshipFilter)
            {
                case RelationshipFilter.Friends:
                    return user.IsFollowedByCaller;
                case RelationshipFilter.Favorite:
                    return user.IsFavorite;
                default:
                    throw new ArgumentOutOfRangeException("relationshipFilter", relationshipFilter, "Unexpected relationship filter value.");
            }
        }
    }
}