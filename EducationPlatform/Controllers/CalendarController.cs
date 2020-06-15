using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EducationPlatform.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EducationPlatform.Controllers
{
    [Authorize]
    public class CalendarController : Controller
    {
        private readonly ICoursesRepository _coursesRepository;
        private readonly IModulesRepository _modulesRepository;

        public CalendarController(
            ICoursesRepository coursesRepository,
            IModulesRepository modulesRepository
        )
        {
            _coursesRepository = coursesRepository;
            _modulesRepository = modulesRepository;
        }

        public IActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = "Admin,Teacher,Student")]
        public IActionResult CourseSchedule(int id)
        {
            var schedule = _coursesRepository.GetById(id);

            if (schedule != null)
            {
                var json = schedule.CourseModule.Select(x => new {
                    module = _modulesRepository.GetById(x.ModuleId),
                    start = x.Date.HasValue ? x.Date.Value.ToString("yyyy-MM-dd") : DateTime.MinValue.ToString("yyyy-MM-dd")
                }).Select(x => new {
                    title = x.module.Name,
                    description = x.module.Description,
                    x.start
                }).ToList();

                return Json(json);
            }

            return Json(new List<object>());
        }
    }
}