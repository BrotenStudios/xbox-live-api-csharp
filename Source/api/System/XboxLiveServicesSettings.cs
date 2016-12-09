using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Microsoft.Xbox.Services.System
{
    public class XboxLiveServicesSettings
    {

        public Microsoft.Xbox.Services.XboxServicesDiagnosticsTraceLevel DiagnosticsTraceLevel
        {
            get;
            set;
        }

        public static XboxLiveServicesSettings SingletonInstance
        {
            get;
            private set;
        }


        public event EventHandler<Microsoft.Xbox.Services.XboxLiveLogCallEventArgs> LogCallRouted;

    }
}
