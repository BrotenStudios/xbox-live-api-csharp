using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Microsoft.Xbox.Services.Multiplayer.Manager
{
    public class PerformQosMeasurementsEventArgs : EventArgs
    {

        public Dictionary<string, string> AddressToDeviceTokens
        {
            get;
            private set;
        }

    }
}
