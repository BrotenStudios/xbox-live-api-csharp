using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Microsoft.Xbox.Services.Tournaments
{
    public enum TournamentRegistrationReason : int
    {
        Unknown = 0,
        RegistrationClosed = 1,
        MemberAlreadyRegistered = 2,
        TournamentFull = 3,
        TeamEliminated = 4,
        TournamentCompleted = 5,
    }

}
