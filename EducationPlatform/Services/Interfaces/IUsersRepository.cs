using EducationPlatform.Models.Entities;
using EducationPlatform.Models.EntityModels;
using EducationPlatform.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EducationPlatform.Services.Interfaces
{
    public interface IUsersRepository
    {
        Task<bool> RegisterUser(UserViewModel model, string role);
        Task<bool> RegisterStudent(UserViewModel user, StudentViewModel model);
        Task<bool> RegisterTeacher(UserViewModel model);
        Task<bool> BlockUser(string id);
        Task<bool> UnBlockUser(string id);
        Task<bool> Update(UserViewModel model);
        IQueryable<AspNetUsers> GetUsersInRole(string roleName);
        Task<bool> Remove(string email);
        Task<User> GetByEmail(string email);
        IQueryable<AspNetUsers> GetNotBannedUsersInRole(string roleName);
    }
}
