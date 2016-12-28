// -----------------------------------------------------------------------
//  <copyright file="SignInUISettings.cs" company="Microsoft">
//      Copyright (c) Microsoft. All rights reserved.
//      Licensed under the MIT license. See LICENSE file in the project root for full license information.
//  </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.Xbox.Services
{
    using global::System;
    using global::System.Collections.Generic;

    public class SignInUISettings
    {
        public SignInUISettings()
        {
            this.EmphasisFeatures = new List<SignInUIEmphasisFeature>();
        }

        public string BackgroundHexColor { get; set; }

        public IList<SignInUIEmphasisFeature> EmphasisFeatures { get; private set; }

        public SignInUIGameCategory TitleCategory { get; set; }

        public void AddEmphasisFeature(SignInUIEmphasisFeature feature)
        {
            this.EmphasisFeatures.Add(feature);
        }

        public void SetBackgroundImage(byte[] image)
        {
            throw new NotImplementedException();
        }
    }
}