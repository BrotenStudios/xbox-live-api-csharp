using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Xbox.Services.Stats.Manager.Models
{
    class StatsValueDocumentModel
    {
        [JsonProperty("ver")]
        public int Version { get; set; }

        public DateTime Timestamp { get; set; }
        public Stats Stats { get; set; }

        public Envelope Envelope { get; set; }
    }

    public class Envelope
    {
        public int ServerVersion { get; set; }
        public int ClientVersion { get; set; }
        public string ClientId { get; set; }
    }
    public class Stats
    {
        public object Tags { get; set; }
        public Dictionary<string, Stat> Title { get; set; }
    }

    public class Stat
    {
        public object GlobalValue { get; set; }
        [JsonProperty("op")]
        public string Operation { get; set; }
    }
}
