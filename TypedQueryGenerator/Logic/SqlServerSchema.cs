﻿
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

namespace TypedQuery.Logic {

    public class SqlServerSchema {

        public SqlServerSchema() {

        }

        public void TestConnection(Sql.ADatabase pDatabase) {

            SqlServer.Tables.Table tablesTable = new SqlServer.Tables.Table();

            Sql.IResult result = Sql.Query
                .Select(tablesTable).Top(1)
                .From(tablesTable)
                .Execute(pDatabase);
        }
        public IList<ITable> GetTableList(Sql.ADatabase pDatabase) {

            SqlServer.Tables.Table tablesTable = new SqlServer.Tables.Table();

            Sql.IResult result = Sql.Query
                .Select(tablesTable.Table_name, tablesTable.Table_schema, tablesTable.Table_Type)
                .From(tablesTable)
                .OrderBy(tablesTable.Table_schema, tablesTable.Table_name)
                .Execute(pDatabase);

            List<ITable> tableList = new List<ITable>(result.Count);

            for(int index = 0; index < result.Count; index++) {

                SqlServer.Tables.Row tablesRow = tablesTable[index, result];

                bool isView = string.Compare(tablesRow.Table_Type, "view", true) == 0;

                tableList.Add(new Table(Sql.DatabaseType.Mssql, tablesRow.Table_name, tablesRow.Table_schema, isView));
            }
            return tableList;
        }

        public bool GetTableDetails(Sql.ADatabase pDatabase, string pTableName, string pSchemaName, out ITableDetails? pTableDetails, out string pErrorText) {

            if(string.IsNullOrEmpty(pTableName)) {
                throw new ArgumentException("pTableName cannot be null or empty");
            }

            if(string.IsNullOrEmpty(pSchemaName)) {
                throw new ArgumentException("pSchemaName cannot be null or empty");
            }

            pTableDetails = null;
            pErrorText = string.Empty;

            TableDetails tableDetails;

            if(true) {

                SqlServer.Tables.Table tablesTable = new SqlServer.Tables.Table();

                Sql.IResult result = Sql.Query
                    .Select(tablesTable.Table_name, tablesTable.Table_schema, tablesTable.Table_Type)
                    .From(tablesTable)
                    .Where(tablesTable.Table_name == pTableName & tablesTable.Table_schema == pSchemaName)
                    .Execute(pDatabase);

                if(result.Count == 0) {
                    pErrorText = "Cannot find table '" + pTableName + "' in database for the schema '" + pSchemaName + "'";
                    return false;
                }

                if(result.Count > 1) {
                    pErrorText = "Found table '" + pTableName + " more than once in database for the schema '" + pSchemaName + "'";
                    return false;
                }

                SqlServer.Tables.Row tablesRow = tablesTable[0, result];

                bool isView = string.Compare(tablesRow.Table_Type, "view", true) == 0;

                tableDetails = new TableDetails(tablesRow.Table_name, tablesRow.Table_schema, isView);
            }

            foreach(IColumn column in GetColumns(pDatabase, pTableName, pSchemaName)) {
                tableDetails.Columns.Add(column);
            }

            tableDetails.PrimaryKey = GetPrimaryKey(pDatabase, pTableName, pSchemaName, tableDetails.Columns);

            foreach(IForeignKey foreignKey in GetForeignKeys(pDatabase, pTableName, pSchemaName, tableDetails.Columns)) {
                tableDetails.ForeignKeys.Add(foreignKey);
            }
            GetComments(tableDetails, pTableName, pSchemaName);
            pTableDetails = tableDetails;
            return true;
        }

