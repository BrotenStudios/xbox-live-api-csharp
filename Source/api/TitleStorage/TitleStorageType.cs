using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Microsoft.Xbox.Services.TitleStorage
{
    public enum TitleStorageType : int
    {
        TrustedPlatformStorage = 0,
        JsonStorage = 1,
        GlobalStorage = 2,
        SessionStorage = 3,
        UntrustedPlatformStorage = 4,
        Universal = 5,
    }

}
