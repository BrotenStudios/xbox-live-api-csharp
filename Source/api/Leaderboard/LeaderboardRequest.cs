// -----------------------------------------------------------------------
//  <copyright file="LeaderboardRequest.cs" company="Microsoft">
//      Copyright (c) Microsoft. All rights reserved.
//      Licensed under the MIT license. See LICENSE file in the project root for full license information.
//  </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.Xbox.Services.Leaderboard
{
    internal class LeaderboardRequest
    {
        public LeaderboardRequest(LeaderboardRequestType requestType)
        {
            this.RequestType = requestType;
        }

        public LeaderboardRequestType RequestType { get; set; }

        public string ContinuationToken { get; set; }
    }
}