////*********************************************************
////
//// Copyright (c) Microsoft. All rights reserved.
//// THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
//// ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
//// IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
//// PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
////
////*********************************************************
using Microsoft.Xbox.Services.System;
using System;
using System.Net;
using System.Net.Http;
using System.Net.WebSockets;
using System.Threading.Tasks;

namespace Microsoft.Xbox.Services.Shared
{
    /// <summary>
    /// Define the retry policy for xboxlive requests
    /// </summary>
    public class XboxLiveRetryPolicy
    {
        private readonly static TimeSpan defaultMaximumExecutionTime = new TimeSpan(0, 0, 30);
        private readonly static TimeSpan defaultRetryInterval = new TimeSpan(0, 0, 2);
        internal readonly static TimeSpan defaultMinimum500WaitTime = new TimeSpan(0, 0, 10);

        /// <summary>
        /// The interval between each retry
        /// </summary>
        public TimeSpan RetryInterval { set; get; }

        /// <summary>
        /// The maxiume retry window
        /// </summary>
        public TimeSpan MaximumExecutionTime { set; get; }

        public XboxLiveRetryPolicy()
        {
            this.RetryInterval = XboxLiveRetryPolicy.defaultRetryInterval;
            this.MaximumExecutionTime = XboxLiveRetryPolicy.defaultMaximumExecutionTime;
        }
    }

    internal class RetryChecker
    {
        public bool RetryEnabled { set; get; }

        internal TimeSpan LeftExecutionTime { set; get; }

        internal TimeSpan PolicyRetryInterval { set; get; }

        private readonly TimeSpan totalExecutionTime;

        private readonly DateTime startTime;

        private bool tokenRefreshedOn401 = false;

        public RetryChecker(XboxLiveRetryPolicy policy)
        {
            if (policy == null)
            {
                this.RetryEnabled = false;
            }
            else
            {
                this.RetryEnabled = true;
                this.LeftExecutionTime = policy.MaximumExecutionTime;
                this.totalExecutionTime = policy.MaximumExecutionTime;
                this.PolicyRetryInterval = policy.RetryInterval;
            }
            this.startTime = DateTime.UtcNow;
        }

        int ExtactHttpStatusCode(WebSocketException ex)
        {
            return 0;
        }

        // This is designed as a test hook to control how time elaspses, so in tests we don't need to 
        // wait exact physical time for the timeout.
        internal void TimeElapse(TimeSpan timeElapse)
        {
            // Use real time if no TimeElapse overwrite
            if (timeElapse == null || timeElapse == TimeSpan.Zero)
            {
                var timeElapsedSinceFirstCall = DateTime.UtcNow - this.startTime;
                this.LeftExecutionTime = this.totalExecutionTime - timeElapsedSinceFirstCall;
            }
            else
            {
                this.LeftExecutionTime -= timeElapse;
            }
        }

        public bool ShouldRetry(HttpResponseMessage response, XboxLiveUser user, out TimeSpan delayTime)
        {
            delayTime = TimeSpan.Zero;
            // Treat as a network error if response is null
            if (response == null)
            {
                return ShouldRetry(HttpStatusCode.OK, true, TimeSpan.Zero, user, out delayTime);
            }
            else if (!response.IsSuccessStatusCode)
            {
                // todo: fix shouldretry
                //return ShouldRetry(response.StatusCode, false, response.Headers.RetryAfter != null ? response.Headers.RetryAfter.Delta : TimeSpan.Zero, user, out delayTime);
            }

            return false;
        }

