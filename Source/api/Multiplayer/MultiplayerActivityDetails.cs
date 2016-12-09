using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Microsoft.Xbox.Services.Multiplayer
{
    public class MultiplayerActivityDetails
    {

        public uint MembersCount
        {
            get;
            private set;
        }

        public uint MaxMembersCount
        {
            get;
            private set;
        }

        public string OwnerXboxUserId
        {
            get;
            private set;
        }

        public bool Closed
        {
            get;
            private set;
        }

        public MultiplayerSessionRestriction JoinRestriction
        {
            get;
            private set;
        }

        public MultiplayerSessionVisibility Visibility
        {
            get;
            private set;
        }

        public uint TitleId
        {
            get;
            private set;
        }

        public string HandleId
        {
            get;
            private set;
        }

        public MultiplayerSessionReference SessionReference
        {
            get;
            private set;
        }

    }
}
