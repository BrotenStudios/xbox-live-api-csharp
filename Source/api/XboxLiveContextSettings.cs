// -----------------------------------------------------------------------
//  <copyright file="XboxLiveContextSettings.cs" company="Microsoft">
//      Copyright (c) Microsoft. All rights reserved.
//      Licensed under the MIT license. See LICENSE file in the project root for full license information.
//  </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.Xbox.Services
{
    using global::System;

    public class XboxLiveContextSettings
    {
        public XboxLiveContextSettings()
        {
            this.DiagnosticsTraceLevel = XboxServicesDiagnosticsTraceLevel.Off;
        }

        public bool UseCoreDispatcherForEventRouting { get; set; }

        public TimeSpan WebsocketTimeoutWindow { get; set; }

        public TimeSpan HttpTimeoutWindow { get; set; }

        public TimeSpan HttpRetryDelay { get; set; }

        public TimeSpan LongHttpTimeout { get; set; }

        public TimeSpan HttpTimeout { get; set; }

        public XboxServicesDiagnosticsTraceLevel DiagnosticsTraceLevel { get; private set; }

        public bool EnableServiceCallRoutedEvents { get; set; }

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