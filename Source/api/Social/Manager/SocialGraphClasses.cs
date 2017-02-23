// Copyright (c) Microsoft Corporation
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// 
namespace Microsoft.Xbox.Services.Social.Manager
{
    using global::System;
    using global::System.Collections.Generic;
    using global::System.Threading;
    using global::System.Threading.Tasks;

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
}