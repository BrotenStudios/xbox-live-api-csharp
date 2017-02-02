using System;
using System.Collections.Generic;
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
                Version = statValuePostDocument.Version,
                Timestamp = DateTime.Now,
                Envelope = new Models.Envelope()
                {
                    ClientId = statValuePostDocument.ClientId,
                    ClientVersion = statValuePostDocument.ClientVersion,
                    ServerVersion = statValuePostDocument.ServerVersion
                },
                Stats = new Models.Stats()
                {
                    Title = new Dictionary<string, Models.Stat>(),
                    Tags = new object()
                }
            };

            foreach(var stat in statValuePostDocument.Stats)
            {
                svdModel.Stats.Title.Add(stat.Key, new Models.Stat()
                {
                    GlobalValue = stat.Value.Value,
                    Operation = "replace"
                });
            }

            req.RequestBody = JsonConvert.SerializeObject(svdModel, new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            });

            return req.GetResponseWithAuth(this.context.User, HttpCallResponseBodyType.JsonBody).ContinueWith(task =>
            {
                XboxLiveHttpResponse response = task.Result;
                if(response.ErrorCode == 0)
                {
                    ++statValuePostDocument.ClientVersion;
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
                    ClientId = svdModel.Envelope.ClientId,
                    ClientVersion = svdModel.Envelope.ClientVersion + 1,
                    ServerVersion = svdModel.Envelope.ServerVersion,
                    Version = svdModel.Version
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