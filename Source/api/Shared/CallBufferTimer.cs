using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text;
using System.Linq;

namespace Microsoft.Xbox.Services.Shared
{
    class CallBufferTimerCompletionContext
    {
        public int Context { get; private set; }
        public int NumObjects { get; private set; }
    }

    class CallBufferReturnObject
    {
        private List<string> _userList;
        private CallBufferTimerCompletionContext _context;
        public List<string> UserList { get { return _userList; } }
        public CallBufferTimerCompletionContext CompletionContext { get { return _context; } }

        public CallBufferReturnObject(List<string> userList, CallBufferTimerCompletionContext context)
        {
            _userList = userList;
            _context = context;
        }
    }

    internal class CallBufferTimer
    {
        private bool isTaskInProgress;
        private bool isQueuedTask;
        private readonly TimeSpan bufferTimePerCallSec;
        private DateTime previousTime;
        private List<string> usersToCall;
        private Dictionary<string, bool> usersToCallMap;
        private CallBufferTimerCompletionContext callBufferTimerCompletionContext;

        public event EventHandler<CallBufferReturnObject> TimerCompleteEvent;
        public CallBufferTimer(TimeSpan bufferTimePerCallSec)
        {
            this.isTaskInProgress = false;
            this.isQueuedTask = false;
            this.bufferTimePerCallSec = bufferTimePerCallSec;
            this.previousTime = new DateTime(0);
            this.usersToCall = new List<string>();
            this.usersToCallMap = new Dictionary<string, bool>();
            this.callBufferTimerCompletionContext = null;
        }

        public void Fire()
        {
            FireHelper();
        }

        public void Fire(List<string> xboxUserIds, CallBufferTimerCompletionContext completionContext = null)
        {
            if(xboxUserIds == null)
            {
                throw new ArgumentNullException("xboxUserIds");
            }

            lock(this.usersToCall)
            {
                this.callBufferTimerCompletionContext = completionContext;
                foreach (string xuid in xboxUserIds)
                {
                    if(!usersToCallMap.ContainsKey(xuid))
                    {
                        this.usersToCall.Add(xuid);
                        this.usersToCallMap[xuid] = true;
                    }
                }

                Task.Run(() =>
                {
                    lock(this.usersToCall)
                    {
                        FireHelper();
                    }
                });
            }
        }

        private void FireHelper()
        {
            if(!this.isTaskInProgress)
            {
                int timeDiff = (int)(this.bufferTimePerCallSec - (DateTime.Now - previousTime)).TotalMilliseconds;
                int timeRemaining = Math.Max(0, timeDiff);
                this.isTaskInProgress = true;
                this.previousTime = DateTime.Now;

                var userCopy = this.usersToCall.ToList();
                var completionContext = this.callBufferTimerCompletionContext;
                Task.Delay(timeRemaining).ContinueWith((continuationAction) =>
                {
                    lock(usersToCall)
                    {
                        isTaskInProgress = false;
                        TimerCompleteEvent(this, new CallBufferReturnObject(userCopy, completionContext));
                        if(isQueuedTask)
                        {
                            isQueuedTask = false;
                            FireHelper();
                        }
                    }
                });
                
                usersToCall.Clear();
                usersToCallMap.Clear();
                callBufferTimerCompletionContext = null;
            }
            else
            {
                isQueuedTask = true;
            }
        }

    }
}
