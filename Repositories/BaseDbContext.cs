using System.Data.Entity;


namespace Repositories
{
    public class BaseDbContext:DbContext
    {
        public virtual DbContext Master { get { return null; } }

        public BaseDbContext(string nameOrConnectionString):base(nameOrConnectionString)
        {

        }

    }
}
