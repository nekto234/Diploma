using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EducationPlatform.Models.ViewModels
{
    public class TwoFactorViewModel
    {
        public string Email { get; set; }
        public int Code { get; set; }
    }
}
