using EducationPlatform.Models.EntityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EducationPlatform.Models.ViewModels.Courses
{
    public class CourseModuleMarksViewModel
    {
        public Course Course { get; set; }
        public Subject Subject { get; set; }
        public Module CurrentModule { get; set; }

        public List<CourseModuleStudentsMark> Marks { get; set; }
    }
}
