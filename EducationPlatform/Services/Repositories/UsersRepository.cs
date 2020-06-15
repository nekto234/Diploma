using EducationPlatform.Models.Entities;
using EducationPlatform.Models.EntityModels;
using EducationPlatform.Models.ViewModels;
using EducationPlatform.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EducationPlatform.Services.Repositories
{
    public class UsersRepository : IUsersRepository
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly EducationPlatformContext _context;
        private readonly IEmailSender _emailSender;
        private readonly LinkGenerator _urlHelper;
        private readonly IHttpContextAccessor _httpContext;

        public UsersRepository(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            EducationPlatformContext context,
            IEmailSender emailSender,
            LinkGenerator urlHelper,
            IHttpContextAccessor httpContext
        )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
            _emailSender = emailSender;
            _urlHelper = urlHelper;
            _httpContext = httpContext;
        }

        public async Task<bool> RegisterStudent(UserViewModel user, StudentViewModel model)
        {
            await RegisterUser(user, "Student");

            var userFromDb = _context.AspNetUsers.Where(x => x.Email == user.Email).FirstOrDefault();

            _context.Student.Add(new Student
            {
                User = userFromDb,
                University = model.University,
                Faculty = model.Faculty,
                StudyYear = model.StudyYear,
                Skills = model.Skills,
                HasAccess = model.HasAccess
            });

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> RegisterUser(UserViewModel model, string role)
        {
            User user = new User
            {
                Email = model.Email,
                UserName = model.Email,
                LastName = model.LastName,
                FirstName = model.FirstName,
                MiddleName = model.MiddleName,
                PhoneNumber = model.PhoneNumber,
                IsBanned = model.IsBanned
            };

            IdentityResult resultCreate;

            if (model.Password != null)
            {
                resultCreate = await _userManager.CreateAsync(user, model.Password);
            } else
            {
                resultCreate = await _userManager.CreateAsync(user);
            }

            if (resultCreate.Succeeded)
            {
                var userRoleId = _context.AspNetRoles.First(x => x.Name == role).Id;

                AspNetUserRoles userRoles = new AspNetUserRoles
                {
                    UserId = user.Id,
                    RoleId = userRoleId
                };

                _context.AspNetUserRoles.Add(userRoles);

                await _context.SaveChangesAsync();

                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var callbackUrl = _urlHelper.GetUriByAction(_httpContext.HttpContext, "ConfirmEmail", "Account", new { userId = user.Id, code, email = user.Email });

                await _emailSender.SendEmailAsync(user.Email, "Підтвердження аккаунту", $"Перейдіть за посиланням: <a href=\"{ callbackUrl }\">Активувати!</a>");

                return true;
            }

            return false;
        }

        public async Task<bool> Update(UserViewModel model)
        {
            var user = _context.AspNetUsers.Where(x => x.Email == model.Email).FirstOrDefault();

            if (user != null)
            {
                user.LastName = model.LastName;
                user.FirstName = model.FirstName;
                user.MiddleName = model.MiddleName;
                user.PhoneNumber = model.PhoneNumber;
                user.IsBanned = model.IsBanned;

                _context.AspNetUsers.Update(user);

                return await _context.SaveChangesAsync() > 0;
            }

            return false;
        }

        public async Task<bool> BlockUser(string id)
        {
            return await SetBan(true, id);
        }

        public async Task<bool> UnBlockUser(string id)
        {
            return await SetBan(false, id);
        }

        private async Task<bool> SetBan(bool value, string id)
        {
            var user = _context.AspNetUsers.Where(x => x.Id == id).FirstOrDefault();

            if (user != null)
            {
                user.IsBanned = value;

                _context.AspNetUserRoles.RemoveRange(_context.AspNetUserRoles.Where(x => x.UserId == id).ToList());

                if (value)
                {
                    var role = _context.AspNetRoles.Where(x => x.Name == "Banned").First();
                   
                    _context.AspNetUserRoles.Add(new AspNetUserRoles
                    {
                        RoleId = role.Id,
                        UserId = id
                    });
                } 
                else
                {
                    var isStudent = await _context.Student.Select(x => x.UserId).ContainsAsync(id);

                    var roleName = isStudent ? "Student" : "Teacher";

                    var role = _context.AspNetRoles.Where(x => x.Name == roleName).First();

                    _context.AspNetUserRoles.Add(new AspNetUserRoles
                    {
                        RoleId = role.Id,
                        UserId = id
                    });

                }

                _context.AspNetUsers.Update(user);

                return await _context.SaveChangesAsync() > 0;
            }

            return false;
        }


        public async Task<bool> RegisterTeacher(UserViewModel model)
        {
            return await RegisterUser(model, "Teacher");
        }

        public IQueryable<AspNetUsers> GetUsersInRole(string roleName)
        {
            return _context.AspNetUsers.Where(x => x.AspNetUserRoles.Any(y => y.Role.Name == roleName));

        }

        public IQueryable<AspNetUsers> GetNotBannedUsersInRole(string roleName)
        {
            return _context.AspNetUsers.Where(x => x.AspNetUserRoles.Any(y => y.Role.Name == roleName)).Where(x => x.IsBanned == false || x.IsBanned == null);
        }

        public async Task<User> GetByEmail(string email)
        {
            return await _userManager.FindByEmailAsync(email);
        }

        public async Task<bool> Remove(string email)
        {
            var user = await GetByEmail(email);
            if (user != null)
            {
                await _userManager.DeleteAsync(await _userManager.FindByEmailAsync(email));
                return true;
            }
            return false;
        }

    }
}
