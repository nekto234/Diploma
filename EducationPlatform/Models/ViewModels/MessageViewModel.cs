using EducationPlatform.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EducationPlatform.Models.ViewModels
{
    public class MessageViewModel
    {
        public User user { get; set; }
        public string message { get; set; }
        public DateTime date { get; set; }
    }
}
