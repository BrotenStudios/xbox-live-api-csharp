// -----------------------------------------------------------------------
//  <copyright file="XboxLiveAppConfiguration.cs" company="Microsoft">
//      Copyright (c) Microsoft. All rights reserved.
//      Internal use only.
//  </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.Xbox.Services
{
    using global::System.Diagnostics;
    using global::System.IO;

    public partial class XboxLiveAppConfiguration
    {
        public static XboxLiveAppConfiguration Load(string path)
        {
            Windows.ApplicationModel.Package package = Windows.ApplicationModel.Package.Current;
            Windows.Storage.StorageFolder installedLocation = package.InstalledLocation;

            string fullPath = Path.Combine(installedLocation.Path, path);
            string content = File.ReadAllText(fullPath);
            Debug.WriteLine(content);

            Stream file;
            try
            {
                file = installedLocation.OpenStreamForReadAsync(path).Result;
            }
            catch (IOException e)
            {
                throw new XboxException("Unable to find configuration file " + path, e);
            }

            string json;
            using (StreamReader reader = new StreamReader(file))
            {
                json = reader.ReadToEnd();
            }

            if (string.IsNullOrWhiteSpace(json))
            {
                throw new XboxException("Unable to load Xbox Live app configuration from " + path);
            }

            return JsonSerialization.FromJson<XboxLiveAppConfiguration>(json);
        }
    }
}