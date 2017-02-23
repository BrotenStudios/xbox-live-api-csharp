// Copyright (c) Microsoft Corporation
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// 
namespace Microsoft.Xbox.Services.Leaderboard
{
    using global::System;
    using global::System.Collections.Generic;
    using global::System.Text;
    using global::System.Threading.Tasks;
    using Newtonsoft.Json.Linq;
    using Social.Models;
    using System;

    public class MockLeaderboardService : ILeaderboardService
    {
        private static readonly Uri leaderboardsBaseUri = new Uri("https://leaderboards.xboxlive.com");
        private readonly XboxLiveUser userContext;
        private readonly XboxLiveContextSettings xboxLiveContextSettings;
        private readonly XboxLiveAppConfiguration appConfig;

        internal MockLeaderboardService(XboxLiveUser userContext, XboxLiveContextSettings xboxLiveContextSettings, XboxLiveAppConfiguration appConfig)
        {
            this.userContext = userContext;
            this.xboxLiveContextSettings = xboxLiveContextSettings;
            this.appConfig = appConfig;
        }

        public Task<LeaderboardResult> GetLeaderboardAsync(string statName, LeaderboardQuery query)
        {

            return Task.FromResult<LeaderboardResult>(CreateLeaderboardResponse());
        }

        public Task<LeaderboardResult> GetSocialLeaderboardAsync(string leaderboardName, string socialGroup, LeaderboardQuery query)
        {
            return Task.FromResult<LeaderboardResult>(CreateLeaderboardResponse());
        }



        internal LeaderboardResult CreateLeaderboardResponse()
        {
            LeaderboardResponse lbResponse = JsonSerialization.FromJson<LeaderboardResponse>(@"{""pagingInfo"":null,""leaderboardInfo"":{""totalCount"":2,""columnDefinition"":{""statName"":""EnemysDefeated"",""type"":""Double""}},""userList"":[{""gamertag"":""2 Dev 152247982"",""xuid"":""2814644785165224"",""percentile"":0.5,""rank"":1,""globalrank"":1,""value"":""0"",""valuemetadata"":null},{""gamertag"":""2JDTDWX JEFF001"",""xuid"":""2814628388933894"",""percentile"":0.0001,""rank"":2,""globalrank"":2,""value"":""0"",""valuemetadata"":null}]}");

            IList<LeaderboardColumn> columns = new List<LeaderboardColumn>() { lbResponse.LeaderboardInfo.Column };

            IList<LeaderboardRow> rows = new List<LeaderboardRow>();
            foreach (LeaderboardRowResponse row in lbResponse.Rows)
            {
                LeaderboardRow newRow = new LeaderboardRow()
                {
                    Gamertag = row.Gamertag,
                    Percentile = row.Percentile,
                    Rank = row.Rank,
                    XboxUserId = row.XboxUserId,
                };
                if (row.Value != null)
                {
                    newRow.Values = new List<string>();
                    newRow.Values.Add(row.Value);
                }
                else
                {
                    newRow.Values = row.Values;
                }
                rows.Add(newRow);
            }

            LeaderboardResult result = new LeaderboardResult(lbResponse.LeaderboardInfo.TotalCount, columns, rows, userContext, xboxLiveContextSettings, appConfig);
            return result;
        }
    }
}