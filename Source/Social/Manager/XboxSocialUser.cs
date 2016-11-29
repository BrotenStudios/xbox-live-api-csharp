using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Microsoft.Xbox.Services.Social.Manager
{
    public class XboxSocialUser
    {

        public PreferredColor PreferredColor
        {
            get;
            private set;
        }

        public TitleHistory TitleHistory
        {
            get;
            private set;
        }

        public SocialManagerPresenceRecord PresenceRecord
        {
            get;
            private set;
        }

        public string Gamerscore
        {
            get;
            private set;
        }

        public string Gamertag
        {
            get;
            private set;
        }

        public bool UseAvatar
        {
            get;
            private set;
        }

        public string DisplayPicUrlRaw
        {
            get;
            private set;
        }

        public string RealName
        {
            get;
            private set;
        }

        public string DisplayName
        {
            get;
            private set;
        }

        public bool IsFollowedByCaller
        {
            get;
            private set;
        }

        public bool IsFollowingUser
        {
            get;
            private set;
        }

        public bool IsFavorite
        {
            get;
            private set;
        }

        public string XboxUserId
        {
            get;
            private set;
        }

    }
}
