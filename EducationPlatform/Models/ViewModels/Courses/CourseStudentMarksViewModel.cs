using EducationPlatform.Models.EntityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EducationPlatform.Models.ViewModels.Courses
{
    public class CourseStudentMarksViewModel
    {
        public Course Course { get; set; }
        public Subject Subject { get; set; }
        public Student CurrentStudent { get; set; }

        public List<CourseStudentModulesMark> Marks { get; set; }
    }
}
