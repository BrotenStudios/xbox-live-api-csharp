using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Microsoft.Xbox.Services
{
    public class ServiceCallLoggingConfig
    {

        public static ServiceCallLoggingConfig SingletonInstance
        {
            get;
            private set;
        }


        public void Enable()
        {
            throw new NotImplementedException();
        }

        public void Disable()
        {
            throw new NotImplementedException();
        }

        public void RegisterForProtocolActivation()
        {
            throw new NotImplementedException();
        }

    }
}
