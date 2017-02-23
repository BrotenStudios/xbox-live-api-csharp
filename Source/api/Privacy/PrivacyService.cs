// Copyright (c) Microsoft Corporation
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// 
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Microsoft.Xbox.Services.Privacy
{
    public class PrivacyService
    {

        public Task<global::System.Collections.ObjectModel.ReadOnlyCollection<string>> GetAvoidListAsync()
        {
            throw new NotImplementedException();
        }

        public Task<PermissionCheckResult> CheckPermissionWithTargetUserAsync(string permissionId, string targetXboxUserId)
        {
            throw new NotImplementedException();
        }

        public Task<global::System.Collections.ObjectModel.ReadOnlyCollection<MultiplePermissionsCheckResult>> CheckMultiplePermissionsWithMultipleTargetUsersAsync(string[] permissionIds, string[] targetXboxUserIds)
        {
            throw new NotImplementedException();
        }

        public Task<global::System.Collections.ObjectModel.ReadOnlyCollection<string>> GetMuteListAsync()
        {
            throw new NotImplementedException();
        }

    }
}
