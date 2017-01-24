// -----------------------------------------------------------------------
//  <copyright file="XboxSocialUser.cs" company="Microsoft">
//      Copyright (c) Microsoft. All rights reserved.
//      Licensed under the MIT license. See LICENSE file in the project root for full license information.
//  </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.Xbox.Services.Social.Manager
{
    using global::System;

    public class XboxSocialUser
    {
        public PreferredColor PreferredColor { get; internal set; }

        public TitleHistory TitleHistory { get; internal set; }

        public SocialManagerPresenceRecord PresenceRecord { get; internal set; }

        public string Gamerscore { get; internal set; }

        public string Gamertag { get; internal set; }

        public bool UseAvatar { get; internal set; }

        public string DisplayPicUrlRaw { get; internal set; }

        public string RealName { get; internal set; }

        public string DisplayName { get; internal set; }

        public bool IsFollowedByCaller { get; internal set; }

        public bool IsFollowingUser { get; internal set; }

        public bool IsFavorite { get; internal set; }

        public ulong XboxUserId { get; internal set; }

        internal ChangeListType GetChanges(XboxSocialUser other)
        {
            ChangeListType changeType = ChangeListType.NoChange;

            if (!string.Equals(this.Gamertag, other.Gamertag, StringComparison.OrdinalIgnoreCase)
                || !string.Equals(this.Gamerscore, other.Gamerscore, StringComparison.OrdinalIgnoreCase)
                || !string.Equals(this.RealName, other.RealName, StringComparison.OrdinalIgnoreCase)
                || !string.Equals(this.DisplayName, other.DisplayName, StringComparison.OrdinalIgnoreCase)
                || !string.Equals(this.DisplayPicUrlRaw, other.DisplayPicUrlRaw, StringComparison.OrdinalIgnoreCase)
                || this.UseAvatar != other.UseAvatar
                || !this.PreferredColor.Equals(other.PreferredColor)
                || !this.TitleHistory.Equals(other.TitleHistory))
            {
                changeType |= ChangeListType.ProfileChange;
            }

            if (this.IsFollowedByCaller != other.IsFollowedByCaller ||
                this.IsFollowingUser != other.IsFollowingUser ||
                this.IsFavorite != other.IsFavorite)
            {
                changeType |= ChangeListType.SocialRelationshipChange;
            }

            if (!this.PresenceRecord.Equals(other.PresenceRecord))
            {
                changeType |= ChangeListType.PresenceChange;
            }

            return changeType;
        }
    }
}