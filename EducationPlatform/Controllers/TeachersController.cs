using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using EducationPlatform.Models.ViewModels;
using EducationPlatform.Models;
using EducationPlatform.Services.Repositories;
using EducationPlatform.Models.Entities;
using EducationPlatform.Models.EntityModels;
using EducationPlatform.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace EducationPlatform.Controllers
{
    public class TeachersController : Controller
    {
        private readonly EducationPlatformContext _context;
        private readonly IUsersRepository _userRepository;

        public TeachersController(IUsersRepository userRepository, EducationPlatformContext context)
        {
            _context = context;
            _userRepository = userRepository;
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Index(string error = null, string message = null)
        {
            TeachersIndexViewModel list = new TeachersIndexViewModel();
            list.Teachers = new List<AspNetUsers>();
            //list.Teachers = _userRepository.GetUsersInRole("Teacher").ToList();
            List<AspNetUsers> users = _context.AspNetUsers.Include(x => x.AspNetUserRoles)
                .Where(x => x.AspNetUserRoles.First().Role.Name.Equals("Teacher") || x.AspNetUserRoles.First().Role.Name.Equals("Banned")).ToList();
            for(int i = 0; i < users.Count(); i++)
            {
                if (_context.AspNetUsers.Count() != 0 && _context.AspNetUsers.Include(x => x.Student).Where(x => x.Student != null).Contains(users[i]))
                    users.Remove(users[i]);
            }
            foreach (var user in users)
            {
                list.Teachers.Add(user);
            }
            if (error != null || message != null)
            {
                ViewBag.Error = error;
                ViewBag.Message = message;
            }
            return View(list);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(UserViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var res = await _userRepository.RegisterTeacher(model);

                    if (!res)
                    {
                        ModelState.AddModelError("Email", "Ця електронна адреса вже зайнята!");
                        return View(model);
                    }
                    return RedirectToAction("Index", "Teachers", new { message = "Викладач успішно створений!" });
                }
                catch
                {
                    ModelState.AddModelError("Email", "Ця електронна адреса вже зайнята!");
                    return View(model);
                }
            }

            return View(model);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        [Route("{controller}/{action}/{email}")]
        public async Task<IActionResult> Edit(string email)
        {
            if (email != null)
            {
                User user = await _userRepository.GetByEmail(email);

                if (user == null)
                {
                    return RedirectToAction("Index", "Teachers");
                }

                UserViewModel teacher = new UserViewModel
                {
                    Email = user.Email,
                    UserName = user.UserName,
                    LastName = user.LastName,
                    FirstName = user.FirstName,
                    MiddleName = user.MiddleName,
                    PhoneNumber = user.PhoneNumber,
                    IsBanned = (bool)user.IsBanned
                };

                return View(teacher);
            }
            return RedirectToAction("Index", "Teachers");
        }

        [HttpPost]
        [Authorize(Roles = "Admin, Teacher")]
        public async Task<IActionResult> Edit(UserViewModel model)
        {
            var res = await _userRepository.Update(model);
            if (res && ModelState.IsValid)
            {
                if (User.IsInRole("Teacher"))
                {
                    return RedirectToAction("Teacher", "Profile");
                }
                if (User.IsInRole("Admin"))
                {
                    return RedirectToAction("Admin", "Profile");
                }
                return RedirectToAction("Index", "Teachers", new { message = "Викладач успішно відредагований!" });
            }
            else
            {
                return RedirectToAction("Index", "Teachers", new { error = "Сталася невідома помилка при редагуванні викладача!" });
            }
        }


        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(string email)
        {
            try
            {
                var res = await _userRepository.Remove(email);
                if (res)
                {
                    return RedirectToAction("Index", "Teachers", new { message = "Викладач успішно видалений!" });
                }
                else
                {
                    return RedirectToAction("Index", "Teachers", new { error = "Сталася невідома помилка при видаленні викладача!" });
                }
            }
            catch
            {
                return RedirectToAction("Index", "Teachers", new { error = "Даний викладач уже викладає на курсах, спершу змініть викладача на існуючих курсах!" });
            }

            
        }

        [HttpGet]
        [Authorize(Roles = "Admin, Teacher")]
        [Route("{controller}/{action}/{email}")]
        public async Task<IActionResult> Setting(string email )
        {
            string role = "";
            var isAdmin = User.IsInRole("Admin");
            if (isAdmin)
            {
                role = "Admin";
            }
            else
            {
                role = "Teacher";
            }
           
            if (email != null)
            {
                User user = await _userRepository.GetByEmail(email);
                if (user == null)
                {
                    return RedirectToAction(role, "Profile", new { error = "Користувача не знайдено!" });
                }

                UserViewModel teacher = new UserViewModel
                {
                    Email = user.Email,
                    UserName = user.UserName,
                    LastName = user.LastName,
                    FirstName = user.FirstName,
                    MiddleName = user.MiddleName,
                    PhoneNumber = user.PhoneNumber
                };
                return View(teacher);
            }
            return RedirectToAction(role, "Profile", new { error = "Користувача не знайдено!" });
        }
    }
}

