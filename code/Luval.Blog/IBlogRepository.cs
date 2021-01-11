using Luval.Blog.Entities;
using Luval.Blog.Models;
using System.Threading;
using System.Threading.Tasks;

namespace Luval.Blog
{
    public interface IBlogRepository
    {
        Task<EntityResult<BlogPost>> CreateOrUpdatePostAsync(BlogPost post, CancellationToken cancellationToken);
        Task<BlogPost> FindBySlugAsync(string slug, CancellationToken cancellationToken);
        Task<string> GetContentByIdAsync(string id, CancellationToken cancellationToken);
    }
}