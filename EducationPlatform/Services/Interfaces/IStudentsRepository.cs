using EducationPlatform.Models.EntityModels;
using EducationPlatform.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EducationPlatform.Services.Interfaces
{
    public interface IStudentsRepository
    {
        Task<Student> GetById(int id);
        Task<Student> GetByUserId(string id);
        Task<bool> Update(int id, StudentFormViewModel model);
        Task<bool> Remove(int id);
        Task<bool> ChangeAccess(int id, bool access);
        Task<bool> HasStudentAccess(string email);
        Task<List<Student>> GetActiveStudents();
        Task<Student> GetByEmail(string email);
        Task<List<Student>> GetStudentsByCourse(int id);
    }
}
