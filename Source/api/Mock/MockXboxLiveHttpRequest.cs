// -----------------------------------------------------------------------
//  <copyright file="MockXboxLiveHttpRequest.cs" company="Microsoft">
//      Copyright (c) Microsoft. All rights reserved.
//      Internal use only.
//  </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.Xbox.Services
{
    using global::System.IO;
    using global::System.Threading.Tasks;

    using Newtonsoft.Json;

    public class MockXboxLiveHttpRequest : XboxLiveHttpRequest
    {
        public static string MockDataPath;

        public MockXboxLiveHttpRequest(XboxLiveContextSettings settings, string method, string serverName, string pathQueryFragment) : base(settings, method, serverName, pathQueryFragment)
        {
        }

        public override Task<XboxLiveHttpResponse> GetResponseWithoutAuth(HttpCallResponseBodyType httpCallResponseBodyType)
        {
            // Save the mock data out for testing.
            string requestData = JsonConvert.SerializeObject(this, Formatting.Indented);

            string outputDir = @"C:\Temp\MockData";
            Directory.CreateDirectory(outputDir);
            string outputPath = Path.Combine(outputDir, "data.txt");
            File.AppendAllText(outputPath, requestData);

            return Task.FromResult(MockXboxLiveData.GetMockResponse(this));
        }
    }
}