
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
using System.Data;

namespace TypedQuery.Logic {

    public static class CodeGenerator {

        public static string GenerateTableAndRowCode(ITableDetails pTableDetails, string pNamespace, ref string pColumnPrefix, bool pIncludeSchema, bool pGuessPrefix, bool pGenerateComments, bool pRemoveUnderscores, bool pGenerateKeyTypes) {

            if(pTableDetails == null) {
                return "Unable to load table details";
            }

            if(string.IsNullOrEmpty(pNamespace)) {
                pNamespace = "Tables";
            }

            if(pGuessPrefix && pTableDetails.Columns.Count > 1) {

                bool stop = false;

                int charIndex = 0;

                string prefix = string.Empty;

                while(!stop) {

                    char? c = null;

                    for(int index = 0; index < pTableDetails.Columns.Count; index++) {

                        IColumn column = pTableDetails.Columns[index];

                        if(charIndex >= column.ColumnName.Length - 1) {
                            stop = true;
                            break;
                        }

                        if(index == 0) {
                            c = column.ColumnName[charIndex];
                        }
                        else if(char.ToLower(c!.Value) != char.ToLower(column.ColumnName[charIndex])) {
                            stop = true;
                            break;
                        }
                    }

                    if(!stop && c != null) {
                        prefix += c.Value;
                        charIndex++;
                    }
                    else {
                        break; //Just incase
                    }
                }

                if(prefix.Length > 1 && prefix.Length < 6) {
                    pColumnPrefix = prefix;
                }
            }

            string endl = Environment.NewLine;
            string tab = "\t";

            StringBuilder code = new StringBuilder();

            code.Append("using System;").Append(endl);

            if(pGenerateKeyTypes) {
                code.Append("using Sql.Types;").Append(endl);
            }
            code.Append("using Sql.Column;").Append(endl);

            code.Append(endl);
            code.Append("namespace ").Append(pNamespace).Append(".").Append(pTableDetails.TableName.Replace(" ", string.Empty)).Append(" {").Append(endl);
            code.Append(endl);

            if(pGenerateComments) {
                code.Append(tab).Append("[Sql.TableAttribute(\"").Append(pTableDetails.Description).Append("\")]").Append(endl);
            }

            code.Append(tab).Append("public sealed class Table : ").Append(typeof(Sql.ATable).ToString()).Append(" {").Append(endl);
            code.Append(endl);
            code.Append(tab).Append(tab).Append("public static readonly Table Instance = new Table();").Append(endl);
            code.Append(endl);

            for(int columnIndex = 0; columnIndex < pTableDetails.Columns.Count; columnIndex++) {

                IColumn column = pTableDetails.Columns[columnIndex];

                if(pGenerateComments) {

                    if(columnIndex > 0) {
                        code.Append(endl);
                    }
                    code.Append(tab).Append(tab).Append("[Sql.ColumnAttribute(\"").Append(column.Description).Append("\")]").Append(endl);
                }
                code.Append(tab).Append(tab).Append("public ").Append(GetColumnType(column, pTableDetails, pGenerateKeyTypes)).Append(" ").Append(FormatName(GetColumnName(column, pColumnPrefix, pRemoveUnderscores))).Append(" { get; private set; }").Append(endl);
            }

            code.Append(endl);
            code.Append(tab).Append(tab).Append("public Table() : base(\"").Append(pTableDetails.TableName).Append("\", \"").Append(pIncludeSchema ? pTableDetails.Schema : string.Empty).Append("\", ").Append(pTableDetails.IsView ? "true" : "false").Append(", typeof(Row)) {").Append(endl);
            code.Append(endl);

            for(int columnIndex = 0; columnIndex < pTableDetails.Columns.Count; columnIndex++) {

                IColumn column = pTableDetails.Columns[columnIndex];
                string primaryKey = column.IsPrimaryKey ? "true" : "false";
                string isAutoGenerated = column.IsAutoGenerated ? "true" : "false";

                code.Append(tab).Append(tab).Append(tab).Append(FormatName(GetColumnName(column, pColumnPrefix, pRemoveUnderscores))).Append(" = new ").Append(GetColumnType(column, pTableDetails, pGenerateKeyTypes)).Append("(this, \"").Append(column.ColumnName).Append("\", ").Append(primaryKey);

                if(column.IsAutoGenerated) {
                    code.Append(", ").Append(isAutoGenerated);
                }

                if(column.MaxLength != null && column.DbType != DbType.Binary && column.DbType != DbType.Byte) {

                    if(column.MaxLength == -1) {
                        code.Append(", int.MaxValue");
                    }
                    else {
                        code.Append(", " + column.MaxLength.Value.ToString());
                    }
                }
                code.Append(");").Append(endl);
            }

            code.Append(endl);

            code.Append(tab).Append(tab).Append(tab).Append("AddColumns(");

            for(int columnIndex = 0; columnIndex < pTableDetails.Columns.Count; columnIndex++) {

                IColumn column = pTableDetails.Columns[columnIndex];

                if(columnIndex > 0) {
                    code.Append(", ");
                }
                code.Append(FormatName(GetColumnName(column, pColumnPrefix, pRemoveUnderscores)));
            }

            code.Append(");").Append(endl);
            code.Append(tab).Append(tab).Append("}").Append(endl);
            code.Append(endl);

            code.Append(tab).Append(tab).Append("public Row GetRow(int pIndex, Sql.IResult pResult) {").Append(endl);
            code.Append(tab).Append(tab).Append(tab).Append("return (Row)pResult.GetRow(this, pIndex);").Append(endl);
            code.Append(tab).Append(tab).Append("}").Append(endl);
            code.Append(tab).Append("}").Append(endl);

            code.Append(endl);

            //
            //	Generate row code
            //
            code.Append(tab).Append("public sealed class Row : ").Append(typeof(Sql.ARow).ToString()).Append(" {").Append(endl);
            code.Append(endl);
            code.Append(tab).Append(tab).Append("private new Table Tbl {").Append(endl);
            code.Append(tab).Append(tab).Append(tab).Append("get { return (Table)base.Tbl; }").Append(endl);
            code.Append(tab).Append(tab).Append("}").Append(endl);
            code.Append(endl);
            code.Append(tab).Append(tab).Append("public Row() : base(Table.Instance) {").Append(endl);
            code.Append(tab).Append(tab).Append("}").Append(endl);
            code.Append(endl);

            for(int columnIndex = 0; columnIndex < pTableDetails.Columns.Count; columnIndex++) {

                IColumn column = pTableDetails.Columns[columnIndex];

                string columnName = FormatName(GetColumnName(column, pColumnPrefix, pRemoveUnderscores));

                if(columnIndex > 0) {
                    code.Append(endl);
                }

                code.Append(tab).Append(tab).Append("public ").Append(GetReturnType(column.DbType, column.IsNullable, column, pTableDetails, pGenerateKeyTypes)).Append(" ").Append(columnName).Append(" {").Append(endl);
                code.Append(tab).Append(tab).Append(tab).Append("get { return Tbl.").Append(columnName).Append(".ValueOf(this); }").Append(endl);

                if(!column.IsAutoGenerated && !pTableDetails.IsView) {
                    code.Append(tab).Append(tab).Append(tab).Append("set { Tbl.").Append(columnName).Append(".SetValue(this, value); }").Append(endl);
                }
                code.Append(tab).Append(tab).Append("}").Append(endl);
            }
            code.Append(tab).Append("}").Append(endl);
            code.Append("}");

            return code.ToString();
        }

