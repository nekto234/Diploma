using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using EducationPlatform.Models.Entities;
using EducationPlatform.Models.EntityModels;
using EducationPlatform.Models.ViewModels;
using EducationPlatform.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EducationPlatform.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly EducationPlatformContext _context;
        private readonly IEmailSender _emailSender;
        private readonly IUsersRepository _usersRepository;

        public AccountController(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            EducationPlatformContext context,
            IEmailSender emailSender,
            IUsersRepository usersRepository
        )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
            _emailSender = emailSender;
            _usersRepository = usersRepository;
        }

        [HttpGet]
        public IActionResult Register()
        {
            if (User.Identity.IsAuthenticated)
            {
                if (User.IsInRole("Admin"))
                {
                    return RedirectToAction("Admin", "Profile");
                }

                if (User.IsInRole("Student"))
                {
                    return RedirectToAction("Student", "Profile");
                }

                if (User.IsInRole("Teacher"))
                {
                    return RedirectToAction("Teacher", "Profile");
                }
            }

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _usersRepository.RegisterStudent(
                        new UserViewModel
                        {
                            Email = model.Email,
                            FirstName = model.FirstName,
                            LastName = model.LastName,
                            MiddleName = model.MiddleName,
                            Password = model.Password,
                            PhoneNumber = model.PhoneNumber,
                            UserName = model.Email,
                            IsBanned = false
                        },
                        new StudentViewModel
                        {
                            University = model.University,
                            Faculty = model.Faculty,
                            StudyYear = model.StudyYear,
                            Skills = model.Skills,
                            HasAccess = false
                        }
                    );
                }
                catch
                {
                    ModelState.AddModelError("Email", "Ця електронна адреса вже зайнята!");
                    return View(model);
                }


                return RedirectToAction("RegisterSuccess", "Account");
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Login(string returnUrl = null, string error = null)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (User.IsInRole("Admin"))
                {
                    return RedirectToAction("Index", "Courses");
                }

                if (User.IsInRole("Student"))
                {
                    return RedirectToAction("Student", "Profile");
                }

                if (User.IsInRole("Teacher"))
                {
                    return RedirectToAction("Teacher", "Profile");
                }
            }

            if (error != null)
            {
                ModelState.AddModelError("", error);
            }
            return View(new LoginViewModel { ReturnUrl = returnUrl });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result =
                    await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);
                if (result.Succeeded)
                {
                    var user = await _usersRepository.GetByEmail(model.Email);
                    if (user.IsBanned.HasValue && user.IsBanned.Value == true)
                    {
                        await _signInManager.SignOutAsync();
                       
                        return RedirectToAction("Banned", "Account");
                    }

                    var twoFactor = _context.TwoFactorUser.FirstOrDefault(x => x.UserId == user.Id);

                    if (twoFactor != null)
                    {
                        Random rand = new Random();
                        int code = rand.Next(1000, 10000);
                        twoFactor.Code = code;

                        await _context.SaveChangesAsync();
                        await _emailSender.SendEmailAsync(user.Email, "Код авторизації", $"{twoFactor.Code}");
                        return RedirectToAction("CheckValidationCode", "Account", new { email = user.Email });
                    }
                    return RedirectToAction("RoleChecker", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Неправильний логін чи пароль!");
                }
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Banned()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            // удаляем аутентификационные куки
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> ConfirmEmail(string userId, string code, string email)
        {
            if (userId == null || code == null)
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{userId}'.");
            }
            var result = await _userManager.ConfirmEmailAsync(user, code);
            if (result.Succeeded)
            {
                await _emailSender.SendEmailAsync(email, "Реєстрація", $"Аккаунт упішно підтверджений!");
            }
            return View(result.Succeeded ? "ConfirmEmail" : "Error");
        }

        [HttpGet]
        public IActionResult CheckValidationCode(string email)
        {
            ViewBag.Email = email;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> TwoFactorAuth(TwoFactorViewModel model)
        {
            var twofactor = _context.TwoFactorUser.Include(x => x.User).Where(x => x.User.Email == model.Email).FirstOrDefault();
            if (twofactor != null)
            {
                if (twofactor.Code.HasValue)
                {
                    if (twofactor.Code.Value == model.Code)
                    {
                        return RedirectToAction("RoleChecker", "Home");
                    }
                }
            }

            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> SetTwoFactorAuthorization(bool grant)
        {
            var user =  await _userManager.GetUserAsync(User);
            var twofactoruser = _context.TwoFactorUser.Include(x => x.User).Where(x => x.User.Email == User.Identity.Name).FirstOrDefault();
            if (grant == true)
            {
                if(twofactoruser == null)
                {
                    _context.TwoFactorUser.Add(new TwoFactorUser 
                    { 
                        UserId = user.Id
                    });
                   await _context.SaveChangesAsync();
                }
            }
            else
            {
                if(twofactoruser!=null)
                {
                    _context.TwoFactorUser.Remove(twofactoruser);
                    await _context.SaveChangesAsync();
                }
            }
            return RedirectToAction("RoleChecker", "Home");
        }



        [HttpGet]
        [AllowAnonymous]
        public IActionResult ForgotPassword(string error = "")
        {
            ViewBag.Error = error;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user == null)
                {
                    return RedirectToAction("ForgotPassword", new { error = "Користувач з таким email не знайдений." });
                }
                var code = await _userManager.GeneratePasswordResetTokenAsync(user);

                var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code, email = user.Email }, protocol: HttpContext.Request.Scheme);
                await _emailSender.SendEmailAsync(model.Email, "Відновлення паролю", $"Щоб відновити пароль перейдіть за посиланням: <a href={callbackUrl}>Відновити!</a>");

                return View("ForgotPasswordSuccess");
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ResetPassword(string email, string code = null)
        {
            if (code == null)
            {
                return RedirectToAction("ForgotPassword", new { error = "Виникла проблема із посиланням, спробуйте ще раз." });
            }
            var model = new ResetPasswordViewModel { Code = code, Email = email };
            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model, string code = null)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            model.Email = user.Email;

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (user == null)
            {
                return RedirectToAction("ForgotPassword", new { error = "Користувач з таким email не знайдений." });
            }
            var result = await _userManager.ResetPasswordAsync(user, model.Code, model.Password);
            if (result.Succeeded)
            {
                await _emailSender.SendEmailAsync(model.Email, "Відновлення паролю", $"Ви успішно змінили свій пароль!");
                return RedirectToAction("ResetPasswordSuccess", "Account");
            }

            ViewBag.Errors = result.Errors.Select(x => x.Description).ToList();

            return View(model);
        }

        [HttpGet]
        public IActionResult ForgotPasswordSuccess()
        {
            return View();
        }

        [HttpGet]
        public IActionResult ResetPasswordSuccess()
        {
            return View();
        }

        [HttpGet]
        public IActionResult RegisterSuccess()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public IActionResult ExternalLogin(string provider, string returnUrl = null)
        {
            var redirectUrl = Url.Action(nameof(ExternalLoginCallback), "Account", new { returnUrl });
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            return Challenge(properties, provider);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ExternalLoginCallback(string returnUrl = null, string remoteError = null)
        {
            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                return RedirectToAction(nameof(Login));
            }

            ViewData["LoginProvider"] = info.LoginProvider;
            var email = info.Principal.FindFirstValue(ClaimTypes.Email);

            var name = info.Principal.FindFirstValue(ClaimTypes.Name).Split(" ");
            var firstname = name[0];
            var surname = name[1];
            var country = info.Principal.FindFirstValue(ClaimTypes.Country);

            var user = await _usersRepository.GetByEmail(email);

            string userEmail = null;

            if (user != null)
            {
                userEmail = user.Email;
            }

            if (userEmail == null)
            {

                try
                {
                    await _usersRepository.RegisterStudent(
                        new UserViewModel
                        {
                            Email = email,
                            FirstName = firstname,
                            LastName = surname,
                            MiddleName = null,
                            UserName = email,
                            IsBanned = false,
                        },
                        new StudentViewModel
                        {
                            University = null,
                            Faculty = null,
                            StudyYear = null,
                            Skills = null,
                            HasAccess = false
                        }
                    );
                }
                catch
                {
                    return RedirectToAction("Login", new { error = "Електронна адреса вже зайнята!" });
                }

                userEmail = email;
            }
            else
            {
                if (user.IsBanned.HasValue && user.IsBanned.Value == true)
                {
                    await _signInManager.SignOutAsync();

                    return RedirectToAction("Banned", "Account");
                }

                if (user.PasswordHash != null)
                {
                    return RedirectToAction("Login", new { error = "Електронна адреса вже зайнята!" });
                }
            }

            var userId = _context.AspNetUsers.Where(c => c.Email == email).Select(c => c.Id).FirstOrDefault();
            var logProv = _context.AspNetUserLogins.Where(c => c.UserId == userId).Select(c => c.LoginProvider).FirstOrDefault();
            var provKey = _context.AspNetUserLogins.Where(c => c.UserId == userId).Select(c => c.ProviderKey).FirstOrDefault();

            if (logProv == null || provKey == null)
            {
                AspNetUserLogins nLog = new AspNetUserLogins
                {
                    LoginProvider = info.LoginProvider,
                    ProviderKey = info.ProviderKey,
                    ProviderDisplayName = info.ProviderDisplayName,
                    UserId = userId
                };

                _context.AspNetUserLogins.Add(nLog);
                _context.SaveChanges();

                logProv = info.LoginProvider;
                provKey = info.ProviderKey;
            }

            var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false, bypassTwoFactor: true);

            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }

            return RedirectToAction("Login", new { error = "Сталася невідома помилка, спробуйте пізніше!" });

        }

        [HttpPost]
        public async Task<IActionResult> Block(string id, string page)
        {
            var result = await _usersRepository.BlockUser(id);
            if (!result)
            {
                return RedirectToAction("Index", page, new { error = "Користувача не вадлося заблокувати!" });
            }
            return RedirectToAction("Index", page, new { message = "Користувач успішно заблокований!" });
        }

        [HttpPost]
        public async Task<IActionResult> UnBlock(string id, string page)
        {
            var result = await _usersRepository.UnBlockUser(id);
            if (!result)
            {
                return RedirectToAction("Index", page, new { error = "Користувача не вадлося розблокувати!" });
            }
            return RedirectToAction("Index", page, new { message = "Користувач успішно розблокований!" });
        }

        

    }
}