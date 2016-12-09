using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Microsoft.Xbox.Services.Multiplayer
{
    public enum NetworkAddressTranslationSetting : int
    {
        Unknown = 0,
        Open = 1,
        Moderate = 2,
        Strict = 3,
    }

}
