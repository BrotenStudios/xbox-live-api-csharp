using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Microsoft.Xbox.Services.System
{
    public class SignOutCompletedEventArgs : EventArgs
    {

        public XboxLiveUser User
        {
            get;
            private set;
        }

    }
}
