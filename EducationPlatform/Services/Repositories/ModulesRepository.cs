using EducationPlatform.Models.EntityModels;
using EducationPlatform.Models.ViewModels;
using EducationPlatform.Services.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace EducationPlatform.Services.Repositories
{
    public class ModulesRepository : IModulesRepository
    {
        private EducationPlatformContext _context;
        private IHostingEnvironment _appEnvironment;

        public ModulesRepository(
            EducationPlatformContext context, 
            IHostingEnvironment appEnvironment
        )
        {
            _context = context;
            _appEnvironment = appEnvironment;
        }
        public async Task Insert(ModuleViewModel model)
        {
            var mod = new Module
            {
                Name = model.Name,
                Description = model.Description,
                HasTest = model.HasTest,
                HasLab = model.HasLab,
                MinTestMark = model.MinTestMark,
                MaxTestMark = model.MaxTestMark,
                MinLabMark = model.MinLabMark,
                MaxLabMark = model.MaxLabMark,
                SubjectId = model.SubjectId
            };
            _context.Module.Add(mod);
            await _context.SaveChangesAsync();
        }

        public ModuleViewModel GetById(int id)
        {
            Module moduleFromDB = _context.Module.Include(x => x.Subject).Where(x => x.ModuleId == id).FirstOrDefault();
            return new ModuleViewModel()
            {
                ModuleId = moduleFromDB.ModuleId,
                Name = moduleFromDB.Name,
                Description = moduleFromDB.Description,
                HasTest = moduleFromDB.HasTest,
                HasLab = moduleFromDB.HasLab,
                MinTestMark = moduleFromDB.MinTestMark,
                MaxTestMark = moduleFromDB.MaxTestMark,
                MinLabMark = moduleFromDB.MinLabMark,
                MaxLabMark = moduleFromDB.MaxLabMark,
                Subject = moduleFromDB.Subject
            };
        }
        public async Task Update(ModuleViewModel model)
        {
            var module = _context.Module.Where(m => m.ModuleId == model.ModuleId).FirstOrDefault();

            if (module != null)
            {
                module.Name = model.Name;
                module.Description = model.Description;
                module.HasTest = model.HasTest;
                module.HasLab = model.HasLab;
                module.MinTestMark = model.MinTestMark;
                module.MaxTestMark = model.MaxTestMark;
                module.MinLabMark = model.MinLabMark;
                module.MaxLabMark = model.MaxLabMark;
                
                _context.Module.Update(module);

                await SaveFilesToModules(model.Files, model.ModuleId);

                await _context.SaveChangesAsync();
            }
           
        }
        public IEnumerable<ModuleViewModel> GetModulesBySubjectId(int id)
        {
            List<ModuleViewModel> modules = new List<ModuleViewModel>();
            foreach (Module model in _context.Module.Include(x => x.Subject).Include(x => x.ModuleFile).Where(x => x.SubjectId == id))
            {
                modules.Add(new ModuleViewModel
                {
                    ModuleId = model.ModuleId,
                    Name = model.Name,
                    Description = model.Description,
                    HasTest = model.HasTest,
                    HasLab = model.HasLab,
                    MinTestMark = model.MinTestMark,
                    MaxTestMark = model.MaxTestMark,
                    MinLabMark = model.MinLabMark,
                    MaxLabMark = model.MaxLabMark,
                    Subject = model.Subject,
                    SubjectId = model.SubjectId,
                    CountFiles = model.ModuleFile.Count
                    
                });
            }
            return modules;
        }
        public async Task Delete(int id)
        {
            Module moduleFromDB = _context.Module.Where(x => x.ModuleId == id).FirstOrDefault();

            await RemoveFiles(id);

            _context.Module.Remove(moduleFromDB);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> RemoveFiles(int moduleId)
        {
            var filesFromDB = await _context.ModuleFile.Where(x => x.ModuleId == moduleId).ToListAsync();

            filesFromDB.ForEach(ffs => {
                if (File.Exists(_appEnvironment.WebRootPath + ffs.FileUrl))
                {
                    File.Delete(_appEnvironment.WebRootPath + ffs.FileUrl);
                }
            });

            _context.ModuleFile.RemoveRange(filesFromDB);

            return true;
        }

        public async Task<bool> SaveFilesToModules(List<IFormFile> files, int moduleId)
        {
            try
            {
                if (moduleId > 0)
                {
                    await this.RemoveFiles(moduleId);
                }

                if (files != null && files.Count > 0)
                {
                    files.ForEach(async _file => {
                        string path = "/ServerFiles/" + _file.FileName;
                        // сохраняем файл в папку Files в каталоге wwwroot
                        using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
                        {
                            await _file.CopyToAsync(fileStream);
                        }


                        ModuleFile moduleFile = new ModuleFile { FileUrl = path, ModuleId = moduleId };

                        _context.ModuleFile.Add(moduleFile);
                    });
                }

                await _context.SaveChangesAsync();

                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<List<ModuleFile>> GetModuleFiles(int moduleId)
        {
            return await _context.ModuleFile.Where(x => x.ModuleId == moduleId).ToListAsync();
        }
    }
}
