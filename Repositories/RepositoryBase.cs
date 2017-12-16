using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Text;

namespace Repositories
{
    public class RepositoryBase<TDbContext> : IRepository<TDbContext> where TDbContext : BaseDbContext, new()
    {
        private static string CommandTimeout = ConfigurationManager.AppSettings["EFCommandTimeout"];
        private DbContext DbWrite = null;

        private TDbContext DbRead = null;

        public DbContext DbWriteContext { get { return DbWrite; } }

        public TDbContext DBContext { get { return DbRead; } }

        public RepositoryBase()
        {
            DbRead = new TDbContext();
            DbWrite = DbRead.Master;
            System.Data.Entity.Infrastructure.Interception.DbInterception.Add(new NoLockInterceptor());
            NoLockInterceptor.SuppressNoLock = false;
            Database.SetInitializer<TDbContext>(null);
            if (!string.IsNullOrWhiteSpace(CommandTimeout))
            {
                int configValue = 0;
                if (Int32.TryParse(CommandTimeout, out configValue))
                {
                    DbRead.Database.CommandTimeout = configValue;
                    DbWrite.Database.CommandTimeout = configValue;
                }
            }
        }

        public TEntity Add<TEntity>(TEntity entity) where TEntity:class
        {
            return DbWriteContext.Set<TEntity>().Add(entity);
        }

        public IQueryable<TEntity> AddRange<TEntity>(IEnumerable<TEntity> entity) where TEntity : class
        {
            return DbWriteContext.Set<TEntity>().AddRange(entity).AsQueryable();
        }

        public TEntity Update<TEntity>(TEntity entity) where TEntity : class
        {
            DbWriteContext.Set<TEntity>().Attach(entity);
            var table = DbWriteContext.Entry(entity);
            table.State = EntityState.Modified;
            return table.Entity;
        }


        public void Commit(bool Candispose = true)
        {
            try
            {
                DbWriteContext.SaveChanges();
            }
            catch (Exception ex)
            {

            }
            finally
            {
                if (Candispose)
                {
                    DbWriteContext.Dispose();
                }
            }
        }

        public TEntity Delete<TEntity>(TEntity entity) where TEntity : class
        {
            return DbWriteContext.Set<TEntity>().Remove(entity);
        }

        public IQueryable<TEntity> DeleteRange<TEntity>(IEnumerable<TEntity> entity) where TEntity : class
        {
            return DbWriteContext.Set<TEntity>().RemoveRange(entity).AsQueryable();
        }

        public void ExcuteCommand(string sql, params object[] parameters)
        {
            DbWriteContext.Database.ExecuteSqlCommand(sql, parameters);
        }

        public IQueryable<TEntity> SqlQuery<TEntity>(string sql, params object[] parameters) where TEntity : class
        {
            return DBContext.Database.SqlQuery<TEntity>(sql, parameters).AsQueryable();
        }
    }
}
