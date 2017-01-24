// -----------------------------------------------------------------------
//  <copyright file="SocialUserGroupLoadedEventArgs.cs" company="Microsoft">
//      Copyright (c) Microsoft. All rights reserved.
//      Licensed under the MIT license. See LICENSE file in the project root for full license information.
//  </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.Xbox.Services.Social.Manager
{
    using global::System;

    public class SocialUserGroupLoadedEventArgs : EventArgs
    {
        public XboxSocialUserGroup SocialUserGroup { get; private set; }
    }
}