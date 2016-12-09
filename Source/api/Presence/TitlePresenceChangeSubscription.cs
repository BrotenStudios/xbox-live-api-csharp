using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Microsoft.Xbox.Services.Presence
{
    public class TitlePresenceChangeSubscription
    {

        public uint TitleId
        {
            get;
            private set;
        }

        public string XboxUserId
        {
            get;
            private set;
        }

        public uint SubscriptionId
        {
            get;
            private set;
        }

        public string ResourceUri
        {
            get;
            private set;
        }

        public Microsoft.Xbox.Services.RealTimeActivity.RealTimeActivitySubscriptionState State
        {
            get;
            private set;
        }

    }
}
