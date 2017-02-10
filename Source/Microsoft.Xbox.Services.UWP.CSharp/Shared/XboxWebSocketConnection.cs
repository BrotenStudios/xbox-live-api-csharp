using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xbox.Services.System;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using System.Threading;

namespace Microsoft.Xbox.Services.Shared
{
    public class XboxWebSocketConnection : IDisposable
    {
        public event EventHandler<WebsocketConnectionStateChangeArgs> OnConnectionStateChanged;

        private XboxLiveUser xboxLiveUser;
        private Uri uri;
        private string subprotocol;
        private XboxLiveServicesSettings serviceSettings;
        private volatile WebsocketConnectionState state = WebsocketConnectionState.Disconnected;
        private IXboxWebsocketClient client = XboxSystemFactory.GetSingletonInstance().CreateWebsocketClient();
        private bool closeCallbackSet = false;
        private bool closeCallbackRequested = false;
        private object stateLock = new object();
        private Task connectingTask = Task.FromResult(false);
        private Task sendingTask = Task.FromResult(false);
        private ConcurrentQueue<string> msgQueue = new ConcurrentQueue<string>();
        private XboxLiveRetryPolicy retryPolicy;
        private bool disposed = false;

        public XboxWebSocketConnection(
            XboxLiveUser user,
            Uri uri,
            string subprotocol,
            XboxLiveServicesSettings serviceSettings
            )
        {
            this.xboxLiveUser = user;
            this.uri = uri;
            this.subprotocol = subprotocol;
            this.serviceSettings = serviceSettings;
        }

        public Task EnsureConnected()
        {
            SetStateHelper(WebsocketConnectionState.Activated);
            lock(this.stateLock)
            {
                if(this.connectingTask.IsCompleted)
                {
                    SetStateHelper(WebsocketConnectionState.Connecting);
                    this.connectingTask = ConnectingTask();
                }
            }

            return this.connectingTask;
        }

        internal void EnsureSending()
        {
            lock (this.sendingTask)
            {
                if (this.sendingTask.Status == TaskStatus.RanToCompletion)
                {
                    this.sendingTask = SendingTask();
                }
            }
        }

        private Task SendingTask()
        {
            return Task.Run(() =>
            {
                //log.Info("Start sending task");
                string nextMsg = null;
                while (this.state == WebsocketConnectionState.Connected)
                {
                    if (!this.msgQueue.TryDequeue(out nextMsg))
                    {
                        //log.Info("Nothing in queue, stop sending");
                        break;
                    }
                    Action<bool> boolAct = null;
                    this.client.Send(nextMsg, boolAct);
                }

                //log.Info("Quit sending task");
            });
        }

        private Task ConnectingTask()
        {
            return Task.Run( () =>
            {
                bool shouldRetry = false;
                RetryChecker retryChecker = new RetryChecker(this.retryPolicy);
                TimeSpan delay = TimeSpan.Zero;
                do
                {
                    try
                    {
                        this.client.Connect(this.xboxLiveUser, this.uri.AbsoluteUri, this.subprotocol).Wait();
                        //log.Info("XboxLiveWebSocketClient connect succeeded");
                        this.state = WebsocketConnectionState.Connected;

                        EnsureSending();
                    }
                    catch (Exception ex)
                    {
                        //log.Error("XboxLiveWebSocketClient connect exception.", ex);
                        shouldRetry = retryChecker.ShouldRetry(ex, this.xboxLiveUser, out delay);
                        if (shouldRetry)
                        {
                            OnRetryInterval(retryChecker, delay);
                        }
                        else
                        {
                            this.state = WebsocketConnectionState.Disconnected;
                            //throw new XboxLiveException("Failed to make websocket connection with xbox live services", null, ex);
                        }
                    }
                }
                while (shouldRetry && this.state != WebsocketConnectionState.Connected);

            });
        }

        private Task OnRetryInterval(RetryChecker retryChecker, TimeSpan retryInterval)
        {
            var task = Task.Delay(retryInterval);

            // Using TimeSpan.Zero as parameter to use the real time
            retryChecker.TimeElapse(TimeSpan.Zero);

            return task;
        }

        private void SetStateHelper(WebsocketConnectionState newState)
        {
            WebsocketConnectionState oldState;
            lock(this.stateLock)
            {
                oldState = this.state;
                this.state = newState;
            }

            if(oldState != newState)
            {
                try
                {
                    if (OnConnectionStateChanged != null)
                    {
                        OnConnectionStateChanged(this, new WebsocketConnectionStateChangeArgs(oldState, newState));
                    }
                }
                catch(Exception)
                {
                    // log error
                }
            }
        }

        public void Dispose()
        {
            if (this.disposed)
                return;
            
            this.client.Dispose();

            this.disposed = true;
        }
    }

    public class WebsocketConnectionStateChangeArgs : EventArgs
    {
        internal WebsocketConnectionStateChangeArgs(WebsocketConnectionState oldState, WebsocketConnectionState newState)
        {
            OldState = oldState;
            NewState = newState;
        }

        public WebsocketConnectionState OldState;
        public WebsocketConnectionState NewState;
    }
}
