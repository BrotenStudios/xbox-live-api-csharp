using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Microsoft.Xbox.Services.RealTimeActivity
{
    public class RealTimeActivitySubscriptionErrorEventArgs : EventArgs
    {

        public string ErrorMessage
        {
            get;
            private set;
        }

        public RealTimeActivitySubscriptionError SubscriptionError
        {
            get;
            private set;
        }

        public string ResourceUri
        {
            get;
            private set;
        }

        public uint SubscriptionId
        {
            get;
            private set;
        }

        public RealTimeActivitySubscriptionState State
        {
            get;
            private set;
        }

    }
}
