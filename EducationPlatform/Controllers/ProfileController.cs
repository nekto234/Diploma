using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EducationPlatform.Models.EntityModels;
using EducationPlatform.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EducationPlatform.Controllers
{
    public class ProfileController : Controller
    {
        private readonly IStudentsRepository _studentRepository;
        private readonly IUsersRepository _usersRepository;
        private readonly EducationPlatformContext _context;

        public ProfileController(
            // user manager to find teacher
            IUsersRepository usersRepository,
            IStudentsRepository studentRepository,
            EducationPlatformContext context
        )
        {
            _usersRepository = usersRepository;
            _studentRepository = studentRepository;
            _context = context;
        }

        [Authorize(Roles = "Student")]
        public async Task<IActionResult> Student()
        {
            ViewBag.HasTwoFactor = _context.TwoFactorUser.Include(x => x.User).Where(x => x.User.Email == User.Identity.Name).FirstOrDefault() != null;           
            Student student = await _studentRepository.GetByEmail(User.Identity.Name);
            return View(student);
        }

        [Authorize(Roles = "Teacher")]
        public async Task<IActionResult> Teacher()
        {
            ViewBag.HasTwoFactor = _context.TwoFactorUser.Include(x => x.User).Where(x => x.User.Email == User.Identity.Name).FirstOrDefault() != null;
            var teacher = await _usersRepository.GetByEmail(User.Identity.Name);
            ViewBag.Courses = _context.Course.Where(c => c.TeacherId == teacher.Id).Count();
            return View(teacher);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Admin()
        {
            ViewBag.HasTwoFactor = _context.TwoFactorUser.Include(x => x.User).Where(x => x.User.Email == User.Identity.Name).FirstOrDefault() != null;
            var admin = await _usersRepository.GetByEmail(User.Identity.Name);
            return View(admin);
        }
    }
}