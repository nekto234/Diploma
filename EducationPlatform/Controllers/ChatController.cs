using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EducationPlatform.Models.Entities;
using EducationPlatform.Models.EntityModels;
using EducationPlatform.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace EducationPlatform.Controllers
{
    [Authorize]
    public class ChatController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly EducationPlatformContext _context;

        public ChatController(
            UserManager<User> userManager,
            EducationPlatformContext context
        )
        {
            _userManager = userManager;
            _context = context;
        }

        [Authorize(Roles = "Admin,Teacher,Student")]
        public IActionResult Index()
        {
            var model = new ChatViewModel
            {
                UserId = _userManager.GetUserId(User),
                Users = _context.OnlineList.Select(x => new
                {
                    user = _userManager.FindByIdAsync(x.UserId).Result
                }).Select(x => x.user).ToList(),
                Messages = _context.Chat.Select(x => new MessageViewModel
                {
                    user = _userManager.FindByIdAsync(x.UserId).Result,
                    message = x.Message,
                    date = x.Date
                }).OrderBy(x => x.date).ToList()
            };

            return View(model);
        }
    }
}