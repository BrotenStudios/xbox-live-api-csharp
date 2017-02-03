using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Xbox.Services.System;

namespace Microsoft.Xbox.Services.Leaderboard
{
    public class LeaderboardResult
    {

        private readonly XboxLiveUser userContext;
        private readonly XboxLiveContextSettings xboxLiveContextSettings;
        private readonly XboxLiveAppConfiguration appConfig;

        public LeaderboardResult(
        XboxLiveUser userContext,
        XboxLiveContextSettings xboxLiveContextSettings,
        XboxLiveAppConfiguration appConfig)
        {
            this.userContext = userContext;
            this.xboxLiveContextSettings = xboxLiveContextSettings;
            this.appConfig = appConfig;
        }

        public LeaderboardResult(
            uint totalRowCount,
            IList<LeaderboardColumn> columns,
            IList<LeaderboardRow> rows,
            XboxLiveUser userContext, 
            XboxLiveContextSettings xboxLiveContextSettings, 
            XboxLiveAppConfiguration appConfig)
        {
            this.TotalRowCount = totalRowCount;
            this.Columns = columns;
            this.Rows = rows;
            this.userContext = userContext;
            this.xboxLiveContextSettings = xboxLiveContextSettings;
            this.appConfig = appConfig;
        }

        public bool HasNext
        {
            get;
            internal set;
        }

        public IList<LeaderboardRow> Rows
        {
            get;
            internal set;
        }

        public IList<LeaderboardColumn> Columns
        {
            get;
            internal set;
        }

        public uint TotalRowCount
        {
            get;
            internal set;
        }

        internal LeaderboardRequest Request { get; set; }

        public virtual Task<LeaderboardResult> GetNextAsync(uint maxItems)
        {
            if (this.Request == null || string.IsNullOrEmpty(this.Request.ContinuationToken))
            {
                throw new XboxException("LeaderboardResult does not have a next page.");
            }
            LeaderboardService service = new LeaderboardService(userContext, xboxLiveContextSettings, appConfig);

            switch (this.Request.RequestType)
            {
                case LeaderboardRequestType.Global:
                    return service.GetLeaderboardInternal(null, null, null, null, null, 0, null, 0, this.Request.ContinuationToken);
                case LeaderboardRequestType.Social:
                    return service.GetLeaderboardForSocialGroupInternal(null, null, null, null, null, 0, null, 0, this.Request.ContinuationToken);
                default:
                    throw new InvalidOperationException("Unable to handle LeaderBoardRequestType " + this.Request.RequestType);
            }
        }

    }
}
