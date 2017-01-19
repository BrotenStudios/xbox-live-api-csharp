using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Microsoft.Xbox.Services.Social.Manager
{
    public class SocialEvent
    {

        public Microsoft.Xbox.Services.Social.Manager.SocialEventArgs EventArgs
        {
            get;
            private set;
        }

        public string ErrorMessage
        {
            get;
            private set;
        }

        public int ErrorCode
        {
            get;
            private set;
        }

        public IList<string> UsersAffected
        {
            get;
            private set;
        }

        public SocialEventType EventType
        {
            get;
            private set;
        }

        public Microsoft.Xbox.Services.System.XboxLiveUser User
        {
            get;
            private set;
        }

    }
}
