using Luval.Data.Attributes;
using Luval.Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Luval.Blog.Entities
{
    public class BlogPostCategory : StringKeyAuditEntity
    {
        public string BlogPostId { get; set; }
        [TableReference]
        public BlogPost Post { get; set; }
        public string BlogCategoryId { get; set; }
        [TableReference]
        public BlogCategory Category { get; set; }
    }
}
