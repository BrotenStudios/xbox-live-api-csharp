// -----------------------------------------------------------------------
//  <copyright file="SocialManagerPresenceRecord.cs" company="Microsoft">
//      Copyright (c) Microsoft. All rights reserved.
//      Licensed under the MIT license. See LICENSE file in the project root for full license information.
//  </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.Xbox.Services.Social.Manager
{
    using global::System;
    using global::System.Collections.Generic;
    using global::System.Linq;

    using Microsoft.Xbox.Services.Presence;

    public class SocialManagerPresenceRecord : IEquatable<SocialManagerPresenceRecord>
    {
        public IList<SocialManagerPresenceTitleRecord> PresenceTitleRecords { get; private set; }

        public UserPresenceState UserState { get; private set; }

        public SocialManagerPresenceRecord(PresenceRecord presenceRecord)
        {
            this.PresenceTitleRecords = new List<SocialManagerPresenceTitleRecord>();
            foreach (PresenceDeviceRecord deviceRecord in presenceRecord.PresenceDeviceRecords)
            {
                foreach (PresenceTitleRecord titleRecord in deviceRecord.PresenceTitleRecords)
                {
                    this.PresenceTitleRecords.Add(new SocialManagerPresenceTitleRecord(deviceRecord.DeviceType, titleRecord));
                }
            }
        }

        public bool IsUserPlayingTitle(uint titleId)
        {
            return this.PresenceTitleRecords.Any(t => t.TitleId == titleId && t.IsTitleActive);
        }

        public void RemoveTitleRecord(uint titleId)
        {
            var titleRecord = this.PresenceTitleRecords.FirstOrDefault(r => r.TitleId == titleId);
            this.PresenceTitleRecords.Remove(titleRecord);
        }

        public bool Equals(SocialManagerPresenceRecord other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            if (this.UserState != other.UserState) return false;
            if (this.PresenceTitleRecords == null) return other.PresenceTitleRecords == null;
            return this.PresenceTitleRecords.All(record => other.PresenceTitleRecords.Contains(record));
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return this.Equals((SocialManagerPresenceRecord)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((this.PresenceTitleRecords != null ? this.PresenceTitleRecords.GetHashCode() : 0) * 397) ^ (int)this.UserState;
            }
        }
    }
}