// -----------------------------------------------------------------------
//  <copyright file="LeaderboardService.cs" company="Microsoft">
//      Copyright (c) Microsoft. All rights reserved.
//      Licensed under the MIT license. See LICENSE file in the project root for full license information.
//  </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.Xbox.Services.Leaderboard
{
    using global::System;
    using global::System.Collections.Generic;
    using global::System.Text;
    using global::System.Threading.Tasks;
    using Newtonsoft.Json.Linq;
    using Social.Models;
    using System;

    public class LeaderboardService
    {
        private static readonly Uri leaderboardsBaseUri = new Uri("https://leaderboards.xboxlive.com");
        private readonly XboxLiveUser userContext;
        private readonly XboxLiveContextSettings xboxLiveContextSettings;
        private readonly XboxLiveAppConfiguration appConfig;

        internal LeaderboardService(XboxLiveUser userContext, XboxLiveContextSettings xboxLiveContextSettings, XboxLiveAppConfiguration appConfig)
        {
            this.userContext = userContext;
            this.xboxLiveContextSettings = xboxLiveContextSettings;
            this.appConfig = appConfig;
        }

        public Task<LeaderboardResult> GetLeaderboardAsync(string leaderboardName, LeaderboardQuery query)
        {
            string xuid = null;
            if(query.SkipResultToMe)
            {
                xuid = userContext.XboxUserId;
            }
            return this.GetLeaderboardInternal(userContext.XboxUserId, appConfig.ServiceConfigurationId, leaderboardName, null, xuid, query.SkipResultsToRank, null, query.MaxItems, null);
        }

        public Task<LeaderboardResult> GetSocialLeaderboardAsync(string leaderboardName, string socialGroup, LeaderboardQuery query)
        {
            string xuid = null;
            if (query.SkipResultToMe)
            {
                xuid = userContext.XboxUserId;
            }
            return this.GetLeaderboardForSocialGroupInternal(userContext.XboxUserId, appConfig.ServiceConfigurationId, leaderboardName, socialGroup, xuid, query.SkipResultsToRank, null, query.MaxItems, null);
        }

        internal Task<LeaderboardResult> GetLeaderboardInternal(string xuid, string serviceConfigurationId, string leaderboardName, string socialGroup, string skipToXboxUserId, uint skipToRank, string[] additionalColumns, uint maxItems, string continuationToken)
        {
            StringBuilder requestPath = new StringBuilder();
            requestPath.AppendFormat("scids/{0}/leaderboards/{1}?", serviceConfigurationId, leaderboardName);

            if (xuid != null)
            {
                AppendQueryParameter(requestPath, "xuid", xuid);
            }

            if (maxItems > 0)
            {
                AppendQueryParameter(requestPath, "maxItems", maxItems);
            }

            if (!string.IsNullOrEmpty(skipToXboxUserId) && skipToRank > 0)
            {
                throw new ArgumentException("Cannot provide both user and rank to skip to.");
            }

            if (continuationToken != null)
            {
                AppendQueryParameter(requestPath, "continuationToken", continuationToken);
            }
            else if (!string.IsNullOrEmpty(skipToXboxUserId))
            {
                AppendQueryParameter(requestPath, "skipToUser", skipToXboxUserId);
            }
            else if (skipToRank > 0)
            {
                AppendQueryParameter(requestPath, "skipToRank", skipToRank);
            }

            if (socialGroup != null)
            {
                AppendQueryParameter(requestPath, "view", "People");
                AppendQueryParameter(requestPath, "viewTarget", socialGroup);
            }

            // Remove the trailing query string bit
            requestPath.Remove(requestPath.Length - 1, 1);

            XboxLiveHttpRequest request = XboxLiveHttpRequest.Create(xboxLiveContextSettings, HttpMethod.Get, leaderboardsBaseUri.ToString(), requestPath.ToString());
            request.ContractVersion = "3";
            return request.GetResponseWithAuth(userContext, HttpCallResponseBodyType.JsonBody)
                .ContinueWith(
                    responseTask =>
                    {
                        LeaderboardRequest leaderboardRequest = new LeaderboardRequest(LeaderboardRequestType.Global);
                        return this.HandleLeaderboardResponse(leaderboardRequest, responseTask);
                    });
        }

        internal Task<LeaderboardResult> GetLeaderboardForSocialGroupInternal(string xboxUserId, string serviceConfigurationId, string statisticName, string socialGroup, string skipToXboxUserId, uint skipToRank, string sortOrder, uint maxItems, string continuationToken)
        {
            string requestPath = string.Format("/scids/{0}/leaderboards/{1}", serviceConfigurationId);
            XboxLiveHttpRequest request = XboxLiveHttpRequest.Create(xboxLiveContextSettings, HttpMethod.Get, leaderboardsBaseUri.ToString(), requestPath);

            return request.GetResponseWithAuth(userContext, HttpCallResponseBodyType.JsonBody)
                .ContinueWith(
                    responseTask =>
                    {
                        LeaderboardRequest leaderboardRequest = new LeaderboardRequest(LeaderboardRequestType.Social);
                        return this.HandleLeaderboardResponse(leaderboardRequest, responseTask);
                    });
        }

        internal LeaderboardResult HandleLeaderboardResponse(LeaderboardRequest request, Task<XboxLiveHttpResponse> responseTask)
        {
            XboxLiveHttpResponse response = responseTask.Result;

            LeaderboardResponse lbResponse = JsonSerialization.FromJson<LeaderboardResponse>(response.ResponseBodyString);

            IList<LeaderboardColumn> columns = new List<LeaderboardColumn>() { lbResponse.LeaderboardInfo.Column };

            IList<LeaderboardRow> rows = new List<LeaderboardRow>();
            foreach(LeaderboardRowResponse row in lbResponse.Rows)
            {
                LeaderboardRow newRow = new LeaderboardRow()
                {
                    Gamertag = row.Gamertag,
                    Percentile = row.Percentile,
                    Rank = row.Rank,
                    XboxUserId = row.XboxUserId,
                };
                if(row.Value != null)
                {
                    newRow.Values = new List<string>();
                    newRow.Values.Add(row.Value);
                }
                else
                {
                    newRow.Values = row.Values;
                }
                rows.Add(newRow);
            }

            request.ContinuationToken = lbResponse.ContinuationToken;

            LeaderboardResult result = new LeaderboardResult(lbResponse.LeaderboardInfo.TotalCount, columns, rows, userContext, xboxLiveContextSettings, appConfig)
            {
                Request = request
            };
            return result;
        }

        private static void AppendQueryParameter(StringBuilder builder, string parameterName, object parameterValue)
        {
            builder.Append(parameterName);
            builder.Append("=");
            builder.Append(parameterValue);
            builder.Append("&");
        }
    }
}