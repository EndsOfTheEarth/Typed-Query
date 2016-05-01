
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

namespace Sql.Database {
	
	internal static class GenertateSql {
		
		internal static string GetSelectQuery(ADatabase pDatabase, Core.QueryBuilder pQueryBuilder, Core.Parameters pParameters) {
			
			IAliasManager aliasManager = new AliasManager();
			
			switch(pDatabase.DatabaseType){
				case DatabaseType.Mssql:
					return SqlServer.GenerateSql.GetSelectQuery(pDatabase, pQueryBuilder, pParameters, aliasManager);
				case DatabaseType.PostgreSql:
					return PostgreSql.GenerateSql.GetSelectQuery(pDatabase, pQueryBuilder, pParameters, aliasManager);
				default:
					throw new Exception("Unknown database type: " + pDatabase.DatabaseType.ToString());
			}		
		}
		
		internal static string GetInsertQuery(ADatabase pDatabase, Core.InsertBuilder pInsertBuilder, Core.Parameters pParameters) {
			
			switch(pDatabase.DatabaseType){
				case DatabaseType.Mssql:
					return SqlServer.GenerateSql.GetInsertQuery(pDatabase, pInsertBuilder, pParameters);
				case DatabaseType.PostgreSql:
					return PostgreSql.GenerateSql.GetInsertQuery(pDatabase, pInsertBuilder, pParameters);
				default:
					throw new Exception("Unknown database type: " + pDatabase.DatabaseType.ToString());
			}
		}
		
		internal static string GetBulkInsertQuery(ADatabase pDatabase, List<Core.InsertBuilder> pInsertBuilders) {
			switch(pDatabase.DatabaseType){
				case DatabaseType.Mssql:
					return SqlServer.GenerateSql.GetBulkInsertQuery(pDatabase, pInsertBuilders);
				case DatabaseType.PostgreSql:
					return PostgreSql.GenerateSql.GetBulkInsertQuery(pDatabase, pInsertBuilders);
				default:
					throw new Exception("Unknown database type: " + pDatabase.DatabaseType.ToString());
			}
		}
		
		internal static string GetInsertSelectQuery(ADatabase pDatabase, Core.InsertSelectBuilder pInsertBuilder, Core.Parameters pParameters) {
			
			switch(pDatabase.DatabaseType){
				case DatabaseType.Mssql:
					return SqlServer.GenerateSql.GetInsertSelectQuery(pDatabase, pInsertBuilder, pParameters);
				case DatabaseType.PostgreSql:
					return PostgreSql.GenerateSql.GetInsertSelectQuery(pDatabase, pInsertBuilder, pParameters);
				default:
					throw new Exception("Unknown database type: " + pDatabase.DatabaseType.ToString());
			}
		}
		
		internal static string GetUpdateQuery(ADatabase pDatabase, Core.UpdateBuilder pUpdateBuilder, Core.Parameters pParameters) {
		
			switch(pDatabase.DatabaseType){
				case DatabaseType.Mssql:
					return SqlServer.GenerateSql.GetUpdateQuery(pDatabase, pUpdateBuilder, pParameters);
				case DatabaseType.PostgreSql:
					return PostgreSql.GenerateSql.GetUpdateQuery(pDatabase, pUpdateBuilder, pParameters);
				default:
					throw new Exception("Unknown database type: " + pDatabase.DatabaseType.ToString());
			}
		}
		
		internal static string GetDeleteQuery(ADatabase pDatabase, Core.DeleteBuilder pDeleteBuilder, Core.Parameters pParameters) {
			
			switch(pDatabase.DatabaseType){
				case DatabaseType.Mssql:
					return SqlServer.GenerateSql.GetDeleteQuery(pDatabase, pDeleteBuilder, pParameters);
				case DatabaseType.PostgreSql:
					return PostgreSql.GenerateSql.GetDeleteQuery(pDatabase, pDeleteBuilder, pParameters);
				default:
					throw new Exception("Unknown database type: " + pDatabase.DatabaseType.ToString());
			}
		}
		
		internal static string GetTruncateQuery(ADatabase pDatabase, ATable pTable) {
			
			switch(pDatabase.DatabaseType){
				case DatabaseType.Mssql:
					return SqlServer.GenerateSql.GetTruncateQuery(pTable);
				case DatabaseType.PostgreSql:
					return PostgreSql.GenerateSql.GetTruncateQuery(pTable);
				default:
					throw new Exception("Unknown database type: " + pDatabase.DatabaseType.ToString());
			}
		}
		
