// -----------------------------------------------------------------------
//  <copyright file="SocialGraphClasses.cs" company="Microsoft">
//      Copyright (c) Microsoft. All rights reserved.
//      Licensed under the MIT license. See LICENSE file in the project root for full license information.
//  </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.Xbox.Services.Social.Manager
{
    using global::System;
    using global::System.Collections.Generic;
    using global::System.Threading;
    using global::System.Threading.Tasks;

    using Microsoft.Xbox.Services.Presence;
    using Microsoft.Xbox.Services.System;

    internal struct UserGroupStatusChange
    {
        public IList<string> addGroup;
        public IList<ulong> removeGroup;
    }

    internal struct FireTimerCompletionContext
    {
        public bool isNull;
        public uint context;
        public int numObjects;
        public TaskCompletionSource<object> tce;
    }

    internal class InternalSocialEvent
    {
        private readonly TaskCompletionSource<object> taskCompletionSource = new TaskCompletionSource<object>();

        private InternalSocialEvent(InternalSocialEventType type)
        {
            this.Type = type;
        }

        public InternalSocialEvent() : this(InternalSocialEventType.Unknown)
        {
        }

        public InternalSocialEvent(InternalSocialEventType eventType, IList<XboxSocialUser> usersAffected) : this(eventType)
        {
            this.UsersAffected = usersAffected;
        }

        public InternalSocialEvent(InternalSocialEventType eventType, IList<SocialManagerPresenceRecord> presenceRecords) : this(eventType)
        {
            this.PresenceRecords = presenceRecords;
        }

        public InternalSocialEvent(InternalSocialEventType eventType, DevicePresenceChangeEventArgs devicePresenceArgs) : this(eventType)
        {
            this.DevicePresenceArgs = devicePresenceArgs;
        }

        public InternalSocialEvent(InternalSocialEventType eventType, TitlePresenceChangeEventArgs titlePresenceArgs) : this(eventType)
        {
            this.TitlePresenceArgs = titlePresenceArgs;
        }

        public InternalSocialEvent(InternalSocialEventType eventType, object errorInfo, IList<ulong> userList) : this(eventType)
        {
            this.UserIdsAffected = userList;
        }

        public InternalSocialEvent(InternalSocialEventType eventType, IList<ulong> userAddList) : this(eventType, userAddList, null)
        {
            this.UserIdsAffected = userAddList;
        }

        internal InternalSocialEventType Type { get; private set; }
        internal IList<XboxSocialUser> UsersAffected { get; private set; }
        internal IList<ulong> UserIdsAffected { get; private set; }
        internal IList<SocialManagerPresenceRecord> PresenceRecords { get; private set; }
        internal DevicePresenceChangeEventArgs DevicePresenceArgs { get; private set; }
        internal TitlePresenceChangeEventArgs TitlePresenceArgs { get; private set; }

        /// <summary>
        /// A task which completes when the event is processed.
        /// </summary>
        internal Task Task
        {
            get
            {
                return this.taskCompletionSource.Task;
            }
        }

        internal void ProcessEvent()
        {
            // Mark the task as complete.
            this.taskCompletionSource.SetResult(null);
        }
    }

    internal class RtaTriggerTimer
    {
        private static TimeSpan TIME_PER_CALL_MS;
        private Action<List<string>, FireTimerCompletionContext> m_fCallback;

        private bool m_iTaskInProgress;
        private DateTime m_previousTime;
        private bool m_queuedTask;
        private Mutex m_timerLock;
        private List<string> m_usersToCall;
        private Dictionary<string, bool> m_usersToCallMap; // duplicating data to make lookup faster. SHould be a better way to do this

        public RtaTriggerTimer()
        {
        }

        private RtaTriggerTimer(
            Action<List<string>, FireTimerCompletionContext> callback
        )
        {
            throw new NotImplementedException();
        }

        private void fire()
        {
            throw new NotImplementedException();
        }

        private void fire(List<string> xboxUserIds, FireTimerCompletionContext usersAddedStruct)
        {
            throw new NotImplementedException();
        }

        private void fire_helper(FireTimerCompletionContext usersAddedStruct)
        {
            throw new NotImplementedException();
        }
    }

    internal struct XboxSocialUserSubscriptions
    {
        //std::shared_ptr<xbox::services::presence::device_presence_change_subscription> devicePresenceChangeSubscription;
        //std::shared_ptr<xbox::services::presence::title_presence_change_subscription> titlePresenceChangeSubscription;
    }

    internal class InternalEventQueue
    {
        private readonly Queue<InternalSocialEvent> eventQueue;
        private const int MaxUsersAffectedPerEvent = 10;

        public InternalEventQueue()
        {
            this.eventQueue = new Queue<InternalSocialEvent>();
        }

        public void Enqueue(InternalSocialEventType socialEventType, List<ulong> userList)
        {
            InternalSocialEvent internalEvent = new InternalSocialEvent(socialEventType, userList);
            this.Enqueue(internalEvent);
        }

        public void Enqueue(InternalSocialEventType socialEventType, List<SocialManagerPresenceRecord> presenceRecords)
        {
            InternalSocialEvent internalEvent = new InternalSocialEvent(socialEventType, presenceRecords);
            this.Enqueue(internalEvent);
        }

        public Task Enqueue(InternalSocialEventType socialEventType, List<XboxSocialUser> userList)
        {
            var numGroupsofUsers = userList.Count / MaxUsersAffectedPerEvent + 1;

            List<Task> eventTasks = new List<Task>();
            for (int i = 0; i < numGroupsofUsers; ++i)
            {
                var evt = new InternalSocialEvent(socialEventType, userList.GetRange(i * numGroupsofUsers, numGroupsofUsers));
                this.Enqueue(evt);
                eventTasks.Add(evt.Task);
            }

            return Task.WhenAll(eventTasks);
        }

        public void Enqueue(InternalSocialEvent socialEvent)
        {
            lock (((global::System.Collections.ICollection)this.eventQueue).SyncRoot)
            {
                this.eventQueue.Enqueue(socialEvent);
            }
        }

        public bool TryDequeue(out InternalSocialEvent internalEvent)
        {
            lock (((global::System.Collections.ICollection)this.eventQueue).SyncRoot)
            {
                internalEvent = this.eventQueue.Count > 0 ? this.eventQueue.Dequeue() : null;
                return internalEvent != null;
            }
        }

        public bool Empty()
        {
            return this.eventQueue.Count == 0;
        }
    }

    internal class EventQueue : IEnumerable<SocialEvent>
    {
        private Queue<SocialEvent> events = new Queue<SocialEvent>();

        public EventQueue(XboxLiveUser user)
        {
            this.State = EventState.Clear;
        }

        public EventState State { get; private set; }

        public void Enqueue(InternalSocialEvent internalEvent, XboxLiveUser user)
        {
            if (internalEvent == null) throw new ArgumentNullException("internalEvent");
            if (internalEvent.Type == InternalSocialEventType.Unknown) throw new ArgumentException("Unable to handle Unknown event type.", "internalEvent");

            SocialEventType eventType;
            switch (internalEvent.Type)
            {
                case InternalSocialEventType.UsersAdded:
                    eventType = SocialEventType.UsersAddedToSocialGraph;
                    break;
                case InternalSocialEventType.UsersRemoved:
                    eventType = SocialEventType.UsersRemovedFromSocialGraph;
                    break;
                case InternalSocialEventType.PresenceChanged:
                case InternalSocialEventType.DevicePresenceChanged:
                case InternalSocialEventType.TitlePresenceChanged:
                    eventType = SocialEventType.PresenceChanged;
                    break;
                case InternalSocialEventType.ProfilesChanged:
                    eventType = SocialEventType.ProfilesChanged;
                    break;
                case InternalSocialEventType.SocialRelationshipsChanged:
                    eventType = SocialEventType.SocialRelationshipsChanged;
                    break;
                case InternalSocialEventType.Unknown:
                case InternalSocialEventType.UsersChanged:
                    // These events are not converted into public events.
                    return;
                default:
                    throw new ArgumentOutOfRangeException("internalEvent", internalEvent.Type, null);
            }

            SocialEvent evt = new SocialEvent(eventType, user, internalEvent.UserIdsAffected);
            this.events.Enqueue(evt);

            this.State = EventState.ReadyToRead;
        }

        public int Count
        {
            get
            {
                return this.events.Count;
            }
        }

        public void Clear()
        {
            this.events.Clear();
        }

        public IEnumerator<SocialEvent> GetEnumerator()
        {
            return this.events.GetEnumerator();
        }

        global::System.Collections.IEnumerator global::System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}