        public static string GenerateClassCode(ITableDetails pTableDetails, string pNamespace, string pColumnPrefix, bool pRemoveUnderscores, bool pGenerateKeyTypes) {

            string endl = Environment.NewLine;
            string tab = "\t";

            if(string.IsNullOrEmpty(pNamespace)) {
                pNamespace = "Logic";
            }

            string className = FormatName(pTableDetails.TableName);

            StringBuilder code = new StringBuilder();

            code.Append("using System;").Append(endl);

            if(pGenerateKeyTypes) {
                code.Append("using Sql.Types;").Append(endl);
            }
            code.Append("using Sql.Column;").Append(endl);
            code.Append(endl);

            code.Append("namespace ").Append(pNamespace).Append(".").Append(pTableDetails.TableName).Append(" {").Append(endl);
            code.Append(endl);

            code.Append(tab).Append("public class ").Append("Info").Append(" {").Append(endl);
            code.Append(endl);

            for(int columnIndex = 0; columnIndex < pTableDetails.Columns.Count; columnIndex++) {

                IColumn column = pTableDetails.Columns[columnIndex];

                code.Append(tab).Append(tab).Append("public ").Append(GetReturnType(column.DbType, column.IsNullable, column, pTableDetails, pGenerateKeyTypes)).Append(" ").Append(FormatName(GetColumnName(column, pColumnPrefix, pRemoveUnderscores))).Append(" { get;");

                if(!column.IsAutoGenerated && !pTableDetails.IsView) {
                    code.Append(" private set; }");
                }
                else {
                    code.Append(" private set; }");
                }
                code.Append(endl);
            }

            code.Append(endl);

            code.Append(tab).Append(tab).Append("public ").Append("Info").Append("(Row pRow) {").Append(endl);
            code.Append(endl);

            for(int columnIndex = 0; columnIndex < pTableDetails.Columns.Count; columnIndex++) {

                IColumn column = pTableDetails.Columns[columnIndex];
                string fieldName = FormatName(GetColumnName(column, pColumnPrefix, pRemoveUnderscores));

                code.Append(tab).Append(tab).Append(tab).Append(fieldName).Append(" = pRow.").Append(fieldName).Append(";").Append(endl);
            }

            code.Append(tab).Append(tab).Append("}").Append(endl);

            if(!pTableDetails.IsView) {

                code.Append(tab).Append(tab).Append("public void CopyToRow(Row pRow) {").Append(endl);
                code.Append(endl);

                for(int columnIndex = 0; columnIndex < pTableDetails.Columns.Count; columnIndex++) {

                    IColumn column = pTableDetails.Columns[columnIndex];
                    string fieldName = FormatName(GetColumnName(column, pColumnPrefix, pRemoveUnderscores));

                    if(!column.IsAutoGenerated) {
                        code.Append(tab).Append(tab).Append(tab).Append("pRow.").Append(fieldName).Append(" = ").Append(fieldName).Append(";").Append(endl);
                    }
                }

                code.Append(tab).Append(tab).Append("}").Append(endl);

                code.Append(endl);
                code.Append(tab).Append(tab).Append("public void SetValues(");

                for(int columnIndex = 0, paramCount = 0; columnIndex < pTableDetails.Columns.Count; columnIndex++) {

                    IColumn column = pTableDetails.Columns[columnIndex];
                    string fieldName = FormatName(GetColumnName(column, pColumnPrefix, pRemoveUnderscores));

                    if(!column.IsAutoGenerated) {

                        if(paramCount > 0) {
                            code.Append(", ");
                        }
                        code.Append(GetReturnType(column.DbType, column.IsNullable, column, pTableDetails, pGenerateKeyTypes)).Append(" p").Append(fieldName);
                        paramCount++;
                    }
                }
                code.Append(") {").Append(endl);

                for(int columnIndex = 0; columnIndex < pTableDetails.Columns.Count; columnIndex++) {

                    IColumn column = pTableDetails.Columns[columnIndex];
                    string fieldName = FormatName(GetColumnName(column, pColumnPrefix, pRemoveUnderscores));

                    if(!column.IsAutoGenerated) {
                        code.Append(tab).Append(tab).Append(tab).Append(fieldName).Append(" = p").Append(fieldName).Append(";").Append(endl);
                    }
                }
                code.Append(tab).Append(tab).Append("}").Append(endl);
            }

            code.Append(tab).Append("}").Append(endl);
            code.Append("}");

            return code.ToString();
        }

