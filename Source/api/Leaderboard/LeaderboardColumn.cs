// -----------------------------------------------------------------------
//  <copyright file="LeaderboardColumn.cs" company="Microsoft">
//      Copyright (c) Microsoft. All rights reserved.
//      Licensed under the MIT license. See LICENSE file in the project root for full license information.
//  </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.Xbox.Services.Leaderboard
{
    using global::System;

    public class LeaderboardColumn
    {
        public Type StatisticType { get; set; }

        public string StatisticName { get; set; }

        public string DisplayName { get; set; }
    }
}