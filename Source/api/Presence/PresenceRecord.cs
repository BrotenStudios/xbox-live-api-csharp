using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Microsoft.Xbox.Services.Presence
{
    public class PresenceRecord
    {

        public IList<PresenceDeviceRecord> PresenceDeviceRecords
        {
            get;
            private set;
        }

        public UserPresenceState UserState
        {
            get;
            private set;
        }

        public string XboxUserId
        {
            get;
            private set;
        }


        public bool IsUserPlayingTitle(uint titleId)
        {
            throw new NotImplementedException();
        }

    }
}
