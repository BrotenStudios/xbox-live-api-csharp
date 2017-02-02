using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text;
using Microsoft.Xbox.Services.System;
using Microsoft.Xbox.Services.Shared;

namespace Microsoft.Xbox.Services.Stats.Manager
{
    public class StatsManager
    {
        private class StatsUserContext
        {
            public StatsValueDocument statsValueDocument;
            public XboxLiveContext xboxLiveContext;
            public StatsService statsService;
            public XboxLiveUser user;
        }

        public static StatsManager statsManager;
        static readonly TimeSpan TIME_PER_CALL_SEC = new TimeSpan(0, 0, 5);
        private Dictionary<string, StatsUserContext> userStatContextMap;
        private List<StatEvent> eventList;
        private CallBufferTimer statTimer;
        private CallBufferTimer statPriorityTimer;

        private void CheckUserValid(XboxLiveUser user)
        {
            if (user == null || user.XboxUserId == null || !userStatContextMap.ContainsKey(user.XboxUserId))
            {
                throw new ArgumentException("user");
            }
        }

        public static StatsManager Singleton
        {
            get
            {
                if (statsManager == null)
                {
                    statsManager = new StatsManager();
                }

                return statsManager;
            }
        }

        private StatsManager()
        {
            this.userStatContextMap = new Dictionary<string, StatsUserContext>();
            this.eventList = new List<StatEvent>();
            this.statTimer = new CallBufferTimer(TIME_PER_CALL_SEC);
            this.statPriorityTimer = new CallBufferTimer(TIME_PER_CALL_SEC);

            this.statTimer.TimerCompleteEvent += CallBufferTimerCallback;
            this.statPriorityTimer.TimerCompleteEvent += CallBufferTimerCallback;
        }

        public void AddLocalUser(XboxLiveUser user)
        {
            if(user == null)
            {
                throw new ArgumentException("user");
            }

            string xboxUserId = user.XboxUserId;
            if(userStatContextMap.Keys.Contains(xboxUserId))
            {
                throw new ArgumentException("User already in map");
            }

            var context = new StatsUserContext();
            this.userStatContextMap.Add(xboxUserId, context);

            var xboxLiveContext = new XboxLiveContext(user);
            var statsService = new StatsService(xboxLiveContext);

            context.xboxLiveContext = xboxLiveContext;
            context.statsService = statsService;
            context.user = user;
            context.statsValueDocument = new StatsValueDocument(null);

            statsService.GetStatsValueDocument().ContinueWith(statsValueDocTask =>
            {
                lock(this.userStatContextMap)
                {
                    bool isSignedIn = user.IsSignedIn;

                    if(isSignedIn)
                    {
                        if(statsValueDocTask.IsCompleted)
                        {
                            if(userStatContextMap.ContainsKey(xboxUserId))
                            {
                                this.userStatContextMap[xboxUserId].statsValueDocument = statsValueDocTask.Result;
                                this.userStatContextMap[xboxUserId].statsValueDocument.FlushEvent += new EventHandler((sender, e) =>
                                {
                                    if(this.userStatContextMap.ContainsKey(xboxUserId))
                                    {
                                        FlushToService(this.userStatContextMap[xboxUserId]);
                                    }
                                });
                            }
                        }
                    }

                    lock (this.eventList)
                    {
                        this.eventList.Add(new StatEvent(StatEventType.LocalUserAdded, user, statsValueDocTask.Exception));
                    }
                }
            });
        }

        public void RemoveLocalUser(XboxLiveUser user)
        {
            CheckUserValid(user);
            var xboxUserId = user.XboxUserId;
            var svd = this.userStatContextMap[xboxUserId].statsValueDocument;
            if(svd.IsDirty)
            {
                svd.DoWork();
                //var serializedSVD = svd.Serialize();  // write offline
                userStatContextMap[xboxUserId].statsService.UpdateStatsValueDocument(svd).ContinueWith((continuationTask) =>
                {
                    if(ShouldWriteOffline(continuationTask.Exception))
                    {
                        // write offline
                    }

                    lock(this.eventList)
                    {
                        this.eventList.Add(new StatEvent(StatEventType.LocalUserRemoved, user, continuationTask.Exception));
                    }
                });
            }
            else
            {
                lock(this.eventList)
                {
                    this.eventList.Add(new StatEvent(StatEventType.LocalUserRemoved, user, null));
                }
            }

            this.userStatContextMap.Remove(xboxUserId);
        }

