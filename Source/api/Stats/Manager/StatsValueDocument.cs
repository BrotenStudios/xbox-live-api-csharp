using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Microsoft.Xbox.Services.Shared;

namespace Microsoft.Xbox.Services.Stats.Manager
{
    public class StatsValueDocument
    {
        class StatPendingState
        {
            public string Name { get; set; }
            public object Value { get; set; }
            public StatValueType Type { get; set; }
        }

        private List<StatPendingState> eventList;

        internal bool IsDirty { get; private set; }
        internal int Version { get; set; }
        internal int ServerVersion { get; set; }
        internal int ClientVersion { get; set; }
        internal string ClientId { get; set; }
        internal Dictionary<string, StatValue> Stats { get; private set; }

        public EventHandler FlushEvent;

        internal StatsValueDocument(Dictionary<string, Models.Stat> statMap)
        {
            IsDirty = false;
            this.eventList = new List<StatPendingState>();

            if (statMap != null)
            {
                Stats = new Dictionary<string, StatValue>(statMap.Count);

                foreach (var stat in statMap)
                {
                    StatValue statValue;
                    if (stat.Value.GlobalValue is string)
                    {
                        statValue = new StatValue(stat.Key, stat.Value.GlobalValue, StatValueType.String);
                    }
                    else
                    {
                        statValue = new StatValue(stat.Key, stat.Value.GlobalValue, StatValueType.Number);
                    }
                    Stats.Add(stat.Key, statValue);
                }
            }
            else
            {
                Stats = new Dictionary<string, StatValue>();
            }
        }

        internal StatValue GetStat(string statName)
        {
            lock (Stats)
            {
                if (!Stats.ContainsKey(statName))
                {
                    throw new ArgumentException("Stat not found in SVD");
                }

                return Stats[statName];
            }
        }

        internal void SetStat(string statName, double statValue)
        {
            lock (Stats)
            {
                IsDirty = true;
                StatPendingState statPendingState = new StatPendingState()
                {
                    Name = statName,
                    Value = statValue,
                    Type = StatValueType.Number
                };

                this.eventList.Add(statPendingState);
            }
        }

        internal void SetStat(string statName, string statValue)
        {
            lock (Stats)
            {
                IsDirty = true;
                StatPendingState statPendingState = new StatPendingState()
                {
                    Name = statName,
                    Value = statValue,
                    Type = StatValueType.String
                };

                this.eventList.Add(statPendingState);
            }
        }

        internal void ClearDirtyState()
        {
            lock (Stats)
            {
                IsDirty = false;
            }
        }

        internal List<string> GetStatNames()
        {
            lock (Stats)
            {
                List<string> statNameList = new List<string>(Stats.Count);
                foreach (var statPair in Stats)
                {
                    statNameList.Add(statPair.Key);
                }

                return statNameList;
            }
        }

        internal void DoWork()
        {
            lock (Stats)
            {
                foreach (var svdEvent in this.eventList)
                {
                    if (!Stats.ContainsKey(svdEvent.Name))
                    {
                        var statValue = new StatValue(svdEvent.Name, svdEvent.Value, svdEvent.Type);
                        Stats.Add(svdEvent.Name, statValue);
                    }
                    else
                    {
                        Stats[svdEvent.Name].SetStat(svdEvent.Value, svdEvent.Type);
                    }
                }

                this.eventList.Clear();
            }
        }
    }
}