using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Microsoft.Xbox.Services
{
    public enum HttpCallResponseBodyType : int
    {
        StringBody = 0,
        VectorBody = 1,
        JsonBody = 2,
    }

}
