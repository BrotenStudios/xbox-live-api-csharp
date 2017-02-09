// -----------------------------------------------------------------------
//  <copyright file="TestBase.cs" company="Microsoft">
//      Copyright (c) Microsoft. All rights reserved.
//      Internal use only.
//  </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.Xbox.Services.UnitTests
{
    using global::System;

    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Microsoft.Xbox.Services.System;

    [TestClass]
    public class TestBase
    {
        private readonly Random rng = new Random();
        public XboxLiveUser user;
        public XboxLiveContext context;

        public TestContext TestContext { get; set; }

        [TestInitialize]
        public virtual void TestInitialize()
        {
            const ulong userBase = 0x0099000000000000;
#pragma warning disable CS0675 // Bitwise-or operator used on a sign-extended operand
            string xuid = (userBase | (ulong)this.rng.Next(0, int.MaxValue)).ToString();
#pragma warning restore CS0675 // Bitwise-or operator used on a sign-extended operand
            string gamertag = "Gamer " + xuid;

            this.user = new XboxLiveUser();
            this.context = new XboxLiveContext(this.user);
        }
    }
}