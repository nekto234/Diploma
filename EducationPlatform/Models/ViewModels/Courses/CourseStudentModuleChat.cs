using EducationPlatform.Models.EntityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EducationPlatform.Models.ViewModels.Courses
{
    public class CourseStudentModuleChat
    {
        public Mark Mark { get; set; }
        public List<Comments> Comments { get; set; }
        public String Comment { get; set; }
        public String UserId { get; set; }
        public int MarkId { get; set; }

    }
}
