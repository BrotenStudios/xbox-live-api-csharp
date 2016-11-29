using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Microsoft.Xbox.Services.Multiplayer
{
    public class MultiplayerRoleInfo
    {
        public MultiplayerRoleInfo() {
        }

        public uint MaxMembersCount
        {
            get;
            set;
        }

        public uint TargetCount
        {
            get;
            set;
        }

        public uint MembersCount
        {
            get;
            private set;
        }

        public IList<string> MemberXboxUserIds
        {
            get;
            private set;
        }

    }
}
