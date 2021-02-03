using Luval.Blog.Entities;
using Luval.Blog.ViewModel;
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
            var cancellationToken = CancellationToken.None;
            if (string.IsNullOrWhiteSpace(id)) return NotFound();
            var post = await BlogRepository.FindBySlugAsync(id, cancellationToken);
            if (post == null) return NotFound();
            if (post.UtcPublishDate == null || post.UtcPublishDate > DateTime.UtcNow.Date) return NotFound();
            var author = await BlogRepository.GetAuthorByUserIdAsync(post.CreatedByUserId, cancellationToken);
            if (author == null) author = new BlogAuthor() { DisplayName = "Unknown" };
            var model = new PostViewModel()
            {
                Post = BlogPostViewModel.From(post),
                PostDate = post.UtcPublishDate.Value.ToString("MMMM dd, yyyy"),
                Author = author
            };
            return View(new[] { model });
        }

        [AllowAnonymous, HttpGet, Route("Blog")]
        public async Task<IActionResult> Index()
        {
            var cancellationToken = CancellationToken.None;
            var posts = (await BlogRepository.GetPublishedPostsAsync(100, DateTime.Today, cancellationToken))
                .OrderByDescending(i => i.UtcPublishDate)
                .ToList();
            var result = new List<PostViewModel>();
            foreach (var post in posts)
            {
                result.Add(new PostViewModel()
                {
                    Post = post,
                    IsPreview = true,
                    PostDate = post.UtcPublishDate.Value.Date.ToString("MMMM dd, yyyy")
                });
            }
            return View(result);
        }

        [AllowAnonymous, HttpGet, Route("Blog/PostContent/{id}")]
        public async Task<IActionResult> GetPostContent(string id)
        {
            if (string.IsNullOrWhiteSpace(id)) return NotFound();
            var postInfo = await BlogRepository.FindByIdAsync(id, CancellationToken.None);
            return Json(new PostContentViewModel(postInfo));
        }

        [HttpGet, Route("Blog/Compose")]
        public IActionResult Compose()
        {
            var post = new BlogPost();
            UserRepository.PrepareEntityForInsert(User, post);
            return View(new PostViewModel() { IsEdit = false, Post = BlogPostViewModel.From(post) });
        }

        [HttpGet, Route("Blog/Edit/{id}")]
        public async Task<IActionResult> EditPost(string id)
        {
            if (!(await BlogRepository.IsPostIdValid(id, CancellationToken.None)))
                return NotFound();
            return View("Compose", new PostViewModel() { IsEdit = true, Post = new BlogPostViewModel() { Id = id } });
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
