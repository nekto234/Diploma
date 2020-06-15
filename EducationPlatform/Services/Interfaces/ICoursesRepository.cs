using EducationPlatform.Models.EntityModels;
using EducationPlatform.Models.ViewModels.Courses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EducationPlatform.Services.Interfaces
{
    public interface ICoursesRepository
    {
        Task Insert(Course model);
        Course GetById(int id);
        Task Update(Course model);
        IEnumerable<Course> GetCourses();
        Task<bool> Delete(int id);

        Task<List<Module>> GetModules(int courseId);
        Task<bool> EditModules(CourseEditModulesViewModel model);
        Task<bool> EditStudents(CourseEditStudentsViewModel model);
        Task<bool> EditSchedules(CourseEditSchedulesViewModel model);
        Task<bool> RemoveModules(int courseId);
        Task<List<CourseModuleEditViewModel>> GetScheduleModules(int courseId);
        Task<List<Student>> GetStudents(int courseId);
        CourseModuleMarksViewModel GetModuleStudentsMarks(int courseId, int moduleId);
        CourseStudentMarksViewModel GetStudentModulesMarks(int courseId, int studentId);
        Task<bool> UpdateModuleStudentsMarks(CourseModuleMarksViewModel model);
        Task<bool> UpdateStudentModulesMarks(CourseStudentMarksViewModel model);
        Task<CourseRatingViewModel> GetRating(int courseId);
        Task<Mark> GetMarkById(int markId);
        Task<List<Comments>> GetCommentByMarkId(int markId);
        Task InsertComment(Comments comment);
    }
}
