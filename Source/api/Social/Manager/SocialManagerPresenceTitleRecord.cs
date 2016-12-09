using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Microsoft.Xbox.Services.Social.Manager
{
    public class SocialManagerPresenceTitleRecord
    {

        public Microsoft.Xbox.Services.Presence.PresenceDeviceType DeviceType
        {
            get;
            private set;
        }

        public bool IsBroadcasting
        {
            get;
            private set;
        }

        public string PresenceText
        {
            get;
            private set;
        }

        public uint TitleId
        {
            get;
            private set;
        }

        public bool IsTitleActive
        {
            get;
            private set;
        }

    }
}
