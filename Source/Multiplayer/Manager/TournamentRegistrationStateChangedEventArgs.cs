using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Microsoft.Xbox.Services.Multiplayer.Manager
{
    public class TournamentRegistrationStateChangedEventArgs : EventArgs
    {

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

    }
}
