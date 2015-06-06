
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
	
	public class GenerateMetaDataSql {
		
		public GenerateMetaDataSql() {
			
		}
		
		public string GenerateSql<TABLE>(TABLE pTable) where TABLE : ATable {
		
			if(pTable == null)
				throw new NullReferenceException("pTable cannot be null");			
			
			StringBuilder sql = new StringBuilder();
			
			object[] tableAttributes = pTable.GetType().GetCustomAttributes(true);
			
			string tableComment = null;
			
			foreach(object attribute in tableAttributes) {
			
				if (attribute is Sql.TableAttribute) {
			        tableComment = Sql.Database.GenertateSql.CreateTableComment(pTable.TableName, ((Sql.TableAttribute) attribute).Description, pTable.DefaultDatabase);
			        break;
			    }
			}
			
			if(tableComment == null)
				tableComment = Sql.Database.GenertateSql.CreateTableComment(pTable.TableName, string.Empty, pTable.DefaultDatabase);
			
			sql.Append(tableComment).Append(System.Environment.NewLine);

			foreach(FieldInfo fieldInfo in pTable.GetType().GetFields()) {
				
				if(typeof(AColumn).IsAssignableFrom(fieldInfo.FieldType)) {
					
					AColumn column = (AColumn)fieldInfo.GetValue(pTable);
					
					object[] columnAttributes = fieldInfo.GetCustomAttributes(true);
					
					string columnComment = null;
					
					string valuesText = string.Empty;
					
					if(column is Sql.Column.IEnumColumn) {
						
						valuesText = GetEnumColumnValues((Sql.Column.IEnumColumn) column);
					}
					
					foreach(object attribute in columnAttributes) {
					
						if (attribute is Sql.ColumnAttribute) {
							
							string description = ((Sql.ColumnAttribute) attribute).Description;
							
							if(!string.IsNullOrWhiteSpace(description))
								description += " ";
						
							if(!string.IsNullOrEmpty(valuesText))
								description += "(" + valuesText + ")";
						
					    	columnComment = Sql.Database.GenertateSql.CreateColumnComment(pTable.TableName, column.ColumnName, description, pTable.DefaultDatabase);
					    	break;
						}
					}
					
					if(columnComment == null) {
						
						string description = string.Empty;
						
						if(!string.IsNullOrEmpty(valuesText))
							description += "(" + valuesText + ")";
						
						columnComment = Sql.Database.GenertateSql.CreateColumnComment(pTable.TableName, column.ColumnName, description, pTable.DefaultDatabase);
					}
					
					sql.Append(columnComment).Append(System.Environment.NewLine);					
				}
			}
			
			foreach(PropertyInfo propertyInfo in pTable.GetType().GetProperties()) {
				
				if(typeof(AColumn).IsAssignableFrom(propertyInfo.PropertyType)) {
					
					AColumn column = (AColumn)propertyInfo.GetValue(pTable, null);
					
					object[] columnAttributes = propertyInfo.GetCustomAttributes(true);
					
					string columnComment = null;
					
					string valuesText = string.Empty;
					
					if(column is Sql.Column.IEnumColumn) {
						
						valuesText = GetEnumColumnValues((Sql.Column.IEnumColumn) column);
					}
					
					foreach(object attribute in columnAttributes) {
					
						if (attribute is Sql.ColumnAttribute) {
							
							string description = ((Sql.ColumnAttribute) attribute).Description;
							
							if(!string.IsNullOrWhiteSpace(description))
								description += " ";
						
							if(!string.IsNullOrEmpty(valuesText))
								description += "(" + valuesText + ")";
						
					    	columnComment = Sql.Database.GenertateSql.CreateColumnComment(pTable.TableName, column.ColumnName, description, pTable.DefaultDatabase);
					    	break;
						}
					}
					
					if(columnComment == null) {
						
						string description = string.Empty;
						
						if(!string.IsNullOrEmpty(valuesText))
							description += "(" + valuesText + ")";
						
						columnComment = Sql.Database.GenertateSql.CreateColumnComment(pTable.TableName, column.ColumnName, description, pTable.DefaultDatabase);
					}
					
					sql.Append(columnComment).Append(System.Environment.NewLine);					
				}
			}
			
			return sql.ToString();
		}
		
		private string GetEnumColumnValues(Sql.Column.IEnumColumn pEnumColumn) {
		
			Array enumValues = pEnumColumn.GetEnumType().GetEnumValues();
			
			StringBuilder text = new StringBuilder();
			
			foreach(object enumValue in enumValues) {
			
				if(text.Length > 0)
					text.Append(", ");
				
				text.Append(enumValue.ToString()).Append(" = ").Append(((int)enumValue).ToString());
			}			
			return text.ToString();
		}
	}
}