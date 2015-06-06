
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
using System.Collections.Generic;
using System.Text;

namespace Sql.Function {

	public enum DatePart {
		
		Year, Month, DayOfMonth, Hour, Minute, Second
	}
	
	public sealed class DateFunction : ANumericFunction {

		private readonly AColumn mColumn;
		private readonly DatePart mDatePart;

		public DateFunction(Column.DateTimeColumn pColumn, DatePart pDatePart) {

			if (((object)pColumn) == null)
				throw new NullReferenceException("pColumn cannot be null");

			mColumn = pColumn;
			mDatePart = pDatePart;
		}

		public DateFunction(Column.NDateTimeColumn pColumn, DatePart pDatePart) {

			if (((object)pColumn) == null)
				throw new NullReferenceException("pColumn cannot be null");

			mColumn = pColumn;
			mDatePart = pDatePart;
		}
		
		public DateFunction(Column.DateTime2Column pColumn, DatePart pDatePart) {

			if (((object)pColumn) == null)
				throw new NullReferenceException("pColumn cannot be null");

			mColumn = pColumn;
			mDatePart = pDatePart;
		}
		
		public DateFunction(Column.NDateTime2Column pColumn, DatePart pDatePart) {

			if (((object)pColumn) == null)
				throw new NullReferenceException("pColumn cannot be null");

			mColumn = pColumn;
			mDatePart = pDatePart;
		}
		
		public DateFunction(Column.DateTimeOffsetColumn pColumn, DatePart pDatePart) {

			if (((object)pColumn) == null)
				throw new NullReferenceException("pColumn cannot be null");

			mColumn = pColumn;
			mDatePart = pDatePart;
		}
		
		public DateFunction(Column.NDateTimeOffsetColumn pColumn, DatePart pDatePart) {

			if (((object)pColumn) == null)
				throw new NullReferenceException("pColumn cannot be null");

			mColumn = pColumn;
			mDatePart = pDatePart;
		}

		public int? this[int pIndex, IResult pResult] {
			get {
				return (int?)pResult.GetValue(this, pIndex);
			}
		}
		public override string GetFunctionSql(ADatabase pDatabase, bool pUseAlias, Sql.Database.IAliasManager pAliasManager) {
			
			string value;
			
			if(pDatabase.DatabaseType == DatabaseType.Mssql) {
				
				switch(mDatePart){
						
					case DatePart.Year:
						value = "YEAR(" + (pUseAlias ? pAliasManager.GetAlias(mColumn.Table) + "." : string.Empty) + mColumn.ColumnName + ")";
						break;
					case DatePart.Month:
						value = "MONTH(" + (pUseAlias ? pAliasManager.GetAlias(mColumn.Table) + "." : string.Empty) + mColumn.ColumnName + ")";
						break;
					case DatePart.DayOfMonth:
						value = "DAY(" + (pUseAlias ? pAliasManager.GetAlias(mColumn.Table) + "." : string.Empty) + mColumn.ColumnName + ")";
						break;
					case DatePart.Hour:
						value = "DATEPART(HOUR," + (pUseAlias ? pAliasManager.GetAlias(mColumn.Table) + "." : string.Empty) + mColumn.ColumnName + ")";
						break;
					case DatePart.Minute:
						value = "DATEPART(MINUTE," + (pUseAlias ? pAliasManager.GetAlias(mColumn.Table) + "." : string.Empty) + mColumn.ColumnName + ")";
						break;
					case DatePart.Second:
						value = "DATEPART(SECOND," + (pUseAlias ? pAliasManager.GetAlias(mColumn.Table) + "." : string.Empty) + mColumn.ColumnName + ")";
						break;
					default:
						throw new Exception("Unknown date part value: '" + mDatePart.ToString() + "'");
				}
			}
			else if(pDatabase.DatabaseType == DatabaseType.PostgreSql) {
				
				switch(mDatePart){
						
					case DatePart.Year:
						value = "EXTRACT(YEAR FROM " + (pUseAlias ? pAliasManager.GetAlias(mColumn.Table) + "." : string.Empty) + mColumn.ColumnName + ")";
						break;
					case DatePart.Month:
						value = "EXTRACT(MONTH FROM " + (pUseAlias ? pAliasManager.GetAlias(mColumn.Table) + "." : string.Empty) + mColumn.ColumnName + ")";
						break;
					case DatePart.DayOfMonth:
						value = "EXTRACT(DAY FROM " + (pUseAlias ? pAliasManager.GetAlias(mColumn.Table) + "." : string.Empty) + mColumn.ColumnName + ")";
						break;
					case DatePart.Hour:
						value = "EXTRACT(HOUR FROM " + (pUseAlias ? pAliasManager.GetAlias(mColumn.Table) + "." : string.Empty) + mColumn.ColumnName + ")";
						break;
					case DatePart.Minute:
						value = "EXTRACT(MINUTE FROM " + (pUseAlias ? pAliasManager.GetAlias(mColumn.Table) + "." : string.Empty) + mColumn.ColumnName + ")";
						break;
					case DatePart.Second:
						value = "EXTRACT(SECOND FROM " + (pUseAlias ? pAliasManager.GetAlias(mColumn.Table) + "." : string.Empty) + mColumn.ColumnName + ")";
						break;
					default:
						throw new Exception("Unknown date part value: '" + mDatePart.ToString() + "'");
				}
			}
			else
				throw new Exception("Unsupportted database type: '" + pDatabase.DatabaseType.ToString() + "'");
			
			return value;
		}
		public override object GetValue(ADatabase pDatabase, System.Data.Common.DbDataReader pReader, int pColumnIndex) {

			if (pReader.IsDBNull(pColumnIndex))
				return null;
			
			Type fieldType = pReader.GetFieldType(pColumnIndex);
			
			if (fieldType == typeof(Int64))
				return (int?)Convert.ToInt32(pReader.GetInt64(pColumnIndex));
			else if (fieldType == typeof(double))
				return (int?)pReader.GetDouble(pColumnIndex);
			
			return (int?)pReader.GetInt32(pColumnIndex);
		}
	}
}