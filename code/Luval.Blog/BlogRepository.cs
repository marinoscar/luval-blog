using Luval.Blog.Entities;
using Luval.Data.Extensions;
using Luval.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Luval.Blog
{
    public class BlogRepository : IBlogRepository
    {
        protected IUnitOfWorkFactory UnitOfWorkFactory { get; private set; }

        public Task CreatePostAsync(BlogPost post, CancellationToken cancellationToken)
        {
            var postUoW = UnitOfWorkFactory.Create<BlogPost, string>();
            return postUoW.AddAndSaveAsync(post, cancellationToken);
        }
    }
}