        private static string GetColumnName(IColumn pColumn, string pColumnPrefix, bool pRemoveUnderscores) {

            string value;

            if(!string.IsNullOrEmpty(pColumnPrefix) && pColumn.ColumnName.ToLower().StartsWith(pColumnPrefix.ToLower())) {
                value = pColumn.ColumnName.Substring(pColumnPrefix.Length);
            }
            else {
                value = pColumn.ColumnName;
            }

            if(pRemoveUnderscores) {

                StringBuilder name = new StringBuilder();

                bool upperCaseNextChar = true;

                for(int index = 0; index < value.Length; index++) {

                    char c = value[index];

                    if(c == '_') {
                        upperCaseNextChar = true;
                    }
                    else {

                        if(upperCaseNextChar) {
                            name.Append(Char.ToUpper(c));
                            upperCaseNextChar = false;
                        }
                        else {
                            name.Append(c);
                        }
                    }
                }
                return name.ToString();
            }
            return value;
        }

        private static string FormatName(string pString) {

            StringBuilder str = new StringBuilder();

            for(int index = 0; index < pString.Length; index++) {

                char c = pString[index];

                if(index == 0 || (index > 0 && pString[index - 1] == '_')) {
                    str.Append((string.Empty + c).ToUpper());
                }
                else {
                    str.Append(c);
                }
            }
            return str.ToString();
        }