        private IList<IColumn> GetColumns(Sql.ADatabase pDatabase, string pTableName, string pSchemaName) {

            SqlServer.Columns.Table columnsTable = new SqlServer.Columns.Table();
            SqlServerSchema.IsIdentity isIdentity = new SqlServerSchema.IsIdentity(columnsTable);

            Sql.IResult result = Sql.Query
                .Select(columnsTable.Column_Name, columnsTable.Data_Type, columnsTable.Is_Nullable, isIdentity, columnsTable.Character_Maximum_Length)
                .From(columnsTable)
                .Where(columnsTable.Table_Schema == pSchemaName & columnsTable.Table_Name == pTableName)
                .OrderBy(columnsTable.Ordinal_Position)
                .Execute(pDatabase);

            List<IColumn> columnList = new List<IColumn>(result.Count);

            for(int index = 0; index < result.Count; index++) {

                SqlServer.Columns.Row columnsRow = columnsTable[index, result];

                System.Data.DbType dataType = GetDataType(columnsRow.Data_Type);
                bool isNullable = string.Compare(columnsRow.Is_Nullable, "yes", true) == 0;
                bool isAutoGenerated = isIdentity[index, result]!.Value == 1;
                int? maxLength = columnsRow.Character_Maximum_Length;

                columnList.Add(new Column(columnsRow.Column_Name, dataType, isNullable, isAutoGenerated, maxLength));
            }
            return columnList;
        }

        private IPrimaryKey? GetPrimaryKey(Sql.ADatabase pDatabase, string pTableName, string pSchemaName, IList<IColumn> pColumns) {

            SqlServer.Table_Constraints.Table tableConstaintsTable = new SqlServer.Table_Constraints.Table();
            SqlServer.Key_Column_Usage.Table keyColumnUsageTable = new SqlServer.Key_Column_Usage.Table();

            Sql.IResult result = Sql.Query
                .Select(tableConstaintsTable.Constraint_Name, keyColumnUsageTable.Column_Name)
                .From(tableConstaintsTable)
                .Join(keyColumnUsageTable, tableConstaintsTable.Constraint_Name == keyColumnUsageTable.Constraint_Name & keyColumnUsageTable.Table_Name == tableConstaintsTable.Table_Name & keyColumnUsageTable.Table_Schema == tableConstaintsTable.Table_Schema)
                .Where(tableConstaintsTable.Table_Name == pTableName & tableConstaintsTable.Table_Schema == pSchemaName & tableConstaintsTable.Constraint_Type == "PRIMARY KEY")
                .OrderBy(keyColumnUsageTable.Ordinal_Position)
                .Execute(pDatabase);

            PrimaryKey? primaryKey = null;

            if(result.Count > 0) {

                primaryKey = new PrimaryKey(tableConstaintsTable[0, result].Constraint_Name);

                for(int index = 0; index < result.Count; index++) {

                    string columnName = keyColumnUsageTable[index, result].Column_Name;

                    foreach(IColumn column in pColumns) {

                        if(string.Compare(column.ColumnName, columnName, true) == 0) {
                            primaryKey.Columms.Add(column);
                            ((Column)column).IsPrimaryKey = true;
                            break;
                        }
                    }
                }
            }
            return primaryKey;
        }

