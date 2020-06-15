using System;
using System.Collections.Generic;

namespace EducationPlatform.Models.EntityModels
{
    public partial class Comments
    {
        public int CommentId { get; set; }
        public int MarkId { get; set; }
        public string UserId { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string Comment { get; set; }

        public virtual Mark Mark { get; set; }
    }
}