        private static string GetColumnType(IColumn pColumn, ITableDetails pTable, bool pGenerateKeyTypes) {

            string value;

            List<KeyColumn> matchingKeyColumns = new List<KeyColumn>();

            foreach(IForeignKey foreignKey in pTable.ForeignKeys) {

                foreach(KeyColumn keyColumn in foreignKey.KeyColumns) {

                    if(keyColumn.ForeignKeyColumn == pColumn) {
                        matchingKeyColumns.Add(keyColumn);
                    }
                }
            }

            if(pColumn.DbType == DbType.Boolean) {
                value = !pColumn.IsNullable ? typeof(Sql.Column.BoolColumn).Name : typeof(Sql.Column.NBoolColumn).Name;
            }
            else if(pColumn.DbType == DbType.DateTime) {
                value = !pColumn.IsNullable ? typeof(Sql.Column.DateTimeColumn).Name : typeof(Sql.Column.NDateTimeColumn).Name;
            }
            else if(pColumn.DbType == DbType.DateTime2) {
                value = !pColumn.IsNullable ? typeof(Sql.Column.DateTime2Column).Name : typeof(Sql.Column.NDateTime2Column).Name;
            }
            else if(pColumn.DbType == DbType.DateTimeOffset) {
                value = !pColumn.IsNullable ? typeof(Sql.Column.DateTimeOffsetColumn).Name : typeof(Sql.Column.NDateTimeOffsetColumn).Name;
            }
            else if(pColumn.DbType == DbType.Decimal) {
                value = !pColumn.IsNullable ? typeof(Sql.Column.DecimalColumn).Name : typeof(Sql.Column.NDecimalColumn).Name;
            }
            else if(pColumn.DbType == DbType.Guid) {

                if(pGenerateKeyTypes && matchingKeyColumns.Count > 0) {

                    if(matchingKeyColumns.Count == 1) {
                        value = (!pColumn.IsNullable ? "GuidKeyColumn" : "NGuidKeyColumn") + "<" + matchingKeyColumns[0].PrimaryKeyTableName + ".Table>";
                    }
                    else {
                        value = (!pColumn.IsNullable ? "GuidKeyColumn" : "NGuidKeyColumn") + "<" + matchingKeyColumns[0].PrimaryKeyTableName + "<??? Column Belongs to multipule foreign keys ???>.Table>";
                    }
                }
                else if(pGenerateKeyTypes && pColumn.IsPrimaryKey) {
                    value = (!pColumn.IsNullable ? "GuidKeyColumn" : "NGuidKeyColumn") + "<" + pTable.TableName + ".Table>";
                }
                else {
                    value = !pColumn.IsNullable ? typeof(Sql.Column.GuidColumn).Name : typeof(Sql.Column.NGuidColumn).Name;
                }
            }
            else if(pColumn.DbType == DbType.Int16) {

                if(pGenerateKeyTypes && matchingKeyColumns.Count > 0) {

                    if(matchingKeyColumns.Count == 1) {
                        value = (!pColumn.IsNullable ? "SmallIntegerKeyColumn" : "NSmallIntegerKeyColumn") + "<" + matchingKeyColumns[0].PrimaryKeyTableName + ".Table>";
                    }
                    else {
                        value = (!pColumn.IsNullable ? "SmallIntegerKeyColumn" : "NSmallIntegerKeyColumn") + "<" + matchingKeyColumns[0].PrimaryKeyTableName + "<??? Column Belongs to multipule foreign keys ???>.Table>";
                    }
                }
                else if(pGenerateKeyTypes && pColumn.IsPrimaryKey) {
                    value = (!pColumn.IsNullable ? "SmallIntegerKeyColumn" : "NSmallIntegerKeyColumn") + "<" + pTable.TableName + ".Table>";
                }
                else {
                    value = !pColumn.IsNullable ? typeof(Sql.Column.SmallIntegerColumn).Name : typeof(Sql.Column.NSmallIntegerColumn).Name;
                }
            }
            else if(pColumn.DbType == DbType.Int32) {

                if(pGenerateKeyTypes && matchingKeyColumns.Count > 0) {

                    if(matchingKeyColumns.Count == 1) {
                        value = (!pColumn.IsNullable ? "IntegerKeyColumn" : "NIntegerKeyColumn") + "<" + matchingKeyColumns[0].PrimaryKeyTableName + ".Table>";
                    }
                    else {
                        value = (!pColumn.IsNullable ? "IntegerKeyColumn" : "NIntegerKeyColumn") + "<" + matchingKeyColumns[0].PrimaryKeyTableName + "<??? Column Belongs to multipule foreign keys ???>.Table>";
                    }
                }
                else if(pGenerateKeyTypes && pColumn.IsPrimaryKey) {
                    value = (!pColumn.IsNullable ? "IntegerKeyColumn" : "NIntegerKeyColumn") + "<" + pTable.TableName + ".Table>";
                }
                else {
                    value = !pColumn.IsNullable ? typeof(Sql.Column.IntegerColumn).Name : typeof(Sql.Column.NIntegerColumn).Name;
                }
            }
            else if(pColumn.DbType == DbType.Int64) {

                if(pGenerateKeyTypes && matchingKeyColumns.Count > 0) {

                    if(matchingKeyColumns.Count == 1) {
                        value = (!pColumn.IsNullable ? "BigIntegerKeyColumn" : "NBigIntegerKeyColumn") + "<" + matchingKeyColumns[0].PrimaryKeyTableName + ".Table>";
                    }
                    else {
                        value = (!pColumn.IsNullable ? "BigIntegerKeyColumn" : "NBigIntegerKeyColumn") + "<" + matchingKeyColumns[0].PrimaryKeyTableName + "<??? Column Belongs to multipule foreign keys ???>.Table>";
                    }
                }
                else if(pGenerateKeyTypes && pColumn.IsPrimaryKey) {
                    value = (!pColumn.IsNullable ? "BigIntegerKeyColumn" : "NBigIntegerKeyColumn") + "<" + pTable.TableName + ".Table>";
                }
                else {
                    value = !pColumn.IsNullable ? typeof(Sql.Column.BigIntegerColumn).Name : typeof(Sql.Column.NBigIntegerColumn).Name;
                }
            }
            else if(pColumn.DbType == DbType.String) {

                if(pGenerateKeyTypes && matchingKeyColumns.Count > 0) {

                    if(matchingKeyColumns.Count == 1) {
                        value = (!pColumn.IsNullable ? "StringKeyColumn" : "NStringKeyColumn") + "<" + matchingKeyColumns[0].PrimaryKeyTableName + ".Table>";
                    }
                    else {
                        value = (!pColumn.IsNullable ? "StringKeyColumn" : "NStringKeyColumn") + "<" + matchingKeyColumns[0].PrimaryKeyTableName + "<??? Column Belongs to multipule foreign keys ???>.Table>";
                    }
                }
                else if(pGenerateKeyTypes && pColumn.IsPrimaryKey) {
                    value = (!pColumn.IsNullable ? "StringKeyColumn" : "NStringKeyColumn") + "<" + pTable.TableName + ".Table>";
                }
                else {
                    value = typeof(Sql.Column.StringColumn).Name;
                }
            }
            else if(pColumn.DbType == DbType.Binary) {
                value = !pColumn.IsNullable ? typeof(Sql.Column.BinaryColumn).Name : typeof(Sql.Column.NBinaryColumn).Name;
            }
            else if(pColumn.DbType == DbType.Byte) {
                value = !pColumn.IsNullable ? typeof(Sql.Column.ByteColumn).Name : typeof(Sql.Column.NByteColumn).Name;
            }
            else if(pColumn.DbType == DbType.Single) {
                value = !pColumn.IsNullable ? typeof(Sql.Column.FloatColumn).Name : typeof(Sql.Column.NFloatColumn).Name;
            }
            else if(pColumn.DbType == DbType.Double) {
                value = !pColumn.IsNullable ? typeof(Sql.Column.DoubleColumn).Name : typeof(Sql.Column.NDoubleColumn).Name;
            }
            else {
                value = "UNKNOWN_COLUMN_TYPE";
            }
            return value;
        }

