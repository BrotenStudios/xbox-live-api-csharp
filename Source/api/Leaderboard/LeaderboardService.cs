// -----------------------------------------------------------------------
//  <copyright file="LeaderboardService.cs" company="Microsoft">
//      Copyright (c) Microsoft. All rights reserved.
//      Licensed under the MIT license. See LICENSE file in the project root for full license information.
//  </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.Xbox.Services.Leaderboard
{
    using global::System;
    using global::System.Text;
    using global::System.Threading.Tasks;

    public class LeaderboardService
    {
        private static readonly Uri leaderboardsBaseUri = new Uri("https://leaderboards.xboxlive.com");
        private readonly XboxLiveContext context;

        internal LeaderboardService(XboxLiveContext context)
        {
            this.context = context;
        }

        public Task<LeaderboardResult> GetLeaderboardAsync(string serviceConfigurationId, string leaderboardName)
        {
            return this.GetLeaderboardAsync(serviceConfigurationId, leaderboardName, null, null, 0, uint.MaxValue);
        }

        public Task<LeaderboardResult> GetLeaderboardAsync(string serviceConfigurationId, string leaderboardName, uint skipToRank, uint maxItems)
        {
            return this.GetLeaderboardAsync(serviceConfigurationId, leaderboardName, null, null, skipToRank, maxItems);
        }

        public Task<LeaderboardResult> GetLeaderboardAsync(string serviceConfigurationId, string leaderboardName, string xuid, string socialGroup, uint maxItems)
        {
            return this.GetLeaderboardAsync(serviceConfigurationId, leaderboardName, xuid, socialGroup, 0, maxItems);
        }

        public Task<LeaderboardResult> GetLeaderboardAsync(string serviceConfigurationId, string leaderboardName, string xuid, string socialGroup, uint skipToRank, uint maxItems)
        {
            return this.GetLeaderboardWithAdditionalColumnsAsync(serviceConfigurationId, leaderboardName, xuid, socialGroup, skipToRank, null, maxItems);
        }

        public virtual Task<LeaderboardResult> GetLeaderboardWithAdditionalColumnsAsync(string serviceConfigurationId, string leaderboardName, string xuid, string socialGroup, uint skipToRank, string[] additionalColumns, uint maxItems)
        {
            return this.GetLeaderboardInternal(xuid, serviceConfigurationId, leaderboardName, socialGroup, null, skipToRank, additionalColumns, maxItems, null);
        }

        public Task<LeaderboardResult> GetLeaderboardWithAdditionalColumnsAsync(string serviceConfigurationId, string leaderboardName, uint skipToRank, string[] additionalColumns, uint maxItems)
        {
            return this.GetLeaderboardWithAdditionalColumnsAsync(serviceConfigurationId, leaderboardName, null, null, skipToRank, additionalColumns, maxItems);
        }

        public Task<LeaderboardResult> GetLeaderboardWithAdditionalColumnsAsync(string serviceConfigurationId, string leaderboardName, string xuid, string socialGroup, string[] additionalColumns, uint maxItems)
        {
            return this.GetLeaderboardWithAdditionalColumnsAsync(serviceConfigurationId, leaderboardName, xuid, socialGroup, 0, additionalColumns, maxItems);
        }

        public Task<LeaderboardResult> GetLeaderboardWithAdditionalColumnsAsync(string serviceConfigurationId, string leaderboardName, string[] additionalColumns)
        {
            return this.GetLeaderboardWithAdditionalColumnsAsync(serviceConfigurationId, leaderboardName, null, null, 0, additionalColumns, 10);
        }

        public Task<LeaderboardResult> GetLeaderboardWithSkipToUserAsync(string serviceConfigurationId, string leaderboardName, string xuid, string socialGroup, string skipToXboxUserId, uint maxItems)
        {
            return this.GetLeaderboardWithSkipToUserWithAdditionalColumnsAsync(serviceConfigurationId, leaderboardName, xuid, socialGroup, skipToXboxUserId, null, maxItems);
        }

        public Task<LeaderboardResult> GetLeaderboardWithSkipToUserAsync(string serviceConfigurationId, string leaderboardName, string skipToXboxUserId, uint maxItems)
        {
            return this.GetLeaderboardWithSkipToUserWithAdditionalColumnsAsync(serviceConfigurationId, leaderboardName, null, null, skipToXboxUserId, null, maxItems);
        }

        public virtual Task<LeaderboardResult> GetLeaderboardWithSkipToUserWithAdditionalColumnsAsync(string serviceConfigurationId, string leaderboardName, string xuid, string socialGroup, string skipToXboxUserId, string[] additionalColumns, uint maxItems)
        {
            return this.GetLeaderboardInternal(xuid, serviceConfigurationId, leaderboardName, socialGroup, skipToXboxUserId, 0, additionalColumns, maxItems, null);
        }

        public Task<LeaderboardResult> GetLeaderboardWithSkipToUserWithAdditionalColumnsAsync(string serviceConfigurationId, string leaderboardName, string skipToXboxUserId, string[] additionalColumns, uint maxItems)
        {
            return this.GetLeaderboardWithSkipToUserWithAdditionalColumnsAsync(serviceConfigurationId, leaderboardName, null, null, skipToXboxUserId, additionalColumns, maxItems);
        }

        public Task<LeaderboardResult> GetLeaderboardForSocialGroupAsync(string xboxUserId, string serviceConfigurationId, string statisticName, string socialGroup, string sortOrder, uint maxItems)
        {
            return this.GetLeaderboardForSocialGroupWithSkipToUserAsync(xboxUserId, serviceConfigurationId, statisticName, socialGroup, null, sortOrder, maxItems);
        }

        public Task<LeaderboardResult> GetLeaderboardForSocialGroupAsync(string xboxUserId, string serviceConfigurationId, string statisticName, string socialGroup, uint maxItems)
        {
            return this.GetLeaderboardForSocialGroupWithSkipToUserAsync(xboxUserId, serviceConfigurationId, statisticName, socialGroup, null, null, maxItems);
        }

        public Task<LeaderboardResult> GetLeaderboardForSocialGroupWithSkipToRankAsync(string xboxUserId, string serviceConfigurationId, string statisticName, string socialGroup, uint skipToRank, string sortOrder, uint maxItems)
        {
            return this.GetLeaderboardForSocialGroupInternal(xboxUserId, serviceConfigurationId, statisticName, socialGroup, null, skipToRank, sortOrder, maxItems, null);
        }

        public Task<LeaderboardResult> GetLeaderboardForSocialGroupWithSkipToUserAsync(string xboxUserId, string serviceConfigurationId, string statisticName, string socialGroup, string skipToXboxUserId, string sortOrder, uint maxItems)
        {
            return this.GetLeaderboardForSocialGroupInternal(xboxUserId, serviceConfigurationId, statisticName, socialGroup, skipToXboxUserId, 0, sortOrder, maxItems, null);
        }

        internal Task<LeaderboardResult> GetLeaderboardInternal(string xuid, string serviceConfigurationId, string leaderboardName, string socialGroup, string skipToXboxUserId, uint skipToRank, string[] additionalColumns, uint maxItems, string continuationToken)
        {
            StringBuilder requestPath = new StringBuilder();
            requestPath.AppendFormat("/scids/{0}/leaderboards/{1}?", serviceConfigurationId, leaderboardName);

            if (xuid != null)
            {
                AppendQueryParameter(requestPath, "xuid", xuid);
            }

            if (maxItems > 0)
            {
                AppendQueryParameter(requestPath, "maxItems", maxItems);
            }

            if (string.IsNullOrEmpty(skipToXboxUserId) && skipToRank > 0)
            {
                throw new ArgumentException("Cannot provide both user and rank to skip to.");
            }

            if (continuationToken != null)
            {
                AppendQueryParameter(requestPath, "continuationToken", continuationToken);
            }
            else if (string.IsNullOrEmpty(skipToXboxUserId))
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
            requestPath.Remove(requestPath.Length - 2, 1);

            XboxLiveHttpRequest request = XboxLiveHttpRequest.Create(this.context.Settings, HttpMethod.Get, leaderboardsBaseUri.ToString(), requestPath.ToString());

            return request.GetResponseWithAuth(this.context.User, HttpCallResponseBodyType.JsonBody)
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
            XboxLiveHttpRequest request = XboxLiveHttpRequest.Create(this.context.Settings, HttpMethod.Get, leaderboardsBaseUri.ToString(), requestPath);

            return request.GetResponseWithAuth(this.context.User, HttpCallResponseBodyType.JsonBody)
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

            request.ContinuationToken = response.Headers["Continuation-Token"];

            LeaderboardResult result = new LeaderboardResult(this.context, null, 10, null, null)
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