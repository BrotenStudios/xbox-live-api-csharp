using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Xbox.Services
{
    public class XboxLiveHttpRequest
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

        private string Url
        {
            get
            {
                return ServerName + PathQueryFragment;
            }
        }

        private string Headers
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                foreach (var header in m_CustomHeaders)
                {
                    sb.AppendFormat("{0}={1};", header.Key, header.Value);
                }

                return sb.ToString();
            }
        }

        private const string AuthorizationHeaderName = "Authorization";
        private const string SignatureHeaderName = "Signature";
        private const string ETagHeaderName = "ETag";
        private const string DateHeaderName = "Date";

        private XboxLiveContextSettings m_ContextSettings;
        private HttpWebRequest m_NetRequest;
        private Dictionary<string, string> m_CustomHeaders = new Dictionary<string, string>();

        private XboxLiveHttpRequest(XboxLiveContextSettings settings, string httpMethod, string serverName, string pathQueryFragment)
        {
            HttpMethod = httpMethod;
            ServerName = serverName;
            PathQueryFragment = pathQueryFragment;
            m_ContextSettings = settings;
#if !WINDOWS_UWP
            m_NetRequest = new HttpWebRequest(new Uri(Url));
#endif
        }

        public Task<XboxLiveHttpResponse> GetResponseWithAuth(System.XboxLiveUser user, HttpCallResponseBodyType httpCallResponseBodyType)
        {
            return user.GetTokenAndSignatureAsync(HttpMethod, Url, Headers).ContinueWith(
                (tokenTask) =>
                {
                    string token = "";
                    token = tokenTask.Result.Token;
#if !WINDOWS_UWP
                    m_NetRequest.Headers.Add(AuthorizationHeaderName, token);
#endif
                    return GetResponseWithoutAuth(httpCallResponseBodyType).Result;
                });
        }

        public Task<XboxLiveHttpResponse> GetResponseWithoutAuth(HttpCallResponseBodyType httpCallResponseBodyType)
        {
            return Task.Factory.FromAsync(m_NetRequest.BeginGetResponse, m_NetRequest.EndGetResponse, null)
                .ContinueWith((wrTask) =>
                {
                    return new XboxLiveHttpResponse((HttpWebResponse)wrTask.Result, httpCallResponseBodyType);
                });
        }

        public void SetRequestBody(string value)
        {
            // we have to use the async BeginGetRequestStream so that we can later use the async BeginGetResponse.  The sync
            // and async APIs can't be mixed
            var task = Task.Factory.FromAsync(m_NetRequest.BeginGetRequestStream, m_NetRequest.EndGetRequestStream, null)
                .ContinueWith((t) =>
                {
                    Stream body = t.Result;
                    StreamWriter sw = new StreamWriter(body);
                    sw.Write(value);
                });
            task.Wait();
        }

        public void SetRequestBody(byte[] value)
        {
            // we have to use the async BeginGetRequestStream so that we can later use the async BeginGetResponse.  The sync
            // and async APIs can't be mixed
            var task = Task.Factory.FromAsync(m_NetRequest.BeginGetRequestStream, m_NetRequest.EndGetRequestStream, null)
                .ContinueWith((t) =>
                {
                    Stream body = t.Result;
                    StreamWriter sw = new StreamWriter(body);
                    sw.Write(value);
                });
            task.Wait();
        }

        public void SetCustomHeader(string headerName, string headerValue)
        {
            if (m_CustomHeaders.ContainsKey(headerName))
            {
                m_CustomHeaders[headerName] = headerValue;
            }
            else
            {
                m_CustomHeaders.Add(headerName, headerValue);
            }
        }

        public static XboxLiveHttpRequest Create(XboxLiveContextSettings settings, string httpMethod, string serverName, string pathQueryFragment)
        {
            return new XboxLiveHttpRequest(settings, httpMethod, serverName, pathQueryFragment);
        }
    }
}
