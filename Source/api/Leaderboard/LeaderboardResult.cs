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
            set;
        }

        public IList<LeaderboardRow> Rows
        {
            get;
            set;
        }

        public IList<LeaderboardColumn> Columns
        {
            get;
            set;
        }

        public uint TotalRowCount
        {
            get;
            set;
        }

        public string DisplayName
        {
            get;
            set;
        }


        public virtual Task<LeaderboardResult> GetNextAsync(uint maxItems)
        {
            throw new NotImplementedException();
        }

    }
}
