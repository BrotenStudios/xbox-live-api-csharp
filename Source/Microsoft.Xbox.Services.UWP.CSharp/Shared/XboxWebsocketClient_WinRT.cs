using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.Xbox.Services.System;
using System.Net.WebSockets;

namespace Microsoft.Xbox.Services.Shared
{
    internal class XboxWebsocketClient_WinRT : IXboxWebsocketClient
    {
        public event EventHandler<WebsocketCloseEventArgs> OnClose;
        public event EventHandler<WebsocketMessageReceivedEventArgs> OnMessageReceived;

        private bool disposed;
        private ClientWebSocket ws;

        public Task<object> Connect(
            XboxLiveUser xblUser,
            string uri,
            string subprotocol)
        {
            TaskCompletionSource<object> completionSource = new TaskCompletionSource<object>();
            xblUser.GetTokenAndSignatureAsync("GET", uri, null)
            .ContinueWith(taskResult =>
            {
                if (taskResult.Exception == null)
                {
                    var tokenAndSignature = taskResult.Result;
                    //this.ws = new ClientWebSocket();
                    //this.ws.Options.SetRequestHeader("Authorization", tokenAndSignature.Token);
                    //this.ws.Options.SetRequestHeader("Signature", tokenAndSignature.Signature);
                    //this.ws.Options.SetRequestHeader("Accept-Language", "en-us");
                    //this.ws.Options.AddSubProtocol(subprotocol);
                    //this.ws.ConnectAsync(new Uri(uri), CancellationToken.None).ContinueWith(connectResult =>
                    //{
                    //    if (this.ws.State != WebSocketState.Open)
                    //    {
                    //        // log
                    //    }

                    //    // Start receive thread 
                    //    ReceiveWorker();
                    //});
                }

                completionSource.SetResult(null);
            });

            return completionSource.Task;
        }

        public void Send(string message, Action<bool> onSendComplete)
        {
            try
            {
                byte[] bytes = Encoding.UTF8.GetBytes(message);

                this.ws.SendAsync(new ArraySegment<byte>(bytes), WebSocketMessageType.Text, true, CancellationToken.None);
            }
            catch (WebSocketException ex)
            {
                //log.Error("Failed to send websocket message", ex);
                //message.SentEvent.SetException(ex); throw?

                if (this.ws.State != WebSocketState.Open)
                {
                    OnClose(this, new WebsocketCloseEventArgs(CloseStateFromWebsocketError(ex.WebSocketErrorCode), ex.Message));
                }
            }
            catch (Exception ex)
            {
                //log.Error("Failed to send websocket message", ex);
                //message.SentEvent.SetException(ex);
            }
        }

        public void Close()
        {
            this.ws.CloseOutputAsync(WebSocketCloseStatus.NormalClosure, "client close", CancellationToken.None);
        }

