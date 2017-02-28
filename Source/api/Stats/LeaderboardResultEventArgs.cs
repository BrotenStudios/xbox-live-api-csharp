// Copyright (c) Microsoft Corporation
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// 
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Xbox.Services.Leaderboard;

namespace Microsoft.Xbox.Services.Stats.Manager
{
    public class LeaderboardResultEventArgs : StatEventArgs
    {
        public LeaderboardResult Result { get; private set; }
        public LeaderboardResultEventArgs(LeaderboardResult result)
        {
            Result = result;
        }
    }
}