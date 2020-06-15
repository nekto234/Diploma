using EducationPlatform.Models.Entities;
using EducationPlatform.Models.EntityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EducationPlatform.Models.ViewModels
{
    public class ChatViewModel
    {
        public string UserId { get; set; }
        public List<User> Users { get; set; }
        public List<MessageViewModel> Messages { get; set; }
    }
}