        private void ReceiveWorker()
        {
            //log.Info("ReceiveWorker started");
            var buffer = new ArraySegment<Byte>(new Byte[64 * 1024]);

            Exception fault = null;

            while (this.ws.State == WebSocketState.Open && fault == null)
            {
                try
                {
                    string readedString = null;
                    bool fullMessage = false;

                    while (!fullMessage)
                    {
                        this.ws.ReceiveAsync(buffer, CancellationToken.None).ContinueWith(receiveResult =>
                        {
                            WebSocketReceiveResult result = receiveResult.Result;
                            if (result.MessageType == WebSocketMessageType.Close)
                            {
                                var message = string.Format(
                                    "Received close message, Status:{0}, Message:{1}",
                                    result.CloseStatus.GetValueOrDefault(WebSocketCloseStatus.NormalClosure),
                                    result.CloseStatusDescription);
                                //log.Info(message);
                                this.DisconnectInternalAsync(WebSocketCloseStatus.NormalClosure, message);
                                return;
                            }

                            if (result.MessageType != WebSocketMessageType.Text)
                            {
                                var message = string.Format("Received Null text message.");
                                //log.Error(message);
                                this.DisconnectInternalAsync(WebSocketCloseStatus.ProtocolError, message);
                                return;
                            }

                            var temp = new ArraySegment<byte>(buffer.Array, buffer.Offset, result.Count);

                            readedString += Encoding.UTF8.GetString(temp.Array, 0, temp.Count);
                            fullMessage = result.EndOfMessage;
                        });
                    }

                    //log.DebugFormat("Message received: {0}", readedString);

                    if (OnMessageReceived != null)
                    {
                        OnMessageReceived(this, new WebsocketMessageReceivedEventArgs(readedString));
                    }

                }
                catch (WebSocketException ex)
                {
                    //log.Error("WebSocketException caught in ReceiveWorker", ex);
                    fault = ex;

                    // If the socket is already aborting or closing, just to notify external event.
                    if (OnClose != null && this.ws.State != WebSocketState.Open)
                    {
                        this.OnClose(this, new WebsocketCloseEventArgs(CloseStateFromWebsocketError(ex.WebSocketErrorCode), ex.Message));
                    }
                }
                // ReceiveWorker is a fire-and-forget async running thread, even it throthis.ws exception, no caller will be catching it. 
                // We catch everything and try to close the socket.
                catch (Exception ex)
                {
                    //log.Error("General exception caught in ReceiveWorker", ex);
                    fault = ex;
                    this.DisconnectInternalAsync(WebSocketCloseStatus.ProtocolError, string.Format("ExRecMsg: {0}", ex.GetBaseException()));
                }
            }

            //log.Info("Exiting ReceiveWorker");
        }

        private Task DisconnectInternalAsync(WebSocketCloseStatus closeStatus, string closeMessage)
        {
            // Websockets supports a maximum of 123 bytes for close message description
            closeMessage = closeMessage.Length > 120 ? closeMessage.Substring(0, 120) : closeMessage;

            if (this.ws.State == WebSocketState.Open)
            {
                return this.ws.CloseOutputAsync(closeStatus, closeMessage, CancellationToken.None).ContinueWith(result =>
                {
                    if (OnClose != null)
                    {
                        OnClose(this, new WebsocketCloseEventArgs(closeStatus, closeMessage));
                    }
                });
            }

            return null;
        }

        private WebSocketCloseStatus CloseStateFromWebsocketError(WebSocketError error)
        {
            WebSocketCloseStatus status = WebSocketCloseStatus.Empty;
            switch (error)
            {
                case WebSocketError.InvalidMessageType:
                    status = WebSocketCloseStatus.InvalidMessageType;
                    break;
                case WebSocketError.UnsupportedProtocol:
                case WebSocketError.UnsupportedVersion:
                case WebSocketError.NotAWebSocket:
                case WebSocketError.HeaderError:
                    status = WebSocketCloseStatus.ProtocolError;
                    break;

                case WebSocketError.Faulted:
                case WebSocketError.ConnectionClosedPrematurely:
                case WebSocketError.NativeError:
                    status = WebSocketCloseStatus.InternalServerError;
                    break;
                default:
                    status = WebSocketCloseStatus.NormalClosure;
                    break;
            }

            return status;
        }

        public void Dispose()
        {
            if (this.disposed)
                return;
            
            this.ws.Dispose();
            this.disposed = true;
        }
    }

    class WebsocketMessageReceivedEventArgs : EventArgs
    {
        public WebsocketMessageReceivedEventArgs(string message)
        {
            _message = message;
        }

        string _message;

        public string Message
        {
            get { return _message; }
        }
    }

    class WebsocketCloseEventArgs : EventArgs
    {
        public WebsocketCloseEventArgs(global::System.Net.WebSockets.WebSocketCloseStatus closeStatus, string reason)
        {
            _closeStatus = closeStatus;
            _reason = reason;
        }

        string _reason;
        global::System.Net.WebSockets.WebSocketCloseStatus _closeStatus;

        public string Reason
        {
            get { return _reason; }
        }

        public global::System.Net.WebSockets.WebSocketCloseStatus CloseStatus
        {
            get { return _closeStatus; }
        }
    }
}
