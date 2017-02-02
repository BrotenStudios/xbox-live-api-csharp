// -----------------------------------------------------------------------
//  <copyright file="SocialManagerExtraDetailLevel.cs" company="Microsoft">
//      Copyright (c) Microsoft. All rights reserved.
//      Licensed under the MIT license. See LICENSE file in the project root for full license information.
//  </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.Xbox.Services.Social.Manager
{
    using global::System;

    [Flags]
    public enum SocialManagerExtraDetailLevel
    {
        None = 0x0,
        TitleHistory = 0x1,
        PreferredColor = 0x2,
    }
}