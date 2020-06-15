using EducationPlatform.Models.EntityModels;

namespace EducationPlatform.Models.ViewModels.Courses
{
    public class CourseStudentModulesMark
    {
        public Module Module { get; set; }
        public string ModuleName { get; set; }
        public int ModuleId { get; set; }
        public int CourseModuleId { get; set; }
        public int TestMark { get; set; }
        public int LabMark { get; set; }
        public int MarkId { get; set; }
        public int StudentId { get; set; }
    }
}