// -----------------------------------------------------------------------
//  <copyright file="XboxException.cs" company="Microsoft">
//      Copyright (c) Microsoft. All rights reserved.
//      Licensed under the MIT license. See LICENSE file in the project root for full license information.
//  </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.Xbox.Services
{
    using global::System;

    public class XboxException : Exception
    {
        public XboxException()
        {
        }

        public XboxException(string message) : base(message)
        {
        }

        public XboxException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}