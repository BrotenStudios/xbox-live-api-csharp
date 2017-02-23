// Copyright (c) Microsoft Corporation
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// 
using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Xbox.Services.System
{
    internal class AuthConfig
    {
        public string Sandbox { get; set; }
        public string XboxLiveRelyingParty { get; set; }
        public string EnvrionmentPrefix { get; set; }
        public string Envrionment { get; set; }
        public bool UseCompactTicket { get; set; }
        public string XboxLiveEndpoint { get; set; }
        public string RPSTicketService { get; set; }
        public string RPSTicketPolicy { get; set; }
        public string UserTokenSiteName { get; set; }
        public AuthConfig()
        {
            XboxLiveEndpoint = "https://xboxlive.com";
            XboxLiveRelyingParty = "https://auth.xboxlive.com";
            UserTokenSiteName = GetEndpointPath("user.auth", "", Envrionment, false);
            RPSTicketPolicy = UseCompactTicket ? "MBI_SSL" : "DELEGATION";
            RPSTicketService = UseCompactTicket ? UserTokenSiteName : "xboxlive.signin";
        }

        public static string GetEndpointPath(string serviceName, string envrionmentPrefix, string envrionment, bool appendProtocol = true)
        {
            string endpointPath = "";
            if(appendProtocol)
            {
                endpointPath += "https://";
            }
            endpointPath += envrionmentPrefix + serviceName + envrionment + ".xboxlive.com";
            return endpointPath;
        }
    }
}
