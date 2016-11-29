using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Microsoft.Xbox.Services
{
    public class XboxLiveHttpCallResponse
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

        public Byte[] ResponseBodyVector
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

    }
}
