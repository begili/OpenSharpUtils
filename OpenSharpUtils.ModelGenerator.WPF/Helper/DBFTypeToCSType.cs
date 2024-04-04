using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace OpenSharpUtils.ModelGenerator.WPF.Helper
{
    public static class DBFTypeToCSType
    {
        public static Type ToCSType(this string dbfType)
        {
            switch (dbfType.ToLower().Trim())
            {
                case "bigint":
                    return typeof(Int64);
                case "binary":
                case "image":
                case "rowversion":
                case "timestamp":
                case "varbinary":
                    return typeof(Byte[]);
                case "bit":
                    return typeof(Boolean);
                case "char":
                case "nchar":
                case "ntext":
                case "nvarchar":
                case "text":
                case "varchar":
                    return typeof(String);
                case "date":
                case "datetime":
                case "datetime2":
                case "smalldatetime":
                    return typeof(DateTime);
                case "datetimeoffset":
                    return typeof(DateTimeOffset);
                case "decimal":
                case "money":
                case "numeric":
                case "smallmoney":
                    return typeof(Decimal);
                case "float":
                    return typeof(Double);
                case "int":
                    return typeof(Int32);
                case "real":
                    return typeof(Single);
                case "smallint":
                    return typeof(Int16);
                case "sql_variant":
                    return typeof(Object);
                case "time":
                    return typeof(TimeSpan);
                case "tinyint":
                    return typeof(Byte);
                case "uniqueidentifier":
                    return typeof(Guid);
                case "xml":
                    return typeof(XDocument);
                default:
                    return null;
            }
        }
    }
}
