using System;
using System.Collections.Generic;
using System.Text;
using System.Data.Entity;

namespace Repositories.Blogs
{
    public class BlogDbContext : BaseDbContext
    {
        private DbContext context=null;
        public BlogDbContext() : base("name=BlogSlaveDb")
        {
            context = new BlogDbContext("name=BlogMasterDb");
        }

        public BlogDbContext(string conncetKey)
            : base(conncetKey)
        {

        }

        public override DbContext Master{ get { return context; } }

        //表
        public virtual DbSet<Blog> Blogs { get; set; }

        public virtual DbSet<Comment> Comments { get; set; }
        
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
