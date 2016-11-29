using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Microsoft.Xbox.Services.TitleStorage
{
    public class TitleStorageQuota
    {

        public ulong QuotaBytes
        {
            get;
            private set;
        }

        public ulong UsedBytes
        {
            get;
            private set;
        }

        public string MultiplayerSessionName
        {
            get;
            private set;
        }

        public string MultiplayerSessionTemplateName
        {
            get;
            private set;
        }

        public string XboxUserId
        {
            get;
            private set;
        }

        public TitleStorageType StorageType
        {
            get;
            private set;
        }

        public string ServiceConfigurationId
        {
            get;
            private set;
        }

    }
}
