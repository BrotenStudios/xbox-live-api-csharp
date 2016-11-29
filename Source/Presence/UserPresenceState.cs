using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Microsoft.Xbox.Services.Presence
{
    public enum UserPresenceState : int
    {
        Unknown = 0,
        Online = 1,
        Away = 2,
        Offline = 3,
    }

}
