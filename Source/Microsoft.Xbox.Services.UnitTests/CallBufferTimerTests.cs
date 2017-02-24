// Copyright (c) Microsoft Corporation
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Xbox.Services.UnitTests
{
    using global::System;
    using global::System.Threading;
    using global::System.Threading.Tasks;

    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Microsoft.Xbox.Services.Shared;

    [TestClass]
    public class CallBufferTimerTests
    {
        [TestMethod]
        public async Task BasicCallback()
        {
            TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();


            CallBufferTimer timer = new CallBufferTimer(TimeSpan.FromSeconds(1));
            timer.TimerCompleteEvent += (sender, o) => { tcs.SetResult(true); };

            timer.Fire();

            var result = await Task.WhenAny(tcs.Task, Task.Delay(TimeSpan.FromSeconds(2)));
            if (result != tcs.Task)
            {
                // This means the delay task completed.
                Assert.Fail("Timer was never called.");
            }

            Assert.IsTrue(tcs.Task.Result);
        }

        [TestMethod]
        public async Task ThrottledCallback()
        {
            TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();

            int count = 0;
            CallBufferTimer timer = new CallBufferTimer(TimeSpan.FromSeconds(1));
            timer.TimerCompleteEvent += (sender, o) =>
            {
                Interlocked.Increment(ref count);
                tcs.SetResult(true);
            };

            for (int i = 0; i < 10; i++)
            {
                timer.Fire();
            }

            var result = await Task.WhenAny(tcs.Task, Task.Delay(TimeSpan.FromSeconds(2)));
            if (result != tcs.Task)
            {
                // This means the delay task completed.
                Assert.Fail("Timer was never called.");
            }

            Assert.IsTrue(tcs.Task.Result);
            Assert.AreEqual(1, count);
        }

        [TestMethod]
        public async Task QueuedThrottledCallback()
        {
            TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();

            int count = 0;
            CallBufferTimer timer = new CallBufferTimer(TimeSpan.FromMilliseconds(500));
            timer.TimerCompleteEvent += (sender, o) =>
            {
                Interlocked.Increment(ref count);
                tcs.SetResult(true);
            };

            for (int i = 0; i < 10; i++)
            {
                timer.Fire();
            }

            // The timer should be called at least twice.
            await Task.Delay(1000);

            var result = await Task.WhenAny(tcs.Task, Task.Delay(TimeSpan.FromSeconds(2)));
            if (result != tcs.Task)
            {
                // This means the delay task completed.
                Assert.Fail("Timer was never called.");
            }

            Assert.IsTrue(tcs.Task.Result);
            Assert.IsTrue(count > 1);
        }
    }
}