using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace EducationPlatform.Models.ViewModels
{
    public class StudentFormViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Поле Прізвище обов'язкове.")]
        [Display(Name ="Прізвище")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Поле Ім'я обов'язкове.")]
        [Display(Name = "ім'я")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Поле По батькові обов'язкове.")]
        [Display(Name = "По батькові")]
        public string MiddleName { get; set; }

        [Required(ErrorMessage = "Поле Email обов'язкове.")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Display(Name = "Номер телефону")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Поле Університет обов'язкове.")]
        [Display(Name = "Університет")]
        public string University { get; set; }


        [Required(ErrorMessage = "Поле Факультет обов'язкове.")]
        [Display(Name = "Факультет")]
        public string Faculty { get; set; }

        [Required(ErrorMessage = "Поле Курс обов'язкове.")]
        [Display(Name = "Курс")]
        public int StudyYear { get; set;  }


        [Required(ErrorMessage = "Поле Навички обов'язкове.")]
        [Display(Name = "Навички")]
        public string Skills { get; set; }

        [Required(ErrorMessage = "Поле Пароль є обов'язкове.")]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Поле Підтвердження паролю є обов'язкове.")]
        [Compare("Password", ErrorMessage = "Паролі не збігаються.")]
        [DataType(DataType.Password)]
        [Display(Name = "Підтвердження паролю")]
        public string ConfirmPassword { get; set; }


        [Display(Name = "Користувача заблоковано")]
        public bool IsBan { get; set; }

        [Display(Name = "Наявність доступу до ресурсу")]
        public bool HasAccess { get; set; }
        public int StudentId { get; internal set; }
    }
}
