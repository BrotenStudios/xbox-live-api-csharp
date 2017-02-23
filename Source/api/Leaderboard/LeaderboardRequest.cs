// Copyright (c) Microsoft Corporation
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// 
namespace Microsoft.Xbox.Services.Leaderboard
{
    internal class LeaderboardRequest
    {
        public LeaderboardRequest(LeaderboardRequestType requestType, string leaderboardName)
        {
            this.RequestType = requestType;
            this.LeaderboardName = leaderboardName;
        }

        public LeaderboardRequestType RequestType { get; set; }

        public string ContinuationToken { get; set; }

        public string LeaderboardName { get; set; }
    }
}