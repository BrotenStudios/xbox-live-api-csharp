// -----------------------------------------------------------------------
//  <copyright file="XboxLiveUser.cs" company="Microsoft">
//      Copyright (c) Microsoft. All rights reserved.
//      Licensed under the MIT license. See LICENSE file in the project root for full license information.
//  </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.Xbox.Services.System
{
    using global::System;
    using global::System.Collections.Generic;
    using global::System.Runtime.InteropServices;
    using global::System.Text;
    using global::System.Threading.Tasks;

    public partial class XboxLiveUser : IXboxLiveUser
    {
        public static event EventHandler<SignOutCompletedEventArgs> SignOutCompleted;

        public string WebAccountId { get; private set; }

        public bool IsSignedIn { get; private set; }

        public string Privileges { get; private set; }

        public string XboxUserId { get; private set; }

        public string AgeGroup { get; private set; }

        public string Gamertag { get; private set; }

    }
}