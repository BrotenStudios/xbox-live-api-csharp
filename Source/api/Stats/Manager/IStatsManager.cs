﻿// Copyright (c) Microsoft Corporation
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// 
using System;
using System.Collections.Generic;
using Microsoft.Xbox.Services.System;

namespace Microsoft.Xbox.Services.Stats.Manager
{
    public interface IStatsManager
    {
        /// <summary> 
        /// Adds a local user to the stats manager
        /// Returns a local_user_added event from do_work
        /// </summary>
        /// <param name="user">The user to add to the statistic manager</param>
        void AddLocalUser(XboxLiveUser user);

        /// <summary> 
        /// Removes a local user from the stats manager
        /// Returns a local_user_removed event from do_work
        /// </summary>
        /// <param name="user">The user to be removed from the statistic manager</param>
        void RemoveLocalUser(XboxLiveUser user);

        /// <summary> 
        /// Returns any events that have been processed
        /// </summary>
        /// <return>A list of events that have happened since previous do_work</return>
        List<StatEvent> DoWork();

        /// <summary> 
        /// Requests the current stat values to be uploaded to the service
        /// This will send immediately instead of automatically during a 30 second window
        /// </summary>
        /// <remarks>This will be throttled if called too often</remarks>
        void RequestFlushToService(XboxLiveUser user, bool isHighPriority = false);

        /// <summary> 
        /// Replaces the numerical stat by the value. Can be positive or negative
        /// </summary>
        /// <param name="user">The local user whose stats to access</param>
        /// <param name="name">The name of the statistic to modify</param>
        /// <param name="value">Value to replace the stat by</param>
        /// <return>Whether or not the setting was successful. Can fail if stat is not of numerical type. Will return updated stat</return>
        void SetStatAsNumber(XboxLiveUser user, string statName, double value);

        /// <summary> 
        /// Replaces the numerical stat by the value. Can be positive or negative
        /// </summary>
        /// <param name="user">The local user whose stats to access</param>
        /// <param name="name">The name of the statistic to modify</param>
        /// <param name="value">Value to replace the stat by</param>
        /// <return>Whether or not the setting was successful. Can fail if stat is not of numerical type. Will return updated stat</return>
        void SetStatAsInteger(XboxLiveUser user, string statName, Int64 value);

        /// <summary> 
        /// Replaces a string stat with the given value.
        /// </summary>
        /// <param name="user">The local user whose stats to access</param>
        /// <param name="name">The name of the statistic to modify</param>
        /// <param name="value">Value to replace the stat by</param>
        /// <return>Whether or not the setting was successful. Can fail if stat is not of string type. Will return updated stat</return>
        void SetStatAsString(XboxLiveUser user, string statName, string value);

        /// <summary> 
        /// Gets a stat value
        /// </summary>
        /// <param name="user">The local user whose stats to access</param>
        /// <param name="name">The name of the statistic to modify</param>
        /// <return>Whether or not the setting was successful along with updated stat</return>
        StatValue GetStat(XboxLiveUser user, string statName);

        /// <summary> 
        /// Gets all stat names in the stat document.
        /// </summary>
        /// <param name="user">The local user whose stats to access</param>
        /// <param name="statNameList">The list to fill with stat names</param>
        /// <return>Whether or not the setting was successful.</return>
        List<string> GetStatNames(XboxLiveUser user);
    }
}
