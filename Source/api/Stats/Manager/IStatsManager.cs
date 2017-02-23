// Copyright (c) Microsoft Corporation
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// 
using System;
using System.Collections.Generic;
using Microsoft.Xbox.Services.System;

namespace Microsoft.Xbox.Services.Stats.Manager
{
    public interface IStatsManager
    {
        void AddLocalUser(XboxLiveUser user);
        void RemoveLocalUser(XboxLiveUser user);
        StatValue GetStat(XboxLiveUser user, string statName);
        List<string> GetStatNames(XboxLiveUser user);
        void SetStatAsNumber(XboxLiveUser user, string statName, double value);
        void SetStatAsInteger(XboxLiveUser user, string statName, Int64 value);
        void SetStatAsString(XboxLiveUser user, string statName, string value);
        void RequestFlushToService(XboxLiveUser user, bool isHighPriority = false);
        List<StatEvent> DoWork();
    }
}
