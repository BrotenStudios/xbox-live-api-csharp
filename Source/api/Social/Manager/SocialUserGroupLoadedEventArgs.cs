using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Microsoft.Xbox.Services.Social.Manager
{
    public class SocialUserGroupLoadedEventArgs : EventArgs
    {

        public XboxSocialUserGroup SocialUserGroup
        {
            get;
            private set;
        }

    }
}
