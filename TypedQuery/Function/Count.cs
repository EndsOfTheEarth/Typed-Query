
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

namespace Sql.Function {

    public sealed class Count : ANumericFunction {

        private readonly AColumn? mColumn;
        private readonly bool mDistinct;

        /// <summary>
        /// Count(*)
        /// </summary>
        public Count() {
            mColumn = null;
            mDistinct = false;
        }

        /// <summary>
        /// Count(column)
        /// </summary>
        /// <param name="pColumn"></param>
        public Count(AColumn pColumn) : this(pColumn, false) {

        }

        /// <summary>
        /// Count(distinct column)
        /// </summary>
        /// <param name="pColumn"></param>
        /// <param name="pDistinct"></param>
        public Count(AColumn pColumn, bool pDistinct) {

            if(pColumn == null) {
                throw new NullReferenceException("pColumn cannot be null");
            }
            mColumn = pColumn;
            mDistinct = pDistinct;
        }
        public override object? GetValue(ADatabase pDatabase, System.Data.Common.DbDataReader pReader, int pColumnIndex) {

            if(pReader.IsDBNull(pColumnIndex)) {
                return null;
            }
            if(pReader.GetFieldType(pColumnIndex) == typeof(Int64)) {
                return (int?)Convert.ToInt32(pReader.GetInt64(pColumnIndex));
            }
            return (int?)pReader.GetInt32(pColumnIndex);
        }

        public int? this[int pIndex, IResult pResult] {
            get {
                return (int?)pResult.GetValue(this, pIndex);
            }
        }

        public override string GetFunctionSql(ADatabase pDatabase, bool pUseAlias, Sql.Database.IAliasManager pAliasManager) {

            string sql;

            if(mColumn == null) {
                sql = "COUNT(*)" + GetWindowFunctionSql(pUseAlias, pAliasManager);
            }
            else {
                sql = "COUNT(" + (mDistinct ? " DISTINCT " : string.Empty) + (pUseAlias ? pAliasManager.GetAlias(mColumn.Table) + "." : string.Empty) + mColumn.ColumnName + ")" + GetWindowFunctionSql(pUseAlias, pAliasManager);
            }
            return sql;
        }
    }
}