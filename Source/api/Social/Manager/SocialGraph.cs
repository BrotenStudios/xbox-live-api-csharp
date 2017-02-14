// -----------------------------------------------------------------------
//  <copyright file="SocialGraph.cs" company="Microsoft">
//      Copyright (c) Microsoft. All rights reserved.
//      Licensed under the MIT license. See LICENSE file in the project root for full license information.
//  </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.Xbox.Services.Social.Manager
{
    using Windows.UI.ViewManagement;

    using global::System;
    using global::System.Collections.Generic;
    using global::System.Diagnostics;
    using global::System.Linq;
    using global::System.Threading;
    using global::System.Threading.Tasks;

    using Microsoft.Xbox.Services.Presence;
    using Microsoft.Xbox.Services.RealTimeActivity;
    using Microsoft.Xbox.Services.System;

    internal class SocialGraph : IDisposable
    {
        private const uint MaxEventsPerFrame = 5;

        private static readonly TimeSpan RefreshDuration = TimeSpan.FromSeconds(30);

        private readonly XboxLiveContext context;
        private readonly XboxLiveUser localUser;
        private readonly SocialManagerExtraDetailLevel detailLevel;

        private readonly EventQueue eventQueue;
        private readonly InternalEventQueue internalEventQueue;
        private UserBuffersHolder userBuffer;

        private SocialGraphState socialGraphState;
        private int numEventsThisFrame;

        private CancellationTokenSource backgroundTaskCancellationTokenSource;

        private bool isPollingRichPresence;
        private CancellationTokenSource richPresenceCancellationTokenSource;

        private readonly ReaderWriterLockSlim refreshLock = new ReaderWriterLockSlim();

        //Action<real_time_activity_connection_state> m_stateRTAFunction;
        //Dictionary<UInt64, XboxSocialUserSubscriptions> m_socialUserSubscriptions;
        //RtaTriggerTimer m_presenceRefreshTimer;
        //RtaTriggerTimer m_presencePollingTimer;
        //RtaTriggerTimer m_socialGraphRefreshTimer;
        //RtaTriggerTimer m_resyncRefreshTimer;
        //xbox::services::social::social_relationship_change_subscription m_socialRelationshipChangeSubscription;
        private PeopleHubService peopleHubService;
        private bool disposed;

        public SocialGraph(XboxLiveUser localUser, SocialManagerExtraDetailLevel detailLevel)
        {
            this.localUser = localUser;
            this.detailLevel = detailLevel;

            this.context = new XboxLiveContext(this.localUser);
            this.peopleHubService = new PeopleHubService(this.context.Settings, this.context.AppConfig);
            this.eventQueue = new EventQueue(this.localUser);
            this.internalEventQueue = new InternalEventQueue();
        }

        public bool IsInitialized { get; private set; }

        public XboxLiveUser LocalUser
        {
            get
            {
                return this.localUser;
            }
        }

        public uint TitleId
        {
            get
            {
                return this.context.AppConfig.TitleId;
            }
        }

        public Dictionary<ulong, XboxSocialUser> ActiveBufferSocialGraph
        {
            get
            {
                return this.userBuffer.Active.SocialUserGraph;
            }
        }

        public IEnumerable<XboxSocialUser> ActiveUsers
        {
            get
            {
                return this.userBuffer.Active.SocialUserGraph.Values;
            }
        }

        public Task Initialize()
        {
            if (this.IsInitialized)
            {
                throw new InvalidOperationException("Unable to initialize SocialGraph twice.");
            }

            var getProfileTask = this.peopleHubService.GetProfileInfo(this.localUser, this.detailLevel);
            var getGraphTask = this.peopleHubService.GetSocialGraph(this.localUser, this.detailLevel);

            return Task.WhenAll(getProfileTask, getGraphTask).ContinueWith(getTasks =>
            {
                // Wait for the task to throw any exceptions.
                getTasks.Wait();

                List<XboxSocialUser> users = getGraphTask.Result;
                users.Add(getProfileTask.Result);

                this.userBuffer = new UserBuffersHolder(users);


                // Kickoff the background tasks.
                this.backgroundTaskCancellationTokenSource = new CancellationTokenSource();
                this.RefreshGraphAsync(this.backgroundTaskCancellationTokenSource.Token);
                this.ProcessEventsAsync(this.backgroundTaskCancellationTokenSource.Token);

                this.IsInitialized = true;
            });
        }

        public Task AddUsers(IList<ulong> users)
        {
            InternalSocialEvent socialEvent = new InternalSocialEvent(InternalSocialEventType.UsersAdded, users);
            this.internalEventQueue.Enqueue(socialEvent);

            return socialEvent.Task;
        }

        public Task RemoveUsers(IList<ulong> users)
        {
            InternalSocialEvent socialEvent = new InternalSocialEvent(InternalSocialEventType.UsersRemoved, users);
            this.internalEventQueue.Enqueue(socialEvent);

            return socialEvent.Task;
        }

        public Task RemoveUsers(IList<XboxSocialUser> users)
        {
            InternalSocialEvent socialEvent = new InternalSocialEvent(InternalSocialEventType.UsersRemoved, users);
            this.internalEventQueue.Enqueue(socialEvent);

            return socialEvent.Task;
        }

        /// <summary>
        /// Process all events for this social graph
        /// </summary>
        /// <param name="events"></param>
        /// <returns>A list of users affected by the processed events.</returns>
        public Dictionary<ulong, XboxSocialUser> DoWork(List<SocialEvent> events)
        {
            this.refreshLock.EnterWriteLock();
            try
            {
                this.numEventsThisFrame = 0;

                if (this.socialGraphState == SocialGraphState.Normal)
                {
                    this.userBuffer.SwapIfEmpty();

                    if (this.eventQueue.Count > 0)
                    {
                        foreach (SocialEvent socialEvent in this.eventQueue)
                        {
                            events.Add(socialEvent);
                        }

                        this.eventQueue.Clear();
                    }
                }

                return this.userBuffer.Active.SocialUserGraph;
            }
            finally
            {
                this.refreshLock.ExitWriteLock();
            }
        }

        private void EnableRichPresencePolling(bool shouldEnablePolling)
        {
            bool wasEnabled = this.isPollingRichPresence;
            this.isPollingRichPresence = shouldEnablePolling;


            if (wasEnabled)
            {
                this.richPresenceCancellationTokenSource.Cancel();
            }

            if (shouldEnablePolling)
            {
                this.richPresenceCancellationTokenSource = new CancellationTokenSource();
                this.RefreshPresenceAsync(this.richPresenceCancellationTokenSource.Token);
            }
        }

        /// <summary>
        /// Background task to refresh the graph.  This would be great to do in a loop but there's not
        /// an easy way to do so without await, so we'll go ahead with the current pattern.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        private void RefreshGraphAsync(CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return;
            }


            this.peopleHubService.GetSocialGraph(this.localUser, this.detailLevel)
                .ContinueWith(t =>
                {
                    try
                    {
                        if (!t.IsFaulted)
                        {
                            this.refreshLock.EnterWriteLock();
                            this.socialGraphState = SocialGraphState.Refresh;

                            List<XboxSocialUser> userRefreshList = new List<XboxSocialUser>();
                            foreach (XboxSocialUser graphUser in this.userBuffer.Inactive.SocialUserGraph.Values)
                            {
                                if (!graphUser.IsFollowedByCaller)
                                {
                                    userRefreshList.Add(graphUser);
                                }
                            }

                            // TODO: We have some RTA triggers to called based on the userRefreshList.

                            // Regardless, we can perform the diff which will give us any change events.
                            this.PerformDiff(t.Result);
                        }
                    }
                    finally
                    {
                        this.socialGraphState = SocialGraphState.Normal;
                        this.refreshLock.ExitWriteLock();
                        // Setup another refresh for the future.
                        Task nextRefresh = Task.Delay(RefreshDuration).ContinueWith(
                            delayTask => this.RefreshGraphAsync(cancellationToken),
                            cancellationToken);
                    }
                });
        }

        private void RefreshPresenceAsync(CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return;
            }

            this.refreshLock.EnterWriteLock();
            try
            {
                this.socialGraphState = SocialGraphState.Refresh;

                // TODO: Fire a presence refresh for these users.
                List<ulong> userIds = this.userBuffer.Inactive.SocialUserGraph.Keys.ToList();
            }
            finally
            {
                this.socialGraphState = SocialGraphState.Normal;
                this.refreshLock.ExitWriteLock();

                // Setup another refresh for the future.
                // TODO: Make this delay the correct value.  Should be something based on RTA.
                Task.Delay(RefreshDuration).ContinueWith(
                    delayTask => this.RefreshPresenceAsync(cancellationToken),
                    cancellationToken);
            }
        }

        private void ProcessEventsAsync(CancellationToken cancellationToken)
        {
            while (this.ProcessNextEvent())
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    return;
                }
            }

            // Queue up a processing task again.
            Task.Delay(TimeSpan.FromMilliseconds(30)).ContinueWith(
                _ => this.ProcessEventsAsync(cancellationToken),
                cancellationToken);
        }

        private void ProcessCachedEvents()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Process the next event that's available and return true if there are more events to process.
        /// </summary>
        /// <returns>True if there are more events to process.</returns>
        private bool ProcessNextEvent()
        {
            if (this.numEventsThisFrame < MaxEventsPerFrame)
            {
                InternalSocialEvent internalEvent;
                if (this.internalEventQueue.TryDequeue(out internalEvent))
                {
                    Interlocked.Increment(ref this.numEventsThisFrame);
                    this.ApplyEvent(internalEvent, true);
                    this.userBuffer.AddEvent(internalEvent);
                    internalEvent.ProcessEvent();
                }
            }

            return !this.internalEventQueue.IsEmpty && this.numEventsThisFrame < MaxEventsPerFrame;
        }

        private void PerformDiff(List<XboxSocialUser> xboxSocialUsers)
        {
            try
            {
                this.socialGraphState = SocialGraphState.Diff;

                List<XboxSocialUser> usersAddedList = new List<XboxSocialUser>();
                List<ulong> usersRemovedList = new List<ulong>();
                List<XboxSocialUser> presenceChangeList = new List<XboxSocialUser>();
                List<XboxSocialUser> socialRelationshipChangeList = new List<XboxSocialUser>();
                List<XboxSocialUser> profileChangeList = new List<XboxSocialUser>();

                foreach (XboxSocialUser currentUser in xboxSocialUsers)
                {
                    XboxSocialUser existingUser;
                    if (!this.userBuffer.Inactive.SocialUserGraph.TryGetValue(currentUser.XboxUserId, out existingUser))
                    {
                        usersAddedList.Add(currentUser);
                        continue;
                    }

                    var changes = existingUser.GetChanges(currentUser);
                    if (changes.HasFlag(ChangeListType.ProfileChange))
                    {
                        presenceChangeList.Add(currentUser);
                    }

                    if (changes.HasFlag(ChangeListType.SocialRelationshipChange))
                    {
                        socialRelationshipChangeList.Add(currentUser);
                    }

                    if (changes.HasFlag(ChangeListType.PresenceChange))
                    {
                        profileChangeList.Add(currentUser);
                    }
                }

                foreach (XboxSocialUser socialUser in this.userBuffer.Inactive.SocialUserGraph.Values)
                {
                    if (socialUser.XboxUserId.ToString() == this.localUser.XboxUserId)
                    {
                        continue;
                    }

                    if (!xboxSocialUsers.Contains(socialUser, XboxSocialUserIdEqualityComparer.Instance))
                    {
                        usersRemovedList.Add(socialUser.XboxUserId);
                    }
                }

                if (usersAddedList.Count > 0)
                {
                    this.internalEventQueue.Enqueue(InternalSocialEventType.UsersChanged, usersAddedList);
                }
                if (usersRemovedList.Count > 0)
                {
                    this.internalEventQueue.Enqueue(InternalSocialEventType.UsersRemoved, usersRemovedList);
                }
                if (presenceChangeList.Count > 0)
                {
                    this.internalEventQueue.Enqueue(InternalSocialEventType.PresenceChanged, presenceChangeList);
                }
                if (profileChangeList.Count > 0)
                {
                    this.internalEventQueue.Enqueue(InternalSocialEventType.ProfilesChanged, profileChangeList);
                }
                if (socialRelationshipChangeList.Count > 0)
                {
                    this.internalEventQueue.Enqueue(InternalSocialEventType.SocialRelationshipsChanged, socialRelationshipChangeList);
                }
            }
            finally
            {
                this.socialGraphState = SocialGraphState.Normal;
            }
        }

        private void ApplyEvent(InternalSocialEvent internalEvent, bool isFreshEvent)
        {
            UserBuffer inactiveBuffer = this.userBuffer.Inactive;
            switch (internalEvent.Type)
            {
                case InternalSocialEventType.UsersAdded:
                {
                    this.ApplyUsersAddedEvent(internalEvent, isFreshEvent);
                    break;
                }
                case InternalSocialEventType.UsersChanged:
                {
                    this.ApplyUsersChangeEvent(internalEvent, isFreshEvent);
                    break;
                }
                case InternalSocialEventType.UsersRemoved:
                {
                    this.ApplyUsersRemovedEvent(internalEvent, inactiveBuffer, isFreshEvent);
                    break;
                }
                case InternalSocialEventType.DevicePresenceChanged:
                {
                    this.ApplyDevicePresenceChangedEvent(internalEvent, inactiveBuffer, isFreshEvent);
                    break;
                }
                case InternalSocialEventType.TitlePresenceChanged:
                {
                    this.ApplyTitlePresenceChangedEvent(internalEvent, inactiveBuffer, isFreshEvent);
                    break;
                }
                case InternalSocialEventType.PresenceChanged:
                {
                    this.ApplyPresenceChangedEvent(internalEvent, inactiveBuffer, isFreshEvent);
                    break;
                }
                case InternalSocialEventType.SocialRelationshipsChanged:
                case InternalSocialEventType.ProfilesChanged:
                {
                    foreach (var affectedUser in internalEvent.UsersAffected)
                    {
                        inactiveBuffer.SocialUserGraph[affectedUser.XboxUserId] = affectedUser;
                    }

                    break;
                }
                default:
                    throw new ArgumentOutOfRangeException("internalEvent", internalEvent.Type, "Unknown event type.");
            }

            if (isFreshEvent)
            {
                this.eventQueue.Enqueue(internalEvent, this.localUser);
                this.userBuffer.AddEvent(internalEvent);
            }
        }

        private void ApplyUsersAddedEvent(InternalSocialEvent socialEvent, bool isFreshEvent)
        {
            List<XboxSocialUser> usersAdded = new List<XboxSocialUser>();

            if (socialEvent.UsersAffected == null)
            {
                Debug.WriteLine("Unable to apply UsersAdded event because we only have userIds.");
                return;
            }

            foreach (XboxSocialUser socialUser in socialEvent.UsersAffected)
            {
                if (this.userBuffer.Inactive.TryAdd(socialUser))
                {
                    usersAdded.Add(socialUser);
                }
            }
        }

        private void ApplyUsersChangeEvent(InternalSocialEvent socialEvent, bool isFreshEvent)
        {
            List<XboxSocialUser> usersToAdd = new List<XboxSocialUser>();
            List<XboxSocialUser> usersToChange = new List<XboxSocialUser>();

            foreach (XboxSocialUser user in socialEvent.UsersAffected)
            {
                if (this.userBuffer.Inactive.SocialUserGraph.ContainsValue(user))
                {
                    usersToChange.Add(user);
                }
                else
                {
                    usersToAdd.Add(user);
                }
            }

            if (usersToAdd.Count > 0)
            {
                this.userBuffer.AddUsers(usersToAdd);

                if (isFreshEvent)
                {
                    //setup_device_and_presence_subscriptions(usersList);

                    InternalSocialEvent internalEvent = new InternalSocialEvent(InternalSocialEventType.UsersAdded, usersToAdd);
                    this.internalEventQueue.Enqueue(internalEvent);
                }
            }

            if (usersToChange.Count > 0 && isFreshEvent)
            {
                InternalSocialEvent internalEvent = new InternalSocialEvent(InternalSocialEventType.ProfilesChanged, usersToChange);
                this.internalEventQueue.Enqueue(internalEvent);
            }
        }

        private void ApplyUsersRemovedEvent(InternalSocialEvent socialEvent, UserBuffer inactiveBuffer, bool isFreshEvent)
        {
            this.userBuffer.Inactive.Enqueue(socialEvent);
            foreach (XboxSocialUser user in socialEvent.UsersAffected)
            {
                
            }
        }

        private void ApplyDevicePresenceChangedEvent(InternalSocialEvent socialEvent, UserBuffer inactiveBuffer, bool isFreshEvent)
        {
            throw new NotImplementedException();
        }

        private void ApplyTitlePresenceChangedEvent(InternalSocialEvent socialEvent, UserBuffer inactiveBuffer, bool isFreshEvent)
        {
            var titlePresenceChanged = socialEvent.TitlePresenceArgs;
            var xuid = Convert.ToUInt64(titlePresenceChanged.XboxUserId);

            var eventUser = inactiveBuffer.SocialUserGraph[xuid];
            if (eventUser != null)
            {
                if (titlePresenceChanged.TitleState == TitlePresenceState.Ended)
                {
                    var titleRecord = eventUser.PresenceDetails.FirstOrDefault(r => r.TitleId == titlePresenceChanged.TitleId);
                    eventUser.PresenceDetails.Remove(titleRecord);
                }
            }
        }

        private void ApplyPresenceChangedEvent(InternalSocialEvent socialEvent, UserBuffer inactiveBuffer, bool isFreshEvent)
        {
            throw new NotImplementedException();
        }

        private void presence_timer_callback(IList<string> users)
        {
            throw new NotImplementedException();
        }

        private Task<IList<XboxSocialUser>> social_graph_timer_callback(IList<string> users, object completionContext)
        {
            TaskCompletionSource<IList<XboxSocialUser>> tcs = new TaskCompletionSource<IList<XboxSocialUser>>();

            this.peopleHubService.GetSocialGraph(this.localUser, this.detailLevel, users).ContinueWith(getSocialGraphTask =>
            {
                if (getSocialGraphTask.IsFaulted)
                {
                    tcs.SetException(getSocialGraphTask.Exception);
                    return;
                }

                List<XboxSocialUser> socialUsers = getSocialGraphTask.Result;
                this.internalEventQueue.Enqueue(InternalSocialEventType.UsersChanged, socialUsers).ContinueWith(enqueueTask =>
                {
                    if (enqueueTask.IsFaulted)
                    {
                        tcs.SetException(enqueueTask.Exception);
                        return;
                    }

                    tcs.SetResult(socialUsers);
                });
            });

            return tcs.Task;
        }

        private void setup_rta()
        {
            throw new NotImplementedException();
        }

        private void setup_rta_subscriptions(
            bool shouldReinitialize = false
        )
        {
            throw new NotImplementedException();
        }

        private void update_graph(IList<XboxSocialUser> userList)
        {
            throw new NotImplementedException();
        }

        private void handle_title_presence_change(TitlePresenceChangeEventArgs titlePresenceChanged)
        {
            throw new NotImplementedException();
        }

        private void handle_device_presence_change(DevicePresenceChangeEventArgs devicePresenceChanged)
        {
            throw new NotImplementedException();
        }

        private void handle_social_relationship_change(SocialRelationshipChangeEventArgs socialRelationshipChanged)
        {
            throw new NotImplementedException();
        }

        private void handle_rta_subscription_error(RealTimeActivitySubscriptionErrorEventArgs rtaErrorEventArgs)
        {
            throw new NotImplementedException();
        }

        private void handle_rta_connection_state_change(RealTimeActivityConnectionState rtaState)
        {
            throw new NotImplementedException();
        }

        private void setup_device_and_presence_subscriptions(IList<ulong> users)
        {
            throw new NotImplementedException();
        }

        private void setup_device_and_presence_subscriptions_helper(IList<ulong> users)
        {
            throw new NotImplementedException();
        }

        private void unsubscribe_users(IList<ulong> users)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            if (!this.disposed)
            {
                this.backgroundTaskCancellationTokenSource.Cancel();
                this.disposed = true;
            }
        }
    }
}