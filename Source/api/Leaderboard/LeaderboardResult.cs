// Copyright (c) Microsoft Corporation
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// 
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Xbox.Services.System;

namespace Microsoft.Xbox.Services.Leaderboard
{
    public class LeaderboardResult
    {

        private readonly XboxLiveUser userContext;
        private readonly XboxLiveContextSettings xboxLiveContextSettings;
        private readonly XboxLiveAppConfiguration appConfig;

        public LeaderboardResult(
        XboxLiveUser userContext,
        XboxLiveContextSettings xboxLiveContextSettings,
        XboxLiveAppConfiguration appConfig)
        {
            this.userContext = userContext;
            this.xboxLiveContextSettings = xboxLiveContextSettings;
            this.appConfig = appConfig;
        }

        public LeaderboardResult(
            uint totalRowCount,
            IList<LeaderboardColumn> columns,
            IList<LeaderboardRow> rows,
            XboxLiveUser userContext, 
            XboxLiveContextSettings xboxLiveContextSettings, 
            XboxLiveAppConfiguration appConfig)
        {
            this.TotalRowCount = totalRowCount;
            this.Columns = columns;
            this.Rows = rows;
            this.userContext = userContext;
            this.xboxLiveContextSettings = xboxLiveContextSettings;
            this.appConfig = appConfig;
        }

        public bool HasNext
        {
            get
            {
                if (this.NextQuery != null)
                {
                    return this.NextQuery.HasNext;
                }

                return false;
            }
        }

        public IList<LeaderboardRow> Rows
        {
            get;
            internal set;
        }

        public IList<LeaderboardColumn> Columns
        {
            get;
            internal set;
        }

        public uint TotalRowCount
        {
            get;
            internal set;
        }

        public LeaderboardQuery NextQuery { get; internal set; }

    }
}
