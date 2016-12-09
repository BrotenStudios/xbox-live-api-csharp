using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Microsoft.Xbox.Services.Social
{
    public class XboxUserProfile
    {

        public string XboxUserId
        {
            get;
            private set;
        }

        public string Gamertag
        {
            get;
            private set;
        }

        public string Gamerscore
        {
            get;
            private set;
        }

        public Uri GameDisplayPictureResizeUri
        {
            get;
            private set;
        }

        public string GameDisplayName
        {
            get;
            private set;
        }

        public Uri ApplicationDisplayPictureResizeUri
        {
            get;
            private set;
        }

        public string ApplicationDisplayName
        {
            get;
            private set;
        }

    }
}
