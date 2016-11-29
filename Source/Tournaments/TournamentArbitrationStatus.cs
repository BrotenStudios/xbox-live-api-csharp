using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Microsoft.Xbox.Services.Tournaments
{
    public enum TournamentArbitrationStatus : int
    {
        Waiting = 0,
        InProgress = 1,
        Complete = 2,
        Playing = 3,
        InComplete = 4,
    }

}
