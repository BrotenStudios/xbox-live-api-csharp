// Copyright (c) Microsoft Corporation
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// 
namespace Microsoft.Xbox.Services.Social.Manager
{
    using global::System;
    using global::System.Collections.Generic;

    using Microsoft.Xbox.Services.System;

    public class SocialEvent
    {
        public SocialEvent(SocialEventType type, XboxLiveUser user, IList<ulong> usersAffected, XboxSocialUserGroup groupAffected = null)
        {
            this.EventType = type;
            this.User = user;
            this.UsersAffected = usersAffected;
            this.GroupAffected = groupAffected;
        }

        public SocialEventType EventType { get; private set; }

        public XboxLiveUser User { get; private set; }

        public IList<ulong> UsersAffected { get; private set; }

        public XboxSocialUserGroup GroupAffected { get; set; }

        public Exception Exception { get; set; }
    }
}