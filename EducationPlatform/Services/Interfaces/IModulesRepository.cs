using EducationPlatform.Models.EntityModels;
using EducationPlatform.Models.ViewModels;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EducationPlatform.Services.Interfaces
{
    public interface IModulesRepository
    {
        Task Insert(ModuleViewModel model);
        ModuleViewModel GetById(int id);
        Task Update(ModuleViewModel model);
        IEnumerable<ModuleViewModel> GetModulesBySubjectId(int id);
        Task Delete(int id);
        Task<bool> SaveFilesToModules(List<IFormFile> files, int moduleId);
        Task<bool> RemoveFiles(int moduleId);
        Task<List<ModuleFile>> GetModuleFiles(int moduleId);
    }
}
