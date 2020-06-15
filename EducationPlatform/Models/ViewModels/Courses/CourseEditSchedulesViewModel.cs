using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EducationPlatform.Models.ViewModels.Courses
{
    public class CourseEditSchedulesViewModel
    {
        public int CourseId { get; set; }
        public CourseViewModel Course { get; set; }

        public List<CourseModuleEditViewModel> Modules { get; set; }
    }
}
