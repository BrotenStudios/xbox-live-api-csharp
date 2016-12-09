using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Microsoft.Xbox.Services.Multiplayer
{
    public class MultiplayerSessionArbitrationServer
    {

        public Dictionary<string, Microsoft.Xbox.Services.Tournaments.TournamentTeamResult> Results
        {
            get;
            private set;
        }

        public uint ResultConfidenceLevel
        {
            get;
            private set;
        }

        public Microsoft.Xbox.Services.Tournaments.TournamentGameResultSource ResultSource
        {
            get;
            private set;
        }

        public Microsoft.Xbox.Services.Tournaments.TournamentArbitrationState ResultState
        {
            get;
            private set;
        }

    }
}
