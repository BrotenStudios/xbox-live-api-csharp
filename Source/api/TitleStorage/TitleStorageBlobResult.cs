// Copyright (c) Microsoft Corporation
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// 
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Microsoft.Xbox.Services.TitleStorage
{
    public class TitleStorageBlobResult
    {

        public TitleStorageBlobMetadata BlobMetadata
        {
            get;
            private set;
        }

        public List<byte> BlobBuffer
        {
            get;
            private set;
        }

    }
}
