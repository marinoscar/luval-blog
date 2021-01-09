using Luval.Blog.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Luval.Blog.Web.Areas.Blog.Controllers
{
    [Area("Blog"), Authorize]
    public class HomeController : Controller
    {
        public HomeController(IBlogRepository blogRepository)
        {
            Repository = blogRepository;
        }

        protected IBlogRepository Repository { get; private set; }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Compose(BlogPost post)
        {
            await Repository.CreatePostAsync(post, CancellationToken.None);
            return RedirectToAction("Post", post.Slug);
        }

        public IActionResult Post(string post)
        {
            return View();
        }
    }
}
