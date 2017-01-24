// -----------------------------------------------------------------------
//  <copyright file="UserBuffer.cs" company="Microsoft">
//      Copyright (c) Microsoft. All rights reserved.
//      Licensed under the MIT license. See LICENSE file in the project root for full license information.
//  </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.Xbox.Services.Social.Manager
{
    using global::System;
    using global::System.Collections.Generic;
    using global::System.Linq;

    internal class UserBuffer
    {
        public UserBuffer()
        {
            this.EventQueue = new InternalEventQueue();
            this.SocialUserGraph = new Dictionary<ulong, XboxSocialUser>();
        }

        // TODO: This needs to be synchronized as well
        public Dictionary<ulong, XboxSocialUser> SocialUserGraph { get; private set; }
        public InternalEventQueue EventQueue { get; private set; }

        public XboxSocialUser TryAdd(XboxSocialUser user)
        {
            lock (((global::System.Collections.ICollection)this.SocialUserGraph).SyncRoot)
            {
                XboxSocialUser existingUser;
                if (this.SocialUserGraph.TryGetValue(user.XboxUserId, out existingUser)) return existingUser;

                this.SocialUserGraph[user.XboxUserId] = user;
                return null;
            }
        }

        public void Enqueue(InternalSocialEvent internalEvent)
        {
            this.EventQueue.Enqueue(internalEvent);
        }

        public bool Empty()
        {
            return this.EventQueue.Empty();
        }
    }

    internal class UserBuffersHolder
    {
        public UserBuffersHolder()
        {
            this.Active = new UserBuffer();
            this.Inactive = new UserBuffer();
        }

        public void SwapIfEmpty()
        {
            if (!this.Inactive.Empty()) return;

            UserBuffer swapBuffer = this.Active;
            this.Active = this.Inactive;
            this.Inactive = swapBuffer;
        }

        public UserBuffer Active { get; private set; }

        public UserBuffer Inactive { get; private set; }

        public void AddEvent(InternalSocialEvent internalSocialEvent)
        {
            this.Active.Enqueue(internalSocialEvent);
        }

        public void AddUsers(List<XboxSocialUser> users)
        {
            foreach (XboxSocialUser user in users)
            {
                this.Inactive.TryAdd(user);
            }
        }

        public void RemoveUsers(List<ulong> users)
        {
            foreach (ulong userId in users)
            {
                this.Inactive.SocialUserGraph.Remove(userId);
            }
        }
    }
}