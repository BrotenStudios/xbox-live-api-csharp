// -----------------------------------------------------------------------
//  <copyright file="MockXboxLiveHttpResponse.cs" company="Microsoft">
//      Copyright (c) Microsoft. All rights reserved.
//      Licensed under the MIT license. See LICENSE file in the project root for full license information.
//  </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.Xbox.Services
{
    using global::System.Collections.Generic;
    using global::System.IO;
    using global::System.Net;
    using global::System.Text;

    using Newtonsoft.Json;

    public class MockXboxLiveHttpResponse : XboxLiveHttpResponse
    {
        public MockXboxLiveHttpResponse(int httpStatus) : this(httpStatus, null, HttpCallResponseBodyType.StringBody, null, new Dictionary<string, string>())
        {
        }

        public MockXboxLiveHttpResponse(int httpStatus, Dictionary<string, string> headers) : this(httpStatus, null, HttpCallResponseBodyType.StringBody, null, headers)
        {
        }

        [JsonConstructor]
        public MockXboxLiveHttpResponse(int httpStatus, string body, HttpCallResponseBodyType bodyType, string characterSet, Dictionary<string, string> headers)
        {
            byte[] bodyBytes = Encoding.UTF8.GetBytes(body ?? "");
            Stream bodyStream = new MemoryStream(bodyBytes);

            WebHeaderCollection webHeaders = new WebHeaderCollection();
            if (headers != null)
            {
                foreach (KeyValuePair<string, string> header in headers)
                {
                    webHeaders[header.Key] = header.Value;
                }
            }

            this.Initialize(httpStatus, bodyStream, bodyType, bodyStream.Length, characterSet, webHeaders);
        }
    }
}