
/*
 * 
 * Copyright (C) 2009-2015 JFo.nz
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
using System.Reflection;
using System.Text;

namespace Sql.Database {
	
	/// <summary>
	/// Definition checker is used to check table and row definitions against the actual tables in the database
	/// </summary>
	public static class CheckDefinitions {
		
		/// <summary>
		/// Check table and row definitions that are in dll files
		/// </summary>
		/// <param name="pFileNames">dll file names to check</param>
		/// <param name="pDatabase">Database to check against</param>
		/// <returns></returns>
		public static string Check(string[] pFileNames, Sql.ADatabase pDatabase) {
			
			if(pFileNames == null || pFileNames.Length == 0)
				throw new Exception("pFileNames cannot be null or empty");
			
			if(pDatabase == null)
				throw new NullReferenceException("pDatabase cannot be null");
			
			StringBuilder output = new StringBuilder();
			
			foreach(string fileName in pFileNames) {
				
				if(fileName.EndsWith("\\TypedQuery.dll")) {
					output.Append("Cannot test TypedQuery.dll as the class types interfere with the definition checker ").Append(System.Environment.NewLine);
					continue;
				}
				
				try {
					Assembly assembly = Assembly.LoadFrom(fileName);
					Type[] types = assembly.GetTypes();
					
					foreach(Type type in types) {
						if(!type.IsAbstract && IsATableType(type))
							output.Append(CheckTable(type, pDatabase));
					}
				}
				catch(Exception e) {
					output.Append(e.Message);
				}
			}
			output.Append("Check Completed");
			return output.ToString();
		}
		
		private static bool IsATableType(Type pType) {
			if(pType == null)
				return false;
			else if(pType == typeof(Sql.ATable))
				return true;
			return IsATableType(pType.BaseType);
		}
		
		/// <summary>
		/// Check a table aginst database
		/// </summary>
		/// <param name="pTableType">Type of ATable to check</param>
		/// <param name="pDatabase">Database to check against</param>
		/// <returns></returns>
		public static string CheckTable(Type pTableType, Sql.ADatabase pDatabase) {
			
			StringBuilder output = new StringBuilder();
			
			try {
				
				Sql.ATable table = (Sql.ATable)Activator.CreateInstance(pTableType);
				
				DbTable dbTable;
				
				if(pDatabase.DatabaseType == DatabaseType.Mssql)
					dbTable = Sql.Database.SqlServer.DatabaseDetails.GetTable(pDatabase, table);
				else if(pDatabase.DatabaseType == DatabaseType.PostgreSql)
					dbTable = Sql.Database.PostgreSql.DatabaseDetails.GetTable(pDatabase, table);
				else
					throw new Exception("Unknown database type: " + pDatabase.DatabaseType.ToString());
				
				if(dbTable == null){
					if(!table.IsTemporaryTable)
						output.Append("Table '" + table.TableName + "' missing from Database.").Append(System.Environment.NewLine);
				}
				else {
					foreach(DbColumn dbColumn in dbTable.Columns){
						Sql.AColumn codeColumn = null;
						
						foreach(Sql.AColumn col in table.Columns){
							if(string.Compare(col.ColumnName, dbColumn.Name, true) == 0){
								codeColumn = col;
								break;
							}
						}
						
						if(codeColumn == null)
							output.Append("Table '" + table.TableName + "' column in database but not in code. Column: '" + dbColumn.Name + "'").Append(System.Environment.NewLine);
						else {
							Type columnType = GetColumnType(dbColumn);
							if(columnType != codeColumn.GetType())
								if(!((columnType == typeof(Sql.Column.IntegerColumn) || columnType == typeof(Sql.Column.ByteColumn)) && ImplementsInterface(codeColumn.GetType(), typeof(Sql.Column.IEnumColumn))))
									output.Append("Table '" + table.TableName + "' column types don't match. Column: '" + dbColumn.Name + "', Types: " + columnType.ToString() + " != " + codeColumn.GetType().ToString()).Append(System.Environment.NewLine);
						}
					}						
					
					foreach(Sql.AColumn codeColumn in table.Columns) {
						
						DbColumn dbColumn = null;
						
						foreach(DbColumn col in dbTable.Columns) {
							if(string.Compare(codeColumn.ColumnName, col.Name, true) == 0) {
								dbColumn = col;
								break;
							}
						}
						if(dbColumn == null)
							output.Append("Table '" + table.TableName + "' column in code but not in database. Column: '" + dbColumn.Name + "'").Append(System.Environment.NewLine);
						else {
							if(dbColumn.IsPrimaryKey && !codeColumn.IsPrimaryKey)
								output.Append("Table '" + table.TableName + "' column is a primary key in database but not in code. Column: '" + dbColumn.Name + "'").Append(System.Environment.NewLine);
							else if(!dbColumn.IsPrimaryKey && codeColumn.IsPrimaryKey)
								output.Append("Table '" + table.TableName + "' column is a primary key in code but not in database. Column: '" + dbColumn.Name + "'").Append(System.Environment.NewLine);
							
							if(codeColumn is Sql.Column.IntegerColumn) {
								if(dbColumn.IsAutoGenerated && !((Sql.Column.IntegerColumn) codeColumn).IsAutoId)
									output.Append("Table '" + table.TableName + "' column is an auto id in database but not in code. Column: '" + dbColumn.Name + "'").Append(System.Environment.NewLine);
								else if(!dbColumn.IsAutoGenerated && ((Sql.Column.IntegerColumn) codeColumn).IsAutoId)
									output.Append("Table '" + table.TableName + "' column is an auto id in code but not in database. Column: '" + dbColumn.Name + "'").Append(System.Environment.NewLine);
							}
						}
					}
				}
				
				if(output.Length == 0)
					output.Append("Table '" + table.TableName + "'").Append(" ok.").Append(System.Environment.NewLine);
			}
			catch(Exception e) {
				string ex = e.InnerException.ToString();
				output.Append(e.Message);
			}
			return output.ToString();
		}
		
		private static bool ImplementsInterface(Type pType, Type pInterfaceType) {
			
			Type[] interfaces = pType.GetInterfaces();
			
			for(int index = 0; index < interfaces.Length; index++){
				if(interfaces[index] == pInterfaceType)
					return true;
			}
			return false;
		}
		
		private static Type GetColumnType(DbColumn pColumn) {

			Type value;

			if (pColumn.DbType == System.Data.DbType.Boolean) {
				value = !pColumn.Nullable ? typeof(Column.BoolColumn) : typeof(Column.NBoolColumn);
			}
			else if (pColumn.DbType == System.Data.DbType.DateTime) {
				value = !pColumn.Nullable ? typeof(Column.DateTimeColumn) : typeof(Column.NDateTimeColumn);
			}
			else if (pColumn.DbType == System.Data.DbType.DateTime2) {
				value = !pColumn.Nullable ? typeof(Column.DateTime2Column) : typeof(Column.NDateTime2Column);
			}
			else if (pColumn.DbType == System.Data.DbType.DateTimeOffset) {
				value = !pColumn.Nullable ? typeof(Column.DateTimeOffsetColumn) : typeof(Column.NDateTimeOffsetColumn);
			}
			else if (pColumn.DbType == System.Data.DbType.Decimal) {
				value = !pColumn.Nullable ? typeof(Column.DecimalColumn) : typeof(Column.NDecimalColumn);
			}
			else if (pColumn.DbType == System.Data.DbType.Guid) {
				value = !pColumn.Nullable ? typeof(Column.GuidColumn) : typeof(Column.NGuidColumn);
			}
			else if(pColumn.DbType == System.Data.DbType.Int16){
				value = !pColumn.Nullable ? typeof(Column.SmallIntegerColumn) : typeof(Column.NSmallIntegerColumn);
			}
			else if (pColumn.DbType == System.Data.DbType.Int32) {
				value = !pColumn.Nullable ? typeof(Column.IntegerColumn) : typeof(Column.NIntegerColumn);
			}
			else if(pColumn.DbType == System.Data.DbType.Int64){
				value = !pColumn.Nullable ? typeof(Column.BigIntegerColumn) : typeof(Column.NBigIntegerColumn);
			}
			else if (pColumn.DbType == System.Data.DbType.String) {
				value = typeof(Column.StringColumn);
			}
			else if(pColumn.DbType == System.Data.DbType.Binary){
				value = !pColumn.Nullable ? typeof(Column.BinaryColumn) : typeof(Column.NBinaryColumn);
			}
			else if(pColumn.DbType == System.Data.DbType.Byte){
				value = !pColumn.Nullable ? typeof(Column.ByteColumn) : typeof(Column.NByteColumn);
			}
			else
				value = typeof(object);

			return value;
		}
	}
}