using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Microsoft.Xbox.Services.System
{
    public class SignInResult
    {
        public SignInResult(SignInStatus status)
        {
            Status = status;
        }

        public SignInStatus Status
        {
            get;
            internal set;
        }

    }
}
