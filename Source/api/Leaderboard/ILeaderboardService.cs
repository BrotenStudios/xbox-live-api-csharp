// -----------------------------------------------------------------------
//  <copyright file="LeaderboardService.cs" company="Microsoft">
//      Copyright (c) Microsoft. All rights reserved.
//      Licensed under the MIT license. See LICENSE file in the project root for full license information.
//  </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.Xbox.Services.Leaderboard
{
    using global::System;
    using global::System.Collections.Generic;
    using global::System.Text;
    using global::System.Threading.Tasks;
    using Newtonsoft.Json.Linq;
    using Social.Models;
    using System;

    public interface ILeaderboardService
    {

        Task<LeaderboardResult> GetLeaderboardAsync(string leaderboardName, LeaderboardQuery query);

        Task<LeaderboardResult> GetSocialLeaderboardAsync(string leaderboardName, string socialGroup, LeaderboardQuery query);

    }
}