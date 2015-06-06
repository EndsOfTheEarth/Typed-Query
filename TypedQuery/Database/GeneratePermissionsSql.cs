
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
	
	public class GeneratePermissionsSql	{
		
		public GeneratePermissionsSql()	{
			
		}
		
		public string GenerateSql<TABLE>(TABLE pTable) where TABLE : ATable {
		
			if(pTable == null)
				throw new NullReferenceException("pTable cannot be null");			
			
			StringBuilder sql = new StringBuilder();
			
			object[] tableAttributes = pTable.GetType().GetCustomAttributes(true);
			
			foreach(object attribute in tableAttributes) {
			
				if (attribute is Sql.GrantTable) {
					
					Sql.GrantTable grantTable = (Sql.GrantTable) attribute;					
			        string grantSql = Sql.Database.GenertateSql.CreateGrantTable(pTable.Schema, pTable.TableName, grantTable.User.Name, grantTable.Privilege, pTable.DefaultDatabase);
			        sql.Append(grantSql).Append(System.Environment.NewLine);
			    }
			}			

			foreach(FieldInfo fieldInfo in pTable.GetType().GetFields()) {
				
				if(typeof(AColumn).IsAssignableFrom(fieldInfo.FieldType)) {
					
					AColumn column = (AColumn)fieldInfo.GetValue(pTable);
					
					object[] columnAttributes = fieldInfo.GetCustomAttributes(true);
					
					foreach(object attribute in columnAttributes) {
					
						if (attribute is Sql.GrantColumn) {
							
							Sql.GrantColumn grantColumn = (Sql.GrantColumn) attribute;
						
					    	string grantSql = Sql.Database.GenertateSql.CreateGrantOrRevokeColumn(PrivAction.GRANT, pTable.Schema, pTable.TableName, column.ColumnName, grantColumn.User.Name, grantColumn.Privilege, pTable.DefaultDatabase);
					    	sql.Append(grantSql).Append(System.Environment.NewLine);
						}
						else if (attribute is Sql.RevokeColumn) {
							
							Sql.RevokeColumn revokeColumn = (Sql.RevokeColumn) attribute;
						
					    	string revokeSql = Sql.Database.GenertateSql.CreateGrantOrRevokeColumn(PrivAction.REVOKE, pTable.Schema, pTable.TableName, column.ColumnName, revokeColumn.User.Name, revokeColumn.Privilege, pTable.DefaultDatabase);
					    	sql.Append(revokeSql).Append(System.Environment.NewLine);
						}
					}					
				}
			}
			
			foreach(PropertyInfo propertyInfo in pTable.GetType().GetProperties()) {
				
				if(typeof(AColumn).IsAssignableFrom(propertyInfo.PropertyType)) {
					
					AColumn column = (AColumn)propertyInfo.GetValue(pTable, null);
					
					object[] columnAttributes = propertyInfo.GetCustomAttributes(true);
					
					foreach(object attribute in columnAttributes) {
					
						if (attribute is Sql.GrantColumn) {
							
							Sql.GrantColumn grantColumn = (Sql.GrantColumn) attribute;
						
					    	string grantSql = Sql.Database.GenertateSql.CreateGrantOrRevokeColumn(PrivAction.GRANT, pTable.Schema, pTable.TableName, column.ColumnName, grantColumn.User.Name, grantColumn.Privilege, pTable.DefaultDatabase);
					    	sql.Append(grantSql).Append(System.Environment.NewLine);
						}
						else if (attribute is Sql.RevokeColumn) {
							
							Sql.RevokeColumn revokeColumn = (Sql.RevokeColumn) attribute;
						
					    	string revokeSql = Sql.Database.GenertateSql.CreateGrantOrRevokeColumn(PrivAction.REVOKE, pTable.Schema, pTable.TableName, column.ColumnName, revokeColumn.User.Name, revokeColumn.Privilege, pTable.DefaultDatabase);
					    	sql.Append(revokeSql).Append(System.Environment.NewLine);
						}
					}					
				}
			}
			
			return sql.ToString();
		}
	}
}