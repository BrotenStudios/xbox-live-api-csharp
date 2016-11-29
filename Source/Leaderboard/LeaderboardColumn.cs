using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Microsoft.Xbox.Services.Leaderboard
{
    public class LeaderboardColumn
    {

        public Type StatisticType
        {
            get;
            private set;
        }

        public string StatisticName
        {
            get;
            private set;
        }

        public string DisplayName
        {
            get;
            private set;
        }

    }
}
