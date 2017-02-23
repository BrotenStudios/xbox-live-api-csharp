// Copyright (c) Microsoft Corporation
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// 
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Microsoft.Xbox.Services
{
    public class XboxLiveLogCallEventArgs : EventArgs
    {

        public string Message
        {
            get;
            private set;
        }

        public string Category
        {
            get;
            private set;
        }

        public XboxServicesDiagnosticsTraceLevel Level
        {
            get;
            private set;
        }

    }
}
