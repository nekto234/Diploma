using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EducationPlatform.Models.ViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Поле Email є обов'язкове.")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Поле Пароль є обов'язкове.")]
        [StringLength(30, MinimumLength = 4, ErrorMessage = "Пароль занаддто короткий")]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Поле Підтвердження паролю є обов'язкове.")]
        [Compare("Password", ErrorMessage = "Паролі не збігаються.")]
        [StringLength(30, MinimumLength =4, ErrorMessage ="Пароль занад то короткий")]
        [DataType(DataType.Password)]
        [Display(Name = "Підтвердження паролю")]
        public string PasswordConfirm { get; set; }

        [Display(Name = "Номер телефону")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Поле Прізвище є обов'язкове.")]
        [Display(Name = "Прізвище")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Поле Ім'я є обов'язкове.")]
        [Display(Name = "Ім'я")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Поле По батькові є обов'язкове.")]
        [Display(Name = "По батькові")]
        public string MiddleName { get; set; }

        [Required(ErrorMessage = "Поле Університет є обов'язкове.")]
        [Display(Name = "Університет")]
        public string University { get; set; }

        [Required(ErrorMessage = "Поле Факультет є обов'язкове.")]
        [Display(Name = "Факультет")]
        public string Faculty { get; set; }

        [Required(ErrorMessage = "Поле Курс є обов'язкове.")]
        [Display(Name = "Курс")]
        public int StudyYear { get; set; }

        [Required(ErrorMessage = "Поле Навички є обов'язкове.")]
        [Display(Name = "Навички")]
        public string Skills { get; set; }
    }
}
