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
    [Authorize]
    public class StatisticsController : Controller
    {
        private readonly EducationPlatformContext _context;
        private readonly ICoursesRepository _coursesRepository;

        public StatisticsController(
            EducationPlatformContext context,
            ICoursesRepository coursesRepository
        )
        {
            _context = context;
            _coursesRepository = coursesRepository;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return Json(new int[0]);
        }

        [HttpGet]
        public async Task<IActionResult> GetCoursesBarData(int count = 10)
        {
            var json = await _context.Course
                .Include(x => x.CourseStudent)
                .Select(x => new
                {
                    x.Name,
                    x.CourseId,
                    Students = x.CourseStudent.Count
                })
                .ToListAsync();

            return Json(json.Take(count));
        }

        [HttpGet]
        public async Task<IActionResult> GetCoursesAvgMarksData(int count = 10)
        {
            var marks = from mark in _context.Mark.Take(count).Include(x => x.CourseModule)
                        group mark by mark.CourseModule.CourseId into m
                        select new
                        {
                            CourseId = m.Key,
                            AvgMark = m.Average(_m => (_m.LabMark ?? 0) + (_m.TestMark ?? 0))
                        };

            var json = marks.Select(x => new
            {
                course = _context.Course.First(y => y.CourseId == x.CourseId).Name,
                x.AvgMark,
            });

            return Json(await json.ToListAsync());
        }

        [HttpGet]
        public async Task<IActionResult> GetModulesAvgMarkData(int courseId)
        {
            var marks = from mark in _context.Mark.Include(x => x.CourseModule)
                        group mark by mark.CourseModule.ModuleId into m
                        select new
                        {
                            ModuleId = m.Key,
                            AvgMark = m.Average(_m => (_m.LabMark ?? 0) + (_m.TestMark ?? 0))
                        };
            var jsonData = marks.Select(x => new
            {
                module = _context.Module.First(y => y.ModuleId == x.ModuleId).Name,
                x.AvgMark,
            });

            return Json(await jsonData.ToListAsync());
        }

        [HttpGet]
        public async Task<IActionResult> GetCourseMarkForStudent(int courseId)
        {
            var marks = from mark in _context.Mark.Include(x => x.CourseModule).Include(x => x.CourseModule.Module)
                        group mark by mark.Student.StudentId into m
                        select new
                        {
                            studentId = m.Key,
                            procentRatin = (100 * m.Sum(x => x.LabMark + x.TestMark) / m.Sum(x => (x.CourseModule.Module.MaxLabMark ?? 0) + (x.CourseModule.Module.MaxTestMark ?? 0)))
                        };

            var json = marks.Select(x => new
            {
                student = _context.Student.First(z => z.StudentId == x.studentId).User,
                x.procentRatin
            });

            return Json(await json.ToListAsync());
        }

        [HttpGet]
        public async Task<IActionResult> MinMaxRating(int courseId)
        {
            var marks = _context.CourseModule.Include(x => x.Module).Where(x => x.CourseId == courseId).Select(x => new
            {
                Name = x.Module.Name,
                MaxLabMark = x.Module.MaxLabMark,
                MinLabMark = x.Module.MinLabMark,
                MaxTestMark = x.Module.MaxTestMark,
                MinTestMark = x.Module.MinTestMark
            });

            var json = marks.Select(x => new
            {
                x.Name,
                x.MaxLabMark,
                x.MinLabMark,
                x.MaxTestMark,
                x.MinTestMark
            });

            return Json(await json.ToListAsync());
        }
    }
}