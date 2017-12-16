using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;

namespace Repositories
{
    public interface IRepository<TDbContext> where TDbContext : DbContext, new()
    {
        DbContext DbWriteContext { get; }

        TDbContext DBContext { get; }

        TEntity Add<TEntity>(TEntity entity) where TEntity : class;

        IQueryable<TEntity> AddRange<TEntity>(IEnumerable<TEntity> entity) where TEntity : class;

        TEntity Update<TEntity>(TEntity entity) where TEntity : class;

        TEntity Delete<TEntity>(TEntity entity) where TEntity : class;

        IQueryable<TEntity> DeleteRange<TEntity>(IEnumerable<TEntity> entity) where TEntity : class;

        void ExcuteCommand(string sql,params object[] parameters);

        IQueryable<TEntity> SqlQuery<TEntity>(string sql, params object[] parameters) where TEntity : class;

    }
}
