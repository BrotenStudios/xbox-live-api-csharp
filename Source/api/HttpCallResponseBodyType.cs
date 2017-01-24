// -----------------------------------------------------------------------
//  <copyright file="HttpCallResponseBodyType.cs" company="Microsoft">
//      Copyright (c) Microsoft. All rights reserved.
//      Licensed under the MIT license. See LICENSE file in the project root for full license information.
//  </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.Xbox.Services
{
    public enum HttpCallResponseBodyType
    {
        StringBody = 0,
        VectorBody = 1,
        JsonBody = 2,
    }
}