using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace Microsoft.Xbox.Services
{
    public class XboxLiveHttpResponse
    {
        public long RetryAfterInSeconds
        {
            get;
            private set;
        }

        public string ResponseDate
        {
            get;
            private set;
        }

        public string ETag
        {
            get;
            private set;
        }

        public string ErrorMessage
        {
            get;
            private set;
        }

        public int ErrorCode
        {
            get;
            private set;
        }

        public int HttpStatus
        {
            get;
            private set;
        }

        public Dictionary<string, string> Headers
        {
            get;
            private set;
        }

        public byte[] ResponseBodyVector
        {
            get;
            private set;
        }

        public string ResponseBodyJson
        {
            get;
            private set;
        }

        public string ResponseBodyString
        {
            get;
            private set;
        }

        public HttpCallResponseBodyType BodyType
        {
            get;
            private set;
        }

        public HttpWebResponse m_Response;

        internal XboxLiveHttpResponse(HttpWebResponse response, HttpCallResponseBodyType bodyType)
        {
            BodyType = bodyType;
            m_Response = response;
            Headers = new Dictionary<string, string>();

            using (Stream body = response.GetResponseStream())
            {
                int vectorSize = response.ContentLength > int.MaxValue ? int.MaxValue : (int)response.ContentLength;
                ResponseBodyVector = new byte[vectorSize];
                body.Read(ResponseBodyVector, 0, ResponseBodyVector.Length);
            }

            Encoding enc;

            switch(response.CharacterSet.ToLower())
            {
                case "utf-8":
                    enc = Encoding.UTF8;
                    break;
                case "ascii":
                    enc = Encoding.ASCII;
                    break;
                default:
                    enc = Encoding.UTF8;
                    break;
            }

            using (MemoryStream ms = new MemoryStream(ResponseBodyVector))
            {
                using (StreamReader sr = new StreamReader(ms, enc))
                {
                    ResponseBodyString = sr.ReadToEnd();
                    ResponseBodyJson = ResponseBodyString;
                }
            }

            for (int i = 0; i < response.Headers.Count; ++i)
            {
                Headers.Add(response.Headers.Keys[i], response.Headers[i]);
            }
        }
    }
}
