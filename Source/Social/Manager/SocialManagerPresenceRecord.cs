using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Microsoft.Xbox.Services.Social.Manager
{
    public class SocialManagerPresenceRecord
    {

        public IList<SocialManagerPresenceTitleRecord> PresenceTitleRecords
        {
            get;
            private set;
        }

        public Microsoft.Xbox.Services.Presence.UserPresenceState UserState
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
