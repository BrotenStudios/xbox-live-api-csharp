using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Microsoft.Xbox.Services.System
{
    public enum SignInStatus : int
    {
        Success = 0,
        UserInteractionRequired = 1,
        UserCancel = 2,
    }

}
