using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Microsoft.Xbox.Services.Multiplayer.Manager
{
    public class TournamentGameSessionReadyEventArgs : EventArgs
    {

        public DateTimeOffset StartTime
        {
            get;
            private set;
        }

    }
}
