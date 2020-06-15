using System;
using System.Collections.Generic;

namespace EducationPlatform.Models.EntityModels
{
    public partial class CourseModule
    {
        public CourseModule()
        {
            Mark = new HashSet<Mark>();
        }

        public int CourseModuleId { get; set; }
        public int CourseId { get; set; }
        public int ModuleId { get; set; }
        public DateTime? Date { get; set; }

        public virtual Course Course { get; set; }
        public virtual Module Module { get; set; }
        public virtual ICollection<Mark> Mark { get; set; }
    }
}
