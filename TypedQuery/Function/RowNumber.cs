
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

namespace Sql.Function {
	
	public class RowNumber : ANumericFunction{
		
		public RowNumber() {
			
		}
		
		public override object GetValue(ADatabase pDatabase, System.Data.Common.DbDataReader pReader, int pColumnIndex) {
			if (pReader.IsDBNull(pColumnIndex))
				return null;
			if (pReader.GetFieldType(pColumnIndex) == typeof(Int64))
				return (int?)Convert.ToInt32(pReader.GetInt64(pColumnIndex));
			return (int?)pReader.GetInt32(pColumnIndex);
		}

		public int? this[int pIndex, IResult pResult] {
			get {
				return (int?)pResult.GetValue(this, pIndex);
			}
		}

		public override string GetFunctionSql(ADatabase pDatabase, bool pUseAlias, Sql.Database.IAliasManager pAliasManager) {
			return "ROW_NUMBER()" + GetWindowFunctionSql(pUseAlias, pAliasManager);
		}
	}
}