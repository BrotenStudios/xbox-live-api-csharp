// Copyright (c) Microsoft Corporation
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// 
namespace Microsoft.Xbox.Services.Social.Manager
{
    using global::System;
    using global::System.Collections.Generic;
    using global::System.Linq;
    using global::System.Threading.Tasks;

    using Microsoft.Xbox.Services.Presence;
    using Microsoft.Xbox.Services.System;

    public class MockSocialManager : ISocialManager
    {
        private static Random rng = new Random();

        internal MockSocialManager()
        {
            this.LocalUsers = new List<XboxLiveUser>();
        }

        public IList<XboxLiveUser> LocalUsers { get; private set; }

        public Task AddLocalUser(XboxLiveUser user, SocialManagerExtraDetailLevel extraDetailLevel)
        {
            this.LocalUsers.Add(user);
            return Task.FromResult(true);
        }

        public void RemoveLocalUser(XboxLiveUser user)
        {
            this.LocalUsers.Remove(user);
        }

        public XboxSocialUserGroup CreateSocialUserGroupFromList(XboxLiveUser user, List<ulong> userIds)
        {
            var group = new XboxSocialUserGroup(user, userIds);

            // Create 'real' users for the userIds
            var users = userIds
                .Select(CreateUser)
                .ToDictionary(u => u.XboxUserId);
            group.UpdateView(users, null);

            return group;
        }

        public XboxSocialUserGroup CreateSocialUserGroupFromFilters(XboxLiveUser user, PresenceFilter presenceFilter, RelationshipFilter relationshipFilter, uint titleId)
        {
            var group = new XboxSocialUserGroup(user, presenceFilter, relationshipFilter, titleId);

            var users = Enumerable.Range(0, 5)
                .Select(id =>
                {
                    var groupUser = CreateUser(0);

                    switch (presenceFilter)
                    {
                        case PresenceFilter.TitleOnline:
                            groupUser.PresenceState = UserPresenceState.Online;
                            groupUser.PresenceDetails = new List<SocialManagerPresenceTitleRecord>
                            {
                                new SocialManagerPresenceTitleRecord
                                {
                                    TitleId = titleId,
                                    IsTitleActive = false,
                                }
                            };
                            break;
                        case PresenceFilter.TitleOffline:
                            groupUser.PresenceState = UserPresenceState.Offline;
                            groupUser.TitleHistory = new TitleHistory
                            {
                                HasUserPlayed = true,
                                LastTimeUserPlayed = DateTime.UtcNow.AddDays(-1),
                            };
                            break;
                        case PresenceFilter.AllOnline:
                            groupUser.PresenceState = UserPresenceState.Online;
                            break;
                        case PresenceFilter.AllOffline:
                            groupUser.PresenceState = UserPresenceState.Offline;
                            break;
                        case PresenceFilter.AllTitle:
                            break;
                    }

                    switch (relationshipFilter)
                    {
                        case RelationshipFilter.Friends:
                            groupUser.IsFollowedByCaller = true;
                            break;
                        case RelationshipFilter.Favorite:
                            groupUser.IsFollowedByCaller = true;
                            groupUser.IsFavorite = true;
                            break;
                    }

                    return groupUser;
                }).ToDictionary(u => u.XboxUserId);
            group.UpdateView(users, new List<SocialEvent>());

            return group;
        }

        public IList<SocialEvent> DoWork()
        {
            return new List<SocialEvent>();
        }

        private static XboxSocialUser CreateUser(ulong id = 0)
        {
            if (id == 0)
            {
                id = (ulong)(rng.NextDouble() * ulong.MaxValue);
            }

            return new XboxSocialUser
            {
                XboxUserId = id,
                Gamertag = "Gamer" + id,
                DisplayName = "User " + id,
                RealName = "Real User " + id,
                Gamerscore = id.ToString(),
                DisplayPicRaw = "http://images-eds.xboxlive.com/image?url=z951ykn43p4FqWbbFvR2Ec.8vbDhj8G2Xe7JngaTToBrrCmIEEXHC9UNrdJ6P7KI4AAOijCgOA3.jozKovAH98vieJP1ResWJCw2dp82QtambLRqzQbSIiqrCug0AvP4&format=png",
                PreferredColor = new PreferredColor
                {
                    PrimaryColor = "1081ca",
                    SecondaryColor = "10314f",
                    TertiaryColor = "105080"
                },
            };
        }
    }
}