using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Microsoft.Xbox.Services.Multiplayer
{
    public class MultiplayerSessionTournamentsServer
    {

        public Microsoft.Xbox.Services.Tournaments.TournamentGameResultSource LastGameResultSource
        {
            get;
            private set;
        }

        public Microsoft.Xbox.Services.Tournaments.TournamentTeamResult LastTeamResult
        {
            get;
            private set;
        }

        public DateTimeOffset LastGameEndTime
        {
            get;
            private set;
        }

        public MultiplayerSessionReference NextGameSessionRef
        {
            get;
            private set;
        }

        public DateTimeOffset NextGameStartTime
        {
            get;
            private set;
        }

        public Microsoft.Xbox.Services.Tournaments.TournamentRegistrationReason RegistrationReason
        {
            get;
            private set;
        }

        public Microsoft.Xbox.Services.Tournaments.TournamentRegistrationState RegistrationState
        {
            get;
            private set;
        }

        public Dictionary<string, MultiplayerSessionReference> Teams
        {
            get;
            private set;
        }

        public Microsoft.Xbox.Services.Tournaments.TournamentReference TournamentReference
        {
            get;
            private set;
        }

    }
}
