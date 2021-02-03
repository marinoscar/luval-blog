using Luval.Blog.Entities;
using Luval.Blog.Models;
using Luval.Blog.ViewModel;
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
using System.Xml.Schema;

namespace Luval.Blog
{
    public class BlogRepository : IBlogRepository
    {

        public BlogRepository(IUnitOfWorkFactory factory)
        {
            UnitOfWorkFactory = factory;
        }

        protected IUnitOfWorkFactory UnitOfWorkFactory { get; private set; }

        public async Task<bool> IsPostIdValid(string postId, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(postId)) throw new ArgumentNullException(nameof(postId));
            var postUoW = UnitOfWorkFactory.Create<BlogPostInfo, string>();
            var res = await postUoW.Entities.Query.GetAsync(i => i.Id == postId, cancellationToken);
            return res != null && res.Any();
        }

        public async Task<BlogAuthor> GetAuthorByUserIdAsync(string userId, CancellationToken cancellationToken)
        {
            var authorUoW = UnitOfWorkFactory.Create<BlogAuthor, string>();
            var res = (await authorUoW.Entities.Query.GetAsync(i => i.CreatedByUserId == userId, cancellationToken)).FirstOrDefault();
            if (res != null && string.IsNullOrWhiteSpace(res.ProfilePicture)) res.ProfilePicture = BlogAuthor.DefaultProfilePic;
            return res;
        }

        public async Task CreateOrUpdateAuthorAsync(BlogAuthor author, CancellationToken cancellationToken)
        {
            var authorUoW = UnitOfWorkFactory.Create<BlogAuthor, string>();
            var dbAuthor = await authorUoW.Entities.Query.GetAsync(author.Id, cancellationToken);
            if (dbAuthor == null)
                await authorUoW.AddAndSaveAsync(author, cancellationToken);
            else
                await authorUoW.UpdateAndSaveAsync(author, cancellationToken);
        }

        public async Task<EntityResult<BlogPost>> CreateOrUpdatePostAsync(BlogPost post, CancellationToken cancellationToken)
        {
            var postUoW = UnitOfWorkFactory.Create<BlogPost, string>();
            var posts = await postUoW.Entities.Query.GetRawAsync("SELECT Id FROM BlogPost Where Id = {0}".FormatSql(post.Id).ToCmd(),
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
            {
                var res = await ValidatePostAsync(post, cancellationToken);
                if (!res.IsSuccess) return res;
                await postUoW.UpdateAndSaveAsync(post, cancellationToken);
            }

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
            var posts = await postInfo.Entities.Query.GetAsync(i => i.Slug == slug, cancellationToken);
            if (posts == null || !posts.Any()) return default(BlogPost);
            return await postUoW.Entities.Query.GetAsync(posts.First().Id, cancellationToken);
        }

        public async Task<BlogPost> FindByIdAsync(string id, CancellationToken cancellationToken)
        {
            var postUoW = UnitOfWorkFactory.Create<BlogPost, string>();
            var result = await postUoW.Entities.Query.GetAsync(id, cancellationToken);
            if (result == null) throw new ArgumentException("Invalid post id");
            return result;
        }

        public async Task<IEnumerable<BlogPostViewModel>> GetPublishedPostsAsync(int take, DateTime startPublishedDate, CancellationToken cancellationToken)
        {
            var postUoW = UnitOfWorkFactory.Create<BlogPostViewModel, string>();
            var posts = await postUoW.Entities.Query.GetAsync(GetBlogPostViewModelQuery(take, startPublishedDate.Date).ToCmd(), cancellationToken);
            return posts;
        }

        private static string GetBlogPostViewModelQuery(int take, DateTime dateTime)
        {
            var q = @"
SELECT TOP({0})
	[BP].[Id], [BP].[Title], [BP].[Slug], [BP].[Tags], [BP].[UtcPublishDate], [BP].[UtcCreatedOn], [BP].[UtcUpdatedOn], [BP].[CreatedByUserId], [BP].[UpdatedByUserId],
	[BA].[DisplayName], [BA].[ProfilePicture], [BA].[Id] As [AuthorId]
FROM 
	[BlogPost] As [BP]
	LEFT JOIN [BlogAuthor] As [BA] ON [BP].[CreatedByUserId] = [BA].[CreatedByUserId]
WHERE [BP].[UtcPublishDate] <= {1}
ORDER BY [BP].[UtcPublishDate] DESC
                     ".FormatSql(take, dateTime);
            return q;
        }

        private Task<IEnumerable<BlogPostInfo>> GetBlogPostInfoBySlugAsync(string slug, CancellationToken cancellationToken)
        {
            var uow = UnitOfWorkFactory.Create<BlogPostInfo, string>();
            return uow.Entities.Query.GetAsync(i => i.Slug == slug, cancellationToken);
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
