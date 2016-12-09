using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Microsoft.Xbox.Services.Tournaments
{
    public enum TournamentArbitrationState : int
    {
        Completed = 0,
        Canceled = 1,
        NoResults = 2,
        PartialResults = 3,
    }

}
