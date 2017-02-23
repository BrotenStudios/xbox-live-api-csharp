// Copyright (c) Microsoft Corporation
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// 
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
        internal string BackgroundImage { get; private set; }

        public void AddEmphasisFeature(SignInUIEmphasisFeature feature)
        {
            this.EmphasisFeatures.Add(feature);
            TitleCategory = SignInUIGameCategory.Standard;
        }

        public void SetBackgroundImage(byte[] image)
        {
            throw new NotImplementedException();
        }

        internal bool Enabled()
        {
            return (BackgroundHexColor != null && BackgroundHexColor.Length != 0) || (BackgroundImage != null && BackgroundImage.Length != 0) || EmphasisFeatures.Count > 0 ||
                   TitleCategory != SignInUIGameCategory.Standard;
        }
    }
}