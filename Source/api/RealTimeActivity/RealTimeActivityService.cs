using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Microsoft.Xbox.Services.RealTimeActivity
{
    public class RealTimeActivityService
    {

        public event EventHandler<RealTimeActivityResyncEventArgs> RealTimeActivityResync;

        public event EventHandler<RealTimeActivitySubscriptionErrorEventArgs> RealTimeActivitySubscriptionError;

        public event EventHandler<RealTimeActivityConnectionStateChangeEventArgs> RealTimeActivityConnectionStateChange;


        public void Activate()
        {
            throw new NotImplementedException();
        }

        public void Deactivate()
        {
            throw new NotImplementedException();
        }

    }
}