        private IList<IForeignKey> GetForeignKeys(Sql.ADatabase pDatabase, string pTableName, string pSchemaName, IList<IColumn> pColumns) {

            SqlServer.Referential_Constraints.Table rcTable = new SqlServer.Referential_Constraints.Table();
            SqlServer.Key_Column_Usage.Table kcu1Table = new SqlServer.Key_Column_Usage.Table();
            SqlServer.Key_Column_Usage.Table kcu2Table = new SqlServer.Key_Column_Usage.Table();

            Sql.IResult result = Sql.Query
                .Select(kcu1Table.Constraint_Name, kcu1Table.Column_Name, kcu2Table.Table_Name, kcu2Table.Column_Name)
                .From(rcTable)
                .Join(kcu1Table, kcu1Table.Constraint_Catalog == rcTable.Constraint_Catalog & kcu1Table.Constraint_Schema == rcTable.Constraint_Schema & kcu1Table.Constraint_Name == rcTable.Constraint_Name)
                .Join(kcu2Table, kcu2Table.Constraint_Catalog == rcTable.Unique_Constraint_Catalog & kcu2Table.Constraint_Schema == rcTable.Unique_Constraint_Schema & kcu2Table.Constraint_Name == rcTable.Unique_Constraint_Name & kcu2Table.Ordinal_Position == kcu1Table.Ordinal_Position)
                .Where(kcu1Table.Table_Name == pTableName & kcu1Table.Table_Schema == pSchemaName)
                .Execute(pDatabase);

            Dictionary<string, IForeignKey> keysLookup = new Dictionary<string, IForeignKey>();
            List<IForeignKey> foreignKeyList = new List<IForeignKey>();

            for(int index = 0; index < result.Count; index++) {

                string constraintName = kcu1Table[index, result].Constraint_Name;
                string foreignColumnName = kcu1Table[index, result].Column_Name;
                string primaryTableName = kcu2Table[index, result].Table_Name;
                string primaryColumnName = kcu2Table[index, result].Column_Name;

                string key = constraintName.ToLower();

                IForeignKey? foreignKey;

                if(!keysLookup.TryGetValue(key, out foreignKey)) {
                    foreignKey = new ForeignKey(constraintName);
                    keysLookup.Add(key, foreignKey);
                    foreignKeyList.Add(foreignKey);
                }

                IColumn? foreignKeyColumn = null;

                foreach(IColumn column in pColumns) {

                    if(string.Compare(column.ColumnName, foreignColumnName, true) == 0) {
                        foreignKeyColumn = column;
                        break;
                    }
                }

                if(foreignKeyColumn == null) {
                    throw new Exception("Could not find column '" + foreignColumnName + "' in the constraint '" + constraintName + "'. This is an unexpected state.");
                }
                foreignKey.KeyColumns.Add(new KeyColumn(foreignKeyColumn, primaryTableName, primaryColumnName));
            }
            return foreignKeyList;
        }

        private void GetComments(TableDetails pTableDetails, string pTableName, string pSchemaName) {

            //TODO:
            //SELECT objname as table_name, value as description FROM fn_listextendedproperty ('MS_DESCRIPTION','schema', 'dbo', 'table', 'sec_user', null, null)
            //EXEC sp_addextendedproperty @name = N'MS_Description', @value = 'description goes here', @level0type = N'Schema', @level0name = 'dbo', @level1type = N'Table',  @level1name = 'sec_user';
        }

        public static List<IStoredProcedureDetail> GetStoredProcedures(System.Data.Common.DbConnection pConnection) {

            List<IStoredProcedureDetail> spList = new List<IStoredProcedureDetail>();

            using(System.Data.Common.DbCommand command = pConnection.CreateCommand()) {

                command.CommandText = "SELECT SCHEMA_NAME(schema_id) AS schema_name, o.name AS object_name, p.parameter_id, p.name AS parameter_name" +
                                        ",TYPE_NAME(p.user_type_id) AS parameter_type, p.is_output as is_output " +
                                        "FROM sys.objects AS o " +
                                        "LEFT JOIN sys.parameters AS p ON o.object_id = p.object_id " +
                                        "WHERE type_desc = 'SQL_STORED_PROCEDURE' " +
                                        "ORDER BY schema_name, object_name, p.parameter_id;";

                command.Connection = pConnection;

                using(System.Data.Common.DbDataReader reader = command.ExecuteReader()) {

                    Dictionary<string, StoredProcedureDetail> lookup = new Dictionary<string, StoredProcedureDetail>();

                    while(reader.Read()) {

                        string schema = reader.GetString(0);
                        string name = reader.GetString(1);

                        string key = schema + "*" + name;

                        StoredProcedureDetail sp;

                        if(!lookup.ContainsKey(key)) {
                            sp = new StoredProcedureDetail(schema, name);
                            lookup.Add(key, sp);
                            spList.Add(sp);
                        }
                        else
                            sp = lookup[key];

                        if(!reader.IsDBNull(2)) {

                            int paramId = reader.GetInt32(2);
                            string paramName = reader.GetString(3);
                            string paramType = reader.GetString(4);
                            bool isOutput = reader.GetBoolean(5);

                            System.Data.ParameterDirection direction = System.Data.ParameterDirection.Input;

                            if(isOutput) {
                                direction = System.Data.ParameterDirection.InputOutput;
                            }
                            sp.AddParameter(new SpParameter(paramId, paramName, GetDataType(paramType), direction));
                        }
                    }
                }
            }
            return spList;
        }

