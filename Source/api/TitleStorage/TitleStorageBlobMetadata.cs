// Copyright (c) Microsoft Corporation
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// 
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Microsoft.Xbox.Services.TitleStorage
{
    public class TitleStorageBlobMetadata
    {
        public TitleStorageBlobMetadata(string serviceConfigurationId, TitleStorageType storageType, string blobPath, TitleStorageBlobType blobType, string xboxUserId, string displayName, string eTag, DateTimeOffset clientTimestamp) {
            var _clientTimestamp = clientTimestamp.ToUniversalTime().ToFileTime();
        }
        public TitleStorageBlobMetadata(string serviceConfigurationId, TitleStorageType storageType, string blobPath, TitleStorageBlobType blobType, string xboxUserId, string displayName, string eTag) {
        }
        public TitleStorageBlobMetadata(string serviceConfigurationId, TitleStorageType storageType, string blobPath, TitleStorageBlobType blobType, string xboxUserId) {
        }

        public string MultiplayerSessionName
        {
            get;
            private set;
        }

        public string MultiplayerSessionTemplateName
        {
            get;
            private set;
        }

        public string XboxUserId
        {
            get;
            private set;
        }

        public string ServiceConfigurationId
        {
            get;
            private set;
        }

        public ulong Length
        {
            get;
            private set;
        }

        public DateTimeOffset ClientTimestamp
        {
            get;
            private set;
        }

        public string ETag
        {
            get;
            private set;
        }

        public string DisplayName
        {
            get;
            private set;
        }

        public TitleStorageType StorageType
        {
            get;
            private set;
        }

        public TitleStorageBlobType BlobType
        {
            get;
            private set;
        }

        public string BlobPath
        {
            get;
            private set;
        }


        public static TitleStorageBlobMetadata CreateTitleStorageBlobMetadataForSessionStorage(string serviceConfigurationId, string blobPath, TitleStorageBlobType blobType, string multiplayerSessionTemplateName, string multiplayerSessionName, string displayName, string eTag)
        {
            throw new NotImplementedException();
        }


        public void set(DateTimeOffset value)
        {
            throw new NotImplementedException();
        }

    }
}
