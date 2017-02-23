// Copyright (c) Microsoft Corporation
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// 
namespace Microsoft.Xbox.Services
{
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
            AppSignInUISettings = new SignInUISettings();
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
            return Load(FileName);
        }
    }
}