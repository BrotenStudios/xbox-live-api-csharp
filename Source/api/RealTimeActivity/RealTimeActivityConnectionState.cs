using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Microsoft.Xbox.Services.RealTimeActivity
{
    public enum RealTimeActivityConnectionState : int
    {
        Connected = 0,
        Connecting = 1,
        Disconnected = 2,
    }

}
