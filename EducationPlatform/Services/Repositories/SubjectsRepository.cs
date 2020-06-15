using EducationPlatform.Models.ViewModels;
using EducationPlatform.Services.Interfaces;
using EducationPlatform.Models.EntityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace EducationPlatform.Services.Repositories
{
    public class SubjectsRepository : ISubjectsRepository
    {
        private EducationPlatformContext _context;

        public SubjectsRepository(EducationPlatformContext context)
        {
            _context = context;
        }
        public async Task Insert(SubjectViewModel model)
        {
            _context.Subject.Add(new Subject
            {
                Name = model.Name,
                Description = model.Description
            });
            await _context.SaveChangesAsync();
        }

        public SubjectViewModel GetById(int id)
        {
            Subject subjectFromDB = _context.Subject.Include(x => x.Module).Where(x => x.SubjectId == id).FirstOrDefault();

            if (subjectFromDB == null) return null;

            return new SubjectViewModel()
            {
                SubjectId = subjectFromDB.SubjectId,
                Name = subjectFromDB.Name,
                Description = subjectFromDB.Description
            };
        }
        public async Task Update(SubjectViewModel model)
        {
            _context.Subject.Update(new Subject()
            {
                SubjectId = model.SubjectId,
                Name = model.Name,
                Description = model.Description
            });
            await _context.SaveChangesAsync();
        }
        public IEnumerable<SubjectViewModel> GetSubjects()
        {
            List<SubjectViewModel> subjects = new List<SubjectViewModel>();
            foreach(Subject subject in _context.Subject.Include(x => x.Module).ToList<Subject>())
            {
                subjects.Add(new SubjectViewModel()
                {
                    SubjectId = subject.SubjectId,
                    Name = subject.Name,
                    Description = subject.Description,
                    Module = subject.Module
                });
            }
            return subjects;
        }
        public async Task Delete(int id)
        {
            Subject subjectFromDB = _context.Subject.Where(x => x.SubjectId == id).FirstOrDefault();
            _context.Subject.Remove(subjectFromDB);
            await _context.SaveChangesAsync();
        }
    }
}
