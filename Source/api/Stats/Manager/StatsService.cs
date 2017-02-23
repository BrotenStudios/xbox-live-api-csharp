// Copyright (c) Microsoft Corporation
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Microsoft.Xbox.Services.Stats.Manager
{
    public class StatsService
    {
       private readonly XboxLiveContextSettings settings;
       private readonly XboxLiveContext context;
       private readonly XboxLiveAppConfiguration config;

        internal StatsService(XboxLiveContext context)
        {
            this.config = context.AppConfig;
            this.context = context;
            this.settings = context.Settings;
        }

        public Task UpdateStatsValueDocument(StatsValueDocument statValuePostDocument)
        {
            string endpoint = XboxLiveEndpoint.GetEndpointForService("statswrite", this.config);
            string pathAndQuery = PathAndQueryStatSubpath(
                this.context.User.XboxUserId,
                this.config.ServiceConfigurationId,
                false
                );

            XboxLiveHttpRequest req = XboxLiveHttpRequest.Create(this.settings, "POST", endpoint, pathAndQuery);
            var svdModel = new Models.StatsValueDocumentModel()
            {
                Revision = statValuePostDocument.Revision,
                Timestamp = DateTime.Now,
                Stats = new Models.Stats()
                {
                    Title = new Dictionary<string, Models.Stat>()
                }
            };

            svdModel.Stats.Title = statValuePostDocument.Stats.ToDictionary(
                stat => stat.Key,
                stat => new Models.Stat()
                {
                    Value = stat.Value.Value
                });

            req.RequestBody = JsonConvert.SerializeObject(svdModel, new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            });

            return req.GetResponseWithAuth(this.context.User, HttpCallResponseBodyType.JsonBody).ContinueWith(task =>
            {
                XboxLiveHttpResponse response = task.Result;
                if(response.ErrorCode == 0)
                {
                    ++statValuePostDocument.Revision;
                }
            });
        }

        public Task<StatsValueDocument> GetStatsValueDocument()
        {
            string endpoint = XboxLiveEndpoint.GetEndpointForService("statsread", this.config);
            string pathAndQuery = PathAndQueryStatSubpath(
                this.context.User.XboxUserId,
                this.config.ServiceConfigurationId,
                false
                );

            XboxLiveHttpRequest req = XboxLiveHttpRequest.Create(this.settings, "GET", endpoint, pathAndQuery);
            return req.GetResponseWithAuth(this.context.User, HttpCallResponseBodyType.JsonBody).ContinueWith(task =>
            {
                XboxLiveHttpResponse response = task.Result;
                var svdModel = JsonConvert.DeserializeObject<Models.StatsValueDocumentModel>(response.ResponseBodyJson);
                var svd = new StatsValueDocument(svdModel.Stats.Title)
                {
                    Revision = svdModel.Revision
                };
                return svd;
            });
        }

        private string PathAndQueryStatSubpath(string xuid, string scid, bool userXuidTag)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("/stats/users/");
            if (userXuidTag)
            {
                sb.AppendFormat("xuid({0})", xuid);
            }
            else
            {
                sb.Append(xuid);
            }

            sb.AppendFormat("/scids/{0}", scid);

            return sb.ToString();
        }
    }
}