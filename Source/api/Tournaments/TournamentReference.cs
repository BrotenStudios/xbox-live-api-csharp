using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Microsoft.Xbox.Services.Tournaments
{
    public class TournamentReference
    {
        public TournamentReference() {
        }

        public string ServiceConfigurationId
        {
            get;
            private set;
        }

        public string Organizer
        {
            get;
            private set;
        }

        public string TournamentId
        {
            get;
            private set;
        }

        public string DefinitionName
        {
            get;
            private set;
        }

    }
}
