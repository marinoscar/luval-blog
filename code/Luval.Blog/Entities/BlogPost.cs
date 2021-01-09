using Luval.Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Luval.Blog.Entities
{
    public class BlogPost : StringKeyAuditEntity
    {
        public string Title { get; set; }
        public string Slug { get; set; }
        public string Tags { get; set; }
        public DateTime? UtcPublishDate { get; set; }
        public string Content { get; set; }
    }
}
