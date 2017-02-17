// -----------------------------------------------------------------------
//  <copyright file="XboxLiveContext.cs" company="Microsoft">
//      Copyright (c) Microsoft. All rights reserved.
//      Licensed under the MIT license. See LICENSE file in the project root for full license information.
//  </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.Xbox.Services
{
    using global::System.IO;
    using Microsoft.Xbox.Services.Achievements;
    using Microsoft.Xbox.Services.ContextualSearch;
    using Microsoft.Xbox.Services.Events;
    using Microsoft.Xbox.Services.GameServerPlatform;
    using Microsoft.Xbox.Services.Leaderboard;
    using Microsoft.Xbox.Services.Matchmaking;
    using Microsoft.Xbox.Services.Multiplayer;
    using Microsoft.Xbox.Services.Presence;
    using Microsoft.Xbox.Services.Privacy;
    using Microsoft.Xbox.Services.RealTimeActivity;
    using Microsoft.Xbox.Services.Social;
    using Microsoft.Xbox.Services.System;
    using Microsoft.Xbox.Services.TitleStorage;
    using Microsoft.Xbox.Services.UserStatistics;

    public partial class XboxLiveContext
    {
        public XboxLiveContext(XboxLiveUser user)
        {
            this.User = user;

            try
            {
                this.AppConfig = XboxLiveAppConfiguration.Instance;
            }
            catch(FileLoadException)
            {
                this.AppConfig = null;
            }
            this.Settings = new XboxLiveContextSettings();
            
            this.LeaderboardService = UseMockServices ? (ILeaderboardService)new MockLeaderboardService(user, Settings, AppConfig) : new LeaderboardService(user, Settings, AppConfig);
        }

        public XboxLiveAppConfiguration AppConfig { get; private set; }

        public XboxLiveContextSettings Settings { get; private set; }

        public ILeaderboardService LeaderboardService { get; private set; }

        public XboxLiveUser User { get; private set; }
    }
}