        public static string GenerateStoredProcedureCode(Logic.IStoredProcedureDetail pProc, string pColumnPrefix, bool pIncludeSchema) {

            string endl = Environment.NewLine;
            string tab = "\t";

            StringBuilder code = new StringBuilder();

            code.Append("using System;").Append(endl);
            code.Append("using System.Data;").Append(endl);
            code.Append("using System.Data.SqlClient;").Append(endl).Append(endl);

            code.Append("namespace Tables.").Append(pProc.Name).Append(" {").Append(endl);
            code.Append(endl);
            code.Append(tab).Append("public sealed class Proc : ").Append(typeof(Sql.AStoredProc).ToString()).Append(" {").Append(endl);
            code.Append(endl);
            code.Append(tab).Append(tab).Append("public static readonly Proc Instance = new Proc();").Append(endl);

            code.Append(endl);
            code.Append(tab).Append(tab).Append("public Proc() : base(DATABASE, \"").Append(pIncludeSchema && !string.IsNullOrEmpty(pProc.Schema) ? pProc.Schema + "." : string.Empty).Append(pProc.Name).Append("\", typeof(Row)) {").Append(endl);
            code.Append(endl);

            code.Append(tab).Append(tab).Append(tab).Append("//AddColumns(");
            code.Append(");").Append(endl);
            code.Append(tab).Append(tab).Append("}").Append(endl);
            code.Append(endl);

            code.Append(tab).Append(tab).Append("public Sql.IResult Execute(");

            for(int index = 0; index < pProc.Parameters.Count; index++) {

                Logic.ISpParameter param = pProc.Parameters[index];

                if(index > 0) {
                    code.Append(", ");
                }

                if(param.Direction == ParameterDirection.Output || param.Direction == ParameterDirection.ReturnValue) {
                    code.Append("out ");
                }
                code.Append(GetReturnType(param.ParamType, false, null, null, false)).Append(" ").Append(param.Name);
            }

            if(pProc.Parameters.Count > 0) {
                code.Append(", ");
            }

            code.Append("Sql.Transaction pTransaction) {").Append(endl).Append(endl);

            for(int index = 0; index < pProc.Parameters.Count; index++) {

                Logic.ISpParameter param = pProc.Parameters[index];

                code.Append(tab).Append(tab).Append(tab).Append("SqlParameter p").Append(index.ToString()).Append(" = new SqlParameter(\"").Append(param.Name).Append("\", SqlDbType.").Append(ConvertToSqlDbType(param.ParamType).ToString()).Append(");").Append(endl);

                code.Append(tab).Append(tab).Append(tab).Append("p").Append(index.ToString()).Append(".Direction = ParameterDirection.").Append(param.Direction.ToString()).Append(";").Append(endl);

                if(param.Direction == ParameterDirection.Input || param.Direction == ParameterDirection.InputOutput) {
                    code.Append(tab).Append(tab).Append(tab).Append("p").Append(index.ToString()).Append(".Value = ").Append(param.Name).Append(";").Append(endl);
                }
                code.Append(endl);
            }

            code.Append(tab).Append(tab).Append(tab).Append("Sql.IResult result = ExecuteProcedure(pTransaction");

            for(int index = 0; index < pProc.Parameters.Count; index++) {
                code.Append(", p").Append(index.ToString());
            }

            code.Append(");").Append(endl).Append(endl);

            for(int index = 0; index < pProc.Parameters.Count; index++) {

                Logic.ISpParameter param = pProc.Parameters[index];

                if(param.Direction == ParameterDirection.InputOutput || param.Direction == ParameterDirection.Output || param.Direction == ParameterDirection.ReturnValue) {
                    code.Append(tab).Append(tab).Append(tab).Append(param.Name).Append(" = (").Append(GetReturnType(param.ParamType, false, null, null, false)).Append(")").Append("p").Append(index.ToString()).Append(".Value;").Append(endl);
                }
            }

            code.Append(tab).Append(tab).Append(tab).Append("return result;").Append(endl);
            code.Append(tab).Append(tab).Append("}").Append(endl).Append(endl);

            code.Append(tab).Append(tab).Append("public Row this[int pIndex, Sql.IResult pResult] {").Append(endl);
            code.Append(tab).Append(tab).Append(tab).Append("get { return (Row)pResult.GetRow(this, pIndex); }").Append(endl);
            code.Append(tab).Append(tab).Append("}").Append(endl);
            code.Append(tab).Append("}").Append(endl);

            code.Append(endl);

            //
            //	Generate row code
            //
            code.Append(tab).Append("public sealed class Row : ").Append(typeof(Sql.ARow).ToString()).Append(" {").Append(endl);
            code.Append(endl);
            code.Append(tab).Append(tab).Append("private new Proc Tbl {").Append(endl);
            code.Append(tab).Append(tab).Append(tab).Append("get { return (Proc)base.Tbl; }").Append(endl);
            code.Append(tab).Append(tab).Append("}").Append(endl);
            code.Append(endl);
            code.Append(tab).Append(tab).Append("public Row() : base(Proc.Instance) {").Append(endl);
            code.Append(tab).Append(tab).Append("}").Append(endl);

            code.Append(tab).Append("}").Append(endl);
            code.Append("}");

            return code.ToString();
        }

