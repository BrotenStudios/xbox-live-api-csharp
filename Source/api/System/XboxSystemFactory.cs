using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xbox.Services;
using Microsoft.Xbox.Services.Shared;

namespace Microsoft.Xbox.Services.System
{
    class XboxSystemFactory
    {
        private static XboxSystemFactory factory;
        public static XboxSystemFactory GetSingletonInstance()
        {
            if(factory == null)
            {
                factory = new XboxSystemFactory();
            }

            return factory;
        }

        private XboxSystemFactory()
        {
        }

        public IXboxWebsocketClient CreateWebsocketClient()
        {
#if WINDOWS_UWP
            return new XboxWebsocketClient_WinRT();
#else
            return null;
#endif
        }
    }
}
