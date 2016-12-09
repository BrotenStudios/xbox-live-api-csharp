using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Microsoft.Xbox.Services
{
    public class SignInUISettings
    {

        public string BackgroundHexColor
        {
            get;
            set;
        }

        public IList<SignInUIEmphasisFeature> EmphasisFeatures
        {
            get;
            private set;
        }

        public SignInUIGameCategory TitleCategory
        {
            get;
            set;
        }


        public void AddEmphasisFeature(SignInUIEmphasisFeature feature)
        {
            throw new NotImplementedException();
        }

        public void SetBackgroundImage(byte[] image)
        {
            throw new NotImplementedException();
        }

    }
}