        public bool ShouldRetry(Exception ex, XboxLiveUser user, out TimeSpan delayTime)
        {
            delayTime = TimeSpan.Zero;
            // Treat as a network error if it is a HttpRequestException
            HttpRequestException httpEx = ex as HttpRequestException;
            WebSocketException websocketEx = ex as WebSocketException;
            if (httpEx != null)
            {
                WebException webEx = httpEx.InnerException as WebException;
                if (webEx != null)
                {
                    if (webEx.Status == WebExceptionStatus.SecureChannelFailure)
                    {
                        return false;
                    }
                }
                return ShouldRetry(HttpStatusCode.OK, true, TimeSpan.Zero, user, out delayTime);
            }
            else if (websocketEx != null)
            {
                var webEx = ex.InnerException as WebException;
                if (webEx != null)
                {
                    HttpWebResponse response = webEx.Response as HttpWebResponse;
                    // No response, we treat as a network error
                    if (response != null )
                    {
                        var retryAfterString = response.Headers["Retry-After"];
                        int retryAfterSeconds = Convert.ToInt32(retryAfterString);

                        return ShouldRetry(response.StatusCode, false, new TimeSpan(0, 0, retryAfterSeconds), user, out delayTime);
                    }
                }

                return ShouldRetry(HttpStatusCode.OK, true, TimeSpan.Zero, user, out delayTime);
            }

            return false;
        }

        public bool ShouldRetry(HttpStatusCode statusCode, bool isNetworkError, TimeSpan serviceOverwriteInterval, XboxLiveUser user, out TimeSpan delayTime)
        {
            delayTime = TimeSpan.Zero;
            
            // If everything is fine, don't retry
            if (!this.RetryEnabled || (statusCode == HttpStatusCode.OK && !isNetworkError))
            {
                return false;
            }

            if (this.LeftExecutionTime.Ticks <= 0)
            {
                return false;
            }

            // For 401, we perform a token refresh and retry right away
            if (statusCode == HttpStatusCode.Unauthorized)
            {
                if (user == null || this.tokenRefreshedOn401)
                {
                    return false;
                }
                else
                {
                    try
                    {
                        user.RefreshToken().Wait();
                    }
                    catch(Exception)
                    {
                        // Token refresh fail, don't retry
                        return false;
                    }
                    this.tokenRefreshedOn401 = true;
                    delayTime = TimeSpan.Zero;
                    return true;
                }
            }

            if (statusCode == HttpStatusCode.RequestTimeout ||
                statusCode == (HttpStatusCode)429 ||
                statusCode == HttpStatusCode.InternalServerError ||
                statusCode == HttpStatusCode.BadGateway ||
                statusCode == HttpStatusCode.ServiceUnavailable ||
                statusCode == HttpStatusCode.GatewayTimeout ||
                isNetworkError)
            {
                delayTime = this.PolicyRetryInterval;
                // The use the larger value from serviceOverwriteInterval and RetryInterval
                if (serviceOverwriteInterval != null && serviceOverwriteInterval != TimeSpan.Zero && serviceOverwriteInterval.Ticks > delayTime.Ticks)
                {
                    delayTime = serviceOverwriteInterval;
                }
                
                if (statusCode == HttpStatusCode.InternalServerError && XboxLiveRetryPolicy.defaultMinimum500WaitTime > delayTime)
                {
                    delayTime = XboxLiveRetryPolicy.defaultMinimum500WaitTime;
                }

                return true;
            }

            return false;
        }

        public bool ShouldRetry(int eventBatchResponseCode, TimeSpan serviceOverwriteInterval, out TimeSpan delayTime)
        {
            delayTime = TimeSpan.Zero;
            if (!this.RetryEnabled || eventBatchResponseCode == 200)
            {
                return false;
            }

            if (this.LeftExecutionTime.Ticks <= 0)
            {
                return false;
            }

            if (eventBatchResponseCode >= 500 && eventBatchResponseCode < 600)
            {
                delayTime = this.PolicyRetryInterval;
                return true;
            }
            else if (eventBatchResponseCode == 429 && serviceOverwriteInterval > this.PolicyRetryInterval)
            {
                delayTime = serviceOverwriteInterval;
                return true;
            }

            return false;
        }

    }
}
