using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Microsoft.Xbox.Services.Presence
{
    public class TitlePresenceChangeEventArgs : EventArgs
    {

        public TitlePresenceState TitleState
        {
            get;
            private set;
        }

        public uint TitleId
        {
            get;
            private set;
        }

        public string XboxUserId
        {
            get;
            private set;
        }

    }
}
