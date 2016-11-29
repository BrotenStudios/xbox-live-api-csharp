using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Microsoft.Xbox.Services.Tournaments
{
    public enum TournamentGameResultState : int
    {
        NoContest = 0,
        Win = 1,
        Loss = 2,
        Draw = 3,
        Rank = 4,
        NoShow = 5,
    }

}