		internal static string GetStoreProcedureQuery(ADatabase pDatabase, ATable pTable, Core.Parameters pParameters, object[] pParams) {
			
			IAliasManager aliasManager = new AliasManager();
			
			switch(pDatabase.DatabaseType){
				case DatabaseType.Mssql:
					return SqlServer.GenerateSql.GetStoreProcedureQuery(pDatabase, pTable, pParameters, pParams, aliasManager);
//				case DatabaseType.PostgreSql:
//					return PostgreSql.GenerateSql.GetStoreProcedureQuery(pTable, pParameters, pParams);
				default:
					throw new Exception("Unknown database type: " + pDatabase.DatabaseType.ToString());
			}
		}
		
		internal static string CreateTableComment(string pTableName, string pDescription, ADatabase pDatabase){
			
			if(string.IsNullOrWhiteSpace(pTableName))
				throw new ArgumentException("pTableName cannot be null or empty");
			
			if(pDatabase == null)
				throw new NullReferenceException("pDatabase cannot be null");
			
			switch(pDatabase.DatabaseType){
				case DatabaseType.Mssql:
					return SqlServer.GenerateSql.CreateTableComment("dbo", pTableName, pDescription);
				case DatabaseType.PostgreSql:
					return PostgreSql.GenerateSql.CreateTableComment("", pTableName, pDescription);
				default:
					throw new Exception("Unknown database type: " + pDatabase.DatabaseType.ToString());
			}
		}
		
		internal static string CreateColumnComment(string pTableName, string pColumnName, string pDescription, ADatabase pDatabase) {
			
			if(string.IsNullOrWhiteSpace(pTableName))
				throw new ArgumentException("pTableName cannot be null or empty");
			
			if(pDatabase == null)
				throw new NullReferenceException("pDatabase cannot be null");
			
			switch(pDatabase.DatabaseType){
				case DatabaseType.Mssql:
					return SqlServer.GenerateSql.CreateColumnComment("dbo", pTableName, pColumnName, pDescription);
				case DatabaseType.PostgreSql:
					return PostgreSql.GenerateSql.CreateColumnComment("", pTableName, pColumnName, pDescription);
				default:
					throw new Exception("Unknown database type: " + pDatabase.DatabaseType.ToString());
			}
		}
		
		internal static string CreateGrantTable(string pSchema, string pTableName, string pUserName, Sql.Privilege pPrivilege, ADatabase pDatabase) {
			
			if(string.IsNullOrWhiteSpace(pTableName))
				throw new ArgumentException("pTableName cannot be null or empty");
			
			if(string.IsNullOrWhiteSpace(pUserName))
				throw new ArgumentException("pUserName cannot be null or empty");
			
			if(pDatabase == null)
				throw new NullReferenceException("pDatabase cannot be null");
			
			switch(pDatabase.DatabaseType){
				case DatabaseType.Mssql:
					return SqlServer.GenerateSql.CreateGrantTable(pSchema, pTableName, pUserName, pPrivilege, pDatabase);
				case DatabaseType.PostgreSql:
					return PostgreSql.GenerateSql.CreateGrantTable(pSchema, pTableName, pUserName, pPrivilege, pDatabase);
				default:
					throw new Exception("Unknown database type: " + pDatabase.DatabaseType.ToString());
			}
		}		
		
		internal static string CreateGrantOrRevokeColumn(PrivAction pPrivAction, string pSchema, string pTableName, string pColumnName, string pUserName, Sql.ColumnPrivilege pPrivilege, ADatabase pDatabase) {
			
			if(string.IsNullOrWhiteSpace(pTableName))
				throw new ArgumentException("pTableName cannot be null or empty");
			
			if(string.IsNullOrWhiteSpace(pUserName))
				throw new ArgumentException("pUserName cannot be null or empty");
			
			if(pDatabase == null)
				throw new NullReferenceException("pDatabase cannot be null");
			
			switch(pDatabase.DatabaseType){
				case DatabaseType.Mssql:
					return SqlServer.GenerateSql.CreateGrantOrRevokeColumn(pPrivAction, pSchema, pTableName, pColumnName, pUserName, pPrivilege, pDatabase);
				case DatabaseType.PostgreSql:
					return PostgreSql.GenerateSql.CreateGrantOrRevokeColumn(pPrivAction, pSchema, pTableName, pColumnName, pUserName, pPrivilege, pDatabase);
				default:
					throw new Exception("Unknown database type: " + pDatabase.DatabaseType.ToString());
			}
		}
	}
	
	public enum PrivAction {
		GRANT,
		REVOKE
	}
}