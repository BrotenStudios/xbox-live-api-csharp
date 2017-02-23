// Copyright (c) Microsoft Corporation
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// 
namespace Microsoft.Xbox.Services.UnitTests.Social
{
    using global::System;
    using global::System.Collections.Generic;
    using global::System.Configuration;
    using global::System.Diagnostics;
    using global::System.Threading.Tasks;
    using Leaderboard;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Microsoft.Xbox.Services;

    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    [TestClass]
    public class LeaderboardTests : TestBase
    {
        [TestInitialize]
        public override void TestInitialize()
        {
            base.TestInitialize();
            MockXboxLiveData.Load(Environment.CurrentDirectory + "\\Leaderboards\\LeaderboardUT.json");
        }

        [TestCleanup]
        public void TestCleanup()
        {
        }

        void VerifyLeaderboardColumn(LeaderboardColumn column, JObject columnToVerify)
        {
            Assert.AreNotEqual(column, null, "LeaderboardColumn was null.");
            Assert.AreEqual(column.StatisticName, columnToVerify["statName"].ToString() );
            Assert.AreEqual(column.StatisticType.ToString(), columnToVerify["type"].ToString());
        }

        void VerifyLeaderboardRow(LeaderboardRow row, JObject rowToVerify)
        {
            Assert.AreNotEqual(row, null, "LeaderboardRow was null.");

            Assert.AreEqual(row.Gamertag, rowToVerify["gamertag"].ToString());
            Assert.AreEqual(row.XboxUserId, rowToVerify["xuid"].ToString());
            Assert.AreEqual(row.Percentile, (double)rowToVerify["percentile"]);
            Assert.AreEqual(row.Rank, (int)rowToVerify["rank"]);

            // TODO Add checks for values
        }

        void VerifyLeaderboardResult(LeaderboardResult result, JObject resultToVerify)
        {
            Assert.AreNotEqual(result, null, "LeaderboardResult was null.");

            JObject leaderboardInfoJson = JObject.Parse(resultToVerify["leaderboardInfo"].ToString());
            Assert.AreEqual(result.TotalRowCount, (uint)leaderboardInfoJson["totalCount"]);

            JObject jsonColumn = JObject.Parse(leaderboardInfoJson["columnDefinition"].ToString());
            VerifyLeaderboardColumn(result.Columns[0], jsonColumn);

            JArray jsonRows = (JArray)(resultToVerify)["userList"];
            int index = 0;
            foreach (var row in jsonRows)
            {
                VerifyLeaderboardRow(result.Rows[index++], (JObject)row);
            }
        }

        [TestMethod]
        public async Task GetLeaderboard()
        {
            LeaderboardResult result = await context.LeaderboardService.GetLeaderboardAsync("Jumps", new LeaderboardQuery());
            MockXboxLiveData.MockRequestData mockRequestData = MockXboxLiveData.MockResponses["defaultLeaderboardData"];
            JObject responseJson = JObject.Parse(mockRequestData.Response.ResponseBodyString);
            Assert.AreEqual("GET", mockRequestData.Request.Method);
            Assert.AreEqual("https://leaderboards.xboxlive.com/scids/00000000-0000-0000-0000-0000694f5acb/leaderboards/stat(Jumps)", mockRequestData.Request.Url);
            Assert.IsTrue(result.HasNext);
            VerifyLeaderboardResult(result, responseJson);

            // Testing continuation token with GetNext.
            LeaderboardResult nextResult = await result.GetNextAsync(100);
            MockXboxLiveData.MockRequestData mockRequestDataWithContinuationToken = MockXboxLiveData.MockResponses["defaultLeaderboardDataWithContinuationToken"];
            JObject responseJsonWithContinuationToken = JObject.Parse(mockRequestDataWithContinuationToken.Response.ResponseBodyString);
            Assert.AreEqual("GET", mockRequestDataWithContinuationToken.Request.Method);
            Assert.AreEqual("https://leaderboards.xboxlive.com/scids/00000000-0000-0000-0000-0000694f5acb/leaderboards/stat(Jumps)?maxItems=100&continuationToken=6", mockRequestDataWithContinuationToken.Request.Url);
            Assert.IsFalse(nextResult.HasNext);
            VerifyLeaderboardResult(nextResult, responseJsonWithContinuationToken);
        }
    }
}