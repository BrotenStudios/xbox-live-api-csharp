namespace Microsoft.Xbox.Services.Social.Manager
{
    using global::System;
    using global::System.Collections.Generic;
    using global::System.Linq;
    using global::System.Text;
    using global::System.Threading.Tasks;

    using Microsoft.Xbox.Services.System;

    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    internal class PeopleHubService
    {
        private readonly XboxLiveUser userContext;
        private readonly XboxLiveContextSettings httpCallSettings;
        private readonly XboxLiveAppConfiguration appConfig;
        private readonly string peopleHubHost;

        public PeopleHubService(XboxLiveUser userContext, XboxLiveContextSettings httpCallSettings, XboxLiveAppConfiguration appConfig)
        {
            this.userContext = userContext;
            this.httpCallSettings = httpCallSettings;
            this.appConfig = appConfig;
            this.peopleHubHost = XboxLiveEndpoint.GetEndpointForService("peoplehub", appConfig);
        }

        public Task<XboxSocialUser> GetProfileInfo(string callerXboxUserId, SocialManagerExtraDetailLevel decorations)
        {
            string path = "/users/me/people/xuids(" + callerXboxUserId + ")";

            if ((decorations | SocialManagerExtraDetailLevel.NoExtraDetail) != SocialManagerExtraDetailLevel.NoExtraDetail)
            {
                path += "/decoration/";

                if (decorations.HasFlag(SocialManagerExtraDetailLevel.TitleHistoryLevel))
                {
                    path += "titlehistory(" + this.appConfig.TitleId + "),";
                }

                if (decorations.HasFlag(SocialManagerExtraDetailLevel.PreferredColorLevel))
                {
                    path += "preferredcolor,";
                }

                path += "presenceDetail";
            }

            XboxLiveHttpRequest request = XboxLiveHttpRequest.Create(
                this.httpCallSettings,
                HttpMethod.Get,
                this.peopleHubHost,
                path);

            request.ContractVersion = "1";

            return request.GetResponseWithAuth(this.userContext, HttpCallResponseBodyType.JsonBody)
                .ContinueWith(responseTask =>
                {
                    var response = responseTask.Result;
                    JObject responseBody = JObject.Parse(response.ResponseBodyString);
                    string personJson = responseBody["people"][0].ToString();
                    XboxSocialUser user = JsonConvert.DeserializeObject<XboxSocialUser>(personJson);
                    List<XboxSocialUser> users = responseBody["people"].ToObject<List<XboxSocialUser>>();
                    return users[0];
                });
        }

        public Task<List<XboxSocialUser>> GetSocialGraph(string callerXboxUserId, SocialManagerExtraDetailLevel decorations)
        {
            return this.GetSocialGraph(callerXboxUserId, decorations, "social", null, false);
        }

        public Task<List<XboxSocialUser>> GetSocialGraph(string callerXboxUserId, SocialManagerExtraDetailLevel decorations, IList<string> xboxLiveUsers)
        {
            return this.GetSocialGraph(callerXboxUserId, decorations, string.Empty, xboxLiveUsers, true);
        }

        private Task<List<XboxSocialUser>> GetSocialGraph(string callerXboxUserId, SocialManagerExtraDetailLevel decorations, string relationshipType, IList<string> xboxLiveUsers, bool isBatch)
        {
            string pathAndQuery = this.CreateSocialGraphSubpath(callerXboxUserId, decorations, relationshipType, isBatch);
            XboxLiveHttpRequest request = XboxLiveHttpRequest.Create(
                this.httpCallSettings,
                isBatch ? HttpMethod.Post : HttpMethod.Get,
                this.peopleHubHost,
                pathAndQuery);

            request.ContractVersion = "1";

            if (isBatch)
            {
                JObject postBody = new JObject(new JProperty("xuids", xboxLiveUsers));
                request.RequestBody = postBody.ToString(Formatting.None);
            }

            return request.GetResponseWithAuth(this.userContext, HttpCallResponseBodyType.JsonBody)
                .ContinueWith(responseTask =>
                {
                    var response = responseTask.Result;
                    JObject responseBody = JObject.Parse(response.ResponseBodyString);
                    List<XboxSocialUser> users = responseBody["people"].ToObject<List<XboxSocialUser>>();
                    return users;
                });
        }

        private string CreateSocialGraphSubpath(string xboxUserId, SocialManagerExtraDetailLevel decorations, string relationshipType, bool isBatch)
        {
            StringBuilder path = new StringBuilder();
            path.Append("/users/xuid(");
            path.Append(xboxUserId);
            path.Append(")/people");
            if (!string.IsNullOrEmpty(relationshipType))
            {
                path.Append("/");
                path.Append(relationshipType);
            }

            if (isBatch)
            {
                path.Append("/batch");
            }

            if ((decorations | SocialManagerExtraDetailLevel.NoExtraDetail) != SocialManagerExtraDetailLevel.NoExtraDetail)
            {
                path.Append("/decoration/");

                if (decorations.HasFlag(SocialManagerExtraDetailLevel.TitleHistoryLevel))
                {
                    path.Append("titlehistory(" + this.appConfig.TitleId + "),");
                }

                if (decorations.HasFlag(SocialManagerExtraDetailLevel.PreferredColorLevel))
                {
                    path.Append("preferredcolor,");
                }

                path.Append("presenceDetail");
            }

            return path.ToString();
        }
    }
}