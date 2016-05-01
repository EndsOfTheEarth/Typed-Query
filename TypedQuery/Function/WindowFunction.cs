
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
using System.Text;

namespace Sql.Function {
	
	internal class WindowFunction {
		
		private AColumn[] mColumns;
		private IOrderByColumn[] mOrderByColumns;
		
		public WindowFunction() {
		}
		
		public void SetOverPartitionBy(params AColumn[] pColumns) {
			
			if(mColumns != null)
				throw new Exception("Cannot call OverPartitionBy(...) more than once");
			
			mColumns = pColumns != null ? pColumns : new AColumn[]{};
		}
		
		public void SetOrderBy(params IOrderByColumn[] pOrderByColumns) {
			
			if(pOrderByColumns == null)
				throw new NullReferenceException("pOrderByColumns cannot be null");
			
			if(pOrderByColumns.Length == 0)
				throw new Exception("pOrderByColumns cannot be empty");
			
			if(mOrderByColumns != null)
				throw new Exception("Cannot call OrderBy(...) more than once");
			
			mOrderByColumns = pOrderByColumns;
			
			for (int index = 0; index < mOrderByColumns.Length; index++) {
				
				if(!(mOrderByColumns[index] is AColumn))
					throw new Exception("All values in pOrderByColumns must be or type AColumn");
			}
		}
		
		public string GetSql(bool pUseAlias, Sql.Database.IAliasManager pAliasManager) {
			
			StringBuilder sql = new StringBuilder();
			
			sql.Append(" OVER(");
			
			if(mColumns != null && mColumns.Length > 0) {
				
				sql.Append("PARTITION BY ");
				
				for (int index = 0; index < mColumns.Length; index++) {
					
					AColumn column = mColumns[index];
					
					if(index > 0)
						sql.Append(",");
					
					if(pUseAlias)
						sql.Append(pAliasManager.GetAlias(column.Table)).Append(".");
					
					sql.Append(column.ColumnName);
				}
			}
			
			if(mOrderByColumns != null && mOrderByColumns.Length > 0) {
				
				sql.Append(" ORDER BY ");
				
				for (int index = 0; index < mOrderByColumns.Length; index++) {
					
					IOrderByColumn orderByColumn = mOrderByColumns[index];
					
					AColumn column = (AColumn) orderByColumn;
					
					if(index > 0)
						sql.Append(",");
					
					if(pUseAlias)
						sql.Append(pAliasManager.GetAlias(column.Table)).Append(".");
					
					sql.Append(column.ColumnName);
					
					if(orderByColumn.GetOrderByColumn.OrderBy == Sql.OrderBy.ASC)
						sql.Append(" ASC");
						
					if(orderByColumn.GetOrderByColumn.OrderBy == Sql.OrderBy.DESC)
						sql.Append(" DESC");
				}
			}
			
			sql.Append(")");
			
			return sql.ToString();
		}
	}
}