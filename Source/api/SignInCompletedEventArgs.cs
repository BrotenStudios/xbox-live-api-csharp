// Copyright (c) Microsoft Corporation
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// 
using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Xbox.Services
{
    public class SignInCompletedEventArgs : EventArgs
    {
        public string XboxUserId { get; private set; }

        public SignInCompletedEventArgs(string xboxUserId)
        {
            XboxUserId = xboxUserId;
        }
    }
}
