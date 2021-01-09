using Luval.Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Luval.Blog.Entities
{
    public class BlogAuthor : StringKeyAuditEntity
    {
        public string ProfilePicture { get; set; }
        public string Website { get; set; }
        public string Bio { get; set; }
    }
}
