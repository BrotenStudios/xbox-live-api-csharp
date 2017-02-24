// Copyright (c) Microsoft Corporation
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// 


namespace Microsoft.Xbox.Services.Shared
{
    using global::System;
    using global::System.Collections.Generic;
    using global::System.Linq;
    using global::System.Threading.Tasks;

    internal class CallBufferTimerCompletionContext
    {
        public int Context { get; private set; }
        public int NumObjects { get; private set; }
    }

    internal class CallBufferReturnObject : EventArgs
    {
        public List<string> UserList { get; private set; }
        public CallBufferTimerCompletionContext CompletionContext { get; private set; }

        public CallBufferReturnObject(List<string> userList, CallBufferTimerCompletionContext context)
        {
            this.UserList = userList;
            this.CompletionContext = context;
        }
    }

    internal class CallBufferTimer
    {
        private readonly TimeSpan duration;
        private readonly List<string> usersToCall = new List<string>();
        private readonly Dictionary<string, bool> usersToCallMap = new Dictionary<string, bool>();

        private bool isTaskInProgress;
        private bool isQueuedTask;
        private DateTime previousTime;
        private CallBufferTimerCompletionContext callBufferTimerCompletionContext;

        public event EventHandler<CallBufferReturnObject> TimerCompleteEvent;

        public CallBufferTimer(TimeSpan duration)
        {
            this.duration = duration;
        }

        public void Fire()
        {
            this.FireHelper();
        }

        public void Fire(List<string> xboxUserIds, CallBufferTimerCompletionContext completionContext = null)
        {
            if (xboxUserIds == null)
            {
                throw new ArgumentNullException("xboxUserIds");
            }

            lock (this.usersToCall)
            {
                this.callBufferTimerCompletionContext = completionContext;
                foreach (string xuid in xboxUserIds)
                {
                    if (!this.usersToCallMap.ContainsKey(xuid))
                    {
                        this.usersToCall.Add(xuid);
                        this.usersToCallMap[xuid] = true;
                    }
                }
            }

            Task.Run(() => { this.FireHelper(); });
        }

        private void FireHelper()
        {
            if (!this.isTaskInProgress)
            {
                TimeSpan timeDiff = (this.duration - (DateTime.Now - this.previousTime));
                if (timeDiff.TotalMilliseconds < 0)
                {
                    timeDiff = TimeSpan.Zero;
                }
                this.isTaskInProgress = true;
                this.previousTime = DateTime.Now;

                List<string> userCopy;
                lock (this.usersToCall)
                {
                    userCopy = this.usersToCall.ToList();
                }
                var completionContext = this.callBufferTimerCompletionContext;
                Task.Delay(timeDiff).ContinueWith((continuationAction) =>
                {
                    this.isTaskInProgress = false;
                    this.TimerCompleteEvent(this, new CallBufferReturnObject(userCopy, completionContext));
                    if (this.isQueuedTask)
                    {
                        this.isQueuedTask = false;
                        this.FireHelper();
                    }
                });

                this.usersToCall.Clear();
                this.usersToCallMap.Clear();
                this.callBufferTimerCompletionContext = null;
            }
            else
            {
                this.isQueuedTask = true;
            }
        }
    }
}