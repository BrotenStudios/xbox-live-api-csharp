using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Microsoft.Xbox.Services.Matchmaking
{
    public class HopperStatisticsResponse
    {

        public uint PlayersWaitingToMatch
        {
            get;
            private set;
        }

        public TimeSpan EstimatedWaitTime
        {
            get;
            private set;
        }

        public string HopperName
        {
            get;
            private set;
        }

    }
}
