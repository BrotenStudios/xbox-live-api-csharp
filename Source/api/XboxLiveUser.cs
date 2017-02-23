// Copyright (c) Microsoft Corporation
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// 
namespace Microsoft.Xbox.Services
{
    using global::System;
    using Microsoft.Xbox.Services.System;

    public partial class XboxLiveUser : IXboxLiveUser
    {
        private IUserImpl userImpl;

        public static event EventHandler<SignInCompletedEventArgs> SignInCompleted;
        public static event EventHandler<SignOutCompletedEventArgs> SignOutCompleted;

        public string WebAccountId
        {
            get
            {
                return this.userImpl.WebAccountId;
            }
        }

        public bool IsSignedIn
        {
            get
            {
                return this.userImpl.IsSignedIn;
            }
        }

        public string Privileges
        {
            get
            {
                return this.userImpl.Privileges;
            }
        }

        public string XboxUserId
        {
            get
            {
                return this.userImpl.XboxUserId;
            }
        }

        public string AgeGroup
        {
            get
            {
                return this.userImpl.AgeGroup;
            }
        }

        public string Gamertag
        {
            get
            {
                return this.userImpl.Gamertag;
            }
        }

    }
}