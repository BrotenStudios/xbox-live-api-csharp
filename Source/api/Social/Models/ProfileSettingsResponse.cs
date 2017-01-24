using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Microsoft.Xbox.Services.Social.Models
{
    public class ProfileSettingsResponse
    {
        public List<XboxUserProfile> profileUsers { get; set; }
    }
}
