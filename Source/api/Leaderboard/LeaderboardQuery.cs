// Copyright (c) Microsoft Corporation
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// 
namespace Microsoft.Xbox.Services.Leaderboard
{
    using global::System;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    public class LeaderboardQuery
    {

        public bool SkipResultToMe
        {
            get;
            set;
        }

        public uint SkipResultsToRank
        {
            get;
            set;
        }

        public uint MaxItems
        {
            get;
            set;
        }

        public SortOrder Order
        {
            get;
            set;
        }

        public string StatName
        {
            get;
            internal set;
        }

        public string SocialGroup
        {
            get;
            internal set;
        }

        public bool HasNext
        {
            get
            {
                if (string.IsNullOrEmpty(ContinuationToken))
                {
                    return false;
                }

                return true;
            }
        }

        internal string ContinuationToken
        {
            get;
            set;
        }
        public LeaderboardQuery()
        {
        }

        public LeaderboardQuery(LeaderboardQuery query)
        {
            this.MaxItems = query.MaxItems;
            this.Order = query.Order;
            this.SkipResultsToRank = query.SkipResultsToRank;
            this.SkipResultToMe = query.SkipResultToMe;
        }

    }
}