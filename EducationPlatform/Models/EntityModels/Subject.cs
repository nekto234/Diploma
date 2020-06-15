using System;
using System.Collections.Generic;

namespace EducationPlatform.Models.EntityModels
{
    public partial class Subject
    {
        public Subject()
        {
            Course = new HashSet<Course>();
            Module = new HashSet<Module>();
        }

        public int SubjectId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public virtual ICollection<Course> Course { get; set; }
        public virtual ICollection<Module> Module { get; set; }
    }
}
