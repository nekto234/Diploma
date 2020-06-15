using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EducationPlatform.Models.Entities;
using EducationPlatform.Models.EntityModels;
using EducationPlatform.Models.ViewModels;
using EducationPlatform.Models.ViewModels.Courses;
using EducationPlatform.Services.Interfaces;
using EducationPlatform.Services.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace EducationPlatform.Controllers
{
    public class CoursesController : Controller
    {
        private readonly ICoursesRepository _coursesRepository;
        private readonly ISubjectsRepository _subjectsRepository;
        private readonly IModulesRepository _modulesRepository;
        private readonly IUsersRepository _usersRepository;
        private readonly IStudentsRepository _studentsRepository;
        private readonly UserManager<User> _userManager;

        public CoursesController(
            ICoursesRepository coursesRepository,
            ISubjectsRepository subjectsRepository,
            IModulesRepository modulesRepository,
            IUsersRepository usersRepository,
            IStudentsRepository studentsRepository,
            UserManager<User> userManager
        )
        {
            _coursesRepository = coursesRepository;
            _subjectsRepository = subjectsRepository;
            _modulesRepository = modulesRepository;
            _usersRepository = usersRepository;
            _studentsRepository = studentsRepository;
            _userManager = userManager;
        }
        [Route("{controller}")]
        [Authorize(Roles = "Admin,Teacher,Student")]
        public IActionResult Index(string message = null, string error = null)
        {
            ViewBag.Message = message;
            ViewBag.Error = error;

            CoursesIndexViewModel coursesIndexViewModel = new CoursesIndexViewModel()
            {
                Courses = _coursesRepository.GetCourses().ToList()
            };
            return View(coursesIndexViewModel);
        }

        [Authorize(Roles = "Admin,Teacher")]
        public IActionResult Create()
        {
            ViewData["Subjects"] = _subjectsRepository.GetSubjects();
            ViewData["Teachers"] = _usersRepository.GetNotBannedUsersInRole("Teacher").ToList();
            return View(new CourseViewModel());
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Teacher")]
        public async Task<IActionResult> Create(CourseViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Course course = new Course()
                    {
                        CourseId = model.CourseId,
                        SubjectId = model.SubjectId,
                        Name = model.Name,
                        TeacherId = model.TeacherId,
                        StartDate = model.Start,
                        EndDate = model.End
                    };
                    await _coursesRepository.Insert(course);
                    return RedirectToAction("EditModules", new { courseId = course.CourseId });
                }
            }
            catch
            {
                return RedirectToAction("Index", "Courses", new { error = "При створенні курсу виникла невідома помилка!" });
            }
            return View(model);
        }

        [Authorize(Roles = "Admin,Teacher,Student")]
        [Route("{controller}/{action}/{courseId}")]
        public async Task<IActionResult> Details(int courseId)
        {
            Course courseFromDB = _coursesRepository.GetById(courseId);
            SubjectViewModel subjectFromDB = _subjectsRepository.GetById(courseFromDB.Subject.SubjectId);
            List<CourseModuleEditViewModel> modulesFromDB = await _coursesRepository.GetScheduleModules(courseId);
            List<Student> students = await _studentsRepository.GetStudentsByCourse(courseId);
            User userFromDB = await _usersRepository.GetByEmail(courseFromDB.Teacher.Email);
            CourseDetailsViewModel courseDetailsViewModel = new CourseDetailsViewModel()
            {
                Course = courseFromDB,
                Subject = new Subject()
                {
                    SubjectId = subjectFromDB.SubjectId,
                    Name = subjectFromDB.Name,
                    Description = subjectFromDB.Description
                },
                Teacher = new AspNetUsers()
                {
                    FirstName = userFromDB.FirstName,
                    LastName = userFromDB.LastName,
                    MiddleName = userFromDB.MiddleName
                },
                Modules = modulesFromDB,
                Students = students
            };
            return View(courseDetailsViewModel);
        }

        [Authorize(Roles = "Admin,Teacher")]
        [Route("{controller}/{action}/{courseId}")]
        public IActionResult Edit(int courseId)
        {
            Course course = _coursesRepository.GetById(courseId);
            ViewData["Subjects"] = _subjectsRepository.GetSubjects();
            ViewData["Teachers"] = _usersRepository.GetNotBannedUsersInRole("Teacher").ToList();
            CourseViewModel model = new CourseViewModel
            {
                CourseId = course.CourseId,
                Name = course.Name,
                TeacherId = course.TeacherId,
                SubjectId = course.SubjectId,
                Teacher = new UserViewModel
                {
                    UserName = course.Teacher.UserName,
                    FirstName = course.Teacher.FirstName,
                    LastName = course.Teacher.LastName,
                    MiddleName = course.Teacher.MiddleName,
                    IsBanned = course.Teacher.IsBanned.Value,
                    Email = course.Teacher.Email,
                    PhoneNumber = course.Teacher.PhoneNumber
                },
                Start = course.StartDate,
                End = course.EndDate
            };
            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Teacher")]
        [Route("{controller}/{action}/{courseId}")]
        public async Task<IActionResult> Edit(CourseViewModel model)
        {
            if (ModelState.IsValid)
            {
                var oldCourse = _coursesRepository.GetById(model.CourseId);

                if (oldCourse.SubjectId != model.SubjectId)
                {
                    await _coursesRepository.RemoveModules(model.CourseId);
                }

                oldCourse.SubjectId = model.SubjectId;
                oldCourse.Name = model.Name;
                oldCourse.TeacherId = model.TeacherId;
                oldCourse.StartDate = model.Start;
                oldCourse.EndDate = model.End;

                await _coursesRepository.Update(oldCourse);

                return RedirectToAction("EditModules", new { courseId = model.CourseId });
            }
            return View(model);
        }

        [Authorize(Roles = "Admin,Teacher")]
        [Route("{controller}/{action}/{courseId}")]
        public async Task<IActionResult> EditModules(int courseId)
        {
            var course = _coursesRepository.GetById(courseId);
            var subjectId = course.SubjectId;
            var subject = _subjectsRepository.GetById(subjectId);
            var modules = _modulesRepository.GetModulesBySubjectId(subjectId);

            var model = new CourseEditModulesViewModel
            {
                CourseId = courseId,

                Course = new CourseViewModel
                {
                    CourseId = courseId,
                    Description = subject.Description,
                    Start = course.StartDate,
                    End = course.EndDate,
                    Subject = subject,
                    Name = course.Name,
                    Teacher = new UserViewModel
                    {
                        Email = course.Teacher.Email,
                        FirstName = course.Teacher.FirstName,
                        LastName = course.Teacher.LastName,
                        MiddleName = course.Teacher.MiddleName,
                    },
                    TeacherId = course.TeacherId,
                    SubjectId = subjectId
                },
                SubjectId = subjectId,
            };

            var modulesInCourses = (await _coursesRepository.GetModules(model.CourseId)).Select(x => x.ModuleId).ToList();

            model.Modules = modules.Select(x => new CourseModuleEditViewModel
            {
                Id = x.ModuleId,
                Name = x.Name,
                Description = x.Description,
                Selected = modulesInCourses.Count == 0 ? true : modulesInCourses.Contains(x.ModuleId)
            }).ToList();

            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Teacher")]
        [Route("{controller}/{action}/{courseId}")]
        public async Task<IActionResult> EditModules(CourseEditModulesViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var result = await _coursesRepository.EditModules(model);

                    if (result)
                    {
                        // success
                       
                        return RedirectToAction("EditStudents", new { courseId = model.CourseId });
                    }
                }
                catch
                {
                    // error
                    ModelState.AddModelError(string.Empty, "Невідома помилка");
                    return RedirectToAction("EditStudents", new { courseId = model.CourseId });
                }
                
            }
            else
            {
                ModelState.TryAddModelError(string.Empty,"Невідома помилка!");
               
            }
            return View(model);
        }

        [Authorize(Roles = "Admin,Teacher")]
        [Route("{controller}/{action}/{courseId}")]
        public async Task<IActionResult> EditStudents(int courseId)
        {
            var course = _coursesRepository.GetById(courseId);
            var students = await _studentsRepository.GetActiveStudents();

            var subjectId = course.SubjectId;
            var subject = _subjectsRepository.GetById(subjectId);

            var model = new CourseEditStudentsViewModel
            {
                CourseId = courseId,

                Course = new CourseViewModel
                {
                    CourseId = courseId,
                    Description = subject.Description,
                    Start = course.StartDate,
                    End = course.EndDate,
                    Subject = subject,
                    Name = course.Name,
                    Teacher = new UserViewModel
                    {
                        Email = course.Teacher.Email,
                        FirstName = course.Teacher.FirstName,
                        LastName = course.Teacher.LastName,
                        MiddleName = course.Teacher.MiddleName,
                    },
                    TeacherId = course.TeacherId,
                    SubjectId = subjectId
                },
            };

            var studentsInCourse = (await _coursesRepository.GetStudents(model.CourseId)).Select(x => x.StudentId).ToList();

            model.Students = students.Select(x => new CourseStudentEditViewModel
            {
                Id = x.StudentId,
                Name = x.User.LastName + " " + x.User.FirstName + " " + x.User.MiddleName,
                Selected = studentsInCourse.Count == 0 ? false : studentsInCourse.Contains(x.StudentId)
            }).ToList();

            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Teacher")]
        [Route("{controller}/{action}/{courseId}")]
        public async Task<IActionResult> EditStudents(CourseEditStudentsViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var result = await _coursesRepository.EditStudents(model);

                    if (result)
                    {
                        // success
                        return RedirectToAction("Index", "Courses", new { message = "Редагування к-сті студентів успішно виконано."});
                    }
                }
                catch
                {
                    // error
                    return RedirectToAction("Index", "Courses", new { error = "При редагуванні к-сті студентів сталась невідома помилка!" });
                }
            }
            else
            {
                ModelState.TryAddModelError(string.Empty, "Невідома помилка!");
            }
            return View(model);
        }

        [Authorize(Roles = "Admin,Teacher")]
        [Route("{controller}/{action}/{courseId}")]
        public async Task<IActionResult> EditSchedule(int courseId)
        {
            var course = _coursesRepository.GetById(courseId);
            var modules = await _coursesRepository.GetScheduleModules(courseId);
            var subject = _subjectsRepository.GetById(course.SubjectId);

            var model = new CourseEditSchedulesViewModel
            {
                CourseId = courseId,

                Course = new CourseViewModel
                {
                    CourseId = courseId,
                    Start = course.StartDate,
                    End = course.EndDate,
                    Subject = subject,
                    Teacher = new Models.ViewModels.UserViewModel
                    {
                        Email = course.Teacher.Email,
                        FirstName = course.Teacher.FirstName,
                        LastName = course.Teacher.LastName,
                        MiddleName = course.Teacher.MiddleName,
                    },
                    TeacherId = course.TeacherId,
                    SubjectId = course.SubjectId

                },
            };

            model.Modules = modules;

            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Teacher")]
        [Route("{controller}/{action}/{courseId}")]
        public async Task<IActionResult> EditSchedule(CourseEditSchedulesViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var result = await _coursesRepository.EditSchedules(model);

                    if (result)
                    {
                        // success
                        return RedirectToAction("Index", "Courses", new { message = "Редагування розкладу успішно виконано."});
                    }
                }
                catch
                {
                    // error
                    return RedirectToAction("Index", "Courses", new {error = "При редагуванні розкладу сталась невідома помилка!"});
                }
            }

            // error for Model
            else
            {
                ModelState.AddModelError(string.Empty, "Невідома помилка!");
            }
            return View(model);
        }

        [Authorize(Roles = "Admin,Teacher,Student")]
        [Route("{controller}/{action}/{courseId}/Students/{studentId}")]
        public IActionResult RatingStudent(int courseId, int studentId)
        {
            try
            {
                var model = _coursesRepository.GetStudentModulesMarks(courseId, studentId);

                return View(model);
            }
            catch
            {
                // not found 
                return RedirectToAction("Index", "Courses");
            }
        }

        [Authorize(Roles = "Admin,Teacher,Student")]
        [Route("{controller}/{action}/{courseId}")]
        public async Task<IActionResult> Rating(int courseId)
        {
            var model = await _coursesRepository.GetRating(courseId);
            return View(model);
        }

        [Authorize(Roles = "Admin,Teacher")]
        [Route("{controller}/{action}/{courseId}/Modules/{moduleId}")]
        public IActionResult SetModuleMark(int courseId, int moduleId)
        {
            CourseModuleMarksViewModel model = _coursesRepository.GetModuleStudentsMarks(courseId, moduleId);

            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Teacher")]
        [Route("{controller}/{action}/{courseId}/Modules/{moduleId}")]
        public async Task<IActionResult> SetModuleMark(CourseModuleMarksViewModel model)
        {

            try
            {
                var result = await _coursesRepository.UpdateModuleStudentsMarks(model);

                if (result)
                {
                    // success
                    return RedirectToAction("Details", "Courses", new { courseId = model.Course.CourseId });
                }
                return RedirectToAction("Index", "Courses", new { error = "При виставленні оцінки за модуль сталася невідома помилка!" });
            }
            catch
            {
                // Error
                return RedirectToAction("Index", "Courses", new { error = "При виставленні оцінки за модуль сталася невідома помилка!" });
            }



            //}
            //// Error for Model (Model.AddError or smth like that)
            //else
            //{
            //    ModelState.AddModelError(string.Empty, "Сталася невідома помилка!");
            //}
            //return View(model);
        }

        [Authorize(Roles = "Admin,Teacher")]
        [Route("{controller}/{action}/{courseId}/Students/{studentId}")]
        public IActionResult SetStudentMark(int courseId, int studentId)
        {
            CourseStudentMarksViewModel model = _coursesRepository.GetStudentModulesMarks(courseId, studentId);

            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Teacher")]
        [Route("{controller}/{action}/{courseId}/Students/{studentId}")]
        public async Task<IActionResult> SetStudentMark(CourseStudentMarksViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var result = await _coursesRepository.UpdateStudentModulesMarks(model);

                    if (result)
                    {
                        // success
                        return RedirectToAction("Details", "Courses", new { courseId = model.Course.CourseId });
                    }
                }
                catch
                {
                    // Error
                    return RedirectToAction("Index", "Courses");
                }

            }
            // Error for Model (Model.AddError or smth like that)
            return View(model);
        }
        [HttpPost]
        [Authorize(Roles = "Admin,Teacher")]
        [Route("{controller}/{action}/{courseId}")]
        public async Task<IActionResult> Delete(int courseId)
        {
            bool res = await _coursesRepository.Delete(courseId);
            if (res)
            {
                return RedirectToAction("Index", "Courses", new { message = "Курс успішно видалений!" });
            }
            else
            {
                return RedirectToAction("Index", "Courses", new { error = "Сталася невідома помилка при видаленні курсу!" });
            }

        }

        [HttpGet]
        [Authorize(Roles = "Admin,Teacher,Student")]
        [Route("{controller}/{action}/{markId}")]
        public async Task<IActionResult> MarkChat(int markId)
        {
            Mark res = await _coursesRepository.GetMarkById(markId);
            if (res == null)
                return RedirectToAction("Index", "Courses" , new { error = "Ви вибрали неіснуючу оцінку" });
            List<Comments> comments = await _coursesRepository.GetCommentByMarkId(markId);
            CourseStudentModuleChat courseStudentModuleChat = new CourseStudentModuleChat()
            {
                Mark = res,
                Comments = comments,
                MarkId = res.MarkId,
                UserId = (await _userManager.FindByNameAsync(User.Identity.Name)).Id
            };
            return View(courseStudentModuleChat);

        }

        [HttpPost]
        [Authorize(Roles = "Admin,Teacher,Student")]
        [Route("{controller}/{action}/{markId}")]
        public async Task<IActionResult> MarkChat(CourseStudentModuleChat model)
        {
            await _coursesRepository.InsertComment(new Comments
            {
                Comment = model.Comment,
                UserId = model.UserId,
                MarkId = model.MarkId
            });
            return RedirectToAction("MarkChat", new { markId = model.MarkId });

        }

    }
}