        public StatValue GetStat(XboxLiveUser user, string statName)
        {
            CheckUserValid(user);
            if (statName == null)
            {
                throw new ArgumentException("statName");
            }

            return this.userStatContextMap[user.XboxUserId].statsValueDocument.GetStat(statName);
        }

        public List<string> GetStatNames(XboxLiveUser user)
        {
            CheckUserValid(user);
            return this.userStatContextMap[user.XboxUserId].statsValueDocument.GetStatNames();
        }

        public void SetStatAsNumber(XboxLiveUser user, string statName, double value)
        {
            CheckUserValid(user);

            if (statName == null)
            {
                throw new ArgumentException("statName");
            }

            this.userStatContextMap[user.XboxUserId].statsValueDocument.SetStat(statName, value);
        }

        public void SetStatAsInteger(XboxLiveUser user, string statName, Int64 value)
        {
            CheckUserValid(user);

            if (statName == null)
            {
                throw new ArgumentException("statName");
            }

            this.userStatContextMap[user.XboxUserId].statsValueDocument.SetStat(statName, (double)value);
        }

        public void SetStatAsString(XboxLiveUser user, string statName, string value)
        {
            CheckUserValid(user);

            if (statName == null)
            {
                throw new ArgumentException("statName");
            }

            this.userStatContextMap[user.XboxUserId].statsValueDocument.SetStat(statName, value);
        }

        public void RequestFlushToService(XboxLiveUser user, bool isHighPriority = false)
        {
            CheckUserValid(user);
            List<string> userVec = new List<string>(1);
            userVec.Add(user.XboxUserId);

            if(isHighPriority)
            {
                this.statPriorityTimer.Fire(userVec);
            }
            else
            {
                this.statTimer.Fire(userVec);
            }
        }

        public List<StatEvent> DoWork()
        {
            lock(this.userStatContextMap)
            {
                var copyList = this.eventList.ToList();

                foreach(var userContextPair in this.userStatContextMap)
                {
                    userContextPair.Value.statsValueDocument.DoWork();
                }

                this.eventList.Clear();
                return copyList;
            }
        }

        private bool ShouldWriteOffline(AggregateException exception)
        {
            return false;   // offline not implemented yet
        }

        private void FlushToService(StatsUserContext statsUserContext)
        {
            //var serializedSVD = statsUserContext.statsValueDocument.Serialize();
            statsUserContext.statsService.UpdateStatsValueDocument(statsUserContext.statsValueDocument).ContinueWith((continuationAction) =>
            {
                if(continuationAction.Status == TaskStatus.Faulted)
                {
                    if(ShouldWriteOffline(continuationAction.Exception))
                    {
                        //WriteOffline(statsUserContext, serializedSVD);    // todo: add offline support
                    }
                    else
                    {
                        // log error
                    }
                }

                lock(this.eventList)
                {
                    this.eventList.Add(new StatEvent(StatEventType.StatUpdateComplete, statsUserContext.user, continuationAction.Exception));
                }
            });
        }

        private void CallBufferTimerCallback(object caller, CallBufferReturnObject returnObject)
        {
            if (returnObject.UserList.Count != 0)
            {
                FlushToServiceCallback(returnObject.UserList[0]);
            }
        }

        private void FlushToServiceCallback(string userXuid)
        {
            if (this.userStatContextMap.ContainsKey(userXuid))
            {
                var statsUserContext = this.userStatContextMap[userXuid];
                var userSVD = statsUserContext.statsValueDocument;
                if (userSVD.IsDirty)
                {
                    userSVD.DoWork();
                    userSVD.ClearDirtyState();
                    FlushToService(statsUserContext);
                }
            }
        }
    }
}