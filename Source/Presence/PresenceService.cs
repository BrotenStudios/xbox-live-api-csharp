using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Microsoft.Xbox.Services.Presence
{
    public class PresenceService
    {

        public event EventHandler<TitlePresenceChangeEventArgs> TitlePresenceChanged;

        public event EventHandler<DevicePresenceChangeEventArgs> DevicePresenceChanged;


        public Task SetPresenceAsync(bool isUserActiveInTitle, PresenceData presenceData)
        {
            throw new NotImplementedException();
        }

        public Task SetPresenceAsync(bool isUserActiveInTitle)
        {
            throw new NotImplementedException();
        }

        public Task<PresenceRecord> GetPresenceAsync(string xboxUserId)
        {
            throw new NotImplementedException();
        }

        public Task<global::System.Collections.ObjectModel.ReadOnlyCollection<PresenceRecord>> GetPresenceForMultipleUsersAsync(string[] xboxUserIds, Microsoft.Xbox.Services.Presence.PresenceDeviceType[] deviceTypes, uint[] titleIds, PresenceDetailLevel detailLevel, bool onlineOnly, bool broadcastingOnly)
        {
            throw new NotImplementedException();
        }

        public Task<global::System.Collections.ObjectModel.ReadOnlyCollection<PresenceRecord>> GetPresenceForMultipleUsersAsync(string[] xboxUserIds)
        {
            throw new NotImplementedException();
        }

        public Task<global::System.Collections.ObjectModel.ReadOnlyCollection<PresenceRecord>> GetPresenceForSocialGroupAsync(string socialGroup, string socialGroupOwnerXboxuserId, Microsoft.Xbox.Services.Presence.PresenceDeviceType[] deviceTypes, uint[] titleIds, PresenceDetailLevel detailLevel, bool onlineOnly, bool broadcastingOnly)
        {
            throw new NotImplementedException();
        }

        public Task<global::System.Collections.ObjectModel.ReadOnlyCollection<PresenceRecord>> GetPresenceForSocialGroupAsync(string socialGroup)
        {
            throw new NotImplementedException();
        }

        public DevicePresenceChangeSubscription SubscribeToDevicePresenceChange(string xboxUserId)
        {
            throw new NotImplementedException();
        }

        public void UnsubscribeFromDevicePresenceChange(DevicePresenceChangeSubscription subscription)
        {
            throw new NotImplementedException();
        }

        public TitlePresenceChangeSubscription SubscribeToTitlePresenceChange(string xboxUserId, uint titleId)
        {
            throw new NotImplementedException();
        }

        public void UnsubscribeFromTitlePresenceChange(TitlePresenceChangeSubscription subscription)
        {
            throw new NotImplementedException();
        }

    }
}
