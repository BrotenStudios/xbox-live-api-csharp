using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Microsoft.Xbox.Services.TitleStorage
{
    public enum TitleStorageETagMatchCondition : int
    {
        NotUsed = 0,
        IfMatch = 1,
        IfNotMatch = 2,
    }

}
