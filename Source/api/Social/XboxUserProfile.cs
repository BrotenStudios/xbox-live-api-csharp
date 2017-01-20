using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Microsoft.Xbox.Services.Social
{
    [Serializable]
    public class XboxUserProfile
    {
        public string XboxUserId
        {
            get;
            internal set;
        }

        public string Gamertag
        {
            get;
            internal set;
        }

        public string Gamerscore
        {
            get;
            internal set;
        }

        public Uri GameDisplayPictureResizeUri
        {
            get;
            internal set;
        }

        public string GameDisplayName
        {
            get;
            internal set;
        }

        public Uri ApplicationDisplayPictureResizeUri
        {
            get;
            internal set;
        }

        public string ApplicationDisplayName
        {
            get;
            internal set;
        }
    }
}
