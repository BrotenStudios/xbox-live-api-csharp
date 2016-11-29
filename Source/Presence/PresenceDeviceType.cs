using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Microsoft.Xbox.Services.Presence
{
    public enum PresenceDeviceType : int
    {
        Unknown = 0,
        WindowsPhone = 1,
        WindowsPhone7 = 2,
        Web = 3,
        Xbox360 = 4,
        PC = 5,
        Windows8 = 6,
        XboxOne = 7,
        WindowsOneCore = 8,
        WindowsOneCoreMobile = 9,
    }

}
