using System;
using System.Collections.Generic;

namespace EducationPlatform.Models.EntityModels
{
    public partial class Student
    {
        public Student()
        {
            CourseStudent = new HashSet<CourseStudent>();
            Mark = new HashSet<Mark>();
        }

        public int StudentId { get; set; }
        public string UserId { get; set; }
        public string University { get; set; }
        public string Faculty { get; set; }
        public int? StudyYear { get; set; }
        public string Skills { get; set; }
        public bool? HasAccess { get; set; }

        public virtual AspNetUsers User { get; set; }
        public virtual ICollection<CourseStudent> CourseStudent { get; set; }
        public virtual ICollection<Mark> Mark { get; set; }
    }
}
