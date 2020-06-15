using System;
using System.Collections.Generic;

namespace EducationPlatform.Models.EntityModels
{
    public partial class Mark
    {
        public Mark()
        {
            Comments = new HashSet<Comments>();
        }

        public int MarkId { get; set; }
        public int? TestMark { get; set; }
        public int? LabMark { get; set; }
        public int StudentId { get; set; }
        public int CourseModuleId { get; set; }

        public virtual CourseModule CourseModule { get; set; }
        public virtual Student Student { get; set; }
        public virtual ICollection<Comments> Comments { get; set; }
    }
}
