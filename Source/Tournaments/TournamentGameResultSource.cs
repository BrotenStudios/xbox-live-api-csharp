using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Microsoft.Xbox.Services.Tournaments
{
    public enum TournamentGameResultSource : int
    {
        None = 0,
        Arbitration = 1,
        Server = 2,
        Adjusted = 3,
    }

}
