using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Microsoft.Xbox.Services.Presence
{
    public class PresenceMediaRecord
    {

        public string Name
        {
            get;
            private set;
        }

        public PresenceMediaIdType MediaIdType
        {
            get;
            private set;
        }

        public string MediaId
        {
            get;
            private set;
        }

    }
}
