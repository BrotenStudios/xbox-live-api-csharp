using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Microsoft.Xbox.Services.ContextualSearch
{
    public class ContextualSearchGameClipThumbnail
    {

        public ContextualSearchGameClipThumbnailType ThumbnailType
        {
            get;
            private set;
        }

        public ulong FileSize
        {
            get;
            private set;
        }

        public Uri Url
        {
            get;
            private set;
        }

    }
}
