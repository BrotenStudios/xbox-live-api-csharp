// Copyright (c) Microsoft Corporation
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// 

namespace Microsoft.Xbox.Services
{
    using global::System;

    public partial class XboxLiveAppConfiguration
    {
        public const string FileName = "XboxServices.config";

        private static XboxLiveAppConfiguration instance;

        public static XboxLiveAppConfiguration Instance
        {
            get
            {
                return instance ?? (instance = Load());
            }
        }

        private XboxLiveAppConfiguration()
        {
            this.AppSignInUISettings = new SignInUISettings();
        }

        public SignInUISettings AppSignInUISettings { get; set; }

        public string PublisherId { get; set; }

        public string PublisherDisplayName { get; set; }

        public string PackageIdentityName { get; set; }

        public string DisplayName { get; set; }

        public string AppId { get; set; }

        public string ProductFamilyName { get; set; }

        internal string EnvironmentPrefix { get; set; }

        internal bool UseFirstPartyToken { get; set; }

        public string ServiceConfigurationId { get; set; }

        public uint TitleId { get; set; }

        public string Sandbox { get; set; }

        public string Environment { get; set; }

        public static XboxLiveAppConfiguration Load()
        {
            try
            {
                // Attempt to load it from a file
                return Load(FileName);
            }
            catch (Exception e)
            {
                // If we're unable to load the file for some reason, we can just use an empty file
                // if mock data is enable.
                if (XboxLiveContext.UseMockServices || XboxLiveContext.UseMockHttp)
                {
                    return new XboxLiveAppConfiguration();
                }

                throw new XboxException(string.Format("Unable to find or load Xbox Live configuration.  Make sure a properly configured {0} exists.", FileName), e);
            }
        }
    }
}