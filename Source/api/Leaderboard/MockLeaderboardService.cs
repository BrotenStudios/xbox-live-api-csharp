// Copyright (c) Microsoft Corporation
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// 

namespace Microsoft.Xbox.Services.Leaderboard
{
    using global::System.Collections.Generic;
    using global::System.Linq;
    using global::System.Threading.Tasks;

    public class MockLeaderboardService : ILeaderboardService
    {
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
            return Task.FromResult(this.CreateLeaderboardResponse());
        }

        public Task<LeaderboardResult> GetSocialLeaderboardAsync(string leaderboardName, string socialGroup, LeaderboardQuery query)
        {
            return Task.FromResult(this.CreateLeaderboardResponse());
        }

        internal LeaderboardResult CreateLeaderboardResponse()
        {
            LeaderboardResponse lbResponse = JsonSerialization.FromJson<LeaderboardResponse>(@"{""pagingInfo"":null,""leaderboardInfo"":{""totalCount"":10,""columnDefinition"":{""statName"":""EnemysDefeated"",""type"":""Double""}},""userList"":[{""gamertag"":""Fake User 1"",""xuid"":""1111111111111111"",""percentile"":0.1,""rank"":1,""globalrank"":10,""value"":""1000"",""valuemetadata"":null},{""gamertag"":""Fake User 2"",""xuid"":""2222222222222222"",""percentile"":0.2,""rank"":2,""globalrank"":20,""value"":""900"",""valuemetadata"":null},{""gamertag"":""Fake User 3"",""xuid"":""3333333333333333"",""percentile"":0.3,""rank"":3,""globalrank"":30,""value"":""800"",""valuemetadata"":null},{""gamertag"":""Fake User 4"",""xuid"":""4444444444444444"",""percentile"":0.4,""rank"":4,""globalrank"":40,""value"":""700"",""valuemetadata"":null},{""gamertag"":""Fake User 5"",""xuid"":""5555555555555555"",""percentile"":0.5,""rank"":5,""globalrank"":50,""value"":""600"",""valuemetadata"":null},{""gamertag"":""Fake User 6"",""xuid"":""6666666666666666"",""percentile"":0.6,""rank"":6,""globalrank"":60,""value"":""500"",""valuemetadata"":null},{""gamertag"":""Fake User 7"",""xuid"":""7777777777777777"",""percentile"":0.7,""rank"":7,""globalrank"":70,""value"":""400"",""valuemetadata"":null},{""gamertag"":""Fake User 8"",""xuid"":""8888888888888888"",""percentile"":0.8,""rank"":8,""globalrank"":80,""value"":""300"",""valuemetadata"":null},{""gamertag"":""Fake User 9"",""xuid"":""9999999999999999"",""percentile"":0.9,""rank"":9,""globalrank"":90,""value"":""200"",""valuemetadata"":null},{""gamertag"":""Fake User 10"",""xuid"":""1010101010101010"",""percentile"":1.0,""rank"":10,""globalrank"":100,""value"":""100"",""valuemetadata"":null},]}");

            IList<LeaderboardColumn> columns = new List<LeaderboardColumn> { lbResponse.LeaderboardInfo.Column };

            IList<LeaderboardRow> rows = lbResponse.Rows.Select(row =>
                new LeaderboardRow
                {
                    Gamertag = row.Gamertag,
                    Percentile = row.Percentile,
                    Rank = row.Rank,
                    XboxUserId = row.XboxUserId,
                    Values = row.Value != null ? new List<string> { row.Value } : row.Values
                }).ToList();

            LeaderboardResult result = new LeaderboardResult(lbResponse.LeaderboardInfo.TotalCount, columns, rows, this.userContext, this.xboxLiveContextSettings, this.appConfig);
            return result;
        }
    }
}