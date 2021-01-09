using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luval.Blog.Web
{
    public class BlogConfigureOptions : IPostConfigureOptions<StaticFileOptions>
    {
        private readonly IWebHostEnvironment _environment;

        public BlogConfigureOptions(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        public void PostConfigure(string name, StaticFileOptions options)
        {

            options.ContentTypeProvider ??= new FileExtensionContentTypeProvider();

            if (options.FileProvider == null && _environment.WebRootFileProvider == null)
            {
                throw new InvalidOperationException("Missing FileProvider.");
            }

            options.FileProvider = options.FileProvider ?? _environment.WebRootFileProvider;


            // Add our provider
            var filesProvider = new ManifestEmbeddedFileProvider(GetType().Assembly, "resources");
            options.FileProvider = new CompositeFileProvider(options.FileProvider, filesProvider);
        }
    }
}
