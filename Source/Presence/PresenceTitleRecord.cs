using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Microsoft.Xbox.Services.Presence
{
    public class PresenceTitleRecord
    {

        public PresenceBroadcastRecord BroadcastRecord
        {
            get;
            private set;
        }

        public PresenceTitleViewState TitleViewState
        {
            get;
            private set;
        }

        public string Presence
        {
            get;
            private set;
        }

        public bool IsTitleActive
        {
            get;
            private set;
        }

        public DateTimeOffset LastModifiedDate
        {
            get;
            private set;
        }

        public string TitleName
        {
            get;
            private set;
        }

        public uint TitleId
        {
            get;
            private set;
        }

    }
}
