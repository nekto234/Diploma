using System;
using System.Collections.Generic;

namespace EducationPlatform.Models.EntityModels
{
    public partial class Module
    {
        public Module()
        {
            CourseModule = new HashSet<CourseModule>();
            ModuleFile = new HashSet<ModuleFile>();
        }

        public int ModuleId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool HasTest { get; set; }
        public bool HasLab { get; set; }
        public int? MinTestMark { get; set; }
        public int? MaxTestMark { get; set; }
        public int? MinLabMark { get; set; }
        public int? MaxLabMark { get; set; }
        public int? SubjectId { get; set; }

        public virtual Subject Subject { get; set; }
        public virtual ICollection<CourseModule> CourseModule { get; set; }
        public virtual ICollection<ModuleFile> ModuleFile { get; set; }
    }
}
