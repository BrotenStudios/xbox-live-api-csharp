using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Microsoft.Xbox.Services
{
    public class XboxLiveHttpCall
    {

        public string HttpMethod
        {
            get;
            private set;
        }

        public string PathQueryFragment
        {
            get;
            private set;
        }

        public string ServerName
        {
            get;
            private set;
        }

        public string ContractVersionHeaderValue
        {
            get;
            set;
        }

        public string ContentTypeHeaderValue
        {
            get;
            set;
        }

        public bool RetryAllowed
        {
            get;
            set;
        }


        public static XboxLiveHttpCall CreateXboxLiveHttpCall(XboxLiveContextSettings settings, string httpMethod, string serverName, string pathQueryFragment)
        {
            throw new NotImplementedException();
        }


        public Task<XboxLiveHttpCallResponse> GetResponseWithAuth(Microsoft.Xbox.Services.System.XboxLiveUser user, HttpCallResponseBodyType httpCallResponseBodyType)
        {
            throw new NotImplementedException();
        }

        public Task<XboxLiveHttpCallResponse> GetResponseWithoutAuth(HttpCallResponseBodyType httpCallResponseBodyType)
        {
            throw new NotImplementedException();
        }

        public void SetRequestBody(string value)
        {
            throw new NotImplementedException();
        }

        public void SetRequestBodyArray(byte[] requestBodyArray)
        {
            throw new NotImplementedException();
        }

        public void SetCustomHeader(string headerName, string headerValue)
        {
            throw new NotImplementedException();
        }

    }
}
