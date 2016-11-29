using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Microsoft.Xbox.Services.Presence
{
    public class DevicePresenceChangeEventArgs : EventArgs
    {

        public bool IsUserLoggedOnDevice
        {
            get;
            private set;
        }

        public PresenceDeviceType DeviceType
        {
            get;
            private set;
        }

        public string XboxUserId
        {
            get;
            private set;
        }

    }
}
