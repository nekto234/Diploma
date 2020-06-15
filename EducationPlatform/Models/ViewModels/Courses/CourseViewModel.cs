using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EducationPlatform.Models.ViewModels.Courses
{
    public class CourseViewModel
    {
        public int CourseId { get; set; }
        public int SubjectId { get; set; }
        public SubjectViewModel Subject { get; set; }
        public string TeacherId { get; set; }
        public UserViewModel Teacher { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
    }
}
