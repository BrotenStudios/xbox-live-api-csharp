using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Microsoft.Xbox.Services.Multiplayer.Manager
{
    public class JoinLobbyCompletedEventArgs : EventArgs
    {

        public string InvitedXboxUserId
        {
            get;
            private set;
        }

    }
}
