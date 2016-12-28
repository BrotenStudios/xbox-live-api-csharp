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
            set;
        }

        public uint Rank
        {
            get;
            set;
        }

        public double Percentile
        {
            get;
            set;
        }

        public string XboxUserId
        {
            get;
            set;
        }

        public string Gamertag
        {
            get;
            set;
        }

    }
}
