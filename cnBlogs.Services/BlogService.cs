using Repositories;
using Repositories.Blogs;
using System;
using System.Collections.Generic;
using System.Linq;

namespace cnBlogs.Services
{
    public class BlogService
    {
        private IRepository<BlogDbContext> repository= new RepositoryBase<BlogDbContext>();
        public IEnumerable<Blog> GetAllBlogs()
        {
            return repository.DBContext.Blogs.ToList();
        }
    }
}
