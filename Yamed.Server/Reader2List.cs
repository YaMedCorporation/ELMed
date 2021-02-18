using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Linq.Mapping;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Linq;
using System.Xml;
using System.Reflection;
using System.Text;
using Yamed.Entity;
using Yamed.Core;

namespace Yamed.Server
{
    public class Reader2List
    {
        public static object TestDbf(string selectCmd, string provider)
        {
            object objList = new object();
            //using (var connection = new OleDbConnection(@"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=D:\!ROSNO; Extended Properties=dBASE IV;"))
            //using (var connection = new OleDbConnection(@"Provider=VFPOLEDB.1;Data Source=D:\!ROSNO;"))
            using (var connection = new OleDbConnection(provider))

            {
                using (var command = new OleDbCommand(selectCmd, connection))
                {
                    command.CommandTimeout = 0;
                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        string[] fields = new string[reader.FieldCount];
                        Type[] types = new Type[reader.FieldCount];
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            fields[i] = reader.GetName(i);
                            Type type = reader.GetFieldType(i);
                            types[i] = type.Equals(typeof(string)) ? type : typeof(Nullable<>).MakeGenericType(type);
                        }

                        var anonymousClass = CustomTypeBuilder.CompileResultType(fields, types);
                        var anonymousObject = ProxyProvider.Instance.NewProxy2(anonymousClass);

                        objList = DataReaderMapToList(anonymousObject, reader);
                    }
                    connection.Close();
                }
            }
            return objList;
        }
        public static void LoadFromTable<T>(string connectionString, DataTable dt, string sqltable)
        {
            string[] rezervsql = { "ADD", "ALL", "ALTER", "AND", "ANY", "AS", "ASC", "AUTHORIZATION", "BACKUP", "BEGIN", "BETWEEN",
                "BREAK", "BROWSE", "BULK", "BY", "CASCADE", "CASE", "CHECK", "CHECKPOINT", "CLOSE", "CLUSTERED", "COALESCE", "COLLATE",
                "COLUMN", "COMMIT", "COMPUTE", "CONSTRAINT", "CONTAINS", "CONTAINSTABLE", "CONTINUE", "CONVERT", "CREATE", "CROSS",
                "CURRENT", "CURRENT_DATE", "CURRENT_TIME", "CURRENT_TIMESTAMP", "CURRENT_USER", "CURSOR", "DATABASE", "DBCC",
                "DEALLOCATE", "DECLARE", "DEFAULT", "DELETE", "DENY", "DESC", "DISK;", "DISTINCT", "DISTRIBUTED", "DOUBLE", "DROP",
                "DUMP;", "ELSE", "END", "ERRLVL", "ESCAPE", "EXCEPT", "EXEC", "EXECUTE", "EXISTS", "EXIT", "EXTERNAL", "FETCH", "FILE",
                "FILLFACTOR", "FOR", "FOREIGN", "FREETEXT", "FREETEXTTABLE", "FROM", "FULL", "FUNCTION", "GOTO", "GRANT", "GROUP",
                "HAVING", "HOLDLOCK", "IDENTITY", "IDENTITY_INSERT", "IDENTITYCOL", "IF", "IN", "INDEX", "INNER", "INSERT", "INTERSECT",
                "INTO", "IS", "JOIN", "KEY", "KILL", "LEFT", "LIKE", "LINENO", "LOAD", "MERGE", "NATIONAL", "NOCHECK", "NONCLUSTERED",
                "NOT", "NULL", "NULLIF", "OF", "OFF", "OFFSETS", "ON", "OPEN", "OPENDATASOURCE", "OPENQUERY", "OPENROWSET", "OPENXML",
                "OPTION", "OR", "ORDER", "OUTER", "OVER", "PERCENT", "PIVOT", "PLAN", "PRECISION", "PRIMARY", "PRINT", "PROC",
                "PROCEDURE", "PUBLIC", "RAISERROR", "READ", "READTEXT", "RECONFIGURE", "REFERENCES", "РЕПЛИКАЦИЯ", "RESTORE", "RESTRICT",
                "RETURN", "REVERT", "REVOKE", "RIGHT", "ROLLBACK", "ROWCOUNT", "ROWGUIDCOL", "RULE", "SAVE", "SCHEMA", "SECURITYAUDIT",
                "SELECT", "SEMANTICKEYPHRASETABLE", "SEMANTICSIMILARITYDETAILSTABLE", "SEMANTICSIMILARITYTABLE", "SESSION_USER", "SET",
                "SETUSER", "SHUTDOWN", "SOME", "STATISTICS", "SYSTEM_USER", "TABLE", "TABLESAMPLE", "TEXTSIZE", "THEN", "TO", "В начало",
                "TRAN", "TRANSACTION", "TRIGGER", "TRUNCATE", "TRY_CONVERT", "TSEQUAL", "UNION", "UNIQUE", "UNPIVOT", "UPDATE",
                "UPDATETEXT", "USE", "Пользователь", "VALUES", "VARYING", "VIEW", "WAITFOR", "WHEN", "WHERE", "WHILE", "WITH",
                "WITHIN GROUP", "WRITETEXT" };
            string sqltype = "";
            string sqlcolumns = "";
            foreach (DataColumn dc in dt.Columns)
            {
                //dt.Columns.Add(d.NAME,d.TYPE);
                string s;
                switch (dc.DataType.Name.ToString())
                {
                    case "Int32":
                        s = "int";
                        break;
                    case "String":
                        s = $"nvarchar({dc.MaxLength})";
                        break;
                    case "Guid":
                        s = "uniqueidentifier";
                        break;
                    case "Boolean":
                        s = "bit";
                        break;
                    case "DateTime":
                        s = "DateTime2";
                        break;
                    case "Decimal":
                        s = "numeric(20,2)";
                        break;
                    default:
                        s = dc.DataType.Name.ToString();
                        break;
                }
                if (rezervsql.Contains(dc.ColumnName))
                {
                    sqltype = sqltype + "[" + dc.ColumnName + "]" + " " + s + ",";
                    sqlcolumns = sqlcolumns + "[" + dc.ColumnName + "]" + ",";
                }
                else
                {
                    sqltype = sqltype + dc.ColumnName + " " + s + ",";
                    sqlcolumns = sqlcolumns + dc.ColumnName + ",";
                }

            }
            sqltype = sqltype.Substring(0, sqltype.Length - 1);
            sqlcolumns = sqlcolumns.Substring(0, sqlcolumns.Length - 1);

            SqlConnection con = new SqlConnection(connectionString);

            SqlCommand cmd0 = new SqlCommand($@" IF exists (select * from sys.table_types where name='ForUpdate')  
                                                 DROP TYPE dbo.ForUpdate   
                                                 CREATE TYPE ForUpdate AS TABLE ({sqltype})
                                                 IF exists (select * from sys.tables where name='{sqltable}') 
                                                 DROP table {sqltable}  
                                                 CREATE TABLE dbo.{sqltable} ({sqltype})", con);

            SqlCommand cmd = new SqlCommand($@"insert into dbo.{sqltable}({sqlcolumns})
                                                select {sqlcolumns} from @dt", con);

            var t = new SqlParameter("@dt", SqlDbType.Structured);
            t.TypeName = "dbo.ForUpdate";
            t.Value = dt;
            cmd.Parameters.Add(t);
            //SqlParameter t = cmd.Parameters.AddWithValue("@t", dt);
            //t.SqlDbType = SqlDbType.Structured;
            //t.TypeName = "dbo.ForUpdate";
            cmd.CommandTimeout = 0;
            con.Open();
            cmd0.ExecuteNonQuery();
            int str = cmd.ExecuteNonQuery();
            int isrt = str;
            con.Close();
        }
        public static void InsertFromTable<T>(string com, string connectionString, DataTable dt, bool deltype)
        {
            string sqltype = "";


            foreach (DataColumn dc in dt.Columns)
            {
                //dt.Columns.Add(d.NAME,d.TYPE);
                string s;
                switch (dc.DataType.Name.ToString())
                {
                    case "Int32":
                        s = "int";
                        break;
                    case "String":
                        s = "nvarchar(500)";
                        break;
                    case "Guid":
                        s = "uniqueidentifier";
                        break;
                    case "Boolean":
                        s = "bit";
                        break;
                    case "Binary":
                        s = "varbinary(20)";
                        break;
                    case "DateTime":
                        s = "DateTime2";
                        break;
                    case "Decimal":
                        s = "numeric(10,2)";
                        break;
                    default:
                        s = dc.DataType.Name.ToString();
                        break;
                }

                sqltype = sqltype + dc.ColumnName + " " + s + ",";

            }
            sqltype = sqltype.Substring(0, sqltype.Length - 1);
            SqlConnection con = new SqlConnection(connectionString);
            SqlCommand cmd0 = new SqlCommand($@" IF exists (select * from sys.table_types where name='ForUpdate_LIS')  
                                                 DROP TYPE dbo.ForUpdate_LIS   
                                                 CREATE TYPE ForUpdate_LIS AS TABLE ({sqltype})", con);

            SqlCommand cmd = new SqlCommand(com, con);

            var t = new SqlParameter("@dt", SqlDbType.Structured);
            t.TypeName = "dbo.ForUpdate_LIS";
            t.Value = dt;
            cmd.Parameters.Add(t);
            cmd.CommandTimeout = 0;
            con.Open();
            cmd0.ExecuteNonQuery();
            int str = cmd.ExecuteNonQuery();
            int isrt = str;
            con.Close();
        }
        public static DataTable ToDataTable<T>(List<T> items)

        {
            DataTable dataTable = new DataTable(typeof(T).Name);
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                dataTable.Columns.Add(prop.Name, prop.PropertyType.GetGenericArguments().Count() > 0 ? prop.PropertyType.GetGenericArguments()[0] : prop.PropertyType);
            }

            foreach (T item in items)
            {
                var values = new object[Props.Length];

                for (int i = 0; i < Props.Length; i++)
                {
                    values[i] = Props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }
            return dataTable;

        }
        public static void UpdateFromTable<T>(string com, string connectionString, DataTable dt, bool deltype)
        {
            string sqltype = "";


            foreach (DataColumn dc in dt.Columns)
            {
                //dt.Columns.Add(d.NAME,d.TYPE);
                string s;
                switch (dc.DataType.Name.ToString())
                {
                    case "Int32":
                        s = "int";
                        break;
                    case "String":
                        s = "nvarchar(500)";
                        break;
                    case "Guid":
                        s = "uniqueidentifier";
                        break;
                    case "Boolean":
                        s = "bit";
                        break;
                    case "DateTime":
                        s = "DateTime2";
                        break;
                    case "Decimal":
                        s = "numeric(10,2)";
                        break;
                    default:
                        s = dc.DataType.Name.ToString();
                        break;
                }

                sqltype = sqltype + dc.ColumnName + " " + s + ",";

            }
            sqltype = sqltype.Substring(0, sqltype.Length - 1);
            SqlConnection con = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand(com, con);

            var t = new SqlParameter("@dt", SqlDbType.Structured);
            t.TypeName = "dbo.IDTable";
            t.Value = dt;
            cmd.Parameters.Add(t);

            cmd.CommandTimeout = 0;
            con.Open();
            if (deltype)
            {
                int str = cmd.ExecuteNonQuery();
                int isrt = str;
            }
            else
            {
                int str = cmd.ExecuteNonQuery();
                int isrt = str;
            }

            con.Close();
        }
        public static string StartFunctionFromTable<T>(string com, string connectionString, DataTable dt, bool deltype)
        {
            string sqltype = "";


            foreach (DataColumn dc in dt.Columns)
            {
                //dt.Columns.Add(d.NAME,d.TYPE);
                string s;
                switch (dc.DataType.Name.ToString())
                {
                    case "Int32":
                        s = "int";
                        break;
                    case "String":
                        s = "nvarchar(500)";
                        break;
                    case "Guid":
                        s = "uniqueidentifier";
                        break;
                    case "Boolean":
                        s = "bit";
                        break;
                    case "DateTime":
                        s = "DateTime2";
                        break;
                    case "Decimal":
                        s = "numeric(10,2)";
                        break;
                    default:
                        s = dc.DataType.Name.ToString();
                        break;
                }

                sqltype = sqltype + dc.ColumnName + " " + s + ",";

            }
            sqltype = sqltype.Substring(0, sqltype.Length - 1);
            SqlConnection con = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand(com, con);
            var t = new SqlParameter("@dt", SqlDbType.Structured);
            t.TypeName = "dbo.IDTable";
            t.Value = dt;
            cmd.Parameters.Add(t);
            cmd.CommandTimeout = 0;
            con.Open();
            string xml = (string)cmd.ExecuteScalar();
            con.Close();
            return xml;
        }
        public static object SelectScalar(string selectCmd, string connectionString)
        {
            object objList = new object();
            using (var connection = new SqlConnection(connectionString))
            {
                using (var command = new SqlCommand(selectCmd, connection))
                {
                    command.CommandTimeout = 0;
                    connection.Open();
                    objList = command.ExecuteScalar();
                    connection.Close();
                }
            }
            return objList;
        }
        public static object CustomAnonymousSelect(string selectCmd, string connectionString)
        {
            object objList = new object();
            using (var connection = new SqlConnection(connectionString))
            {
                using (var command = new SqlCommand(selectCmd, connection))
                {
                    command.CommandTimeout = 0;
                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        var schema = reader.GetSchemaTable();
                        string[] fields = null;
                        Type[] types = null;

                        if (schema == null)
                        {
                            fields = new string[1];
                            types = new Type[1];

                            fields[0] = "СтрокОбработано";
                            types[0] = typeof(int);
                            objList = DataReaderMapToList(ProxyProvider.Instance.NewProxy2(CustomTypeBuilder.CompileResultType(fields, types)), reader, false);
                        }
                        else
                        {
                            fields = new string[schema.Rows.Count];
                            types = new Type[schema.Rows.Count];
                            int ci = 1;
                            for (int i = 0; i < schema.Rows.Count; i++)
                            {
                                var col = schema.Rows[i].Field<string>("ColumnName");
                                col = string.IsNullOrWhiteSpace(col) ? "Column" + ci++ :
                                    fields.Any(x => x == col) ? col + fields.Count(x => x == col) : col;

                                fields[i] = col;
                                Type type = schema.Rows[i].Field<Type>("DataType");

                                if (schema.Rows[i].Field<bool>("AllowDBNull"))
                                    types[i] = type == typeof(string) || type == typeof(byte[])
                                        ? type
                                        : typeof(Nullable<>).MakeGenericType(type);
                                else
                                    types[i] = type;
                            }

                            objList = DataReaderMapToList(ProxyProvider.Instance.NewProxy2(CustomTypeBuilder.CompileResultType(fields, types)), reader);

                            //var anonymousClass = CustomTypeBuilder.CompileResultType(fields, types);
                            //var anonymousObject = ProxyProvider.Instance.NewProxy2(anonymousClass);

                            fields = null;
                            types = null;
                        }

                    }
                    connection.Close();
                }
            }
            return objList;
        }


        public static object GetAnonymousTable(string table, string connectionString)
        {
            object objList;
            //var cis = CustomSelect<SprEditor.UniSprClasses.ColumnsInformationSchema>($"select * from information_schema.columns where table_name = '{table}'", connectionString);

            using (var connection = new SqlConnection(connectionString))
            {
                using (var command = new SqlCommand("Select * From " + table, connection))
                {
                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        var schema = reader.GetSchemaTable();

                        string[] fields = new string[schema.Rows.Count];
                        Type[] types = new Type[schema.Rows.Count];

                        for (int i = 0; i < schema.Rows.Count; i++)
                        {
                            fields[i] = schema.Rows[i].Field<string>("ColumnName");
                            Type type = schema.Rows[i].Field<Type>("DataType");

                            if (schema.Rows[i].Field<bool>("AllowDBNull"))
                                types[i] = type == typeof(string) || type == typeof(byte[])
                                    ? type
                                    : typeof(Nullable<>).MakeGenericType(type);
                            else
                                types[i] = type;
                        }
                        var anonymousClass = CustomTypeBuilder.CompileResultType(fields, types);
                        var anonymousObject = ProxyProvider.Instance.NewProxy2(anonymousClass);

                        objList = DataReaderMapToList(anonymousObject, reader);
                    }
                    connection.Close();
                }
            }
            //cis.Clear();
            return objList;
        }

        public static object DataReaderMapToList(object anonymousType, IDataReader dr, bool isRead = true)
        {
            var type = anonymousType.GetType();
            var genericListType = typeof(List<>);
            var listOfType = genericListType.MakeGenericType(type);
            var list = (IList)Activator.CreateInstance(listOfType);

            var obj = Activator.CreateInstance(type);
            var propList = obj.GetType().GetProperties().Where(IsAcceptableDbType).ToList();
            
            while (dr.Read())
            {
                obj = Activator.CreateInstance(type);

                for (int i = 0; i < propList.Count; i++)
                {
                    if (!dr.IsDBNull(i))
                    {
                        propList[i].SetValue(obj, dr.GetValue(i), null);
                    }
                }
                //foreach (PropertyInfo prop in propList)
                //{
                //    if (!dr.IsDBNull(dr.GetOrdinal(prop.Name)))
                //    {
                //        prop.SetValue(obj, dr.GetValue(dr.GetOrdinal(prop.Name)), null);
                //    }
                //}

                list.Add(obj);
            }

            if (isRead == false)
            {
                propList[0].SetValue(obj, dr.RecordsAffected, null);
                list.Add(obj);
            }


            return list;
        }


        public static List<T> CustomSelect<T>(string selectCmd, string connectionString)
        {
            List<T> list;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(selectCmd, con))
                {
                    con.Open();
                    cmd.CommandTimeout = 0;
                    SqlDataReader dr = cmd.ExecuteReader();
                    list = DataReaderMapToList<T>(dr);
                    dr.Close();
                }
                con.Close();
            }
            return list;
        }

        public static List<T> DataReaderMapToList<T>(IDataReader dr)
        {
            List<T> list = new List<T>(); 
            T obj = default(T);

            obj = Activator.CreateInstance<T>();
            var propList = obj.GetType().GetProperties().Where(IsAcceptableDbType).ToList();

            while (dr.Read())
            {
                obj = Activator.CreateInstance<T>();
                foreach (PropertyInfo prop in propList)
                {
                    if (!dr.IsDBNull(dr.GetOrdinal(prop.Name)))
                    {
                        prop.SetValue(obj, dr.GetValue(dr.GetOrdinal(prop.Name)), null);
                    }
                }
                list.Add(obj);
            }
            return list;
        }

        public static bool IsAcceptableDbTypeInsUpd(PropertyInfo pi)
        {
            //if (pi.Name == "PRVS_VERS")
            //{
            //    //var atr = pi.GetCustomAttributes();
            //    var ippp = pi;
            //}

            var info = pi.GetCustomAttributes(typeof(ColumnAttribute), true).OfType<ColumnAttribute>().ToList();

            //if (pi.Name == "FIO" || pi.Name == "NameWithID" || pi.Name == "FioWithId") return false;
            return (pi.PropertyType.BaseType ==
                   Type.GetType("System.ValueType") ||
                   pi.PropertyType ==
                   Type.GetType("System.String")) &&
                   (!info.Any() || info.Any(x => x.IsPrimaryKey || (x.IsPrimaryKey == false && x.IsDbGenerated == false))) && !(new[] { "FIO", "NameWithID", "FioWithId" }.Contains(pi.Name));

        }

        public static bool IsAcceptableDbType(PropertyInfo pi)
        {
            return (pi.PropertyType.BaseType ==
                       Type.GetType("System.ValueType") ||
                       pi.PropertyType ==
                       Type.GetType("System.String") ||
                       pi.PropertyType == 
                       Type.GetType("System.Byte[]")
                       );
        }

        public static string CustomUpdateCommand<T>(string dbName, IList list, string id)
        {
            T listObj = default(T);
            listObj = Activator.CreateInstance<T>();
            var propList = listObj.GetType().GetProperties().Where(IsAcceptableDbTypeInsUpd).ToList();

            var sbList = new List<StringBuilder>();
            foreach (var obj in list)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat("UPDATE {0} SET ", dbName);
                foreach (PropertyInfo prop in propList)
                {

                    if (prop.Name != id)
                    {
                        string value = "";
                        var propValue = prop.GetValue(obj, null);


                        if (propValue is DateTime)
                        {
                            var t = (DateTime)propValue;
                            value = "'" + t.ToString("yyyyMMdd HH:mm") + "'";
                        }
                        else if (propValue is TimeSpan)
                        {
                            var t = (TimeSpan)propValue;
                            value = "'" + t.ToString() + "'";
                        }
                        else if (propValue is int)
                        {
                            var val = (int)propValue;
                            value = val.ToString();
                        }
                        else if (propValue is byte)
                        {
                            var val = (byte)propValue;
                            value = val.ToString();
                        }
                        else if (propValue is decimal)
                        {
                            var val = (decimal)propValue;
                            value = val.ToString().Replace(",", ".");
                        }
                        else if (propValue is Guid)
                        {
                            var val = (Guid)propValue;
                            value = "'" + val + "'";
                        }
                        else if (propValue is bool)
                        {
                            var val = (bool)propValue;
                            value = val.ToString().Replace("False", "0").Replace("True", "1");
                        }
                        else if (propValue == null)
                        {
                            value = "NULL";
                        }
                        else
                        {
                            value = "'" + ((string)propValue).Replace("'", "''") + "'";
                        }

                        sb.AppendFormat("{0} = {1}", prop.Name, value);
                        sb.Append(", ");
                        }
                }
                sb.Remove(sb.Length - 2, 2);
                sb.AppendFormat(" WHERE {0} = '{1}'", id, propList.Single(x => x.Name == id).GetValue(obj, null));
                sbList.Add(sb);
            }
            StringBuilder updateBuilder = new StringBuilder();
            foreach (StringBuilder stringBuilder in sbList)
            {
                updateBuilder.AppendLine(stringBuilder.ToString());
            }
            var updString = updateBuilder.ToString();
            return updString;
        }

        public static int AnonymousInsertCommand(string tableName, IEnumerable<dynamic> list, string id, string connectionString)
        {
            PropertyInfo[] props = null;

            var sbList = new List<StringBuilder>();
            foreach (var obj in list)
            {
                if (props == null)
                {
                    props = obj.GetType().GetProperties();
                }
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat("INSERT INTO {0} ( ", tableName);
                foreach (PropertyInfo prop in props)
                {
                    if (prop.Name != id)
                    {
                        sb.AppendFormat("{0}", prop.Name);
                        sb.Append(", ");
                    }
                }
                sb.Remove(sb.Length - 2, 2);
                sb.Append(") VALUES(");

                foreach (PropertyInfo prop in props)
                {
                    if (prop.Name != id)
                    {
                        string value = "";
                        var propValue = prop.GetValue(obj, null);


                        if (propValue is DateTime)
                        {
                            var t = (DateTime)propValue;
                            value = "'" + t.ToString("yyyyMMdd HH:mm") + "'";
                        }
                        else if (propValue is TimeSpan)
                        {
                            var t = (TimeSpan)propValue;
                            value = "'" + t.ToString() + "'";
                        }
                        else if (propValue is int)
                        {
                            var val = (int)propValue;
                            value = val.ToString();
                        }
                        else if (propValue is byte)
                        {
                            var val = (byte)propValue;
                            value = val.ToString();
                        }
                        else if (propValue is decimal)
                        {
                            var val = (decimal)propValue;
                            value = val.ToString().Replace(",", ".");
                        }
                        else if (propValue is Guid)
                        {
                            var val = (Guid)propValue;
                            value = "'" + val + "'";
                        }
                        else if (propValue is bool)
                        {
                            var val = (bool)propValue;
                            value = val.ToString().Replace("False", "0").Replace("True", "1");
                        }
                        else if (propValue == null)
                        {
                            value = "NULL";
                        }
                        else
                        {
                            value = "'" + ((string)propValue).Replace("'", "''") + "'";
                        }

                        sb.AppendFormat("{0}", value);
                        sb.Append(", ");
                    }
                }
                sb.Remove(sb.Length - 2, 2);
                sb.Append(")");

                sbList.Add(sb);
            }
            StringBuilder insertBuilder = new StringBuilder();
            foreach (StringBuilder stringBuilder in sbList)
            {
                insertBuilder.AppendLine(stringBuilder.ToString());
            }
            var insString = insertBuilder.ToString();
            List<IDScope> idScope = new List<IDScope>();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(insString, con))
                {
                    con.Open();
                    SqlTransaction transaction = con.BeginTransaction();
                    cmd.Transaction = transaction;
                    cmd.CommandTimeout = 0;
                    cmd.ExecuteNonQuery();

                    cmd.CommandText = "Select SCOPE_IDENTITY() as ID";
                    SqlDataReader dr = cmd.ExecuteReader();
                    idScope = DataReaderMapToList<IDScope>(dr);
                    dr.Close();
                    transaction.Commit();
                }
                con.Close();
            }
            return Convert.ToInt32(idScope.First().ID);
        }


        public static int CustomInsertCommand(string dbName, IList list, string id, string connectionString)
        {
            var type = list.GetType().GetGenericArguments()[0];

            var propList = type.GetProperties().Where(IsAcceptableDbTypeInsUpd).ToList();

            var sbList = new List<StringBuilder>();
            foreach (var obj in list)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat("INSERT INTO {0} ( ", dbName);
                foreach (PropertyInfo prop in propList)
                {
                    if (prop.Name != id)
                    {
                        sb.AppendFormat("{0}", prop.Name);
                        sb.Append(", ");
                    }
                }
                sb.Remove(sb.Length - 2, 2);
                sb.Append(") VALUES(");

                foreach (PropertyInfo prop in propList)
                {
                    if (prop.Name != id)
                    {
                        string value = "";
                        var propValue = prop.GetValue(obj, null);

                        if (propValue is DateTime)
                        {
                            var t = (DateTime) propValue;
                            value = "'" + t.ToString("yyyyMMdd HH:mm") + "'";
                        }
                        else if (propValue is TimeSpan)
                        {
                            var t = (TimeSpan)propValue;
                            value = "'" + t.ToString() + "'";
                        }
                        else if (propValue is int)
                        {
                            var val = (int)propValue;
                            value = val.ToString();
                        }
                        else if (propValue is byte)
                        {
                            var val = (byte)propValue;
                            value = val.ToString();
                        }
                        else if (propValue is decimal)
                        {
                            var val = (decimal)propValue;
                            value = val.ToString().Replace(",", ".");
                        }
                        else if (propValue is Guid)
                        {
                            var val = (Guid)propValue;
                            value = "'" + val + "'";
                        }
                        else if (propValue is bool)
                        {
                            var val = (bool)propValue;
                            value = val.ToString().Replace("False", "0").Replace("True", "1");
                        }
                        else if (propValue == null)
                        {
                            value = "NULL";
                        }
                        else
                        {
                            value = "'" + ((string)propValue).Replace("'", "''") + "'";
                        }

                        sb.AppendFormat("{0}", value);
                        sb.Append(", ");
                    }
                }
                sb.Remove(sb.Length - 2, 2);
                sb.Append(")");

                //sb.AppendFormat(" WHERE {0} = '{1}'", id, propList.Single(x => x.Name == id).GetValue(obj, null));
                sbList.Add(sb);
            }
            StringBuilder insertBuilder = new StringBuilder();
            foreach (StringBuilder stringBuilder in sbList)
            {
                insertBuilder.AppendLine(stringBuilder.ToString());
            }
            var insString = insertBuilder.ToString();
            List<IDScope> idScope = new List<IDScope>();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(insString, con))
                {
                    con.Open();
                    SqlTransaction transaction = con.BeginTransaction();
                    cmd.Transaction = transaction;
                    cmd.CommandTimeout = 0;
                    cmd.ExecuteNonQuery();

                    cmd.CommandText = "Select SCOPE_IDENTITY() as ID";
                    SqlDataReader dr = cmd.ExecuteReader();
                    idScope = DataReaderMapToList<IDScope>(dr);
                    dr.Close();
                    transaction.Commit();
                }
                con.Close();
            }
            return Convert.ToInt32(idScope.First().ID);
        }

        public static int ObjectInsertCommand(string tableName, object obj, string id, string connectionString)
        {
            var propList = obj.GetType().GetProperties().Where(IsAcceptableDbTypeInsUpd).ToList();

            var sbList = new List<StringBuilder>();

            var nullableList = CustomSelect<DBaseInfoClass.ColumnsInformationSchema>(
                $"select TABLE_NAME, COLUMN_NAME, ORDINAL_POSITION, COLUMN_DEFAULT, IS_NULLABLE, DATA_TYPE from information_schema.columns where table_name = '{tableName}'",
                SprClass.LocalConnectionString);


            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("INSERT INTO {0} ( ", tableName);
            foreach (PropertyInfo prop in propList)
            {
                var row = nullableList.SingleOrDefault(x => ((string)ObjHelper.GetAnonymousValue(x, "COLUMN_NAME")).ToUpper() == prop.Name.ToUpper());
                var isNullable = (string) ObjHelper.GetAnonymousValue(row, "IS_NULLABLE") != "NO";
                var def = !string.IsNullOrWhiteSpace((string)ObjHelper.GetAnonymousValue(row, "COLUMN_DEFAULT"));

                var isConstruct = !(!isNullable && def && prop.GetValue(obj, null) == null);
                if (prop.Name != id && isConstruct)
                {
                    sb.AppendFormat("{0}", prop.Name);
                    sb.Append(", ");
                }
            }
            sb.Remove(sb.Length - 2, 2);
            sb.Append(") VALUES(");

            foreach (PropertyInfo prop in propList)
            {
                var row = nullableList.SingleOrDefault(x => ((string)ObjHelper.GetAnonymousValue(x, "COLUMN_NAME")).ToUpper() == prop.Name.ToUpper());
                var isNullable = (string)ObjHelper.GetAnonymousValue(row, "IS_NULLABLE") != "NO";
                var def = !string.IsNullOrWhiteSpace((string)ObjHelper.GetAnonymousValue(row, "COLUMN_DEFAULT"));

                var isConstruct = !(!isNullable && def && prop.GetValue(obj, null) == null);

                if (prop.Name != id && isConstruct)
                {
                    string value = "";
                    var propValue = prop.GetValue(obj, null);


                    if (propValue is DateTime)
                    {
                        var t = (DateTime)propValue;
                        value = "'" + t.ToString("yyyyMMdd HH:mm") + "'";
                    }
                    else if (propValue is TimeSpan)
                    {
                        var t = (TimeSpan)propValue;
                        value = "'" + t.ToString() + "'";
                    }
                    else if (propValue is int)
                    {
                        var val = (int)propValue;
                        value = val.ToString();
                    }
                    else if (propValue is byte)
                    {
                        var val = (byte)propValue;
                        value = val.ToString();
                    }
                    else if (propValue is decimal)
                    {
                        var val = (decimal)propValue;
                        value = val.ToString().Replace(",", ".");
                    }
                    else if (propValue is Guid)
                    {
                        var val = (Guid)propValue;
                        value = "'" + val + "'";
                    }
                    else if (propValue is bool)
                    {
                        var val = (bool)propValue;
                        value = val.ToString().Replace("False", "0").Replace("True", "1");
                    }
                    else if (propValue == null)
                    {
                        value = "NULL";
                    }
                    else
                    {
                        value = "'" + ((string) propValue).Replace("'", "''") + "'";
                    }

                    sb.AppendFormat("{0}", value);
                    sb.Append(", ");
                }
            }
            sb.Remove(sb.Length - 2, 2);
            sb.Append(")");

            //sb.AppendFormat(" WHERE {0} = '{1}'", id, propList.Single(x => x.Name == id).GetValue(obj, null));
            sbList.Add(sb);
            //}
            StringBuilder insertBuilder = new StringBuilder();
            foreach (StringBuilder stringBuilder in sbList)
            {
                insertBuilder.AppendLine(stringBuilder.ToString());
            }
            var insString = insertBuilder.ToString();
            List<IDScope> idScope = new List<IDScope>();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(insString, con))
                {
                    con.Open();
                    //SqlTransaction transaction = con.BeginTransaction();
                    //cmd.Transaction = transaction;
                    cmd.CommandTimeout = 0;
                    cmd.ExecuteNonQuery();

                    cmd.CommandText = "Select SCOPE_IDENTITY() as ID";
                    SqlDataReader dr = cmd.ExecuteReader();
                    idScope = DataReaderMapToList<IDScope>(dr);
                    dr.Close();
                    //transaction.Commit();
                }
                con.Close();
            }
            return Convert.ToInt32(idScope.First().ID);
        }

        public class IDScope
        {
            public decimal ID { get; set; }
        }

        static public void CustomExecuteQuery(string command, string connectionString)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(command, con))
                {
                    con.Open();
                    cmd.CommandTimeout = 0;
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
        }

        public static string CustomUpdateCommand(string tableName, IList list, string id)
        {
            var type = list.GetType().GetGenericArguments()[0];

            var propList = type.GetProperties().Where(IsAcceptableDbTypeInsUpd).ToList();
            var sbList = new List<StringBuilder>();
            foreach (var obj in list)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat("UPDATE {0} SET ", tableName);
                foreach (PropertyInfo prop in propList)
                {

                    if (prop.Name != id)
                    {
                        string value = "";
                        var propValue = prop.GetValue(obj, null);

                        if (propValue is DateTime)
                        {
                            var t = (DateTime)propValue;
                            value = "'" + t.ToString("yyyyMMdd HH:mm") + "'";
                        }
                        else if (propValue is TimeSpan)
                        {
                            var t = (TimeSpan)propValue;
                            value = "'" + t.ToString() + "'";
                        }
                        else if (propValue is int)
                        {
                            var val = (int)propValue;
                            value = val.ToString();
                        }
                        else if (propValue is byte)
                        {
                            var val = (byte)propValue;
                            value = val.ToString();
                        }
                        else if (propValue is decimal)
                        {
                            var val = (decimal)propValue;
                            value = val.ToString().Replace(",", ".");
                        }
                        else if (propValue is Guid)
                        {
                            var val = (Guid)propValue;
                            value = "'" + val + "'";
                        }
                        else if (propValue is bool)
                        {
                            var val = (bool)propValue;
                            value = val.ToString().Replace("False", "0").Replace("True", "1");
                        }
                        else if (propValue == null)
                        {
                            value = "NULL";
                        }
                        else
                        {
                            value = "'" + ((string)propValue).Replace("'", "''") + "'";
                        }

                        sb.AppendFormat("{0} = {1}", prop.Name, value);
                        sb.Append(", ");
                    }
                }
                sb.Remove(sb.Length - 2, 2);
                sb.AppendFormat(" WHERE {0} = '{1}'", id, propList.Single(x => x.Name == id).GetValue(obj, null));
                sbList.Add(sb);
            }
            StringBuilder updateBuilder = new StringBuilder();
            foreach (StringBuilder stringBuilder in sbList)
            {updateBuilder.AppendLine(stringBuilder.ToString());
            }
            var updString = updateBuilder.ToString();
            return updString;
        }
        public static string CustomUpdateCommand(string tableName, object obj, string id)
        {
            var propList = obj.GetType().GetProperties().Where(IsAcceptableDbTypeInsUpd).ToList();
            var sbList = new List<StringBuilder>();
            //foreach (var obj in list)
            //{
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat("UPDATE {0} SET ", tableName);
            foreach (PropertyInfo prop in propList)
            {

                if (prop.Name != id)
                {
                    string value = "";
                    var propValue = prop.GetValue(obj, null);


                    if (propValue is DateTime)
                    {
                        var t = (DateTime)propValue;
                        value = "'" + t.ToString("yyyyMMdd HH:mm") + "'";
                    }
                    else if (propValue is TimeSpan)
                    {
                        var t = (TimeSpan)propValue;
                        value = "'" + t.ToString() + "'";
                    }
                    else if (propValue is int)
                    {
                        var val = (int)propValue;
                        value = val.ToString();
                    }
                    else if (propValue is byte)
                    {
                        var val = (byte)propValue;
                        value = val.ToString();
                    }
                    else if (propValue is decimal)
                    {
                        var val = (decimal)propValue;
                        value = val.ToString().Replace(",", ".");
                    }
                    else if (propValue is Guid)
                    {
                        var val = (Guid)propValue;
                        value = "'" + val + "'";
                    }
                    else if (propValue is bool)
                    {
                        var val = (bool)propValue;
                        value = val.ToString().Replace("False", "0").Replace("True", "1");
                    }
                    else if (propValue == null)
                    {
                        value = "NULL";
                    }
                    else
                    {
                        value = "'" + Convert.ToString(propValue).Replace("'", "''") + "'";
                    }

                    sb.AppendFormat("{0} = {1}", prop.Name, value);
                    sb.Append(", ");
                }
            }
            sb.Remove(sb.Length - 2, 2);

            sb.AppendFormat(" WHERE {0} = '{1}'", id, propList.Single(x => x.Name == id).GetValue(obj, null));
                sbList.Add(sb);
            //}
            StringBuilder updateBuilder = new StringBuilder();
            foreach (StringBuilder stringBuilder in sbList)
            {
                updateBuilder.AppendLine(stringBuilder.ToString());
            }
            var updString = updateBuilder.ToString();
            return updString;
        }

        public static string CustomDeleteCommand(string dbName, IEnumerable list, string id)
        {
            var sbList = new List<StringBuilder>();
            foreach (var obj in list)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat("DELETE {0}", dbName);
                sb.AppendFormat(" WHERE {0} = '{1}'", id, obj);
                sbList.Add(sb);
            }
            StringBuilder deleteBuilder = new StringBuilder();
            foreach (StringBuilder stringBuilder in sbList)
            {
                deleteBuilder.AppendLine(stringBuilder.ToString());
            }
            var delString = deleteBuilder.ToString();
            return delString;
        }

        public static int CustomDeleteCommand(string dbName, object obj, string id, string connectionString)
        {
            var sbList = new List<StringBuilder>();
            //foreach (var obj in list)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat("DELETE {0}", dbName);
                sb.AppendFormat(" WHERE {0} = '{1}'", id, obj);
                sbList.Add(sb);
            }

            StringBuilder deleteBuilder = new StringBuilder();
            foreach (StringBuilder stringBuilder in sbList)
            {
                deleteBuilder.AppendLine(stringBuilder.ToString());
            }

            var delString = deleteBuilder.ToString();
            List<IDScope> idScope;

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(delString, con))
                {
                    con.Open();
                    SqlTransaction transaction = con.BeginTransaction();
                    cmd.Transaction = transaction;
                    cmd.CommandTimeout = 0;
                    cmd.ExecuteNonQuery();

                    cmd.CommandText = "Select SCOPE_IDENTITY() as ID";
                    SqlDataReader dr = cmd.ExecuteReader();
                    idScope = DataReaderMapToList<IDScope>(dr);
                    dr.Close();
                    transaction.Commit();
                }
                con.Close();
            }
            return Convert.ToInt32(idScope.First().ID);

        }

        public static void BulkInsert(object list, int cnt, string connectionString)
        {
            var type = list.GetType().GetGenericArguments()[0];

            var genericListType = typeof(List<>);
            var listOfType = genericListType.MakeGenericType(type);
            var tempList = (IList)Activator.CreateInstance(listOfType);
            var pkCol = DBaseInfo.GetPrimaryKeyInfo(type.Name, connectionString);
            var info = (IList)DBaseInfo.GetColumnInfo(type.Name, pkCol.Column_Name, connectionString);
            var isIdentity = (bool)ObjHelper.GetAnonymousValue(info[0], "is_identity");

            var i = 0;
            foreach (var item in (IList)list)
            {
                tempList.Add(item);
                i++;
                if (i == cnt)
                {
                    Reader2List.CustomInsertCommand(type.Name, tempList, isIdentity ? pkCol.Column_Name : "not_identity",
                        connectionString);
                    i = 0;
                    tempList.Clear();
                }
            }
            if (tempList.Count > 0)
            {
                Reader2List.CustomInsertCommand(type.Name, tempList, isIdentity ? pkCol.Column_Name : "not_identity",
                    connectionString);
                tempList.Clear();
            }
        }


    }
}
