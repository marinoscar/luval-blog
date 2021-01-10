using Luval.Blog.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luval.Blog.Web.Models
{
    public class PostViewModel
    {
        public BlogPost Post { get; set; }
        public bool IsEdit { get; set; }
        public string Author { get; set; }
        public string PostDate { get; set; }
    }
}
