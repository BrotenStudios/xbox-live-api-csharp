// -----------------------------------------------------------------------
//  <copyright file="XboxLiveContext.cs" company="Microsoft">
//      Copyright (c) Microsoft. All rights reserved.
//      Licensed under the MIT license. See LICENSE file in the project root for full license information.
//  </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.Xbox.Services
{
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

    public class XboxLiveContext
    {
        public XboxLiveContext(XboxLiveUser user)
        {
            this.User = user;
        }

        public static bool UseMockData { get; set; }

        public XboxLiveAppConfiguration AppConfig { get; } = new XboxLiveAppConfiguration();

        public XboxLiveContextSettings Settings { get; } = new XboxLiveContextSettings();

        public EventsService EventsService { get; } = new EventsService();

        public ContextualSearchService ContextualSearchService { get; } = new ContextualSearchService();

        public StringService StringService { get; } = new StringService();

        public PrivacyService PrivacyService { get; } = new PrivacyService();

        public TitleStorageService TitleStorageService = new TitleStorageService();

        public GameServerPlatformService GameServerPlatformService { get; } = new GameServerPlatformService();

        public PresenceService PresenceService { get; } = new PresenceService();

        public RealTimeActivityService RealTimeActivityService { get; } = new RealTimeActivityService();

        public MultiplayerService MultiplayerService { get; } = new MultiplayerService();

        public MatchmakingService MatchmakingService { get; } = new MatchmakingService();

        public UserStatisticsService UserStatisticsService { get; } = new UserStatisticsService();

        public LeaderboardService LeaderboardService { get; } = new LeaderboardService();

        public AchievementService AchievementService { get; } = new AchievementService();

        public ReputationService ReputationService { get; } = new ReputationService();

        public SocialService SocialService { get; } = new SocialService();

        public ProfileService ProfileService { get; } = new ProfileService();

        public XboxLiveUser User { get; }
    }
}