using EducationPlatform.Models.EntityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EducationPlatform.Models.ViewModels
{
    public class ModuleFileListViewModel
    {
        public List<ModuleFile> Files { get; set; }
        public Module Module { get; set; }
    }
}