        private static SqlDbType ConvertToSqlDbType(DbType pDbType) {

            if(pDbType == DbType.Int16) {
                return SqlDbType.SmallInt;
            }
            if(pDbType == DbType.Int32) {
                return SqlDbType.Int;
            }
            if(pDbType == DbType.Int64) {
                return SqlDbType.BigInt;
            }
            if(pDbType == DbType.String) {
                return SqlDbType.VarChar;
            }
            if(pDbType == DbType.Decimal) {
                return SqlDbType.Decimal;
            }
            if(pDbType == DbType.DateTime) {
                return SqlDbType.DateTime;
            }
            if(pDbType == DbType.DateTime2) {
                return SqlDbType.DateTime2;
            }
            if(pDbType == DbType.DateTimeOffset) {
                return SqlDbType.DateTimeOffset;
            }
            if(pDbType == DbType.Byte) {
                return SqlDbType.TinyInt;
            }
            if(pDbType == DbType.Double) {
                return SqlDbType.Float;
            }
            if(pDbType == DbType.Single) {
                return SqlDbType.Real;
            }
            return 0;
        }

        public static string GetReturnType(DbType pDbType, bool pIsNullable, IColumn? pColumn, ITableDetails? pTable, bool pGenerateKeyTypes) {

            string value;

            List<KeyColumn> matchingKeyColumns = new List<KeyColumn>();

            if(pColumn != null && pTable != null) {

                foreach(IForeignKey foreignKey in pTable.ForeignKeys) {

                    foreach(KeyColumn keyColumn in foreignKey.KeyColumns) {

                        if(keyColumn.ForeignKeyColumn == pColumn) {
                            matchingKeyColumns.Add(keyColumn);
                        }
                    }
                }
            }

            if(pDbType == DbType.Boolean) {
                value = !pIsNullable ? "bool" : "bool?";
            }
            else if(pDbType == DbType.DateTime) {
                value = !pIsNullable ? "DateTime" : "DateTime?";
            }
            else if(pDbType == DbType.DateTime2) {
                value = !pIsNullable ? "DateTime" : "DateTime?";
            }
            else if(pDbType == DbType.DateTimeOffset) {
                value = !pIsNullable ? "DateTimeOffset" : "DateTimeOffset?";
            }
            else if(pDbType == DbType.Decimal) {
                value = !pIsNullable ? "decimal" : "decimal?";
            }
            else if(pDbType == DbType.Guid) {

                if(pGenerateKeyTypes && pColumn != null && pColumn.IsPrimaryKey) {
                    value = $"GuidKey<{ (pTable != null ? pTable.TableName : "???")}.Table>" + (pIsNullable ? "?" : string.Empty);
                }
                else if(pGenerateKeyTypes && matchingKeyColumns.Count > 0) {

                    if(matchingKeyColumns.Count == 1) {
                        value = $"GuidKey<{matchingKeyColumns[0].PrimaryKeyTableName}.Table>" + (pIsNullable ? "?" : string.Empty);
                    }
                    else {
                        value = $"GuidKey<{matchingKeyColumns[0].PrimaryKeyTableName}<??? Column Belongs to multipule foreign keys ???>.Table>" + (pIsNullable ? "?" : string.Empty);
                    }
                }
                else {
                    value = !pIsNullable ? "Guid" : "Guid?";
                }
            }
            else if(pDbType == DbType.Int16) {

                if(pGenerateKeyTypes && pColumn != null && pColumn.IsPrimaryKey) {
                    value = $"Int16Key<{(pTable != null ? pTable.TableName : "???")}.Table>" + (pIsNullable ? "?" : string.Empty);
                }
                else if(pGenerateKeyTypes && matchingKeyColumns.Count > 0) {

                    if(matchingKeyColumns.Count == 1) {
                        value = $"Int16Key<{matchingKeyColumns[0].PrimaryKeyTableName}.Table>" + (pIsNullable ? "?" : string.Empty);
                    }
                    else {
                        value = $"Int16Key<{matchingKeyColumns[0].PrimaryKeyTableName}.Table><??? Column Belongs to multipule foreign keys ???>" + (pIsNullable ? "?" : string.Empty);
                    }
                }
                else {
                    value = !pIsNullable ? "short" : "short?";
                }
            }
            else if(pDbType == DbType.Int32) {

                if(pGenerateKeyTypes && pColumn != null && pColumn.IsPrimaryKey) {
                    value = $"Int32Key<{(pTable != null ? pTable.TableName : "???")}.Table>" + (pIsNullable ? "?" : string.Empty);
                }
                else if(pGenerateKeyTypes && matchingKeyColumns.Count > 0) {

                    if(matchingKeyColumns.Count == 1) {
                        value = $"Int32Key<{matchingKeyColumns[0].PrimaryKeyTableName}.Table>" + (pIsNullable ? "?" : string.Empty);
                    }
                    else {
                        value = $"Int32Key<{matchingKeyColumns[0].PrimaryKeyTableName}.Table><??? Column Belongs to multipule foreign keys ???>" + (pIsNullable ? "?" : string.Empty);
                    }
                }
                else {
                    value = !pIsNullable ? "int" : "int?";
                }
            }
            else if(pDbType == DbType.Int64) {

                if(pGenerateKeyTypes && pColumn != null && pColumn.IsPrimaryKey) {
                    value = $"Int64Key<{(pTable != null ? pTable.TableName : "???")}.Table>" + (pIsNullable ? "?" : string.Empty);
                }
                else if(pGenerateKeyTypes && matchingKeyColumns.Count > 0) {

                    if(matchingKeyColumns.Count == 1) {
                        value = $"Int64Key<{matchingKeyColumns[0].PrimaryKeyTableName}.Table>" + (pIsNullable ? "?" : string.Empty);
                    }
                    else {
                        value = $"Int64Key<{matchingKeyColumns[0].PrimaryKeyTableName}.Table><??? Column Belongs to multipule foreign keys ???>" + (pIsNullable ? "?" : string.Empty);
                    }
                }
                else {
                    value = !pIsNullable ? "long" : "long?";
                }
            }
            else if(pDbType == DbType.String) {

                if(pGenerateKeyTypes && pColumn != null && pColumn.IsPrimaryKey) {
                    value = $"StringKey<{(pTable != null ? pTable.TableName : "???")}.Table>" + (pIsNullable ? "?" : string.Empty);
                }
                else if(pGenerateKeyTypes && matchingKeyColumns.Count > 0) {

                    if(matchingKeyColumns.Count == 1) {
                        value = $"StringKey<{matchingKeyColumns[0].PrimaryKeyTableName}.Table>" + (pIsNullable ? "?" : string.Empty);
                    }
                    else {
                        value = $"StringKey<{matchingKeyColumns[0].PrimaryKeyTableName}.Table><??? Column Belongs to multipule foreign keys ???>" + (pIsNullable ? "?" : string.Empty);
                    }
                }
                else {
                    value = "string";
                }
            }
            else if(pDbType == DbType.Binary) {
                value = !pIsNullable ? "byte[]" : "byte[]";
            }
            else if(pDbType == DbType.Byte) {
                value = !pIsNullable ? "byte" : "byte?";
            }
            else if(pDbType == DbType.Single) {
                value = !pIsNullable ? "float" : "float?";
            }
            else if(pDbType == DbType.Double) {
                value = !pIsNullable ? "double" : "double?";
            }
            else {
                value = "UNKNOWN_COLUMN_TYPE";
            }
            return value;
        }
    }
}