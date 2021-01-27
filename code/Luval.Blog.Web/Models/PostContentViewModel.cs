using Luval.Blog.Entities;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luval.Blog.Web.Models
{
    public class PostContentViewModel : BlogPost
    {
        public PostContentViewModel()
        {

        }

        public PostContentViewModel(BlogPost post)
        {
            Id = post.Id;
            Title = post.Title;
            UtcPublishDate = post.UtcPublishDate;
            CreatedByUserId = post.CreatedByUserId;
            UtcCreatedOn = post.UtcCreatedOn;
            UtcUpdatedOn = post.UtcUpdatedOn;
            UpdatedByUserId = post.UpdatedByUserId;
            Content = post.Content;
            if (UtcPublishDate != null)
                PublishDate = UtcPublishDate.Value.ToString("MM/dd/yyyy");
        }
        public string PublishDate { get; set; }
    }
}
