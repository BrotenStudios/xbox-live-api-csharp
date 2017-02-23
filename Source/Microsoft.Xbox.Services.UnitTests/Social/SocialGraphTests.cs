// Copyright (c) Microsoft Corporation
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// 
namespace Microsoft.Xbox.Services.UnitTests.Social
{
    using global::System.Threading.Tasks;

    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Microsoft.Xbox.Services.Social.Manager;

    [TestClass]
    public class SocialGraphTests : TestBase
    {
        private SocialGraph graph;

        [TestInitialize]
        public override void TestInitialize()
        {
            base.TestInitialize();
            this.graph = new SocialGraph(user, SocialManagerExtraDetailLevel.PreferredColor);
        }

        [TestCleanup]
        public void TestCleanup()
        {
            this.graph.Dispose();
        }

        [TestMethod]
        public async Task InitializeGraph()
        {
            await this.graph.Initialize();
        }
    }
}