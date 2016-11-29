using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Microsoft.Xbox.Services.TitleStorage
{
    public class TitleStorageService
    {

        public Task<TitleStorageQuota> GetQuotaAsync(string serviceConfigurationId, TitleStorageType storageType)
        {
            throw new NotImplementedException();
        }

        public Task<TitleStorageQuota> GetQuotaForSessionStorageAsync(string serviceConfigurationId, string multiplayerSessionTemplateName, string multiplayerSessionName)
        {
            throw new NotImplementedException();
        }

        public Task<TitleStorageBlobMetadataResult> GetBlobMetadataAsync(string serviceConfigurationId, TitleStorageType storageType, string blobPath, string xboxUserId, uint skipItems, uint maxItems)
        {
            throw new NotImplementedException();
        }

        public Task<TitleStorageBlobMetadataResult> GetBlobMetadataForSessionStorageAsync(string serviceConfigurationId, string blobPath, string multiplayerSessionTemplateName, string multiplayerSessionName, uint skipItems, uint maxItems)
        {
            throw new NotImplementedException();
        }

        public Task DeleteBlobAsync(TitleStorageBlobMetadata blobMetadata, bool deleteOnlyIfETagMatches)
        {
            throw new NotImplementedException();
        }

        public Task<TitleStorageBlobResult> DownloadBlobAsync(TitleStorageBlobMetadata blobMetadata, List<byte> blobBuffer, TitleStorageETagMatchCondition etagMatchCondition, string selectQuery, uint preferredDownloadBlockSize)
        {
            throw new NotImplementedException();
        }

        public Task<TitleStorageBlobResult> DownloadBlobAsync(TitleStorageBlobMetadata blobMetadata, List<byte> blobBuffer, TitleStorageETagMatchCondition etagMatchCondition, string selectQuery)
        {
            throw new NotImplementedException();
        }

        public Task<TitleStorageBlobMetadata> UploadBlobAsync(TitleStorageBlobMetadata blobMetadata, List<byte> blobBuffer, TitleStorageETagMatchCondition etagMatchCondition, uint preferredUploadBlockSize)
        {
            throw new NotImplementedException();
        }

    }
}
