using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Microsoft.Xbox.Services.Multiplayer
{
    public enum MultiplayerSessionStatus : int
    {
        Unknown = 0,
        Active = 1,
        Inactive = 2,
        Reserved = 3,
    }

}
