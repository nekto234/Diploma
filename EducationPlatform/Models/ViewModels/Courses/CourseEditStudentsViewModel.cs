using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EducationPlatform.Models.ViewModels.Courses
{
    public class CourseEditStudentsViewModel
    {
        public int CourseId { get; set; }
        public CourseViewModel Course { get; set; }
        public List<CourseStudentEditViewModel> Students { get; set; }
    }
}
