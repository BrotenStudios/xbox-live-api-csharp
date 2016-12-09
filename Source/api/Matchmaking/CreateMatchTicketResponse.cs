using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Microsoft.Xbox.Services.Matchmaking
{
    public class CreateMatchTicketResponse
    {

        public TimeSpan EstimatedWaitTime
        {
            get;
            private set;
        }

        public string MatchTicketId
        {
            get;
            private set;
        }

    }
}
