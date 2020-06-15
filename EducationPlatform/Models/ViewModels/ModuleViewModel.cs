using EducationPlatform.Models.EntityModels;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EducationPlatform.Models.ViewModels
{
    public class ModuleViewModel
    {
        public int ModuleId { get; set; }

        [Required(ErrorMessage = "Поле Назва обов'язкове.")]
        [Display(Name = "Name")]
        public string Name { get; set; }
        public string Description { get; set; }
        public bool HasTest { get; set; }
        public bool HasLab { get; set; }
        public int? MinTestMark { get; set; }
        public int? MaxTestMark { get; set; }
        public int? MinLabMark { get; set; }
        public int? MaxLabMark { get; set; }
        public Subject Subject { get; set; }
        public int? SubjectId { get; set; }

        public List<IFormFile> Files { get; set; }
        public int CountFiles { get; internal set; }
    }
}
