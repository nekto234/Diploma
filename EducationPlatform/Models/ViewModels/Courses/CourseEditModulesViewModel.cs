using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EducationPlatform.Models.ViewModels.Courses
{
    public class CourseEditModulesViewModel
    {
        public int CourseId { get; set; }
        public int SubjectId { get; set; }
        public CourseViewModel Course { get; set; }

        public List<CourseModuleEditViewModel> Modules { get; set; }
    }
}
