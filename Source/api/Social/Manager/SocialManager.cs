// -----------------------------------------------------------------------
//  <copyright file="SocialManager.cs" company="Microsoft">
//      Copyright (c) Microsoft. All rights reserved.
//      Internal use only.
//  </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.Xbox.Services.Social.Manager
{
    using global::System;
    using global::System.Collections.Generic;
    using global::System.Linq;
    using global::System.Threading.Tasks;

    using Microsoft.Xbox.Services.System;

    public class SocialManager : ISocialManager
    {
        private static ISocialManager instance;

        private readonly Queue<SocialEvent> eventQueue = new Queue<SocialEvent>();
        private readonly List<XboxLiveUser> localUsers = new List<XboxLiveUser>();

        private readonly object syncRoot = new object();
        private readonly Dictionary<XboxLiveUser, SocialGraph> userGraphs = new Dictionary<XboxLiveUser, SocialGraph>(new XboxUserIdEqualityComparer());
        private readonly Dictionary<XboxLiveUser, HashSet<WeakReference>> userGroupsMap = new Dictionary<XboxLiveUser, HashSet<WeakReference>>(new XboxUserIdEqualityComparer());

        private SocialManager()
        {
        }

        public static ISocialManager Instance
        {
            get
            {
                return instance ?? (instance = XboxLiveContext.UseMockData ? new MockSocialManager() : (ISocialManager)new SocialManager());
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

            SocialGraph graph = new SocialGraph(user, extraDetailLevel);
            return graph.Initialize().ContinueWith(
                initializeTask =>
                {
                    // Wait on the task to throw an exceptions.
                    initializeTask.Wait();

                    lock (this.syncRoot)
                    {
                        this.userGraphs[user] = graph;
                        this.localUsers.Add(user);
                    }
                });
        }

        public void RemoveLocalUser(XboxLiveUser user)
        {
            if (user == null) throw new ArgumentNullException("user");

            lock (this.syncRoot)
            {
                this.localUsers.Remove(user);
                this.userGraphs.Remove(user);
            }
        }

        public IList<SocialEvent> DoWork()
        {
            // TODO make this threadsafe
            List<SocialEvent> events = this.eventQueue.ToList();
            this.eventQueue.Clear();

            lock (this.syncRoot)
            {
                foreach (SocialGraph graph in this.userGraphs.Values)
                {
                    graph.DoWork(events);

                    HashSet<WeakReference> userGroups;
                    if (this.userGroupsMap.TryGetValue(graph.LocalUser, out userGroups))
                    {
                        // Graph the social groups for this user and update them.
                        foreach (WeakReference groupReference in userGroups.ToList())
                        {
                            XboxSocialUserGroup group = groupReference.Target as XboxSocialUserGroup;
                            if (group == null)
                            {
                                userGroups.Remove(groupReference);
                                continue;
                            }

                            group.UpdateView(graph.ActiveBufferSocialGraph, events);
                        }
                    }
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
            if (userGraph.IsInitialized)
            {
                group.InitializeGroup(userGraph.ActiveUsers);
            }

            this.AddUserGroup(user, group);

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
            if (userGraph.IsInitialized)
            {
                group.InitializeGroup(userGraph.ActiveUsers);
            }

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

        private void AddUserGroup(XboxLiveUser user, XboxSocialUserGroup group)
        {
            lock (this.syncRoot)
            {
                HashSet<WeakReference> userGroups;
                if (!this.userGroupsMap.TryGetValue(user, out userGroups))
                {
                    this.userGroupsMap[user] = userGroups = new HashSet<WeakReference>();
                }

                WeakReference groupReference = new WeakReference(group);
                userGroups.Add(groupReference);
            }
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

        /// <summary>
        /// Used by tests to reset the state of the SocialManager.
        /// </summary>
        internal static void Reset()
        {
            foreach (XboxLiveUser user in Instance.LocalUsers.ToList())
            {
                Instance.RemoveLocalUser(user);
            }

            instance = null;
        }
    }
}