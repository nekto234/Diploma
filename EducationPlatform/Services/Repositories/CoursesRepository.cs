using EducationPlatform.Models.EntityModels;
using EducationPlatform.Models.ViewModels.Courses;
using EducationPlatform.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EducationPlatform.Services.Repositories
{
    public class CoursesRepository: ICoursesRepository
    {
        private EducationPlatformContext _context;
        public CoursesRepository(EducationPlatformContext context)
        {
            _context = context;
        }
        public async Task Insert(Course model)
        {
            _context.Course.Add(model);
            await _context.SaveChangesAsync();
        }

        public Course GetById(int id)
        {
            return _context.Course
                .Include(x => x.Teacher)
                .Include(x => x.CourseModule)
                .Include(x => x.CourseStudent)
                .Include(x => x.Subject)
                .Where(x => x.CourseId == id)
                .FirstOrDefault();
        }
        public async Task Update(Course model)
        {
            _context.Course.Update(model);
            await _context.SaveChangesAsync();
        }
        public IEnumerable<Course> GetCourses()
        {
            return _context.Course
                .Include(x => x.Teacher)
                .Include(x => x.CourseModule)
                .Include(x => x.CourseStudent)
                .ThenInclude(x => x.Student)
                .ThenInclude(x => x.User)
                .ToList<Course>();
        }
        public async Task<bool> Delete(int id)
        {
            Course courseFromDB = _context.Course.Where(x => x.CourseId == id).FirstOrDefault();
            List<CourseModule> courseModulesFromDb = _context.CourseModule.Where(x => x.CourseId == courseFromDB.CourseId).ToList();
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    foreach(CourseModule courseModule in courseModulesFromDb)
                    {
                        _context.Mark.RemoveRange(_context.Mark.Where(x => x.CourseModuleId == courseModule.CourseModuleId));
                    }
                    _context.CourseModule.RemoveRange(_context.CourseModule.Where(x => x.CourseId == courseFromDB.CourseId));
                    _context.CourseStudent.RemoveRange(_context.CourseStudent.Where(x => x.CourseId == courseFromDB.CourseId));
                    _context.Course.Remove(courseFromDB);
                    await _context.SaveChangesAsync();
                    transaction.Commit();
                }
                catch(Exception e)
                {
                    transaction.Rollback();
                    return false;
                }
            }
            return true;
        }

        public async Task<List<Module>> GetModules(int courseId)
        {
            return await _context.CourseModule.Include(x => x.Module).Where(x => x.CourseId == courseId).Select(x => x.Module).ToListAsync();
        }

        public async Task<bool> EditModules(CourseEditModulesViewModel model)
        {
            var courseId = model.CourseId;
            var courseStudents = await GetStudents(courseId);

            var notSelectedModules = model.Modules.Where(x => !x.Selected).Select(x => x.Id).ToList();

            _context.CourseModule.RemoveRange(await _context.CourseModule.Where(x => notSelectedModules.Contains(x.ModuleId)).ToListAsync());
            _context.Mark.RemoveRange(await _context.Mark.Include(x => x.CourseModule).Where(x => x.CourseModule.CourseId == courseId && notSelectedModules.Contains(x.CourseModule.ModuleId)).ToListAsync());

            var existedModules = await _context.CourseModule.Where(cm => cm.CourseId == courseId).Select(x => x.ModuleId).ToListAsync();


            var newModules = model.Modules.Where(x => x.Selected && !existedModules.Contains(x.Id)).Select(x => x.Id).ToList();

            newModules.ForEach(async moduleId =>
            {
                var cm = new CourseModule
                {
                    CourseId = courseId,
                    ModuleId = moduleId
                };

                await _context.CourseModule.AddAsync(cm);

                if (courseStudents != null && courseStudents.Count > 0)
                {
                    courseStudents.ForEach(async student =>
                    {
                        await _context.Mark.AddAsync(
                            new Mark
                            {
                                StudentId = student.StudentId,
                                CourseModuleId = cm.CourseModuleId,
                            }
                        );
                    });
                }
            });

            return await _context.SaveChangesAsync() >= 0;
        }

        public async Task<bool> EditStudents(CourseEditStudentsViewModel model)
        {
            var courseId = model.CourseId;

            var courseModules = await _context.CourseModule.Where(x => x.CourseId == model.CourseId).Select(x => x.CourseModuleId).ToListAsync();

            var notSelectedStudents = model.Students.Where(x => !x.Selected).Select(x => x.Id).ToList();

            _context.CourseStudent.RemoveRange(await _context.CourseStudent.Where(x => x.CourseId == courseId && notSelectedStudents.Contains(x.StudentId.Value)).ToListAsync());
            _context.Mark.RemoveRange(await _context.Mark.Include(x => x.CourseModule).Where(x => x.CourseModule.CourseId == courseId && notSelectedStudents.Contains(x.StudentId)).ToListAsync());

            var existedStudents = await _context.CourseStudent.Where(cm => cm.CourseId == courseId).Select(x => x.StudentId).ToListAsync();

            var newStudents = model.Students.Where(x => x.Selected && !existedStudents.Contains(x.Id)).Select(x => x.Id).ToList();

            newStudents.ForEach(async studentId =>
            {
                await _context.CourseStudent.AddAsync(
                    new CourseStudent
                    {
                        CourseId = courseId,
                        StudentId = studentId
                    }
                );

                courseModules.ForEach(async courseModuleId =>
                {
                    await _context.Mark.AddAsync(new Mark
                    {
                        CourseModuleId = courseModuleId,
                        StudentId = studentId,
                    });
                });

            });

            return await _context.SaveChangesAsync() >= 0;
        }

        public async Task<List<Student>> GetStudents(int courseId)
        {
            return await _context.CourseStudent.Include(x => x.Student).Where(x => x.CourseId.Value == courseId).Select(x => x.Student).ToListAsync();
        }

        public async Task<bool> RemoveModules(int courseId)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var courseModulesFromDB = await _context
                        .CourseModule
                        .Where(c => c.CourseId == courseId)
                        .ToListAsync();
                    
                    foreach (CourseModule courseModule in courseModulesFromDB)
                    {
                        _context.Mark.RemoveRange(_context.Mark.Where(x => x.CourseModuleId == courseModule.CourseModuleId));
                    }
                    _context.CourseModule.RemoveRange(courseModulesFromDB);
                    await _context.SaveChangesAsync();
                    transaction.Commit();
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    return false;
                }
            }
            return true;
        }

        public async Task<List<CourseModuleEditViewModel>> GetScheduleModules(int courseId)
        {
            return await _context.CourseModule.Include(x => x.Module).Where(x => x.CourseId == courseId).Select(x => new CourseModuleEditViewModel
            {
                Id = x.ModuleId,
                Name = x.Module.Name,
                Description = x.Module.Description,
                Date = x.Date.HasValue ? x.Date.Value : DateTime.MinValue
            }).ToListAsync();
        }

        public async Task<bool> EditSchedules(CourseEditSchedulesViewModel model)
        {
            var courseModule = await _context
                    .CourseModule
                    .Where(y => y.CourseId == model.CourseId && model.Modules.Select(x => x.Id).Contains(y.ModuleId))
                    .ToListAsync();

            courseModule.ForEach(x =>
            {
                x.Date = model.Modules.First(y => y.Id == x.ModuleId).Date;
            });

            _context.CourseModule.UpdateRange(courseModule);

            return await _context.SaveChangesAsync() >= 0;
        }

        public CourseModuleMarksViewModel GetModuleStudentsMarks(int courseId, int moduleId)
        {
            var course = GetById(courseId);
            var courseModule = course.CourseModule.Where(x => x.ModuleId == moduleId).First();


            var marks = _context.Mark.Include(x => x.CourseModule.Module).Where(x => x.CourseModuleId == courseModule.CourseModuleId).Select(x => new CourseModuleStudentsMark
            {
                CourseModuleId = x.CourseModuleId,
                TestMark = x.TestMark ?? 0,
                LabMark = x.LabMark ?? 0,
                StudentId = x.StudentId,
                StudentName = x.Student.User.LastName + " " + x.Student.User.FirstName + " " + x.Student.User.MiddleName
            }).ToList();

            return new CourseModuleMarksViewModel
            {
                Course = course,
                CurrentModule = _context.Module.First(x => x.ModuleId == moduleId),
                Marks = marks,
                Subject = course.Subject
            };
        }

        public async Task<bool> UpdateModuleStudentsMarks(CourseModuleMarksViewModel model)
        {
            var marksIds = model.Marks.Select(x => x.CourseModuleId);
            var marksToUpdate = await _context.Mark.Where(x => marksIds.Contains(x.CourseModuleId)).ToListAsync();

            model.Marks.ForEach(x => {
                var toUpdate = marksToUpdate.First(y => y.StudentId == x.StudentId);

                toUpdate.LabMark = x.LabMark;
                toUpdate.TestMark = x.TestMark;

                _context.Mark.Update(toUpdate);
            });

            return await _context.SaveChangesAsync() >= 0;
        }

        public CourseStudentMarksViewModel GetStudentModulesMarks(int courseId, int studentId)
        {
            var course = GetById(courseId);
            var courseModules = course.CourseModule.ToList();
            var courseModulesIds = courseModules.Select(x => x.CourseModuleId).ToList();


            var marks = _context
                .Mark
                .Include(x => x.CourseModule)
                .Include(x => x.CourseModule.Module)
                .Include(x => x.MarkId)
                .Where(x => courseModulesIds.Contains(x.CourseModuleId) && x.StudentId == studentId)
                .Select(x => new CourseStudentModulesMark
            {
                CourseModuleId = x.CourseModuleId,
                TestMark = x.TestMark ?? 0,
                LabMark = x.LabMark ?? 0,
                ModuleId = x.CourseModule.ModuleId,
                ModuleName = x.CourseModule.Module.Name,
                Module = x.CourseModule.Module,
                MarkId = x.MarkId,
                StudentId = x.StudentId
            }).ToList();

            return new CourseStudentMarksViewModel
            {
                Course = course,
                CurrentStudent = _context.Student.Include(x => x.User).First(x => x.StudentId == studentId),
                Marks = marks,
                Subject = course.Subject
            };
        }

        public async Task<bool> UpdateStudentModulesMarks(CourseStudentMarksViewModel model)
        {
            var marksIds = model.Marks.Select(x => x.CourseModuleId);
            var marksToUpdate = await _context.Mark.Include(x => x.CourseModule).Where(x => marksIds.Contains(x.CourseModuleId)).ToListAsync();

            model.Marks.ForEach(x => {
                var toUpdate = marksToUpdate.First(y => y.CourseModule.ModuleId == x.ModuleId);
                toUpdate.StudentId = x.StudentId;
                toUpdate.LabMark = x.LabMark;
                toUpdate.TestMark = x.TestMark;
                toUpdate.Student = _context.Student.FirstOrDefault(z => z.StudentId == x.StudentId);

                _context.Mark.Update(toUpdate);
            });

            return await _context.SaveChangesAsync() >= 0;
        }

        public async Task<CourseRatingViewModel> GetRating(int courseId)
        {
            var course = GetById(courseId);

            var students = await _context
                .CourseStudent
                .Include(x => x.Student)
                .ThenInclude(x => x.User)
                .Include(x => x.Student)
                .ThenInclude(x => x.Mark)
                .Include(x => x.Course)
                .ThenInclude(x => x.CourseModule)
                
                .Where(x => x.CourseId == courseId)
                .ToListAsync();

            var marks = new List<CourseModuleStudentsMark>();

            students.ForEach(x =>
            {
                var courseMod = x.Student.Mark;
                var marksInCourseByStudent = x.Student.Mark
                    .Where(y => y.StudentId == x.StudentId)
                    .Where(y => _context.CourseModule.Where(z => y.CourseModuleId == z.CourseModuleId).SingleOrDefault().CourseId == courseId);

                var testMark = marksInCourseByStudent.Where(y => y.TestMark.HasValue && y.TestMark.Value > 0).Sum(y => y.TestMark.Value);
                var labMark = marksInCourseByStudent.Where(y => y.LabMark.HasValue &&  y.LabMark.Value > 0).Sum(y => y.LabMark.Value);

                marks.Add(new CourseModuleStudentsMark {
                    LabMark = labMark,
                    TestMark = testMark,
                    StudentId = x.StudentId.Value,
                    StudentName = x.Student.User.LastName + " " + x.Student.User.FirstName + " " + x.Student.User.MiddleName
                });
            });

            var modules = await _context
                .CourseModule
                .Include(x => x.Module)
                .Where(x => x.CourseId == course.CourseId)
                .Select(x => x.Module)
                .ToListAsync();

            int minLabSum = modules.Where(y => y.HasLab).Sum(y => y.MinLabMark.Value);
            int maxLabSum = modules.Where(y => y.HasLab).Sum(y => y.MaxLabMark.Value);
            int minTestSum = modules.Where(y => y.HasTest).Sum(y => y.MinTestMark.Value);
            int maxTestSum = modules.Where(y => y.HasTest).Sum(y => y.MaxTestMark.Value);

            return new CourseRatingViewModel
            {
                Course = course,
                CourseId = courseId,
                Marks = marks,
                MinLabSum = minLabSum,
                MaxLabSum = maxLabSum,
                MinTestSum = minTestSum,
                MaxTestSum = maxTestSum,
            };
        }

        public async Task<Mark> GetMarkById(int markId)
        {
            var markFromDB = await _context.Mark
                .Include(x => x.Student)
                .Include(x => x.CourseModule)
                .ThenInclude(x => x.Module)
                .Include(x => x.CourseModule)
                .ThenInclude(x => x.Course)
                .ThenInclude(x => x.Teacher)
                .Where(x => x.MarkId == markId).SingleOrDefaultAsync();
            return markFromDB;
        }

        public async Task<List<Comments>> GetCommentByMarkId(int markId)
        {
            var commentsFromDB = await _context.Comments
                .Include(x => x.Mark)
                .Where(x => x.MarkId == markId).ToListAsync();
            return commentsFromDB;
        }
        public async Task InsertComment(Comments comment)
        {
            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();
        }
    }
}
