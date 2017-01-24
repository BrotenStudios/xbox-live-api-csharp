// -----------------------------------------------------------------------
//  <copyright file="SocialManager.cs" company="Microsoft">
//      Copyright (c) Microsoft. All rights reserved.
//      Licensed under the MIT license. See LICENSE file in the project root for full license information.
//  </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.Xbox.Services.Social.Manager
{
    using global::System;
    using global::System.Collections.Generic;
    using global::System.Linq;
    using global::System.Threading.Tasks;

    using Microsoft.Xbox.Services.System;

    public class SocialManager
    {
        private static SocialManager instance;

        private readonly Queue<SocialEvent> eventQueue = new Queue<SocialEvent>();
        private readonly List<XboxLiveUser> localUsers = new List<XboxLiveUser>();

        private readonly Dictionary<XboxLiveUser, SocialGraph> userGraphs = new Dictionary<XboxLiveUser, SocialGraph>(new XboxUserIdEqualityComparer());
        private readonly Dictionary<XboxLiveUser, HashSet<XboxSocialUserGroup>> userGroupsMap = new Dictionary<XboxLiveUser, HashSet<XboxSocialUserGroup>>(new XboxUserIdEqualityComparer());

        private SocialManager()
        {
        }

        public static SocialManager Instance
        {
            get
            {
                return instance ?? (instance = new SocialManager());
            }
        }

        public IList<XboxLiveUser> LocalUsers
        {
            get
            {
                return this.localUsers.AsReadOnly();
            }
        }

        public Task AddLocalUser(XboxLiveUser user, SocialManagerExtraDetailLevel extraDetailLevel)
        {
            if (user == null) throw new ArgumentNullException("user");

            if (this.userGraphs.ContainsKey(user))
            {
                throw new XboxException("User already exists in graph.");
            }

            this.localUsers.Add(user);

            SocialGraph graph = new SocialGraph(user, extraDetailLevel);
            return graph.Initialize().ContinueWith(initializeTask => { this.userGraphs[user] = graph; });
        }

        public void RemoveLocalUser(XboxLiveUser user)
        {
            if (user == null) throw new ArgumentNullException("user");

            this.localUsers.Remove(user);
            this.userGraphs.Remove(user);
        }

        public IList<SocialEvent> DoWork()
        {
            // TODO make this threadsafe
            List<SocialEvent> events = this.eventQueue.ToList();
            this.eventQueue.Clear();

            foreach (SocialGraph graph in this.userGraphs.Values)
            {
                graph.DoWork(events);

                foreach (XboxSocialUserGroup group in this.userGroupsMap.SelectMany(g => g.Value))
                {
                    group.UpdateView(graph.ActiveBufferSocialGraph, events);
                }
                
            }

            return events.ToList();
        }

        public XboxSocialUserGroup CreateSocialUserGroupFromFilters(XboxLiveUser user, PresenceFilter presenceFilter, RelationshipFilter relationshipFilter, uint titleId)
        {
            if (user == null) throw new ArgumentNullException("user");

            SocialGraph userGraph;
            if (!this.userGraphs.TryGetValue(user, out userGraph))
            {
                throw new ArgumentException("You must add a local user before you can create a social group for them.", "user");
            }

            XboxSocialUserGroup group = new XboxSocialUserGroup(user, presenceFilter, relationshipFilter, userGraph.TitleId);
            this.AddUserGroup(user, group);

            if (userGraph.IsInitialized)
            {
                group.InitializeFilterList(userGraph.ActiveUsers);
            }

            return group;
        }

        public XboxSocialUserGroup CreateSocialUserGroupFromList(XboxLiveUser user, List<ulong> userIds)
        {
            if (user == null) throw new ArgumentNullException("user");
            if (userIds == null) throw new ArgumentNullException("userIds");

            SocialGraph userGraph;
            if (!this.userGraphs.TryGetValue(user, out userGraph))
            {
                throw new ArgumentException("You must add a local user before you can create a social group for them.", "user");
            }

            XboxSocialUserGroup group = new XboxSocialUserGroup(user, userIds);
            this.AddUserGroup(user, group);

            userGraph.AddUsers(userIds).ContinueWith(addUsersTask =>
            {
                if (!addUsersTask.IsFaulted)
                {
                    this.eventQueue.Enqueue(new SocialEvent(SocialEventType.SocialUserGroupLoaded, user, null, group) { Exception = addUsersTask.Exception });
                }

                this.eventQueue.Enqueue(new SocialEvent(SocialEventType.SocialUserGroupLoaded, user, userIds, group));
            });
            
            return group;
        }

        public void DestroySocialUserGroup(XboxSocialUserGroup group)
        {
            if (group == null) throw new ArgumentNullException("group");

            SocialGraph userGraph;
            if (!this.userGraphs.TryGetValue(group.LocalUser, out userGraph))
            {
                throw new ArgumentException("The user this group is associated with does not exist.", "group");
            }

            HashSet<XboxSocialUserGroup> usersGroups;
            if (!this.userGroupsMap.TryGetValue(group.LocalUser, out usersGroups) || !usersGroups.Contains(group))
            {
                throw new ArgumentException("Social user group not found.", "group");
            }

            usersGroups.Remove(group);
            userGraph.RemoveUsers(group.Users.ToList());
        }

        private void AddUserGroup(XboxLiveUser user, XboxSocialUserGroup group)
        {
            HashSet<XboxSocialUserGroup> userGroups;
            if (!this.userGroupsMap.TryGetValue(user, out userGroups))
            {
                this.userGroupsMap[user] = userGroups = new HashSet<XboxSocialUserGroup>();
            }

            userGroups.Add(group);
        }

        public void UpdateSocialUserGroup(XboxSocialUserGroup group, List<ulong> users)
        {
            this.userGraphs[group.LocalUser].AddUsers(users).ContinueWith(
                addUsersTask =>
                {
                    SocialEvent socialEvent = new SocialEvent(SocialEventType.SocialUserGroupUpdated, group.LocalUser, users);
                    this.eventQueue.Enqueue(socialEvent);
                });
        }
    }
}