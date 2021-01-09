using Luval.Blog.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace Luval.Blog
{
    public interface IBlogRepository
    {
        Task CreatePostAsync(BlogPost post, CancellationToken cancellationToken);
    }
}