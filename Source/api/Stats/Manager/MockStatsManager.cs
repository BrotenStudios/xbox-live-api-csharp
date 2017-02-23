// Copyright (c) Microsoft Corporation
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// 
using System;
using System.Collections.Generic;
using Microsoft.Xbox.Services.System;
using System.Linq;

namespace Microsoft.Xbox.Services.Stats.Manager
{
    class MockStatsManager : IStatsManager
    {
        private StatsValueDocument statValueDocument;
        private List<StatEvent> statEventList;
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
            this.statEventList.Add(new StatEvent(StatEventType.LocalUserAdded, user, null));
        }
        public void RemoveLocalUser(XboxLiveUser user)
        {
            this.LocalUsers.Remove(user);
            this.statEventList.Add(new StatEvent(StatEventType.LocalUserRemoved, user, null));
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
        }
        public void SetStatAsInteger(XboxLiveUser user, string statName, Int64 value)
        {
            statValueDocument.SetStat(statName, (double)value);
        }
        public void SetStatAsString(XboxLiveUser user, string statName, string value)
        {
            statValueDocument.SetStat(statName, value);
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
    }
}
