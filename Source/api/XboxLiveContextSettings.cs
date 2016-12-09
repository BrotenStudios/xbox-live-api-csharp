using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Microsoft.Xbox.Services
{
    public class XboxLiveContextSettings
    {
        public XboxLiveContextSettings() {
        }

        public bool UseCoreDispatcherForEventRouting
        {
            get;
            set;
        }

        public TimeSpan WebsocketTimeoutWindow
        {
            get;
            set;
        }

        public TimeSpan HttpTimeoutWindow
        {
            get;
            set;
        }

        public TimeSpan HttpRetryDelay
        {
            get;
            set;
        }

        public TimeSpan LongHttpTimeout
        {
            get;
            set;
        }

        public TimeSpan HttpTimeout
        {
            get;
            set;
        }

        public XboxServicesDiagnosticsTraceLevel DiagnosticsTraceLevel
        {
            get;
            set;
        }

        public bool EnableServiceCallRoutedEvents
        {
            get;
            set;
        }


        public event EventHandler<XboxServiceCallRoutedEventArgs> ServiceCallRouted;


        public void DisableAssertsForXboxLiveThrottlingInDevSandboxes(XboxLiveContextThrottleSetting setting)
        {
            throw new NotImplementedException();
        }

        public void DisableAssertsForMaximumNumberOfWebsocketsActivated(XboxLiveContextRecommendedSetting setting)
        {
            throw new NotImplementedException();
        }

    }
}
