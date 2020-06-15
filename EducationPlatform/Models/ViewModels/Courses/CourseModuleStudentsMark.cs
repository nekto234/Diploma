namespace EducationPlatform.Models.ViewModels.Courses
{
    public class CourseModuleStudentsMark
    {
        public ModuleViewModel Module { get; set; }
        public string StudentName { get; set; }
        public int StudentId { get; set; }
        public int CourseModuleId { get; set; }
        public int TestMark { get; set; }
        public int LabMark { get; set; }
    }
}