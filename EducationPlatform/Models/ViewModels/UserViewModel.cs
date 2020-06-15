using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EducationPlatform.Models.ViewModels
{
    public class UserViewModel
    {
        public string Email { get; set; }
        public string UserName { get; set; }

        [Required(ErrorMessage = "Поле Прізвище обов'язкове.")]
        [Display(Name = "Прізвище")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Поле Ім'я обов'язкове.")]
        [Display(Name = "Ім'я")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Поле По Батькові обов'язкове.")]
        [Display(Name = "По Батькові")]
        public string MiddleName { get; set; }

        [Required(ErrorMessage = "Поле Номер телефону обов'язкове.")]
        [Display(Name = " Номер телефону")]
        public string PhoneNumber { get; set; }

        public string Password { get; set; }
        public bool IsBanned { get; set; }
    }
}
