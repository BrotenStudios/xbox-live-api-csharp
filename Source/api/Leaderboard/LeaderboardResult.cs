using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Microsoft.Xbox.Services.Leaderboard
{
    using global::Leaderboard;

    public class LeaderboardResult
    {
        private readonly XboxLiveContext context;

        public LeaderboardResult()
        {
        }

        public LeaderboardResult(
            XboxLiveContext context,
            string displayName,
            uint totalRowCount,
            IList<LeaderboardColumn> columns,
            IList<LeaderboardRow> rows)
        {
            this.context = context;

            this.DisplayName = displayName;
            this.TotalRowCount = totalRowCount;
            this.Columns = columns;
            this.Rows = rows;

        }

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

        internal LeaderboardRequest Request { get; set; }

        public virtual Task<LeaderboardResult> GetNextAsync(uint maxItems)
        {
            if (this.Request == null || string.IsNullOrEmpty(this.Request.ContinuationToken))
            {
                throw new XboxException("LeaderboardResult does not have a next page.");
            }

            switch (this.Request.RequestType)
            {
                case LeaderboardRequestType.Global:
                    return this.context.LeaderboardService.GetLeaderboardInternal(null, null, null, null, null, 0, null, 0, this.Request.ContinuationToken);
                case LeaderboardRequestType.Social:
                    return this.context.LeaderboardService.GetLeaderboardForSocialGroupInternal(null, null, null, null, null, 0, null, 0, this.Request.ContinuationToken);
                default:
                    throw new InvalidOperationException("Unable to handle LeaderBoardRequestType " + this.Request.RequestType);
            }
        }

    }
}
