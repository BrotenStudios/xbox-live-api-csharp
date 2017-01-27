// -----------------------------------------------------------------------
//  <copyright file="ProfileService.cs" company="Microsoft">
//      Copyright (c) Microsoft. All rights reserved.
//      Licensed under the MIT license. See LICENSE file in the project root for full license information.
//  </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.Xbox.Services.Social
{
    using global::System;
    using global::System.Collections.Generic;
    using global::System.Threading.Tasks;

    public class ProfileService
    {
        protected XboxLiveContextSettings settings;
        protected XboxLiveContext context;
        protected XboxLiveAppConfiguration config;

        internal ProfileService(XboxLiveAppConfiguration config, XboxLiveContext context, XboxLiveContextSettings settings)
        {
            this.config = config;
            this.context = context;
            this.settings = settings;
        }

        public Task<XboxUserProfile> GetUserProfileAsync(string xboxUserId)
        {
            if (string.IsNullOrEmpty(xboxUserId))
            {
                throw new ArgumentException("invalid xboxUserId", "xboxUserId");
            }

            List<string> profiles = new List<string> { xboxUserId };

            return this.GetUserProfilesAsync(profiles).ContinueWith(task => task.Result[0]);
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

            string endpoint = XboxLiveEndpoint.GetEndpointForService("profile", this.config);
            XboxLiveHttpRequest req = XboxLiveHttpRequest.Create(this.settings, "POST", endpoint, "/users/batch/profile/settings");

            req.ContractVersion = "2";
            req.ContentType = "application/json; charset=utf-8";
            Models.ProfileSettingsRequest reqBodyObject = new Models.ProfileSettingsRequest(xboxUserIds, true);
            req.RequestBody = JsonSerialization.ToJson(reqBodyObject);
            return req.GetResponseWithAuth(this.context.User, HttpCallResponseBodyType.JsonBody).ContinueWith(task =>
            {
                XboxLiveHttpResponse response = task.Result;
                Models.ProfileSettingsResponse responseBody = new Models.ProfileSettingsResponse();
                responseBody = JsonSerialization.FromJson<Models.ProfileSettingsResponse>(response.ResponseBodyString);

                List<XboxUserProfile> outputUsers = new List<XboxUserProfile>();
                foreach(Models.ProfileUser entry in responseBody.profileUsers)
                {
                    XboxUserProfile profile = new XboxUserProfile()
                    {
                        XboxUserId = entry.id,
                        Gamertag = entry.Gamertag(),
                        GameDisplayName = entry.GameDisplayName(),
                        GameDisplayPictureResizeUri = new Uri(entry.GameDisplayPic()),
                        ApplicationDisplayName = entry.AppDisplayName(),
                        ApplicationDisplayPictureResizeUri = new Uri(entry.AppDisplayPic()),
                        Gamerscore = entry.Gamerscore()
                    };

                    outputUsers.Add(profile);
                }

                return outputUsers;
            });
        }

        public Task<List<XboxUserProfile>> GetUserProfilesForSocialGroupAsync(string socialGroup)
        {
            throw new NotImplementedException();
        }
    }
}