// -----------------------------------------------------------------------
//  <copyright file="XboxLiveAppConfiguration.cs" company="Microsoft">
//      Copyright (c) Microsoft. All rights reserved.
//      Internal use only.
//  </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.Xbox.Services
{
    using global::System.IO;

    using Newtonsoft.Json;

    public class XboxLiveAppConfiguration
    {
        private const string appConfigurationPath = "XboxServices.config";

        private static XboxLiveAppConfiguration instance;

        public static XboxLiveAppConfiguration Instance
        {
            get
            {
                if (instance == null)
                {
                    //string configContent = File.ReadAllText(appConfigurationPath);
                    string configContent = @"
                    {
                      ""ServiceConfigurationId"" : ""00000000-0000-0000-0000-0000694f5acb"",
                      ""TitleId"": ""1766808267"",
                      ""Environment"": """",
                      ""Sandbox"": ""JDTDWX.0"",
                    }";
                    instance = JsonConvert.DeserializeObject<XboxLiveAppConfiguration>(configContent);
                }

                return instance;
            }
        }

        private XboxLiveAppConfiguration()
        {
        }

        public SignInUISettings AppSignInUISettings { get; set; }

        public string Sandbox { get; set; }

        public string Environment { get; set; }

        public string ServiceConfigurationId { get; set; }

        public uint TitleId { get; set; }
    }
}