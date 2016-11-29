using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Microsoft.Xbox.Services.Social.Manager
{
    public class TitleHistory
    {

        public DateTimeOffset LastTimeUserPlayed
        {
            get;
            private set;
        }

        public bool HasUserPlayed
        {
            get;
            private set;
        }

    }
}
