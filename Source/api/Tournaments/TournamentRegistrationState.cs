using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Microsoft.Xbox.Services.Tournaments
{
    public enum TournamentRegistrationState : int
    {
        Unknown = 0,
        Pending = 1,
        Withdrawn = 2,
        Rejected = 3,
        Registered = 4,
        Completed = 5,
    }

}
