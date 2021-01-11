using Luval.Blog.Entities;
using Luval.Blog.Web.Models;
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

        [AllowAnonymous, HttpGet, Route("Blog/{id}")]
        public async Task<IActionResult> Index(string id)
        {
            if (string.IsNullOrWhiteSpace(id)) return NotFound();
            var post = await BlogRepository.FindBySlugAsync(id, CancellationToken.None);
            if (post == null) return NotFound();
            if (post.UtcPublishDate == null || post.UtcPublishDate > DateTime.UtcNow.Date) return NotFound();
            var model = new PostViewModel()
            {
                RowNum = 1,
                Post = post,
                PostDate = post.UtcPublishDate.Value.ToString("MMMM dd, yyyy"),
                Author = "Oscar"
            };
            return View(model);
        }

        [AllowAnonymous, HttpGet, Route("Blog/PostContent/{id}")]
        public async Task<IActionResult> GetPostContent(string id)
        {
            if (string.IsNullOrWhiteSpace(id)) return NotFound();
            var content = await BlogRepository.GetContentByIdAsync(id, CancellationToken.None);
            return Json(new { Id = id, Content = content  });
        }

        [HttpGet, Route("Blog/Compose")]
        public IActionResult Compose()
        {
            var post = new BlogPost();
            UserRepository.PrepareEntityForInsert(User, post);
            return View(post);
        }


        [HttpPost, Route("Blog/SavePost")]
        public async Task<IActionResult> SavePost(BlogPost post)
        {
            if (post == null)
                return BadRequest();
            UserRepository.PrepareEntityForUpdate(User, post);
            var response = await BlogRepository.CreateOrUpdatePostAsync(post, CancellationToken.None);
            return Json(response);
        }
    }
}
