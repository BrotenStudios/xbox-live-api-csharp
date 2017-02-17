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

            this.AchievementService = new AchievementService();
            this.ContextualSearchService = new ContextualSearchService();
            this.EventsService = new EventsService();
            this.GameServerPlatformService = new GameServerPlatformService();
            this.LeaderboardService = UseMockServices ? (ILeaderboardService)new MockLeaderboardService(user, Settings, AppConfig) : new LeaderboardService(user, Settings, AppConfig);
            this.MatchmakingService = new MatchmakingService();
            this.MultiplayerService = new MultiplayerService();
            this.PresenceService = new PresenceService();
            this.PrivacyService = new PrivacyService();
            this.ProfileService = new ProfileService(this.AppConfig, this, this.Settings);
            this.RealTimeActivityService = new RealTimeActivityService();
            this.ReputationService = new ReputationService();
            this.SocialService = new SocialService();
            this.StringService = new StringService();
            this.TitleStorageService = new TitleStorageService();
            this.UserStatisticsService = new UserStatisticsService();
        }

        public XboxLiveAppConfiguration AppConfig { get; private set; }

        public XboxLiveContextSettings Settings { get; private set; }

        public EventsService EventsService { get; private set; }

        public ContextualSearchService ContextualSearchService { get; private set; }

        public StringService StringService { get; private set; }

        public PrivacyService PrivacyService { get; private set; }

        public TitleStorageService TitleStorageService { get; private set; }

        public GameServerPlatformService GameServerPlatformService { get; private set; }

        public PresenceService PresenceService { get; private set; }

        public RealTimeActivityService RealTimeActivityService { get; private set; }

        public MultiplayerService MultiplayerService { get; private set; }

        public MatchmakingService MatchmakingService { get; private set; }

        public UserStatisticsService UserStatisticsService { get; private set; }

        public ILeaderboardService LeaderboardService { get; private set; }

        public AchievementService AchievementService { get; private set; }

        public ReputationService ReputationService { get; private set; }

        public SocialService SocialService { get; private set; }

        public ProfileService ProfileService { get; private set; }

        public XboxLiveUser User { get; private set; }
    }
}