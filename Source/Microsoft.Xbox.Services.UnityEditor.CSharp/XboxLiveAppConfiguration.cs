// Copyright (c) Microsoft Corporation
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// 
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