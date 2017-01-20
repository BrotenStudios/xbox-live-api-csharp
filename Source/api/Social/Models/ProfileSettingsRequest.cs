using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Microsoft.Xbox.Services.Social.Models
{
    [Serializable]
    internal class ProfileSettingsRequest
    {
        public ProfileSettingsRequest(IEnumerable<string> xuidList, bool useDefaultSettings)
        {
            if (xuidList == null)
            {
                throw new ArgumentNullException("xuidList");
            }

            userIds = xuidList;

            InitializeSettings(useDefaultSettings);
        }

        public IEnumerable<string> userIds { get; set; }

        public List<string> settings { get; set; }

        private void InitializeSettings(bool useDefaultSettings)
        {
            settings = new List<string>();

            if (useDefaultSettings)
            {
                settings.Add("AppDisplayName");
                settings.Add("AppDisplayPicRaw");
                settings.Add("GameDisplayName");
                settings.Add("GameDisplayPicRaw");
                settings.Add("Gamerscore");
                settings.Add("Gamertag");
            }
        }
    }
}
