// -----------------------------------------------------------------------
//  <copyright file="SocialGraphTests.cs" company="Microsoft">
//      Copyright (c) Microsoft. All rights reserved.
//      Licensed under the MIT license. See LICENSE file in the project root for full license information.
//  </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.Xbox.Services.UnitTests.Social
{
    using global::System.Threading.Tasks;
    using Leaderboard;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Microsoft.Xbox.Services.Social.Manager;

    [TestClass]
    public class LeaderboardTests : TestBase
    {
        [TestInitialize]
        public override void TestInitialize()
        {
            XboxLiveContext.UseMockData = true;
            base.TestInitialize();
        }

        [TestCleanup]
        public void TestCleanup()
        {
        }

        [TestMethod]
        public async Task GetLeaderboard()
        {
            LeaderboardResult res = await context.LeaderboardService.GetLeaderboardAsync("MostEnemysDefeated", new LeaderboardQuery());

            Assert.AreEqual((int)res.TotalRowCount, res.Rows.Count, "Row counts Differ");
            Assert.AreNotEqual(res.Rows, null, "Null Rows");
            Assert.AreNotEqual(res.Columns, null, "Null Columns");
        }

        [TestMethod]
        public async Task GetSocialLeaderboard()
        {
            LeaderboardResult res = await context.LeaderboardService.GetSocialLeaderboardAsync("MostEnemysDefeated", "", new LeaderboardQuery());

            Assert.AreEqual((int)res.TotalRowCount, res.Rows.Count, "Row counts Differ");
            Assert.AreNotEqual(res.Rows, null, "Null Rows");
            Assert.AreNotEqual(res.Columns, null, "Null Columns");
        }
    }
}