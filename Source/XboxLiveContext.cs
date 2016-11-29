using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Microsoft.Xbox.Services
{
    public class XboxLiveContext
    {
        public XboxLiveContext(Microsoft.Xbox.Services.System.XboxLiveUser user) {
        }

        public XboxLiveAppConfiguration AppConfig
        {
            get;
            private set;
        }

        public XboxLiveContextSettings Settings
        {
            get;
            private set;
        }

        public Microsoft.Xbox.Services.Events.EventsService EventsService
        {
            get;
            private set;
        }

        public Microsoft.Xbox.Services.ContextualSearch.ContextualSearchService ContextualSearchService
        {
            get;
            private set;
        }

        public Microsoft.Xbox.Services.System.StringService StringService
        {
            get;
            private set;
        }

        public Microsoft.Xbox.Services.Privacy.PrivacyService PrivacyService
        {
            get;
            private set;
        }

        public Microsoft.Xbox.Services.TitleStorage.TitleStorageService TitleStorageService
        {
            get;
            private set;
        }

        public Microsoft.Xbox.Services.GameServerPlatform.GameServerPlatformService GameServerPlatformService
        {
            get;
            private set;
        }

        public Microsoft.Xbox.Services.Presence.PresenceService PresenceService
        {
            get;
            private set;
        }

        public Microsoft.Xbox.Services.RealTimeActivity.RealTimeActivityService RealTimeActivityService
        {
            get;
            private set;
        }

        public Microsoft.Xbox.Services.Multiplayer.MultiplayerService MultiplayerService
        {
            get;
            private set;
        }

        public Microsoft.Xbox.Services.Matchmaking.MatchmakingService MatchmakingService
        {
            get;
            private set;
        }

        public Microsoft.Xbox.Services.UserStatistics.UserStatisticsService UserStatisticsService
        {
            get;
            private set;
        }

        public Microsoft.Xbox.Services.Leaderboard.LeaderboardService LeaderboardService
        {
            get;
            private set;
        }

        public Microsoft.Xbox.Services.Achievements.AchievementService AchievementService
        {
            get;
            private set;
        }

        public Microsoft.Xbox.Services.Social.ReputationService ReputationService
        {
            get;
            private set;
        }

        public Microsoft.Xbox.Services.Social.SocialService SocialService
        {
            get;
            private set;
        }

        public Microsoft.Xbox.Services.Social.ProfileService ProfileService
        {
            get;
            private set;
        }

        public Microsoft.Xbox.Services.System.XboxLiveUser User
        {
            get;
            private set;
        }

    }
}
