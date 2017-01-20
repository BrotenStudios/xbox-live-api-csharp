using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Microsoft.Xbox.Services
{
    internal class XboxLiveEndpoint
    {
        internal static string GetEndpointForService(string serviceName, XboxLiveAppConfiguration config, string protocol = "https")
        {
            return String.Format("{0}://{1}{2}.xboxlive.com",  protocol, serviceName, config.Environment);
        }
    }
}
