using Luval.Data.Entities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Luval.Blog.Entities
{
    public class BlogConfiguration : StringKeyAuditEntity
    {
        public BlogConfiguration()
        {
            Title = "My Blog";
            TimeZone = TimeZoneInfo.Local.Id;
            DateFormat = CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern;
        }
        public string Title { get; set; }
        public string TimeZone { get; set; }
        public string DateFormat { get; set; }
        public string Permalink { get; set; }
        public string PostHeader { get; set; }
        public string PostFooter { get; set; }

    }
}
