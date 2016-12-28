using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Microsoft.Xbox.Services.Leaderboard
{
    public class LeaderboardService
    {
        public Task<LeaderboardResult> GetLeaderboardAsync(string serviceConfigurationId, string leaderboardName)
        {
            return this.GetLeaderboardAsync(serviceConfigurationId, leaderboardName, null, null, 0, UInt32.MaxValue);
        }

        public Task<LeaderboardResult> GetLeaderboardAsync(string serviceConfigurationId, string leaderboardName, uint skipToRank, uint maxItems)
        {
            return this.GetLeaderboardAsync(serviceConfigurationId, leaderboardName, null, null, skipToRank, maxItems);
        }

        public Task<LeaderboardResult> GetLeaderboardAsync(string serviceConfigurationId, string leaderboardName, string xuid, string socialGroup, uint maxItems)
        {
            return this.GetLeaderboardAsync(serviceConfigurationId, leaderboardName, xuid, socialGroup, 0, maxItems);
        }

        public Task<LeaderboardResult> GetLeaderboardAsync(string serviceConfigurationId, string leaderboardName, string xuid, string socialGroup, uint skipToRank, uint maxItems)
        {
            return this.GetLeaderboardWithAdditionalColumnsAsync(serviceConfigurationId, leaderboardName, xuid, socialGroup, skipToRank, null, maxItems);
        }

        public Task<LeaderboardResult> GetLeaderboardWithAdditionalColumnsAsync(string serviceConfigurationId, string leaderboardName, string xuid, string socialGroup, uint skipToRank, string[] additionalColumns, uint maxItems)
        {
            if (XboxLiveContext.UseMockData)
            {
                return Task.FromResult(MockData.CreateLeaderboardResult(leaderboardName, 100, 3, skipToRank, maxItems));
            }

            return null;
        }

        public Task<LeaderboardResult> GetLeaderboardWithAdditionalColumnsAsync(string serviceConfigurationId, string leaderboardName, uint skipToRank, string[] additionalColumns, uint maxItems)
        {
            return this.GetLeaderboardWithAdditionalColumnsAsync(serviceConfigurationId, leaderboardName, null, null, skipToRank, additionalColumns, maxItems);
        }

        public Task<LeaderboardResult> GetLeaderboardWithAdditionalColumnsAsync(string serviceConfigurationId, string leaderboardName, string xuid, string socialGroup, string[] additionalColumns, uint maxItems)
        {
            return this.GetLeaderboardWithAdditionalColumnsAsync(serviceConfigurationId, leaderboardName, xuid, socialGroup, 0, additionalColumns, maxItems);
        }

        public Task<LeaderboardResult> GetLeaderboardWithAdditionalColumnsAsync(string serviceConfigurationId, string leaderboardName, string[] additionalColumns)
        {
            return this.GetLeaderboardWithAdditionalColumnsAsync(serviceConfigurationId, leaderboardName, null, null, 0, additionalColumns, 10);
        }

        public Task<LeaderboardResult> GetLeaderboardWithSkipToUserAsync(string serviceConfigurationId, string leaderboardName, string xuid, string socialGroup, string skipToXboxUserId, uint maxItems)
        {
            return this.GetLeaderboardWithSkipToUserWithAdditionalColumnsAsync(serviceConfigurationId, leaderboardName, xuid, socialGroup, skipToXboxUserId, null, maxItems);
        }

        public Task<LeaderboardResult> GetLeaderboardWithSkipToUserAsync(string serviceConfigurationId, string leaderboardName, string skipToXboxUserId, uint maxItems)
        {
            return this.GetLeaderboardWithSkipToUserWithAdditionalColumnsAsync(serviceConfigurationId, leaderboardName, null, null, skipToXboxUserId, null, maxItems);
        }

        public Task<LeaderboardResult> GetLeaderboardWithSkipToUserWithAdditionalColumnsAsync(string serviceConfigurationId, string leaderboardName, string xuid, string socialGroup, string skipToXboxUserId, string[] additionalColumns, uint maxItems)
        {
            throw new NotImplementedException();
        }

        public Task<LeaderboardResult> GetLeaderboardWithSkipToUserWithAdditionalColumnsAsync(string serviceConfigurationId, string leaderboardName, string skipToXboxUserId, string[] additionalColumns, uint maxItems)
        {
            return this.GetLeaderboardWithSkipToUserWithAdditionalColumnsAsync(serviceConfigurationId, leaderboardName, null, null, skipToXboxUserId, additionalColumns, maxItems);
        }

        public Task<LeaderboardResult> GetLeaderboardForSocialGroupAsync(string xboxUserId, string serviceConfigurationId, string statisticName, string socialGroup, string sortOrder, uint maxItems)
        {
            return this.GetLeaderboardForSocialGroupWithSkipToUserAsync(xboxUserId, serviceConfigurationId, statisticName, socialGroup, null, sortOrder, maxItems);
        }

        public Task<LeaderboardResult> GetLeaderboardForSocialGroupAsync(string xboxUserId, string serviceConfigurationId, string statisticName, string socialGroup, uint maxItems)
        {
            return this.GetLeaderboardForSocialGroupWithSkipToUserAsync(xboxUserId, serviceConfigurationId, statisticName, socialGroup, null, null, maxItems);
        }

        public Task<LeaderboardResult> GetLeaderboardForSocialGroupWithSkipToRankAsync(string xboxUserId, string serviceConfigurationId, string statisticName, string socialGroup, uint skipToRank, string sortOrder, uint maxItems)
        {
            if (XboxLiveContext.UseMockData)
            {
                return Task.FromResult(MockData.CreateLeaderboardResult(statisticName, 100, 3, 0, maxItems));
            }

            throw new NotImplementedException();
        }

        public Task<LeaderboardResult> GetLeaderboardForSocialGroupWithSkipToUserAsync(string xboxUserId, string serviceConfigurationId, string statisticName, string socialGroup, string skipToXboxUserId, string sortOrder, uint maxItems)
        {
            if (XboxLiveContext.UseMockData)
            {
                return Task.FromResult(MockData.CreateLeaderboardResult(statisticName, 100, 3, 0, maxItems));
            }

            throw new NotImplementedException();
        }

    }
}
