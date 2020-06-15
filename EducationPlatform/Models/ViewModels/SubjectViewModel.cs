using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using EducationPlatform.Models.EntityModels;

namespace EducationPlatform.Models.ViewModels
{
    public class SubjectViewModel
    {
        public int SubjectId { get; set; }
        [Required(ErrorMessage = "Поле Назва є обов'язкове.")]
        [Display(Name = "Назва")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Поле Опис є обов'язкове.")]
        [Display(Name = "Опис")]
        public string Description { get; set; }

        public IEnumerable<Module> Module  { get; set; }
    }
}
