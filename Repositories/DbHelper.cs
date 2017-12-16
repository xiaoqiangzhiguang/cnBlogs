using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Repositories
{
    public class DbHelper
    {
        public static IList<Column> GetColumns(string database,string tableName,string connectionString)
        {
            using (var conn = new SqlConnection(connectionString))
            {
                string sql = string.Format(@"select 
ColumnName=c.name,
ColumnDesc=isnull(f.value,''),
ColumnType=t.name,
IsPk=case   when   exists(SELECT   1   FROM   {0}.sysobjects   where   xtype='PK'   and   name   in   (
        SELECT   name   FROM   {0}.sysindexes   WHERE   indid   in(
            SELECT   indid   FROM   {0}.sysindexkeys   WHERE   id   =   c.id   AND   colid=c.colid
         )))   then   1   else  0   end,
DefaultVal=ISNULL(m.text,'')
 from {0}.syscolumns c join {0}.sysobjects o on  c.id = o.id and  o.type='U' and o.name<>'dtproperties'
left join {0}.systypes t on c.xusertype = t.xusertype 
left join {0}.syscomments m on m.id = c.cdefault
left join  {0}.sys.extended_properties f on c.id = f.major_id and f.minor_id = c.colid
where o.name='{1}'", database, tableName);
                var cmd = new SqlCommand(sql, conn);
                var adapter = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                if(dt != null && dt.Rows.Count > 0)
                {
                    return dt.Rows.Cast<DataRow>().Select(m => new Column() {
                        ColumnName = m.Field<string>("ColumnName"),
                        ColumnDesc = m.Field<string>("ColumnDesc"),
                        ColumnType = m.Field<string>("ColumnType"),
                        IsPk = m.Field<int>("IsPk"),
                        DefaultVal = m.Field<string>("DefaultVal"),
                    }).ToList();
                }
            }
            return null;
        }
    }
    
    public class Column
    {
        public string ColumnName { get; set; }

        public string ColumnDesc { get; set; }

        public string ColumnType { get; set; }

        public string CSharpType { get {
                return MapColumnTypeToCSharpType.Map(ColumnType);
            } }

        public int IsPk { get; set; }

        public string DefaultVal { get; set; }
    }

    public class MapColumnTypeToCSharpType
    {
        public static string Map(string columnType)
        {
            var csharpType = string.Empty;
            if (string.IsNullOrEmpty(columnType))
            {
                return csharpType;
            }
            switch (columnType.ToLower())
            {
                case "datetime":
                case "date":
                case "datetime2":
                case "smalldatetime":
                    csharpType = "DateTime";
                    break;
                case "datetimeoffset": csharpType = "DateTimeOffset"; break;
                case "bigint": csharpType = "long"; break;
                case "binary":
                case "image":
                case "timestamp":
                case "varbinary":
                    csharpType = "byte[]"; break;
                case "bit": csharpType = "bool"; break;
                case "char":
                case "nchar":
                case "ntext":
                case "nvarchar":
                case "text":
                case "varchar":
                case "xml":
                    csharpType = "string"; break;
                case "decimal":
                case "smallmoney":
                case "money":
                case "numeric":
                    csharpType = "decimal"; break;
                case "float": csharpType = "double"; break;
                case "int": csharpType = "int"; break;
                case "real": csharpType = "Single"; break;
                case "smallint": csharpType = "short"; break;
                case "sql_variant":
                case "sysname":
                    csharpType = "object"; break;
                case "time": csharpType = "TimeSpan"; break;
                case "tinyint": csharpType = "byte"; break;
                case "uniqueidentifier": csharpType = "Guid"; break;
                default: csharpType = "object"; break;
            }
            return csharpType;
        }

        public static string MapDefaultVal(string type,string defaultVal)
        {
            if(string.IsNullOrEmpty(defaultVal))
            {
                return null;
            }
            defaultVal = defaultVal.Replace("(","").Replace(")", "").Replace("\'", "");
            switch (type)
            {
                case "DateTime":
                    if (defaultVal.ToLower().Contains("getdate"))
                    {
                        return  "DateTime.Now";
                    }
                    return string.Format("DateTime.Parse(\"{0}\")", defaultVal);
                case "string":
                    return defaultVal == string.Empty ? "string.Empty": string.Format("\"{0}\"", defaultVal);
                case "int":
                case "long":
                case "double":
                    return defaultVal;
                default:
                    return defaultVal;
            }
        }
    }

    

}
