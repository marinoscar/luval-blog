using Luval.Blog.Entities;
using Luval.Blog.Models;
using Luval.Data;
using Luval.Data.Extensions;
using Luval.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Luval.Blog
{
    public class BlogRepository : IBlogRepository
    {

        public BlogRepository(IUnitOfWorkFactory factory)
        {
            UnitOfWorkFactory = factory;
        }

        protected IUnitOfWorkFactory UnitOfWorkFactory { get; private set; }

        public async Task<EntityResult<BlogPost>> CreateOrUpdatePostAsync(BlogPost post, CancellationToken cancellationToken)
        {
            var postUoW = UnitOfWorkFactory.Create<BlogPost, string>();
            var posts = await postUoW.Entities.GetRawAsync("SELECT Id FROM BlogPost Where Id = {0}".FormatSql(post.Id).ToCmd(),
                                                         cancellationToken);
            if (posts != null && !posts.Any())
            {
                post.UtcCreatedOn = post.UtcUpdatedOn;
                post.CreatedByUserId = post.UpdatedByUserId;
                var res = await ValidatePostAsync(post, cancellationToken);
                if (!res.IsSuccess) return res;
                await postUoW.AddAndSaveAsync(post, cancellationToken);
            }
            else
                await postUoW.UpdateAndSaveAsync(post, cancellationToken);

            return new EntityResult<BlogPost>() { IsSuccess = true, Entity = post, Message = "Post saved successfully" };
        }

        public async Task<EntityResult<BlogPost>> ValidatePostAsync(BlogPost post, CancellationToken cancellationToken)
        {
            if (post == null) throw new ArgumentNullException(nameof(post));
            if (string.IsNullOrWhiteSpace(post.Title))
            {
                var ex = new ArgumentNullException("Title", "Post title cannot be null, empty or whitespace");
                return new EntityResult<BlogPost>() { Entity = post, IsSuccess = false, Exception = ex, Message = ex.Message };
            }
            UpdateCreateSlug(post);
            var slug = (await GetBlogPostInfoBySlugAsync(post.Slug, cancellationToken)).FirstOrDefault();
            if (slug != null)
            {
                var ex = new ArgumentException("The post title is duplicate in the database");
                return new EntityResult<BlogPost>() { Entity = post, IsSuccess = false, Exception = ex, Message = ex.Message };
            }
            return new EntityResult<BlogPost>() { Entity = post, IsSuccess = true };
        }

        public async Task<BlogPost> FindBySlugAsync(string slug, CancellationToken cancellationToken)
        {
            var postInfo = UnitOfWorkFactory.Create<BlogPostInfo, string>();
            var postUoW = UnitOfWorkFactory.Create<BlogPost, string>();
            var posts = await postInfo.Entities.GetAsync(i => i.Slug == slug, cancellationToken);
            if (posts == null || !posts.Any()) return default(BlogPost);
            return await postUoW.Entities.GetAsync(posts.First().Id, cancellationToken);
        }

        public async Task<string> GetContentByIdAsync(string id, CancellationToken cancellationToken)
        {
            var postUoW = UnitOfWorkFactory.Create<BlogPostContent, string>();
            var result = await postUoW.Entities.GetAsync(id, cancellationToken);
            if (result == null) throw new ArgumentException("Invalid post id");
            return result.Content;
        }

        private Task<IEnumerable<BlogPostInfo>> GetBlogPostInfoBySlugAsync(string slug, CancellationToken cancellationToken)
        {
            var uow = UnitOfWorkFactory.Create<BlogPostInfo, string>();
            return uow.Entities.GetAsync(i => i.Slug == slug, cancellationToken);
        }

        private void UpdateCreateSlug(BlogPost post)
        {
            var title = post.Title;
            if (title.Length > 249)
                title = title.Substring(0, 249);
            var slugs = title.Split(' ').Where(i => !string.IsNullOrWhiteSpace(i)).Select(i => i.ToLowerInvariant());
            post.Slug = string.Join("-", slugs);
        }
    }
}
