using System;
using System.Collections.Generic;

namespace EducationPlatform.Models.EntityModels
{
    public partial class TwoFactorUser
    {
        public int TwoFactorUserId { get; set; }
        public string UserId { get; set; }
        public int? Code { get; set; }

        public virtual AspNetUsers User { get; set; }
    }
}
