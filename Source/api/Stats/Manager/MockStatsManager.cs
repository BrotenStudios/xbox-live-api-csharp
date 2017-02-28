// Copyright (c) Microsoft Corporation
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// 
using System;
using System.Collections.Generic;
using Microsoft.Xbox.Services.System;
using System.Linq;
using Microsoft.Xbox.Services.Leaderboard;
using global::System.Threading.Tasks;

namespace Microsoft.Xbox.Services.Stats.Manager
{
    class MockStatsManager : IStatsManager
    {
        private StatsValueDocument statValueDocument;
        private List<StatEvent> statEventList;
        private MockLeaderboardService mockLBService;

        internal MockStatsManager()
        {
            this.LocalUsers = new List<XboxLiveUser>();
            Dictionary<string, Models.Stat> statMap = new Dictionary<string, Models.Stat>
            {
                {
                    "DefaultNum", new Models.Stat()
                    {
                        Value = 1.5f
                    }
                },
                {
                    "DefaultString", new Models.Stat()
                    {
                        Value = "stringVal"
                    }
                },
                {
                    "Default", new Models.Stat()
                    {
                        Value = 1
                    }
                }
            };

            statValueDocument = new StatsValueDocument(statMap);

            statEventList = new List<StatEvent>();
        }

        public IList<XboxLiveUser> LocalUsers { get; private set; }

        public void AddLocalUser(XboxLiveUser user)
        {
            this.LocalUsers.Add(user);
            mockLBService = new MockLeaderboardService(user, new XboxLiveContextSettings(), XboxLiveAppConfiguration.Instance);
            this.statEventList.Add(new StatEvent(StatEventType.LocalUserAdded, user, null, new StatEventArgs()));
        }
        public void RemoveLocalUser(XboxLiveUser user)
        {
            this.LocalUsers.Remove(user);
            this.statEventList.Add(new StatEvent(StatEventType.LocalUserRemoved, user, null, new StatEventArgs()));
        }
        public StatValue GetStat(XboxLiveUser user, string statName)
        {
            return statValueDocument.GetStat(statName);
        }
        public List<string> GetStatNames(XboxLiveUser user)
        {
            return statValueDocument.GetStatNames();
        }
        public void SetStatAsNumber(XboxLiveUser user, string statName, double value)
        {
            statValueDocument.SetStat(statName, value);
            RequestFlushToService(user);
        }
        public void SetStatAsInteger(XboxLiveUser user, string statName, Int64 value)
        {
            statValueDocument.SetStat(statName, (double)value);
            RequestFlushToService(user);
        }
        public void SetStatAsString(XboxLiveUser user, string statName, string value)
        {
            statValueDocument.SetStat(statName, value);
            RequestFlushToService(user);
        }
        public void RequestFlushToService(XboxLiveUser user, bool isHighPriority = false)
        {
            statValueDocument.DoWork();
        }
        public List<StatEvent> DoWork()
        {
            var copyList = this.statEventList.ToList();

            statValueDocument.DoWork();
            this.statEventList.Clear();
            return copyList;
        }
        public void GetLeaderboard(XboxLiveUser user, string statName, LeaderboardQuery query)
        {
            if (mockLBService == null)
            {
                throw new ArgumentException("Local User needs to be added.");
            }
            mockLBService.GetLeaderboardAsync(statName, query);
        }

        public void GetSocialLeaderboard(XboxLiveUser user, string statName, string socialGroup, LeaderboardQuery query)
        {
            if (mockLBService == null)
            {
                throw new ArgumentException("Local User needs to be added.");
            }
            mockLBService.GetSocialLeaderboardAsync(statName, socialGroup, query);

        }
    }
}
