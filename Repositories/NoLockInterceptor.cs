using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Data.Entity.Infrastructure.Interception;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Repositories
{
    public class NoLockInterceptor : DbCommandInterceptor
    {
        private static readonly Regex _tableAliasRegex =
            new Regex(@"(?<tableAlias>AS \[Extent\d+\](?! WITH \(NOLOCK\)))",
                RegexOptions.Multiline | RegexOptions.IgnoreCase);

        public override void NonQueryExecuting(DbCommand command, DbCommandInterceptionContext<int> interceptionContext)
        {
            base.NonQueryExecuting(command, interceptionContext);
        }

        [ThreadStatic]
        public static bool SuppressNoLock;


        System.Diagnostics.Stopwatch watch = null;

        public NoLockInterceptor()
        {
            watch = new System.Diagnostics.Stopwatch();
        }

        public override void ScalarExecuting(DbCommand command,
            DbCommandInterceptionContext<object> interceptionContext)
        {
            watch.Start();
            if (!SuppressNoLock)
            {
                command.CommandText =
                    _tableAliasRegex.Replace(command.CommandText, "${tableAlias} WITH (NOLOCK)");
            }
        }

        public override void ScalarExecuted(DbCommand command, DbCommandInterceptionContext<object> interceptionContext)
        {
            long totalseaconds = watch.ElapsedMilliseconds;
            watch.Reset();
            if (totalseaconds > 2000)
            {
            }

            base.ScalarExecuted(command, interceptionContext);
        }

        public override void ReaderExecuting(DbCommand command, DbCommandInterceptionContext<DbDataReader> interceptionContext)
        {

            watch.Start();

            if (!SuppressNoLock)
            {
                command.CommandText =
                    _tableAliasRegex.Replace(command.CommandText, "${tableAlias} WITH (NOLOCK)");
            }
        }


        public override void ReaderExecuted(DbCommand command, DbCommandInterceptionContext<DbDataReader> interceptionContext)
        {
            long totalseaconds = watch.ElapsedMilliseconds;
        
            if (totalseaconds > 500)
            {
            }
            watch.Reset();
            base.ReaderExecuted(command, interceptionContext);
        }
    }
}
