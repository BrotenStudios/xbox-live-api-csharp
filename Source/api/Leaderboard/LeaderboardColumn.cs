// -----------------------------------------------------------------------
//  <copyright file="LeaderboardColumn.cs" company="Microsoft">
//      Copyright (c) Microsoft. All rights reserved.
//      Licensed under the MIT license. See LICENSE file in the project root for full license information.
//  </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.Xbox.Services.Leaderboard
{
    using global::System;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    public enum LeaderboardStatType
    {
        Integer,
        Double,
        String,
        Unknown
    }

    public class LeaderboardColumn
    {
        [JsonProperty ("type")]
        public LeaderboardStatType StatisticType { get;  set; }

        [JsonProperty ("statName")]
        public string StatisticName { get; set; }
    }
}