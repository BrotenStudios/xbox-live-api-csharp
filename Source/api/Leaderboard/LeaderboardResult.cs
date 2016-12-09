using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Microsoft.Xbox.Services.Leaderboard
{
    public class LeaderboardResult
    {

        public bool HasNext
        {
            get;
            private set;
        }

        public IList<LeaderboardRow> Rows
        {
            get;
            private set;
        }

        public IList<LeaderboardColumn> Columns
        {
            get;
            private set;
        }

        public uint TotalRowCount
        {
            get;
            private set;
        }

        public string DisplayName
        {
            get;
            private set;
        }


        public Task<LeaderboardResult> GetNextAsync(uint maxItems)
        {
            throw new NotImplementedException();
        }

    }
}
