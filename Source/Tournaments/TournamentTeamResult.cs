using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Microsoft.Xbox.Services.Tournaments
{
    public class TournamentTeamResult
    {
        public TournamentTeamResult() {
        }

        public ulong Ranking
        {
            get;
            set;
        }

        public TournamentGameResultState State
        {
            get;
            set;
        }

    }
}
