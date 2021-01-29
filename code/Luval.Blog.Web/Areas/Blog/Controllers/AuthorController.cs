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
    public class AuthorController : Controller
    {

        public AuthorController(IBlogRepository blogRepository, IApplicationUserRepository userRepository)
        {
            BlogRepository = blogRepository;
            UserRepository = userRepository;
        }
        protected IBlogRepository BlogRepository { get; private set; }
        protected IApplicationUserRepository UserRepository { get; private set; }

        [HttpGet, Route("Blog/Author/{id}")]
        public async Task<IActionResult> Index()
        {
            var currentUser = await UserRepository.GetUserAsync(User);
            var author = await BlogRepository.GetAuthorByUserIdAsync(currentUser.Id, CancellationToken.None);
            return View(author);
        }

        [HttpGet, Route("Blog/Author/Edit")]
        public async Task<IActionResult> Edit()
        {
            var currentUser = await UserRepository.GetUserAsync(User);
            var author = await BlogRepository.GetAuthorByUserIdAsync(currentUser.Id, CancellationToken.None);
            if (author == null)
            {
                author = CreateFromUser(currentUser);
                await BlogRepository.CreateOrUpdateAuthorAsync(author, CancellationToken.None);
            }
            return View(author);
        }

        [HttpPost, Route("Blog/Author/Update")]
        public async Task<IActionResult> Update(BlogAuthor blogAuthor)
        {
            if (blogAuthor == null) return NoContent();
            UserRepository.PrepareEntityForUpdate(User, blogAuthor);
            await BlogRepository.CreateOrUpdateAuthorAsync(blogAuthor, CancellationToken.None);
            return Ok();
        }

        internal static BlogAuthor CreateFromUser(ApplicationUser user)
        {
            return new BlogAuthor()
            {
                CreatedByUserId = user.Id,
                UpdatedByUserId = user.Id,
                DisplayName = string.IsNullOrWhiteSpace(user.DisplayName) ? user.Email.Split('@')[0] : user.DisplayName
            };
        }
    }
}
