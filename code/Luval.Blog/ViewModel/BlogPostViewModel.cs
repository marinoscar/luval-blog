using Luval.Blog.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Luval.Blog.ViewModel
{
    public class BlogPostViewModel : BlogPost
    {
        private string _pic;
        public BlogPostViewModel()
        {
            ProfilePicture = BlogAuthor.DefaultProfilePic;
        }
        public static BlogPostViewModel From(BlogPost p)
        {
            return new BlogPostViewModel()
            {
                Id = p.Id,
                Slug = p.Slug,
                Title = p.Title,
                Content = p.Content,
                CreatedByUserId = p.CreatedByUserId,
                Tags = p.Tags,
                UpdatedByUserId = p.UpdatedByUserId,
                UtcCreatedOn = p.UtcCreatedOn,
                UtcPublishDate = p.UtcPublishDate,
                UtcUpdatedOn = p.UtcUpdatedOn
            };
        }
        public string DisplayName { get; set; }
        public string AuthorId { get; set; }

        public string ProfilePicture
        {
            get { return _pic; }
            set
            {
                if (string.IsNullOrWhiteSpace(value) || DBNull.Value.Equals(value)) return;
                _pic = value;
            }
        }
    }
}
