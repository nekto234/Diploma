using EducationPlatform.Models.EntityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EducationPlatform.Models.ViewModels.Courses
{
    public class CourseDetailsViewModel
    {
        public Course Course { get; set; }
        public Subject Subject { get; set; }
        public AspNetUsers Teacher { get; set; }
        public List<CourseModuleEditViewModel> Modules { get; set; }
        public List<Student> Students { get; set; }
    }
}
