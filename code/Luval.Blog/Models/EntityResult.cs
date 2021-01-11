using System;
using System.Collections.Generic;
using System.Text;

namespace Luval.Blog.Models
{
    public class EntityResult<TEntity>
    {
        public TEntity Entity { get; set; }
        public Exception Exception { get; set; }
        public string Message { get; set; }
        public bool IsSuccess { get; set; }
    }
}
