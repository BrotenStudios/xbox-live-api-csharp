// Copyright (c) Microsoft Corporation
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// 
using Newtonsoft.Json;

namespace Microsoft.Xbox.Services.Leaderboard
{
    using global::System;
    using global::System.Collections.Generic;
    using global::System.Text;
    using global::System.Threading.Tasks;
    using Newtonsoft.Json.Linq;
    using Social.Models;
    using System;

    public class LeaderboardService : ILeaderboardService
    {
        private static readonly Uri leaderboardsBaseUri = new Uri("https://leaderboards.xboxlive.com");
        private readonly XboxLiveUser userContext;
        private readonly XboxLiveContextSettings xboxLiveContextSettings;
        private readonly XboxLiveAppConfiguration appConfig;
        private static string leaderboardAPIContract = "4";

        internal LeaderboardService(XboxLiveUser userContext, XboxLiveContextSettings xboxLiveContextSettings, XboxLiveAppConfiguration appConfig)
        {
            this.userContext = userContext;
            this.xboxLiveContextSettings = xboxLiveContextSettings;
            this.appConfig = appConfig;
        }

        public Task<LeaderboardResult> GetLeaderboardAsync(string statName, LeaderboardQuery query)
        {
            string xuid = null;
            if(query.SkipResultToMe)
            {
                xuid = userContext.XboxUserId;
            }
            return this.GetLeaderboardInternal(userContext.XboxUserId, appConfig.ServiceConfigurationId, statName, null, query, LeaderboardRequestType.Global);
        }

        public Task<LeaderboardResult> GetSocialLeaderboardAsync(string leaderboardName, string socialGroup, LeaderboardQuery query)
        {
            if (string.IsNullOrEmpty(socialGroup))
            {
                throw new XboxException("socialGroup parameter is required for getting Leaderboard for social group.");
            }

            if (string.Equals(socialGroup, "people", StringComparison.CurrentCultureIgnoreCase))
            {
                socialGroup = "all";
            }
            return this.GetLeaderboardInternal(userContext.XboxUserId, appConfig.ServiceConfigurationId, leaderboardName, socialGroup, query, LeaderboardRequestType.Social);
        }

        internal Task<LeaderboardResult> GetLeaderboardInternal(string xuid, string serviceConfigurationId, string leaderboardName, string socialGroup, LeaderboardQuery query, LeaderboardRequestType leaderboardType)
        {
            string requestPath = "";

            string skipToXboxUserId = null;
            if (query.SkipResultToMe)
            {
                skipToXboxUserId = userContext.XboxUserId;
            }
            if (leaderboardType == LeaderboardRequestType.Social)
            {
                requestPath  = CreateSocialLeaderboardUrlPath(serviceConfigurationId, leaderboardName, xuid, query.MaxItems, skipToXboxUserId, query.SkipResultsToRank, query.ContinuationToken, socialGroup);
            }
            else
            {
                requestPath = CreateLeaderboardUrlPath(serviceConfigurationId, leaderboardName, xuid, query.MaxItems, skipToXboxUserId, query.SkipResultsToRank, query.ContinuationToken, socialGroup);
            }

            XboxLiveHttpRequest request = XboxLiveHttpRequest.Create(xboxLiveContextSettings, HttpMethod.Get, leaderboardsBaseUri.ToString(), requestPath);
            request.ContractVersion = leaderboardAPIContract;
            return request.GetResponseWithAuth(userContext, HttpCallResponseBodyType.JsonBody)
                .ContinueWith(
                    responseTask =>
                    {
                        var leaderboardRequestType = LeaderboardRequestType.Global;
                        if (socialGroup != null)
                        {
                            leaderboardRequestType = LeaderboardRequestType.Social;
                        }
                        LeaderboardRequest leaderboardRequest = new LeaderboardRequest(leaderboardRequestType, leaderboardName);
                        LeaderboardQuery nextQuery = new LeaderboardQuery(query);
                        nextQuery.StatName = leaderboardName;
                        nextQuery.SocialGroup = socialGroup;
                        return this.HandleLeaderboardResponse(leaderboardRequest, responseTask, nextQuery);
                    });
        }

        internal LeaderboardResult HandleLeaderboardResponse(LeaderboardRequest request, Task<XboxLiveHttpResponse> responseTask, LeaderboardQuery nextQuery)
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
            if (lbResponse.PagingInfo != null)
            {
                nextQuery.ContinuationToken = lbResponse.PagingInfo.ContinuationToken;
            }

            LeaderboardResult result = new LeaderboardResult(lbResponse.LeaderboardInfo.TotalCount, columns, rows, userContext, xboxLiveContextSettings, appConfig)
            {
                NextQuery = nextQuery
            };
            return result;
        }

        private string CreateLeaderboardUrlPath(string serviceConfigurationId, string leaderboardName, string xuid, uint maxItems, string skipToXboxUserId, uint skipToRank, string continuationToken, string socialGroup)
        {
            StringBuilder requestPath = new StringBuilder();
            requestPath.AppendFormat("scids/{0}/leaderboards/stat({1})?", serviceConfigurationId, leaderboardName);

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

            return requestPath.ToString();
        }

        private string CreateSocialLeaderboardUrlPath(string serviceConfigurationId, string leaderboardName, string xuid, uint maxItems, string skipToXboxUserId, uint skipToRank, string continuationToken, string socialGroup)
        {
            StringBuilder requestPath = new StringBuilder();
            requestPath.AppendFormat("users/xuid({0})/scids/{1}/stats/{2}/people/{3}?", xuid, serviceConfigurationId, leaderboardName, socialGroup);

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

            // Remove the trailing query string bit
            requestPath.Remove(requestPath.Length - 1, 1);

            return requestPath.ToString();
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