using Luval.Blog.Entities;
using Luval.Web.Security;
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
        public HomeController(IBlogRepository blogRepository, IApplicationUserRepository userRepository)
        {
            BlogRepository = blogRepository;
            UserRepository = userRepository;
        }

        protected IBlogRepository BlogRepository { get; private set; }
        protected IApplicationUserRepository UserRepository { get; private set; }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet, Route("Blog/Compose")]
        public  IActionResult Compose()
        {
            var post = new BlogPost();
            UserRepository.PrepareEntityForInsert(User, post);
            return View(post);
        }

        public async Task<IActionResult> SavePost(BlogPost post)
        {
            await BlogRepository.CreatePostAsync(post, CancellationToken.None);
            return RedirectToAction("Post", post.Slug);
        }

        public IActionResult Post(string post)
        {
            return View();
        }
    }
}
