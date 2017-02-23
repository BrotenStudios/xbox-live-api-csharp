// Copyright (c) Microsoft Corporation
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// 
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Microsoft.Xbox.Services
{
    public class SignInResult
    {
        public SignInResult(SignInStatus status)
        {
            Status = status;
        }

        public SignInStatus Status
        {
            get;
            internal set;
        }

    }
}
