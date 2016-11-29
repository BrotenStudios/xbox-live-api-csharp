using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Microsoft.Xbox.Services
{
    public class XboxLiveAppConfiguration
    {

        public SignInUISettings AppSignInUISettings
        {
            get;
            private set;
        }

        public string Sandbox
        {
            get;
            private set;
        }

        public string Environment
        {
            get;
            private set;
        }

        public string ServiceConfigurationId
        {
            get;
            private set;
        }

        public uint TitleId
        {
            get;
            private set;
        }

        public static XboxLiveAppConfiguration SingletonInstance
        {
            get;
            private set;
        }

    }
}
