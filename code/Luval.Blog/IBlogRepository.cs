﻿using Luval.Blog.Entities;
using Luval.Blog.Models;
using Luval.Blog.ViewModel;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Luval.Blog
{
    public interface IBlogRepository
    {
        Task CreateOrUpdateAuthorAsync(BlogAuthor author, CancellationToken cancellationToken);
        Task<BlogAuthor> GetAuthorByUserIdAsync(string userId, CancellationToken cancellationToken);
        Task<EntityResult<BlogPost>> CreateOrUpdatePostAsync(BlogPost post, CancellationToken cancellationToken);
        Task<BlogPost> FindBySlugAsync(string slug, CancellationToken cancellationToken);
        Task<BlogPost> FindByIdAsync(string id, CancellationToken cancellationToken);
        Task<bool> IsPostIdValid(string postId, CancellationToken cancellationToken);
        Task<IEnumerable<BlogPostViewModel>> GetPublishedPostsAsync(int take, DateTime startPublishedDate, CancellationToken cancellationToken);
    }
}