        private static Dictionary<string, System.Data.DbType>? sTypeLookup;

        private static System.Data.DbType GetDataType(string pDataType) {

            if(sTypeLookup == null) {

                sTypeLookup = new Dictionary<string, System.Data.DbType>();

                sTypeLookup.Add("bigint", System.Data.DbType.Int64);
                sTypeLookup.Add("bit", System.Data.DbType.Boolean);
                sTypeLookup.Add("char", System.Data.DbType.String);
                sTypeLookup.Add("nchar", System.Data.DbType.String);
                sTypeLookup.Add("date", System.Data.DbType.Date);
                sTypeLookup.Add("time", System.Data.DbType.Time);
                sTypeLookup.Add("datetime", System.Data.DbType.DateTime);
                sTypeLookup.Add("datetime2", System.Data.DbType.DateTime2);
                sTypeLookup.Add("datetimeoffset", System.Data.DbType.DateTimeOffset);
                sTypeLookup.Add("float", System.Data.DbType.Double);
                sTypeLookup.Add("real", System.Data.DbType.Single);
                sTypeLookup.Add("image", System.Data.DbType.Binary);
                sTypeLookup.Add("int", System.Data.DbType.Int32);
                sTypeLookup.Add("ntext", System.Data.DbType.String);
                sTypeLookup.Add("numeric", System.Data.DbType.Decimal);
                sTypeLookup.Add("decimal", System.Data.DbType.Decimal);
                sTypeLookup.Add("money", System.Data.DbType.Decimal);
                sTypeLookup.Add("nvarchar", System.Data.DbType.String);
                sTypeLookup.Add("smallint", System.Data.DbType.Int16);
                sTypeLookup.Add("tinyint", System.Data.DbType.Byte);
                sTypeLookup.Add("uniqueidentifier", System.Data.DbType.Guid);
                sTypeLookup.Add("varbinary", System.Data.DbType.Binary);
                sTypeLookup.Add("varchar", System.Data.DbType.String);
            }
            string key = pDataType.ToLower();

            return sTypeLookup.ContainsKey(key) ? sTypeLookup[key] : System.Data.DbType.Object;
        }

        private class IsIdentity : Sql.Function.ANumericFunction {

            private readonly SqlServer.Columns.Table mColumnTable;

            public IsIdentity(SqlServer.Columns.Table pColumnTable) {

                if(pColumnTable == null) {
                    throw new NullReferenceException("ColumnTable cannot be null");
                }
                mColumnTable = pColumnTable;
            }

            public int? this[int pIndex, Sql.IResult pResult] {
                get {
                    return (int?)pResult.GetValue(this, pIndex);
                }
            }
            public override string GetFunctionSql(Sql.ADatabase pDatabase, bool pUseAlias, Sql.Database.IAliasManager pAliasManager) {
                return "columnproperty(object_id(quotename(" + mColumnTable.Table_Schema.ColumnName + ") + '.' + quotename(" + mColumnTable.Table_Name.ColumnName + ")), " + mColumnTable.Column_Name.ColumnName + ", 'IsIdentity')";
            }
            public override object GetValue(Sql.ADatabase pDatabase, System.Data.Common.DbDataReader pReader, int pColumnIndex) {
                return pReader.GetInt32(pColumnIndex);
            }
        }
    }
}