using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using EducationPlatform.Models;
using Microsoft.AspNetCore.Authorization;

namespace EducationPlatform.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("RoleChecker", "Home");
            }

            return View();
        }

        public IActionResult RoleChecker()
        {
            if (User.IsInRole("Student"))
            {
                return RedirectToAction("Student", "Profile");
            }

            if (User.IsInRole("Teacher"))
            {
                return RedirectToAction("Teacher", "Profile");
            }

            if (User.IsInRole("Admin"))
            {
                return RedirectToAction("Admin", "Profile");
            }

            return RedirectToAction("Student", "Profile");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
