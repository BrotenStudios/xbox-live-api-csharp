using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Microsoft.Xbox.Services.TitleStorage
{
    public enum TitleStorageBlobType : int
    {
        Unknown = 0,
        Binary = 1,
        Json = 2,
        Config = 3,
    }

}
