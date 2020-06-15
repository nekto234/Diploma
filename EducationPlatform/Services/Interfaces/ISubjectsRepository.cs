using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EducationPlatform.Models.ViewModels;
using Microsoft.AspNetCore.Http;

namespace EducationPlatform.Services.Interfaces
{
    public interface ISubjectsRepository
    {
        Task Insert(SubjectViewModel model);
        SubjectViewModel GetById(int id);
        Task Update(SubjectViewModel model);
        IEnumerable<SubjectViewModel> GetSubjects();
        Task Delete(int id);
    }
}
