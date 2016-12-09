using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Microsoft.Xbox.Services.ContextualSearch
{
    public class ContextualSearchGameClip
    {

        public IList<ContextualSearchGameClipStat> Stats
        {
            get;
            private set;
        }

        public string XboxUserId
        {
            get;
            private set;
        }

        public ulong Views
        {
            get;
            private set;
        }

        public ContextualSearchGameClipType GameClipType
        {
            get;
            private set;
        }

        public IList<ContextualSearchGameClipThumbnail> Thumbnails
        {
            get;
            private set;
        }

        public IList<ContextualSearchGameClipUriInfo> GameClipUris
        {
            get;
            private set;
        }

        public string GameClipLocale
        {
            get;
            private set;
        }

        public string GameClipId
        {
            get;
            private set;
        }

        public ulong DurationInSeconds
        {
            get;
            private set;
        }

        public string ClipName
        {
            get;
            private set;
        }

    }
}
