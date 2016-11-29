using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Microsoft.Xbox.Services.GameServerPlatform
{
    public class GameServerMetadataResult
    {

        public IList<GameServerImageSet> GameServerImageSets
        {
            get;
            private set;
        }

        public IList<GameVariant> GameVariants
        {
            get;
            private set;
        }

    }
}
