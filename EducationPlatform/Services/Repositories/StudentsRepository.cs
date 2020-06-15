using EducationPlatform.Models.EntityModels;
using EducationPlatform.Models.ViewModels;
using EducationPlatform.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EducationPlatform.Services.Repositories
{
    public class StudentsRepository : IStudentsRepository
    {
        private EducationPlatformContext _context;

        public StudentsRepository(EducationPlatformContext context)
        {
            _context = context;
        }

        public async Task<bool> HasStudentAccess(string email)
        {
            var student = await _context.Student.Include(x => x.User).FirstOrDefaultAsync(x => x.User.Email == email);

            if (student == null)
            {
                return false;
            }

            return student.HasAccess.Value;
        }

        public async Task<Student> GetByEmail(string email)
        {
            var student = await _context.Student.Include(x => x.User).FirstOrDefaultAsync(x => x.User.Email == email);
            return student;
        }

        public async Task<List<Student>> GetActiveStudents()
        {
            return await _context.Student
                .Include(x => x.User)
                .Where(x => x.HasAccess.HasValue && x.HasAccess.Value && !(x.User.IsBanned.HasValue && x.User.IsBanned.Value))
                .ToListAsync();
        } 

        public async Task<Student> GetById(int id)
        {
            return await _context.Student.Include(x => x.User).FirstOrDefaultAsync(x => x.StudentId == id);
        }

        public async Task<bool> Update(int id, StudentFormViewModel model)
        {
            var student = await GetById(id);

            if (student != null)
            {
                student.University = model.University;
                student.Faculty = model.Faculty;
                student.Skills = model.Skills;
                student.StudyYear = model.StudyYear;
                student.HasAccess = model.HasAccess;

                _context.Student.Update(student);
                return await _context.SaveChangesAsync() > 0;
            }

            return false;
        }

        public async Task<bool> Remove(int id)
        {
            var student = await GetById(id);

            if (student != null)
            {
                _context.Student.Remove(student);
                _context.AspNetUsers.Remove(student.User);

                return await _context.SaveChangesAsync() > 0;
            }

            return false;
        }

        public async Task<bool> ChangeAccess(int id, bool access)
        {
            var student = await GetById(id);

            if (student != null)
            {
                student.HasAccess = access;
                _context.Student.Update(student);

                return await _context.SaveChangesAsync() > 0;
            }

            return false;
        }
        public async Task<List<Student>> GetStudentsByCourse(int id)
        {
            Course courseFromDB = await _context.Course.Where(x => x.CourseId == id).SingleOrDefaultAsync();
            List<CourseStudent> courseStudents = _context.CourseStudent.Include(x => x.Student).Include(x => x.Student.User).Where(x => x.CourseId == id).ToList();
            List<Student> students = new List<Student>();
            foreach(var courseStudent in courseStudents)
            {
                students.Add(courseStudent.Student);
            }
            return students;
        }

        public async Task<Student> GetByUserId(string id)
        {
            return await _context.Student.Include(x => x.User).FirstAsync(x => x.UserId == id);
        }
    }
}
