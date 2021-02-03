using Luval.Blog.Entities;
using Luval.Blog.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luval.Blog.Web.Models
{
    public class PostViewModel
    {
        public BlogPostViewModel Post { get; set; }
        public bool IsEdit { get; set; }
        public bool IsPreview { get; set; }
        public BlogAuthor Author { get; set; }
        public string PostDate { get; set; }
    }
}
