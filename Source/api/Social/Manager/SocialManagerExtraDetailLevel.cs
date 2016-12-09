using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Microsoft.Xbox.Services.Social.Manager
{
    [Flags]
    public enum SocialManagerExtraDetailLevel : uint
    {
        NoExtraDetail = 0,
        TitleHistoryLevel = 1,
        PreferredColorLevel = 2,
    }

}
