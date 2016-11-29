using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Microsoft.Xbox.Services.Social.Manager
{
    public enum PresenceFilter : int
    {
        Unknown = 0,
        TitleOnline = 1,
        TitleOffline = 2,
        AllOnline = 3,
        AllOffline = 4,
        AllTitle = 5,
        All = 6,
    }

}
