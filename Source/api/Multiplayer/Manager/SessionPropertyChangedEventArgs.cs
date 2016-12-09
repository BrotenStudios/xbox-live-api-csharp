using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Microsoft.Xbox.Services.Multiplayer.Manager
{
    public class SessionPropertyChangedEventArgs : EventArgs
    {

        public string Properties
        {
            get;
            private set;
        }

    }
}
