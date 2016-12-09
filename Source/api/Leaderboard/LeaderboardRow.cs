using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Microsoft.Xbox.Services.Leaderboard
{
    public class LeaderboardRow
    {

        public IList<string> Values
        {
            get;
            private set;
        }

        public uint Rank
        {
            get;
            private set;
        }

        public double Percentile
        {
            get;
            private set;
        }

        public string XboxUserId
        {
            get;
            private set;
        }

        public string Gamertag
        {
            get;
            private set;
        }

    }
}
