using Luval.Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Luval.Blog.Entities
{
    public class BlogCategory : StringKeyAuditEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
