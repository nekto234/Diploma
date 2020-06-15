using EducationPlatform.Models.EntityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EducationPlatform.Models.ViewModels.Courses
{
    public class CourseRatingViewModel
    {
        public int CourseId { get; set; }
        public Course Course { get; set; }

        public List<CourseModuleStudentsMark> Marks { get; set; }
        public int MinLabSum { get; internal set; }
        public int MaxLabSum { get; internal set; }
        public int MinTestSum { get; internal set; }
        public int MaxTestSum { get; internal set; }
    }
}
