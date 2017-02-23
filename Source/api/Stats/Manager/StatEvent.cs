// Copyright (c) Microsoft Corporation
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// 
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Xbox.Services.System;

namespace Microsoft.Xbox.Services.Stats.Manager
{
    public enum StatEventType
    {
        LocalUserAdded,
        LocalUserRemoved,
        StatUpdateComplete
    }

    public class StatEvent
    {
        public StatEventType EventType { get; private set; }
        public XboxLiveUser LocalUser { get; private set; }
        public Exception ErrorInfo { get; private set; }

        public StatEvent(StatEventType eventType, XboxLiveUser user, Exception errorInfo)
        {
            EventType = eventType;
            LocalUser = user;
            ErrorInfo = errorInfo;
        }
    }
}