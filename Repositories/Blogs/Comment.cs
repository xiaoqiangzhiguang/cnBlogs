using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Blogs
{
    public class Comment
    {
        public long Id { get; set; }
        public string Content { get; set; }
        public string AuthorTo { get; set; }
        public string AuthorFrom { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime? UpdateTime { get; set; }
        public int IsDelete { get; set; }
    }
}
