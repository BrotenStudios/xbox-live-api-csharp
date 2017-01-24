// -----------------------------------------------------------------------
//  <copyright file="SignOutCompletedEventArgs.cs" company="Microsoft">
//      Copyright (c) Microsoft. All rights reserved.
//      Licensed under the MIT license. See LICENSE file in the project root for full license information.
//  </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.Xbox.Services.System
{
    using global::System;

    public class SignOutCompletedEventArgs : EventArgs
    {
        public SignOutCompletedEventArgs(XboxLiveUser user)
        {
            this.User = user;
        }

        public XboxLiveUser User { get; private set; }
    }
}