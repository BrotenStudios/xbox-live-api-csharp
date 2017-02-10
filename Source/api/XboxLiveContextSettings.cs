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
#if WINDOWS_UWP
        private static Windows.UI.Core.CoreDispatcher s_coreDispatcher;
#endif

        public XboxLiveContextSettings()
        {
            this.DiagnosticsTraceLevel = XboxServicesDiagnosticsTraceLevel.Off;
        }

#if WINDOWS_UWP
        public static Windows.UI.Core.CoreDispatcher Dispatcher
        {
            get { return s_coreDispatcher; }
            internal set
            {
                if(s_coreDispatcher != null)
                {
                    s_coreDispatcher = value;
                }
                else
                {
                    try
                    {
                        var currentView = Windows.ApplicationModel.Core.CoreApplication.GetCurrentView();
                        if(currentView != null && currentView.CoreWindow != null)
                        {
                            s_coreDispatcher = currentView.CoreWindow.Dispatcher;
                        }
                    }
                    catch(Exception)
                    {
                    }
                }

                if(s_coreDispatcher != null)
                {
                    s_coreDispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, new Windows.UI.Core.DispatchedHandler(() =>
                    {
                        // todo: generate locales
                    }));
                }
            }
        }
#endif

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