using System;
using System.Collections.Generic;

namespace EducationPlatform.Models.EntityModels
{
    public partial class Course
    {
        public Course()
        {
            CourseModule = new HashSet<CourseModule>();
            CourseStudent = new HashSet<CourseStudent>();
        }

        public int CourseId { get; set; }
        public string TeacherId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Name { get; set; }
        public int SubjectId { get; set; }

        public virtual Subject Subject { get; set; }
        public virtual AspNetUsers Teacher { get; set; }
        public virtual ICollection<CourseModule> CourseModule { get; set; }
        public virtual ICollection<CourseStudent> CourseStudent { get; set; }
    }
}
