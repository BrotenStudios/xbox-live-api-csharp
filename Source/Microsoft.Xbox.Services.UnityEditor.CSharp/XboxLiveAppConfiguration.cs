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

    public partial class XboxLiveAppConfiguration
    {
        public static XboxLiveAppConfiguration Load(string path)
        {
            string json = File.ReadAllText(path);
            return JsonSerialization.FromJson<XboxLiveAppConfiguration>(json);
        }
    }
}