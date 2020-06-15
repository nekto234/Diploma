using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using EducationPlatform.Models.ViewModels;
using EducationPlatform.Services.Interfaces;
using EducationPlatform.Models.EntityModels;
using EducationPlatform.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace EducationPlatform.Controllers
{
    public class StudentsController : Controller
    {
        private readonly IUsersRepository _usersRepository;
        private readonly IStudentsRepository _studentsRepository;
        private readonly EducationPlatformContext _context;

        public StudentsController(IUsersRepository usersRepository, IStudentsRepository studentsRepository, EducationPlatformContext context)
        {
            _usersRepository = usersRepository;
            _studentsRepository = studentsRepository;
            _context = context;
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Index(string error = null, string message = null)
        {
            var students = _context.Student.Include(x => x.User).ToList();

            ViewBag.Error = error;
            ViewBag.Message = message;

            return View(students);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(StudentFormViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                  await _usersRepository.RegisterStudent(
                        new UserViewModel
                        {
                            Email = model.Email,
                            FirstName = model.FirstName,
                            LastName = model.LastName,
                            MiddleName = model.MiddleName,
                            Password = model.Password,
                            PhoneNumber = model.PhoneNumber,
                            UserName = model.Email,
                            IsBanned = model.IsBan
                        },
                        new StudentViewModel
                        {
                            University = model.University,
                            Faculty = model.Faculty,
                            StudyYear = model.StudyYear,
                            Skills = model.Skills,
                            HasAccess = model.HasAccess
                        }
                    );
                   
                }
                catch
                {
                    ModelState.AddModelError("Email", "Ця електронна адреса вже зайнята!");
                    return View(model);
                }


                return RedirectToAction("Index", "Students", new { message = "Студент успішно доданий!" });
            }

            return View(model);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id)
        {
            var stud = await _studentsRepository.GetById(id);

            if (stud == null)
            {
                return RedirectToAction("Index", "Students", new { error = "Студента не знайдено!" });
            }

            StudentFormViewModel model = new StudentFormViewModel
            {
                LastName = stud.User.LastName,
                FirstName = stud.User.FirstName,
                MiddleName = stud.User.MiddleName,
                PhoneNumber = stud.User.PhoneNumber,
                University = stud.University,
                Faculty = stud.Faculty,
                Email = stud.User.Email,
                HasAccess = stud.HasAccess.Value,
                IsBan = stud.User.IsBanned.Value,
                Skills = stud.Skills,
                StudyYear = stud.StudyYear.HasValue ? stud.StudyYear.Value : 0
            };


            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Student")]
        public async Task<IActionResult> Edit(StudentFormViewModel model, int StudentId = -1)
        {
            if (ModelState.IsValid || ModelState.ErrorCount == 2)
            {
                var resU = await _usersRepository.Update(new UserViewModel {
                    Email = model.Email,
                    PhoneNumber = model.PhoneNumber,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    MiddleName = model.MiddleName,
                    IsBanned = model.IsBan
                });
                var resS = await _studentsRepository.Update(StudentId > 0 ? StudentId : model.Id, model);

                if (resU && resS)
                {
                    if (User.IsInRole("Student"))
                    {
                        return RedirectToAction("Student", "Profile");
                    }

                    return RedirectToAction("Index", "Students", new { message = "Студент успішно відредагований!" });
                }
                else if(!resU && !resS)
                {
                    return RedirectToAction("Index", "Students", new { error = "Сталася невідома помилка при редагуванні студента!" });
                }
            }

            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var res = await _studentsRepository.Remove(id);

                if (res)
                {
                    return RedirectToAction("Index", "Students", new { message = "Студент успішно видалений!" });
                }

                return RedirectToAction("Index", "Students", new { error = "СТалася невідома помилка при видаленні студента!" });
            } 
            catch
            {
                return RedirectToAction("Index", "Students", new { error = "Студент задіяний на курсі, спершу видаліть курс!" });
            }           
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GrantAccess(int id)
        {
            var res = await _studentsRepository.ChangeAccess(id, true);

            if (res)
            {
                return RedirectToAction("Index", "Students", new { message = "Студент успішно підтверджений!" });
            }

            return RedirectToAction("Index", "Students", new { error = "Сталася невідома помилка при підтвердженні студента!" });
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> RevokeAccess(int id)
        {
            var res = await _studentsRepository.ChangeAccess(id, false);

            if (res)
            {
                return RedirectToAction("Index", "Students", new { message = "Ви успішно зняли доступ!" });
            }

            return RedirectToAction("Index", "Students", new { error = "Сталася невідома помилка при знятті доступу студента!" });
        }


        [HttpGet]
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> Setting(string id)
        {
            try
            {
                var student = await _studentsRepository.GetByUserId(id);
                if (student == null)
                {
                    return RedirectToAction("Index", "Profile", new { error = "Студента не знайдено!" });
                }
                StudentFormViewModel model = new StudentFormViewModel
                {
                    LastName = student.User.LastName,
                    FirstName = student.User.FirstName,
                    MiddleName = student.User.MiddleName,
                    PhoneNumber = student.User.PhoneNumber,
                    University = student.University,
                    Faculty = student.Faculty,
                    Email = student.User.Email,
                    Skills = student.Skills,
                    StudyYear = student.StudyYear.HasValue ? student.StudyYear.Value : 0,
                    StudentId = student.StudentId,
                    HasAccess = student.HasAccess.Value
                };

                return View(model);
            } catch
            {
                return RedirectToAction("Index", "Profile", new { error = "Студента не знайдено!" });
            }
        }
    }
}