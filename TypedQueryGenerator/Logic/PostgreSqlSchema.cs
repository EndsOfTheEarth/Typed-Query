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

    public class PostgreSqlSchema {

        public void TestConnection(Sql.ADatabase pDatabase) {

            Postgresql.Tables.Table tablesTable = new Postgresql.Tables.Table();

            Sql.IResult result = Sql.Query
                .Select(tablesTable).Top(1)
                .From(tablesTable)
                .Execute(pDatabase);
        }
        public IList<ITable> GetTableList(Sql.ADatabase pDatabase) {

            Postgresql.Tables.Table tablesTable = new Postgresql.Tables.Table();

            Sql.IResult result = Sql.Query
                .Select(tablesTable.Table_name, tablesTable.Table_schema)
                .From(tablesTable)
                .OrderBy(tablesTable.Table_schema, tablesTable.Table_name)
                .Execute(pDatabase);

            List<ITable> tableList = new List<ITable>(result.Count);

            for(int index = 0; index < result.Count; index++) {

                Postgresql.Tables.Row tablesRow = tablesTable[index, result];

                tableList.Add(new Table(Sql.DatabaseType.PostgreSql, tablesRow.Table_name, tablesRow.Table_schema, false));
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

                Postgresql.Tables.Table tablesTable = new Postgresql.Tables.Table();

                Sql.IResult result = Sql.Query
                    .Select(tablesTable.Table_name, tablesTable.Table_schema, tablesTable.Table_type)
                    .From(tablesTable)
                    .Where(tablesTable.Table_name == pTableName.ToLower() & tablesTable.Table_schema == pSchemaName.ToLower())
                    .OrderBy(tablesTable.Table_schema, tablesTable.Table_name)
                    .Execute(pDatabase);

                if(result.Count == 0) {
                    pErrorText = "Cannot find table '" + pTableName + "' in database for the schema '" + pSchemaName + "'";
                    return false;
                }

                if(result.Count > 1) {
                    pErrorText = "Found table '" + pTableName + " more than once in database for the schema '" + pSchemaName + "'";
                    return false;
                }

                Postgresql.Tables.Row tablesRow = tablesTable[0, result];

                bool isView = string.Compare(tablesRow.Table_type, "view", true) == 0;
                tableDetails = new TableDetails(tablesRow.Table_name, tablesRow.Table_schema, isView);
            }

            foreach(IColumn column in GetColumns(pDatabase, pTableName, pSchemaName)) {
                tableDetails.Columns.Add(column);
            }

            tableDetails.PrimaryKey = GetPrimaryKey(pDatabase, pTableName, pSchemaName, tableDetails.Columns);

            foreach(IForeignKey foreignKey in GetForeignKeys(pDatabase, pTableName, pSchemaName, tableDetails.Columns)) {
                tableDetails.ForeignKeys.Add(foreignKey);
            }
            GetComments(pDatabase, tableDetails, pTableName, pSchemaName);
            pTableDetails = tableDetails;
            return true;
        }

        private IList<IColumn> GetColumns(Sql.ADatabase pDatabase, string pTableName, string pSchemaName) {

            Postgresql.Columns.Table columnsTable = new Postgresql.Columns.Table();

            Sql.IResult result = Sql.Query
                .Select(columnsTable.Column_name, columnsTable.Udt_name, columnsTable.Is_nullable, columnsTable.Column_default, columnsTable.Character_maximum_length)
                .From(columnsTable)
                .Where(columnsTable.Table_name == pTableName.ToLower() & columnsTable.Table_schema == pSchemaName.ToLower())
                .OrderBy(columnsTable.Ordinal_position)
                .Execute(pDatabase);

            List<IColumn> columnList = new List<IColumn>(result.Count);

            for(int index = 0; index < result.Count; index++) {

                Postgresql.Columns.Row columnsRow = columnsTable[index, result];

                System.Data.DbType dataType = GetDataType(columnsRow.Udt_name);
                bool isNullable = string.Compare(columnsRow.Is_nullable, "yes", true) == 0;
                bool isAutoGenerated = (columnsRow.Column_default ?? string.Empty).StartsWith("nextval");
                int? maxLength = columnsRow.Character_maximum_length;

                columnList.Add(new Column(columnsRow.Column_name, dataType, isNullable, isAutoGenerated, maxLength));
            }
            return columnList;
        }

        private IPrimaryKey? GetPrimaryKey(Sql.ADatabase pDatabase, string pTableName, string pSchemaName, IList<IColumn> pColumns) {

            Postgresql.TableConstraints.Table tcTable = new Postgresql.TableConstraints.Table();
            Postgresql.ConstraintColumnUsage.Table ccuTable = new Postgresql.ConstraintColumnUsage.Table();

            Sql.IResult result = Sql.Query
                .Select(tcTable.Constraint_name, ccuTable.Column_name)
                .From(tcTable)
                .Join(ccuTable, ccuTable.Table_schema == tcTable.Table_schema & ccuTable.Table_name == tcTable.Table_name & ccuTable.Constraint_name == tcTable.Constraint_name)
                .Where(tcTable.Table_name == pTableName.ToLower() & tcTable.Table_schema == pSchemaName.ToLower() & tcTable.Constraint_type == "PRIMARY KEY")
                .Execute(pDatabase);

            PrimaryKey? primaryKey = null;

            if(result.Count > 0) {

                primaryKey = new PrimaryKey(tcTable[0, result].Constraint_name);

                for(int index = 0; index < result.Count; index++) {

                    string columnName = ccuTable[index, result].Column_name;

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

            Postgresql.TableConstraints.Table tcTable = new Postgresql.TableConstraints.Table();
            Postgresql.ReferentialConstraints.Table rcTable = new Postgresql.ReferentialConstraints.Table();
            Postgresql.KeyColumnUsage.Table kcuForeignTable = new Postgresql.KeyColumnUsage.Table();
            Postgresql.KeyColumnUsage.Table kcuPrimaryTable = new Postgresql.KeyColumnUsage.Table();

            Sql.IResult result = Sql.Query
                .Select(kcuForeignTable.Constraint_name, kcuForeignTable.Column_name, kcuPrimaryTable.Table_name, kcuPrimaryTable.Column_name)
                .From(tcTable)
                .Join(rcTable, rcTable.Constraint_name == tcTable.Constraint_name)
                .Join(kcuForeignTable, kcuForeignTable.Constraint_name == rcTable.Constraint_name)
                .Join(kcuPrimaryTable, kcuPrimaryTable.Ordinal_position == kcuForeignTable.Position_in_unique_constraint & rcTable.Unique_constraint_name == kcuPrimaryTable.Constraint_name)
                .Where(tcTable.Table_name == pTableName.ToLower() & tcTable.Table_schema == pSchemaName.ToLower())
                .OrderBy(rcTable.Constraint_name, kcuForeignTable.Ordinal_position)
                .Execute(pDatabase);

            Dictionary<string, IForeignKey> keysLookup = new Dictionary<string, IForeignKey>();
            List<IForeignKey> foreignKeyList = new List<IForeignKey>();

            for(int index = 0; index < result.Count; index++) {

                string constraintName = kcuForeignTable[index, result].Constraint_name;
                string foreignColumnName = kcuForeignTable[index, result].Column_name;
                string primaryTableName = kcuPrimaryTable[index, result].Table_name;
                string primaryColumnName = kcuPrimaryTable[index, result].Column_name;

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

        private void GetComments(Sql.ADatabase pDatabase, TableDetails pTableDetails, string pTableName, string pSchemaName) {

            Postgresql.Tables.Table tablesTable = new Postgresql.Tables.Table();

            Postgresql.Pg_Class.Table pgClassTable = new Postgresql.Pg_Class.Table();
            Postgresql.Pg_Description.Table pgDescTable = new Postgresql.Pg_Description.Table();

            Sql.IResult result = Sql.Query
                .Select(pgDescTable.Description)
                .From(tablesTable)
                .Join(pgClassTable, tablesTable.Table_name == pgClassTable.Name & pgClassTable.Kind == "r")
                .Join(pgDescTable, pgDescTable.ObjOid == pgClassTable.Oid & pgDescTable.ObjSubId == 0)
                .Where(tablesTable.Table_schema == pSchemaName.ToLower() & tablesTable.Table_name == pTableName.ToLower())
                .Execute(pDatabase);

            if(result.Count == 1) {
                pTableDetails.Description = pgDescTable[0, result].Description;
            }
            else if(result.Count > 1) {
                throw new Exception("Table description query returned more than one row. Only on table description row is expected");
            }

            Postgresql.pg_attribute.Table pgAttributeTable = new Postgresql.pg_attribute.Table();

            result = Sql.Query
                .Select(pgAttributeTable.Name, pgDescTable.Description)
                .From(pgAttributeTable)
                .Join(pgClassTable, pgClassTable.Oid == pgAttributeTable.Relid)
                .Join(pgDescTable, pgClassTable.Oid == pgDescTable.ObjOid & pgAttributeTable.Num == pgDescTable.ObjSubId)
                .Where(pgClassTable.Name == pTableName.ToLower())
                .Execute(pDatabase);

            for(int index = 0; index < result.Count; index++) {

                string columnName = pgAttributeTable[index, result].Name;
                string description = pgDescTable[index, result].Description;

                bool foundColumn = false;

                foreach(IColumn column in pTableDetails.Columns) {

                    if(string.Compare(column.ColumnName, columnName, true) == 0) {
                        ((Column)column).Description = description;
                        foundColumn = true;
                        break;
                    }
                }

                if(!foundColumn) {
                    throw new Exception("Cannot find column = '" + columnName + "' in list of columns. This is unexpected.");
                }
            }
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
                sTypeLookup.Add("date", System.Data.DbType.DateTime);
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
                sTypeLookup.Add("int2", System.Data.DbType.Int16);
                sTypeLookup.Add("_oid", System.Data.DbType.Object);
                sTypeLookup.Add("varchar", System.Data.DbType.String);
                sTypeLookup.Add("regproc", System.Data.DbType.Object);
                sTypeLookup.Add("bpchar", System.Data.DbType.String);
                sTypeLookup.Add("text", System.Data.DbType.String);
                sTypeLookup.Add("_regtype", System.Data.DbType.Object);
                sTypeLookup.Add("timestamp", System.Data.DbType.DateTime);
                sTypeLookup.Add("numeric", System.Data.DbType.Decimal);
                sTypeLookup.Add("oid", System.Data.DbType.Int64);
            }
            string key = pDataType.ToLower();
            return sTypeLookup.ContainsKey(key) ? sTypeLookup[key] : System.Data.DbType.Object;
        }
    }
}