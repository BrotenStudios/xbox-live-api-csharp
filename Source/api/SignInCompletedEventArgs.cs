using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Xbox.Services
{
    public class SignInCompletedEventArgs : EventArgs
    {
        public string XboxUserId { get; private set; }

        public SignInCompletedEventArgs(string xboxUserId)
        {
            XboxUserId = xboxUserId;
        }
    }
}
