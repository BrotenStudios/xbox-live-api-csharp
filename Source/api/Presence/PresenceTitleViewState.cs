using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Microsoft.Xbox.Services.Presence
{
    public enum PresenceTitleViewState : int
    {
        Unknown = 0,
        FullScreen = 1,
        Filled = 2,
        Snapped = 3,
        Background = 4,
    }

}
