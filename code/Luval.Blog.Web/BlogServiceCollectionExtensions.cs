using Luval.Data;
using Luval.Data.Sql;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luval.Blog.Web
{
    public static class BlogServiceCollectionExtensions
    {
        public static void AddBlog(this IServiceCollection services, string sqlConnectionString)
        {
            services.ConfigureOptions(typeof(BlogConfigureOptions));
            services.AddTransient<IBlogRepository>((sp) => {
                return new BlogRepository(new SqlServerUnitOfWorkFactory(sqlConnectionString));
            });
        }
    }
}
