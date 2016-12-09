using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Microsoft.Xbox.Services.Achievements
{
    public class AchievementTimeWindow
    {

        public DateTimeOffset EndDate
        {
            get;
            private set;
        }

        public DateTimeOffset StartDate
        {
            get;
            private set;
        }

    }
}
