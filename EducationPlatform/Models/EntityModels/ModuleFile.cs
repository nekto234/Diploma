using System;
using System.Collections.Generic;

namespace EducationPlatform.Models.EntityModels
{
    public partial class ModuleFile
    {
        public int ModuleFileId { get; set; }
        public int ModuleId { get; set; }
        public string FileUrl { get; set; }

        public virtual Module Module { get; set; }
    }
}
