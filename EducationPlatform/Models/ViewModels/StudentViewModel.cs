using EducationPlatform.Models.EntityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EducationPlatform.Models.ViewModels
{
    public class StudentViewModel
    {
        public string University { get; set; }
        public string Faculty { get; set; }
        public int? StudyYear { get; set; }
        public string Skills { get; set; }
        public bool? HasAccess { get; set; }

        public AspNetUsers User { get; set; }
    }
}
