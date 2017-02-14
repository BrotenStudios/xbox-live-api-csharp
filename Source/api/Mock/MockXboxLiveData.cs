// -----------------------------------------------------------------------
//  <copyright file="MockXboxLiveData.cs" company="Microsoft">
//      Copyright (c) Microsoft. All rights reserved.
//      Licensed under the MIT license. See LICENSE file in the project root for full license information.
//  </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.Xbox.Services
{
    using global::System;
    using global::System.Collections.Generic;
    using global::System.IO;

    using Newtonsoft.Json;

    public static class MockXboxLiveData
    {
        private static readonly Dictionary<XboxLiveHttpRequest, XboxLiveHttpResponse> mockResponses = new Dictionary<XboxLiveHttpRequest, XboxLiveHttpResponse>(new XboxLiveHttpRequestEqualityComparer());

        public static void Load(string path)
        {
            if (!File.Exists(path))
            {
                return;
            }

            string rawData = File.ReadAllText(path);
            List<MockRequestData> pairs = JsonConvert.DeserializeObject<List<MockRequestData>>(rawData);

            foreach (MockRequestData pair in pairs)
            {
                AddMockResponse(pair.Request, pair.Response);
            }
        }

        public static void AddMockResponse(XboxLiveHttpRequest request, XboxLiveHttpResponse response)
        {
            mockResponses[request] = response;
        }

        public static XboxLiveHttpResponse GetMockResponse(XboxLiveHttpRequest request)
        {
            XboxLiveHttpResponse response;
            if (!mockResponses.TryGetValue(request, out response))
            {
                Dictionary<string, string> headers = new Dictionary<string, string>
                {
                    { "X-XblCorrelationId", Guid.NewGuid().ToString() },
                    { "Date", DateTime.UtcNow.ToString("R") },
                };

                response = new MockXboxLiveHttpResponse(404, headers);
            }

            return response;
        }

        private class MockRequestData
        {
            public MockXboxLiveHttpRequest Request { get; set; }
            public MockXboxLiveHttpResponse Response { get; set; }
        }
    }
}