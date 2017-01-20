using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Microsoft.Xbox.Services.Social
{
    public class ProfileService
    {
        internal ProfileService(XboxLiveAppConfiguration config, XboxLiveContext context, XboxLiveContextSettings settings)
        {
            m_Config = config;
            m_Context = context;
            m_Settings = settings;
        }

        public Task<XboxUserProfile> GetUserProfileAsync(string xboxUserId)
        {
            if (string.IsNullOrEmpty(xboxUserId))
            {
                throw new ArgumentException("invalid xboxUserId", "xboxUserId");
            }

            List<string> profiles = new List<string>();
            profiles.Add(xboxUserId);

            return GetUserProfilesAsync(profiles).ContinueWith((task) =>
            {
                return task.Result[0];
            });
        }

        public Task<List<XboxUserProfile>> GetUserProfilesAsync(List<string> xboxUserIds)
        {
            if (xboxUserIds == null)
            {
                throw new ArgumentNullException("xboxUserIds");
            }
            if (xboxUserIds.Count == 0)
            {
                throw new ArgumentOutOfRangeException("xboxUserIds", "Empty list of user ids");
            }

            string endpoint = XboxLiveEndpoint.GetEndpointForService("profile", m_Config);
            XboxLiveHttpRequest req = XboxLiveHttpRequest.Create(m_Settings, "POST", endpoint, "/users/batch/profile/settings");

            req.ContractVersionHeaderValue = "2";
            Models.ProfileSettingsRequest reqBodyObject = new Models.ProfileSettingsRequest(xboxUserIds, true);
            string reqBodyString = JsonSerialization.ToJson(reqBodyObject);

            req.SetRequestBody(reqBodyString);

            return req.GetResponseWithAuth(m_Context.User, HttpCallResponseBodyType.JsonBody).ContinueWith<List<XboxUserProfile>>((task) =>
            {
                XboxLiveHttpResponse response = task.Result;
                Models.ProfileSettingsResponse responseBody = JsonSerialization.FromJson<Models.ProfileSettingsResponse>(response.ResponseBodyString);

                return responseBody.profileUsers;
            });
        }

        public Task<List<XboxUserProfile>> GetUserProfilesForSocialGroupAsync(string socialGroup)
        {
            throw new NotImplementedException();
        }

        protected XboxLiveContextSettings m_Settings;
        protected XboxLiveContext m_Context;
        protected XboxLiveAppConfiguration m_Config;
    }
}
