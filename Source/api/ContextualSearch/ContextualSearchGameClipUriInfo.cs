using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Microsoft.Xbox.Services.ContextualSearch
{
    public class ContextualSearchGameClipUriInfo
    {

        public DateTimeOffset Expiration
        {
            get;
            private set;
        }

        public ContextualSearchGameClipUriType UriType
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
