
/*
 * 
 * Copyright (C) 2009-2016 JFo.nz
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

namespace Sql.Database {
	
	public static class GenerateCode {
		
		private static string GetColumnName(DbColumn pColumn, string pColumnPrefix){
			
			string value;
			
			if(!string.IsNullOrEmpty(pColumnPrefix) && pColumn.Name.StartsWith(pColumnPrefix))
				value = pColumn.Name.Substring(pColumnPrefix.Length);
			else
				value = pColumn.Name;

			return value;
		}

		private static string GetColumnType(DbColumn pColumn, DbTable pTable) {

			string value;

			if (pColumn.DbType == System.Data.DbType.Boolean) {
				value = !pColumn.Nullable ? typeof(Column.BoolColumn).ToString() : typeof(Column.NBoolColumn).ToString();
			}
			else if (pColumn.DbType == System.Data.DbType.DateTime) {
				value = !pColumn.Nullable ? typeof(Column.DateTimeColumn).ToString() : typeof(Column.NDateTimeColumn).ToString();
			}
			else if(pColumn.DbType == System.Data.DbType.DateTime2) {
				value = !pColumn.Nullable ? typeof(Column.DateTime2Column).ToString() : typeof(Column.NDateTime2Column).ToString();
			}
			else if(pColumn.DbType == System.Data.DbType.DateTimeOffset) {
				value = !pColumn.Nullable ? typeof(Column.DateTimeOffsetColumn).ToString() : typeof(Column.NDateTimeOffsetColumn).ToString();
			}
			else if (pColumn.DbType == System.Data.DbType.Decimal) {
				value = !pColumn.Nullable ? typeof(Column.DecimalColumn).ToString() : typeof(Column.NDecimalColumn).ToString();
			}
			else if (pColumn.DbType == System.Data.DbType.Guid) {
				
				if(!string.IsNullOrWhiteSpace(pColumn.ForeignKeyToTable))
					value = (!pColumn.Nullable ? "Sql.Column.GuidKeyColumn" : "Sql.Column.NGuidKeyColumn") + "<" + pColumn.ForeignKeyToTable + ".Table>";
				else if(pColumn.IsPrimaryKey)
					value = (!pColumn.Nullable ? "Sql.Column.GuidKeyColumn" : "Sql.Column.NGuidKeyColumn") + "<" + pTable.Name + ".Table>";
				else
					value = !pColumn.Nullable ? typeof(Column.GuidColumn).ToString() : typeof(Column.NGuidColumn).ToString();
			}
			else if(pColumn.DbType == System.Data.DbType.Int16) {
				
				if(!string.IsNullOrWhiteSpace(pColumn.ForeignKeyToTable))
					value = (!pColumn.Nullable ? "Sql.Column.SmallIntegerKeyColumn" : "Sql.Column.NSmallIntegerKeyColumn") + "<" + pColumn.ForeignKeyToTable + ".Table>";
				else if(pColumn.IsPrimaryKey)
					value = (!pColumn.Nullable ? "Sql.Column.SmallIntegerKeyColumn" : "Sql.Column.NSmallIntegerKeyColumn") + "<" + pTable.Name + ".Table>";
				else
					value = !pColumn.Nullable ? typeof(Column.SmallIntegerColumn).ToString() : typeof(Column.NSmallIntegerColumn).ToString();
			}
			else if (pColumn.DbType == System.Data.DbType.Int32) {
				
				if(!string.IsNullOrWhiteSpace(pColumn.ForeignKeyToTable))
					value = (!pColumn.Nullable ? "Sql.Column.IntegerKeyColumn" : "Sql.Column.NIntegerKeyColumn") + "<" + pColumn.ForeignKeyToTable + ".Table>";
				else if(pColumn.IsPrimaryKey)
					value = (!pColumn.Nullable ? "Sql.Column.IntegerKeyColumn" : "Sql.Column.NIntegerKeyColumn") + "<" + pTable.Name + ".Table>";
				else
					value = !pColumn.Nullable ? typeof(Column.IntegerColumn).ToString() : typeof(Column.NIntegerColumn).ToString();
			}
			else if(pColumn.DbType == System.Data.DbType.Int64) {
				
				if(!string.IsNullOrWhiteSpace(pColumn.ForeignKeyToTable))
					value = (!pColumn.Nullable ? "Sql.Column.BigIntegerKeyColumn" : "Sql.Column.NBigIntegerKeyColumn") + "<" + pColumn.ForeignKeyToTable + ".Table>";
				else if(pColumn.IsPrimaryKey)
					value = (!pColumn.Nullable ? "Sql.Column.BigIntegerKeyColumn" : "Sql.Column.NBigIntegerKeyColumn") + "<" + pTable.Name + ".Table>";
				else
					value = !pColumn.Nullable ? typeof(Column.BigIntegerColumn).ToString() : typeof(Column.NBigIntegerColumn).ToString();
			}
			else if (pColumn.DbType == System.Data.DbType.String) {
				value = typeof(Column.StringColumn).ToString();
			}
			else if(pColumn.DbType == System.Data.DbType.Binary){
				value = !pColumn.Nullable ? typeof(Column.BinaryColumn).ToString() : typeof(Column.NBinaryColumn).ToString();
			}
			else if(pColumn.DbType == System.Data.DbType.Byte){
				value = !pColumn.Nullable ? typeof(Column.ByteColumn).ToString() : typeof(Column.NByteColumn).ToString();
			}
			else if(pColumn.DbType == System.Data.DbType.Single) {
				value = !pColumn.Nullable ? typeof(Column.FloatColumn).ToString() : typeof(Column.NFloatColumn).ToString();
			}
			else if(pColumn.DbType == System.Data.DbType.Double) {
				value = !pColumn.Nullable ? typeof(Column.DoubleColumn).ToString() : typeof(Column.NDoubleColumn).ToString();
			}
			else
				value = "UNKNOWN_COLUMN_TYPE";

			return value;
		}

		public static string GetReturnType(System.Data.DbType pDbType, bool pIsNullable) {

			string value;

			if (pDbType == System.Data.DbType.Boolean) {
				value = !pIsNullable ? "bool" : "bool?";
			}
			else if (pDbType == System.Data.DbType.DateTime) {
				value = !pIsNullable ? "DateTime" : "DateTime?";
			}
			else if (pDbType == System.Data.DbType.DateTime2) {
				value = !pIsNullable ? "DateTime" : "DateTime?";
			}
			else if (pDbType == System.Data.DbType.DateTimeOffset) {
				value = !pIsNullable ? "DateTimeOffset" : "DateTimeOffset?";
			}
			else if (pDbType == System.Data.DbType.Decimal) {
				value = !pIsNullable ? "decimal" : "decimal?";
			}
			else if (pDbType == System.Data.DbType.Guid) {
				value = !pIsNullable ? "Guid" : "Guid?";
			}
			else if(pDbType == System.Data.DbType.Int16) {
				value = !pIsNullable ? "short" : "short?";
			}
			else if (pDbType == System.Data.DbType.Int32) {
				value = !pIsNullable ? "int" : "int?";
			}
			else if(pDbType == System.Data.DbType.Int64) {
				value = !pIsNullable ? "long" : "long?";
			}
			else if (pDbType == System.Data.DbType.String) {
				value = "string";
			}
			else if(pDbType == System.Data.DbType.Binary) {
				value = !pIsNullable ? "byte[]" : "byte[]";
			}
			else if(pDbType == System.Data.DbType.Byte) {
				value = !pIsNullable ? "byte" : "byte?";
			}
			else if(pDbType == System.Data.DbType.Single) {
				value = !pIsNullable ? "float" : "float?";
			}
			else if(pDbType == System.Data.DbType.Double) {
				value = !pIsNullable ? "double" : "double?";
			}
			else
				value = "UNKNOWN_COLUMN_TYPE";

			return value;
		}

		private static string FirstLetterUpperCase(string pString) {

			if (pString.Length == 1)
				return pString.ToUpper();
			else if (pString.Length > 1)
				return pString.Substring(0, 1).ToUpper() + pString.Substring(1, pString.Length - 1);
			else
				return pString;
		}

		public static string GenerateTableAndRowCode(DbTable pTable, ref string pColumnPrefix, bool pIncludeSchema, bool pGuessPrefix) {

			if(pGuessPrefix && pTable.Columns.Count > 1){
				
				bool stop = false;
				
				int charIndex = 0;
				
				string prefix = string.Empty;
				
				while(!stop){
					
					char? c = null;
					
					for (int index = 0; index < pTable.Columns.Count; index++) {
						
						DbColumn column = pTable.Columns[index];
						
						if(charIndex >= column.Name.Length) {
							stop = true;
							break;
						}
						if(index == 0) {
							c = column.Name[charIndex];
						}
						else if(c != column.Name[charIndex]){
							stop = true;
							break;
						}
					}
					
					if(!stop && c != null){
						prefix += c.Value;
						charIndex++;
					}
					else
						break; //Just incase
				}
				
				if(prefix.Length > 2 && prefix.Length < 6)
					pColumnPrefix = prefix;
			}
			
			string endl = Environment.NewLine;
			string tab = "\t";

			StringBuilder code = new StringBuilder();
			
			code.Append("using System;").Append(endl);
			code.Append("using System.Collections.Generic;").Append(endl).Append(endl);
			
			code.Append("namespace Tables.").Append(pTable.Name).Append(" {").Append(endl);
			code.Append(endl);
			code.Append(tab).Append("public sealed class Table : ").Append(typeof(Sql.ATable).ToString()).Append(" {").Append(endl);
			code.Append(endl);
			code.Append(tab).Append(tab).Append("public static readonly Table Instance = new Table();").Append(endl);
			code.Append(tab).Append(tab).Append("//public static readonly Table Instance2 = new Table();").Append(endl);
			code.Append(tab).Append(tab).Append("//public static readonly Table Instance3 = new Table();").Append(endl);
			code.Append(endl);

			for (int columnIndex = 0; columnIndex < pTable.Columns.Count; columnIndex++) {
				DbColumn column = pTable.Columns[columnIndex];
				code.Append(tab).Append(tab).Append("public ").Append(GetColumnType(column, pTable)).Append(" ").Append(FirstLetterUpperCase(GetColumnName(column, pColumnPrefix))).Append(" { get; private set; }").Append(endl);
			}

			code.Append(endl);
			code.Append(tab).Append(tab).Append("public Table() : base(DATABASE, \"").Append(pIncludeSchema && !string.IsNullOrEmpty(pTable.SchemaName) ? pTable.SchemaName + "." : string.Empty).Append(pTable.Name).Append("\", typeof(Row)) {").Append(endl);
			code.Append(endl);
			
			for (int columnIndex = 0; columnIndex < pTable.Columns.Count; columnIndex++) {
				DbColumn column = pTable.Columns[columnIndex];
				string primaryKey = column.IsPrimaryKey ? "true" : "false";
				string isAutoGenerated = column.IsAutoGenerated ? "true" : "false";
				
				code.Append(tab).Append(tab).Append(tab).Append(FirstLetterUpperCase(GetColumnName(column, pColumnPrefix))).Append(" = new ").Append(GetColumnType(column, pTable)).Append("(this, \"").Append(column.Name).Append("\", ").Append(primaryKey);
				if(column.IsAutoGenerated)
					code.Append(", ").Append(isAutoGenerated);
				code.Append(");").Append(endl);
			}

			code.Append(endl);

			code.Append(tab).Append(tab).Append(tab).Append("AddColumns(");
			
			for (int columnIndex = 0; columnIndex < pTable.Columns.Count; columnIndex++) {
				DbColumn column = pTable.Columns[columnIndex];
				if (columnIndex > 0)
					code.Append(",");
				code.Append(FirstLetterUpperCase(GetColumnName(column, pColumnPrefix)));
			}

			code.Append(");").Append(endl);
			code.Append(tab).Append(tab).Append("}").Append(endl);
			code.Append(endl);

			code.Append(tab).Append(tab).Append("public Row this[int pIndex, Sql.IResult pResult]{").Append(endl);
			code.Append(tab).Append(tab).Append(tab).Append("get { return (Row)pResult.GetRow(this, pIndex); }").Append(endl);
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

			for (int columnIndex = 0; columnIndex < pTable.Columns.Count; columnIndex++) {
				DbColumn column = pTable.Columns[columnIndex];

				string columnName = FirstLetterUpperCase(GetColumnName(column, pColumnPrefix));
				
				if(columnIndex > 0)
					code.Append(endl);

				code.Append(tab).Append(tab).Append("public ").Append(GetReturnType(column.DbType, column.Nullable)).Append(" ").Append(columnName).Append(" {").Append(endl);
				code.Append(tab).Append(tab).Append(tab).Append("get { return Tbl.").Append(columnName).Append(".ValueOf(this); }").Append(endl);

				if(!column.IsAutoGenerated && !column.IsView)
					code.Append(tab).Append(tab).Append(tab).Append("set { Tbl.").Append(columnName).Append(".SetValue(this, value); }").Append(endl);

				code.Append(tab).Append(tab).Append("}").Append(endl);
			}
			code.Append(tab).Append("}").Append(endl);
			code.Append("}");

			return code.ToString();
		}
		
		public static string GenerateClassCode(DbTable pTable, string pColumnPrefix) {

			string endl = Environment.NewLine;
			string tab = "\t";

			StringBuilder code = new StringBuilder();
			
			code.Append("using System;").Append(endl);
			code.Append("using System.Collections.Generic;").Append(endl).Append(endl);
			
			code.Append("namespace Tables.").Append(pTable.Name).Append(" {").Append(endl);
			code.Append(endl);
			code.Append(tab).Append("public class Info {").Append(endl);
			code.Append(endl);
			
			code.Append(tab).Append(tab).Append("private readonly Row mRow;").Append(endl).Append(endl);

			for (int columnIndex = 0; columnIndex < pTable.Columns.Count; columnIndex++) {
				DbColumn column = pTable.Columns[columnIndex];

				string columnName = FirstLetterUpperCase(GetColumnName(column, pColumnPrefix));
				
				if(columnIndex > 0)
					code.Append(endl);

				bool includeSet = !column.IsAutoGenerated && !column.IsView;
				
				code.Append(tab).Append(tab).Append("public ").Append(GetReturnType(column.DbType, column.Nullable)).Append(" ").Append(columnName).Append(" { get;") .Append(includeSet ? " set; }" : " }");
			}
			
			code.Append(endl);
			code.Append(endl);			
			code.Append(tab).Append(tab).Append("public Info(Row pRow) {").Append(endl).Append(endl);
			
			code.Append(tab).Append(tab).Append(tab).Append("mRow = pRow;").Append(endl).Append(endl);
			
			for (int columnIndex = 0; columnIndex < pTable.Columns.Count; columnIndex++) {
				DbColumn column = pTable.Columns[columnIndex];

				string columnName = FirstLetterUpperCase(GetColumnName(column, pColumnPrefix));
				
				code.Append(tab).Append(tab).Append(tab).Append(columnName).Append(" = pRow.").Append(columnName).Append(";").Append(endl);
			}
			
			code.Append(tab).Append(tab).Append("}").Append(endl);
			
			code.Append(endl);
			
			code.Append(tab).Append(tab).Append("public void Save(Sql.Transaction pTransaction) {").Append(endl).Append(endl);
			
			for (int columnIndex = 0; columnIndex < pTable.Columns.Count; columnIndex++) {
				
				DbColumn column = pTable.Columns[columnIndex];

				string columnName = FirstLetterUpperCase(GetColumnName(column, pColumnPrefix));
				
				if(!column.IsAutoGenerated && !column.IsView)
					code.Append(tab).Append(tab).Append(tab).Append("mRow.").Append(columnName).Append(" = ").Append(columnName).Append(";").Append(endl);
			}			
			
			code.Append(tab).Append(tab).Append(tab).Append("mRow.Update(pTransaction);").Append(endl);
			code.Append(tab).Append(tab).Append("}").Append(endl);
			code.Append(tab).Append("}").Append(endl);
			code.Append("}");
			
			return code.ToString();
		}
		
		public static string CreateDatabaseClass(string pDatabaseName, string pConnectionString, DatabaseType pDatabaseType) {
			
			if(string.IsNullOrEmpty(pDatabaseName))
				throw new Exception("pDatabaseName cannot be null or empty");
			
			string endl = Environment.NewLine;
			
			StringBuilder code = new StringBuilder();
			
			code.Append("using System;").Append(endl);
			
			if(pDatabaseType == DatabaseType.Mssql)
				code.Append("using System.Data.SqlClient;").Append(endl);
			
			code.Append(endl);
			
			if(pDatabaseType == DatabaseType.PostgreSql) {
				code.Append("using Npgsql;").Append(endl);
				code.Append(endl);
			}
			
			code.Append("namespace Sql {").Append(endl);
			code.Append(endl);		
			code.Append("	public class MyDatabase : Sql.ADatabase {").Append(endl);
			code.Append(endl);
			code.Append("		public readonly static Sql.ADatabase Instance = new MyDatabase();").Append(endl);
			code.Append(endl);			
			code.Append("		private MyDatabase() : base(\"").Append(pDatabaseName).Append("\", Sql.DatabaseType.").Append(pDatabaseType.ToString()).Append(") {").Append(endl);
			code.Append("		}").Append(endl);
			code.Append(endl);
			code.Append("		protected override string ConnectionString {").Append(endl);
			code.Append("			get {").Append(endl);			
			code.Append("				return \"").Append(pConnectionString).Append("\";").Append(endl);					
			code.Append("			}").Append(endl);
			code.Append("		}").Append(endl);
			code.Append(endl);
			code.Append("		public override System.Data.Common.DbConnection GetConnection(bool pCanBeReadonly) {").Append(endl);
			code.Append("			lock(this) {").Append(endl);
			
			if(pDatabaseType == DatabaseType.Mssql) {
				code.Append("				SqlConnection connection = new SqlConnection(ConnectionString);").Append(endl);
				code.Append("				connection.Open();").Append(endl);
				code.Append("				return connection;").Append(endl);
			}
			else if(pDatabaseType == DatabaseType.PostgreSql) {				
				code.Append("				Npgsql.NpgsqlConnection connection = new Npgsql.NpgsqlConnection(ConnectionString);").Append(endl);
				code.Append("				connection.Open();").Append(endl);
				code.Append("				return connection;").Append(endl);
			}
			else
				code.Append("				//TODO: Return new connection").Append(endl);
			
			code.Append("			}").Append(endl);
			code.Append("		}").Append(endl);
			code.Append("	}").Append(endl);
			code.Append("}");
			
			return code.ToString();
		}
		
		public static string GenerateStoredProcedureCode(StoredProcedure pProc, string pColumnPrefix, bool pIncludeSchema) {

			string endl = Environment.NewLine;
			string tab = "\t";

			StringBuilder code = new StringBuilder();
			
			code.Append("using System;").Append(endl);
			code.Append("using System.Data;");
			code.Append("using System.Data.SqlClient;");
			
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
			
			for (int index = 0; index < pProc.Parameters.Count; index++) {
				
				Sql.Database.SpParameter param = pProc.Parameters[index];
				
				if(index > 0)
					code.Append(", ");
				
				if(param.Direction == ParameterDirection.Output || param.Direction == ParameterDirection.ReturnValue)
					code.Append("out ");
				
				code.Append(GetReturnType(param.ParamType, false)).Append(" ").Append(param.Name);
			}
			
			if(pProc.Parameters.Count > 0)
				code.Append(", ");
			
			code.Append("Sql.Transaction pTransaction){").Append(endl).Append(endl);
			
			for (int index = 0; index < pProc.Parameters.Count; index++) {
				
				Sql.Database.SpParameter param = pProc.Parameters[index];
				
				code.Append(tab).Append(tab).Append(tab).Append("SqlParameter p").Append(index.ToString()).Append(" = new SqlParameter(\"").Append(param.Name).Append("\", SqlDbType.").Append(ConvertToSqlDbType(param.ParamType).ToString()).Append(");").Append(endl);
				
					code.Append(tab).Append(tab).Append(tab).Append("p").Append(index.ToString()).Append(".Direction = ParameterDirection.").Append(param.Direction.ToString()).Append(";").Append(endl);
				
					if(param.Direction == ParameterDirection.Input || param.Direction == ParameterDirection.InputOutput)
						code.Append(tab).Append(tab).Append(tab).Append("p").Append(index.ToString()).Append(".Value = ").Append(param.Name).Append(";").Append(endl);
					
					code.Append(endl);
			}
			
			code.Append(tab).Append(tab).Append(tab).Append("Sql.IResult result = ExecuteProcedure(pTransaction");
			
			for (int index = 0; index < pProc.Parameters.Count; index++)
				code.Append(", p").Append(index.ToString());
			
			code.Append(");").Append(endl).Append(endl);
			
			for (int index = 0; index < pProc.Parameters.Count; index++) {
				Sql.Database.SpParameter param = pProc.Parameters[index];
				
				if(param.Direction == ParameterDirection.InputOutput || param.Direction == ParameterDirection.Output || param.Direction == ParameterDirection.ReturnValue)
					code.Append(tab).Append(tab).Append(tab).Append(param.Name).Append(" = (").Append(GetReturnType(param.ParamType, false)).Append(")").Append("p").Append(index.ToString()).Append(".Value;").Append(endl);
			}
			
			code.Append(tab).Append(tab).Append(tab).Append("return result;").Append(endl);
			code.Append(tab).Append(tab).Append("}").Append(endl).Append(endl);
			
			code.Append(tab).Append(tab).Append("public Row this[int pIndex, Sql.IResult pResult]{").Append(endl);
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
		
		private static SqlDbType ConvertToSqlDbType(System.Data.DbType pDbType) {
			
			if(pDbType == DbType.Int16)
				return SqlDbType.SmallInt;
			if(pDbType == DbType.Int32)
				return SqlDbType.Int;
			if(pDbType == DbType.Int64)
				return SqlDbType.BigInt;
			if(pDbType == DbType.String)
				return SqlDbType.VarChar;
			if(pDbType == DbType.Decimal)
				return SqlDbType.Decimal;
			if(pDbType == DbType.DateTime)
				return SqlDbType.DateTime;
			if(pDbType == DbType.DateTime2)
				return SqlDbType.DateTime2;			
			if(pDbType == DbType.DateTimeOffset)
				return SqlDbType.DateTimeOffset;			
			if(pDbType == DbType.Byte)
				return SqlDbType.TinyInt;			
			if(pDbType == DbType.Double)
				return SqlDbType.Float;
			if(pDbType == DbType.Single)
				return SqlDbType.Real;
			
			return 0;
		}
	}
}