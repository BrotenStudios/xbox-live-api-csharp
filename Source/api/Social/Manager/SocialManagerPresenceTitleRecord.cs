// -----------------------------------------------------------------------
//  <copyright file="SocialManagerPresenceTitleRecord.cs" company="Microsoft">
//      Copyright (c) Microsoft. All rights reserved.
//      Licensed under the MIT license. See LICENSE file in the project root for full license information.
//  </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.Xbox.Services.Social.Manager
{
    using global::System;

    using Microsoft.Xbox.Services.Presence;

    public class SocialManagerPresenceTitleRecord : IEquatable<SocialManagerPresenceTitleRecord>
    {
        public SocialManagerPresenceTitleRecord(PresenceDeviceType deviceType, PresenceTitleRecord titleRecord)
        {
            this.DeviceType = deviceType;
            this.TitleId = titleRecord.TitleId;
            this.IsBroadcasting = titleRecord.BroadcastRecord.StartTime != DateTimeOffset.MinValue;
            this.IsTitleActive = titleRecord.IsTitleActive;
            this.Presence = titleRecord.Presence;
        }

        public PresenceDeviceType DeviceType { get; private set; }

        public uint TitleId { get; private set; }

        public bool IsBroadcasting { get; private set; }

        public bool IsTitleActive { get; private set; }

        public string Presence { get; private set; }

        public bool Equals(SocialManagerPresenceTitleRecord other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return this.TitleId == other.TitleId 
                && this.IsBroadcasting == other.IsBroadcasting
                && this.IsTitleActive == other.IsTitleActive
                && string.Equals(this.Presence, other.Presence);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return this.Equals((SocialManagerPresenceTitleRecord)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (int)this.TitleId;
                hashCode = (hashCode * 397) ^ this.IsBroadcasting.GetHashCode();
                hashCode = (hashCode * 397) ^ this.IsTitleActive.GetHashCode();
                hashCode = (hashCode * 397) ^ (this.Presence != null ? this.Presence.GetHashCode() : 0);
                return hashCode;
            }
        }
    }
}