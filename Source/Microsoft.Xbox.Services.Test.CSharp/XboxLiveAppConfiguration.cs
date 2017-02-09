// -----------------------------------------------------------------------
//  <copyright file="XboxLiveAppConfiguration.cs" company="Microsoft">
//      Copyright (c) Microsoft. All rights reserved.
//      Internal use only.
//  </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.Xbox.Services
{
    using global::System;
    using global::System.IO;

    using Newtonsoft.Json;

    public partial class XboxLiveAppConfiguration
    {
        public static XboxLiveAppConfiguration Load(string path)
        {
            return new XboxLiveAppConfiguration
            {
                ServiceConfigurationId = "00000000-0000-0000-0000-0000694f5acb",
                TitleId = 1766808267,
                Environment = string.Empty,
                Sandbox = "JDTDWX.0",
            };
        }
    }
}