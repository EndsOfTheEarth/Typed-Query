
/*
 * 
 * Copyright (C) 2009-2019 JFo.nz
 * 
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU Lesser General Public License as published by
 * the Free Software Foundation, version 3 of the License.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU Lesser General Public License
 * along with this program.  If not, see http://www.gnu.org/licenses/.
 **/

using System;
using System.Collections.Generic;
using System.Text;

namespace Sql.Database.PostgreSql {

    public static class DatabaseDetails {

        public static DbTable? GetTable(ADatabase pDatabase, Sql.ATable pTable) {
            IList<DbTable> tables = GetTables(pTable.TableName, pDatabase.GetConnection(false));
            return tables.Count > 0 ? tables[0] : null;
        }

        public static IList<DbTable> GetTables(System.Data.Common.DbConnection pConnection) {
            return GetTables(null, pConnection);
        }

        private static IList<DbTable> GetTables(string? pTableName, System.Data.Common.DbConnection pConnection) {

            IList<DbTable> tableList;

            using(System.Data.Common.DbCommand command = Transaction.CreateCommand(pConnection, null)) {

                string whereClause = !string.IsNullOrEmpty(pTableName) ? "where tables.table_name ILIKE '" + pTableName + "' " : string.Empty;

                command.CommandText = "select tables.table_name, tables.table_schema, columns.column_name, " +
                    "columns.is_nullable, columns.udt_name, columns.column_default, key_usage.ordinal_position " +
                    "from information_schema.tables as tables " +
                    "join information_schema.columns as columns on tables.table_catalog = columns.table_catalog and tables.table_schema = columns.table_schema and  tables.table_name = columns.table_name " +
                    "left join information_schema.key_column_usage as key_usage on tables.table_catalog = key_usage.table_catalog and tables.table_schema = key_usage.table_schema and tables.table_name = key_usage.table_name and columns.column_name = key_usage.column_name " +
                    whereClause +
                    "order by table_schema, table_name, columns.ordinal_position";

                command.Connection = pConnection;

                System.Data.Common.DbDataReader reader = command.ExecuteReader();

                tableList = new List<DbTable>();
                Dictionary<string, DbTable> tables = new Dictionary<string, DbTable>();

                Dictionary<string, DbColumn> columnLookup = new Dictionary<string, DbColumn>();

                while(reader.Read()) {

                    string tableName = reader.GetString(0);
                    string schemaName = reader.GetString(1);
                    string columnName = reader.GetString(2);
                    string isNullable = reader.GetString(3);
                    string dataType = reader.GetString(4);
                    string columnDefault = !reader.IsDBNull(5) ? reader.GetString(5) : string.Empty;
                    bool isPrimaryKey = !reader.IsDBNull(6);

                    if(!tables.ContainsKey(tableName)) {
                        DbTable table = new DbTable(tableName, schemaName, new List<DbColumn>());
                        tables.Add(tableName, table);
                        tableList.Add(table);
                    }

                    string columnKey = tableName + "*" + columnName;

                    if(!columnLookup.ContainsKey(columnKey)) {

                        bool isAutoColumn = columnDefault.StartsWith("nextval");

                        IList<DbColumn> columns = tables[tableName].Columns;
                        DbColumn column = new DbColumn(columnName, GetDataType(dataType), isNullable == "YES", isPrimaryKey, isAutoColumn, false);
                        columns.Add(column);

                        columnLookup.Add(columnKey, column);
                    }
                    else {  //In some cases the query brings through duplicate rows for primary keys with more than one column. This could be fixed in the future by breaking the load into two queries.
                        if(isPrimaryKey) {
                            columnLookup[columnKey].IsPrimaryKey = true;
                        }
                    }
                }
            }
            pConnection.Close();
            return tableList;
        }

        private static Dictionary<string, System.Data.DbType>? sTypeLookup;

        private static System.Data.DbType GetDataType(string pDataType) {

            if(sTypeLookup == null) {

                sTypeLookup = new Dictionary<string, System.Data.DbType>();

                sTypeLookup.Add("anyarray", System.Data.DbType.Object);
                sTypeLookup.Add("inet", System.Data.DbType.Object);
                sTypeLookup.Add("_text", System.Data.DbType.Object);
                sTypeLookup.Add("xid", System.Data.DbType.Object);
                sTypeLookup.Add("_char", System.Data.DbType.Object);
                sTypeLookup.Add("name", System.Data.DbType.Object);
                sTypeLookup.Add("oidvector", System.Data.DbType.Object);
                sTypeLookup.Add("_aclitem", System.Data.DbType.Object);
                sTypeLookup.Add("bytea", System.Data.DbType.Binary);
                sTypeLookup.Add("_int2", System.Data.DbType.Object);
                sTypeLookup.Add("timestamptz", System.Data.DbType.DateTime);
                sTypeLookup.Add("int4", System.Data.DbType.Int32);
                sTypeLookup.Add("int8", System.Data.DbType.Int64);
                sTypeLookup.Add("uuid", System.Data.DbType.Guid);
                sTypeLookup.Add("_float4", System.Data.DbType.Single);
                sTypeLookup.Add("char", System.Data.DbType.String);
                sTypeLookup.Add("int2vector", System.Data.DbType.Object);
                sTypeLookup.Add("bool", System.Data.DbType.Boolean);
                sTypeLookup.Add("interval", System.Data.DbType.Object);
                sTypeLookup.Add("abstime", System.Data.DbType.DateTime);
                sTypeLookup.Add("float4", System.Data.DbType.Single);
                sTypeLookup.Add("float8", System.Data.DbType.Double);
                sTypeLookup.Add("oid", System.Data.DbType.Object);
                sTypeLookup.Add("int2", System.Data.DbType.Int16);
                sTypeLookup.Add("_oid", System.Data.DbType.Object);
                sTypeLookup.Add("varchar", System.Data.DbType.String);
                sTypeLookup.Add("regproc", System.Data.DbType.Object);
                sTypeLookup.Add("text", System.Data.DbType.String);
                sTypeLookup.Add("_regtype", System.Data.DbType.Object);
                sTypeLookup.Add("timestamp", System.Data.DbType.DateTime);
                sTypeLookup.Add("numeric", System.Data.DbType.Decimal);
            }
            string key = pDataType.ToLower();
            return sTypeLookup.ContainsKey(key) ? sTypeLookup[key] : System.Data.DbType.Object;
        }
    }
}