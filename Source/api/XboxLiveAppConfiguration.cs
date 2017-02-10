// -----------------------------------------------------------------------
//  <copyright file="XboxLiveAppConfiguration.cs" company="Microsoft">
//      Copyright (c) Microsoft. All rights reserved.
//      Internal use only.
//  </copyright>
// -----------------------------------------------------------------------

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

        public string Sandbox { get; set; }

        public string Environment { get; set; }

        internal string EnvironmentPrefix { get; set; }
        internal bool UseFirstPartyToken { get; set; }
        public string ServiceConfigurationId { get; set; }

        public uint TitleId { get; set; }

        public string ProductFamilyName { get; set; }

        public string AppId { get; set; }

        public static XboxLiveAppConfiguration Load()
        {
            return Load(FileName);
        }
    }
}