using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EducationPlatform.Models.ViewModels;
using EducationPlatform.Services.Repositories;
using Microsoft.AspNetCore.Mvc;
using EducationPlatform.Models.Entities;
using EducationPlatform.Models.EntityModels;
using EducationPlatform.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace EducationPlatform.Controllers
{
    public class SubjectsController : Controller
    {
        private readonly ISubjectsRepository _subjectRepository;
        private readonly IModulesRepository _moduleRepository;
        private readonly EducationPlatformContext _context;

        public SubjectsController(
            ISubjectsRepository subjectRepository,
            IModulesRepository modulesRepository,
            EducationPlatformContext context
        )
        {
            _subjectRepository = subjectRepository;
            _moduleRepository = modulesRepository;
            _context = context;
        }
        [Authorize(Roles = "Admin,Teacher")]
        public IActionResult Index(string error = null)
        {
            ViewBag.Error = error;
            return View(_subjectRepository.GetSubjects());
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Teacher")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Teacher")]
        public async Task<IActionResult> Create(SubjectViewModel model)
        {
            if (ModelState.IsValid)
            {
                await _subjectRepository.Insert(model);
                return RedirectToAction("Index", "Subjects");
            }
            return View(model);
        }
        [Authorize(Roles = "Admin,Teacher")]
        public IActionResult Edit(int id)
        {
            SubjectViewModel subjectViewModel = _subjectRepository.GetById(id);
            return View("Edit", _subjectRepository.GetById(id));
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Teacher")]
        public async Task<IActionResult> Edit(SubjectViewModel model)
        {
            if (ModelState.IsValid)
            {
                await _subjectRepository.Update(model);
                return RedirectToAction("Index", "Subjects");
            }
            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Teacher")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _subjectRepository.Delete(id);
                return RedirectToAction("Index", "Subjects");
            }
            catch
            {
                return RedirectToAction("Index", "Subjects", new { error = "Даний предмет уже пов'язаний із курсом, спершу видаліть курс!" });
            }

        }

        [Authorize(Roles = "Admin,Teacher")]
        [Route("{controller}/{subjectId}/Modules")]
        public IActionResult Modules(int subjectId, string error)
        {
            ViewBag.Error = error;
            ViewBag.SubjectId = subjectId;
            List<ModuleViewModel> modules = _moduleRepository.GetModulesBySubjectId(subjectId).ToList();
            return View(modules);
        }

        [HttpGet]
        [Route("{controller}/{action}/{moduleId}")]
        public async Task<IActionResult> ModuleFiles(int moduleId)
        {
            var module = _context.Module.Where(x => x.ModuleId == moduleId).FirstOrDefault();

            if (module == null)
            {
                return RedirectToAction("Index", "Subjects");
            }
            
            var fileList = await _moduleRepository.GetModuleFiles(moduleId);
            var model = new ModuleFileListViewModel
            {
                Files = fileList,
                Module = module
            };
            return View(model);
        }

        [Authorize(Roles = "Admin,Teacher")]
        [Route("{controller}/{action}/{subjectId}")]
        public IActionResult CreateModule(int subjectId)
        {
            SubjectViewModel subject = _subjectRepository.GetById(subjectId);

            if (subject != null)
            {
                ViewBag.SubjectId = subjectId;
                return View();
            }

            return RedirectToAction("Index", "Subjects", new { error = "Предмет не знайдено" });
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Teacher")]
        [Route("{controller}/{action}/{subjectId}")]
        public async Task<IActionResult> CreateModule(ModuleViewModel model)
        {
            if (ModelState.IsValid)
            {
                await _moduleRepository.Insert(model);
                return RedirectToAction("Modules", "Subjects", new { subjectId = model.SubjectId });
            }
            return View(model);
        }

        [Authorize(Roles = "Admin,Teacher")]
        [Route("{controller}/{action}/{moduleId}")]
        public IActionResult EditModule(int moduleId)
        {
            ModuleViewModel module = _moduleRepository.GetById(moduleId);
            return View(module);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Teacher")]
        [Route("{controller}/{action}/{moduleId}")]
        public async Task<IActionResult> EditModule(ModuleViewModel model)
        {
            if (ModelState.IsValid)
            {
                await _moduleRepository.Update(model);
                return RedirectToAction("Modules", "Subjects", new { subjectId = model.Subject.SubjectId });
            }
            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Teacher")]
        [Route("{controller}/{action}/{moduleId}")]
        public async Task<IActionResult> DeleteModule(int moduleId, int subjectId)
        {
            try
            {
                await _moduleRepository.Delete(moduleId);
                return RedirectToAction("Modules", "Subjects", new { subjectId });
            }
            catch
            {
                return RedirectToAction("Modules", "Subjects", new { subjectId, error = "Ви не можете видалити цей модуль, оскільки він задіяний у курсі!" });
            }
        }
    }
}