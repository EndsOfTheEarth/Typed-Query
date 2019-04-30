
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
using Sql.Column;

namespace Sql.Function {

    public sealed class SumInteger : ANumericFunction {

        private readonly AColumn mColumn;

        public SumInteger(Column.IntegerColumn pColumn) {

            if(((object)pColumn) == null) {
                throw new NullReferenceException("pColumn cannot be null");
            }
            mColumn = pColumn;
        }

        public SumInteger(Column.NIntegerColumn pColumn) {

            if(((object)pColumn) == null) {
                throw new NullReferenceException("pColumn cannot be null");
            }
            mColumn = pColumn;
        }

        public int? this[int pIndex, IResult pResult] {
            get {
                return (int?)pResult.GetValue(this, pIndex);
            }
        }
        public override string GetFunctionSql(ADatabase pDatabase, bool pUseAlias, Sql.Database.IAliasManager pAliasManager) {
            return "SUM(" + (pUseAlias ? pAliasManager.GetAlias(mColumn.Table) + "." : string.Empty) + mColumn.ColumnName + ")" + GetWindowFunctionSql(pUseAlias, pAliasManager);
        }
        public override object GetValue(ADatabase pDatabase, System.Data.Common.DbDataReader pReader, int pColumnIndex) {

            if(pReader.IsDBNull(pColumnIndex)) {
                return null;
            }
            if(pReader.GetFieldType(pColumnIndex) == typeof(Int64)) {
                return (int?)Convert.ToInt32(pReader.GetInt64(pColumnIndex));
            }
            return (int?)pReader.GetInt32(pColumnIndex);
        }
    }

    public sealed class SumBigInteger : ANumericFunction {

        private readonly AColumn mColumn;

        public SumBigInteger(Column.BigIntegerColumn pColumn) {

            if(((object)pColumn) == null) {
                throw new NullReferenceException("pColumn cannot be null");
            }
            mColumn = pColumn;
        }

        public SumBigInteger(Column.NBigIntegerColumn pColumn) {

            if(((object)pColumn) == null) {
                throw new NullReferenceException("pColumn cannot be null");
            }
            mColumn = pColumn;
        }

        public Int64? this[int pIndex, IResult pResult] {
            get {
                return (Int64?)pResult.GetValue(this, pIndex);
            }
        }
        public override string GetFunctionSql(ADatabase pDatabase, bool pUseAlias, Sql.Database.IAliasManager pAliasManager) {
            return "SUM(" + (pUseAlias ? pAliasManager.GetAlias(mColumn.Table) + "." : string.Empty) + mColumn.ColumnName + ")" + GetWindowFunctionSql(pUseAlias, pAliasManager);
        }
        public override object GetValue(ADatabase pDatabase, System.Data.Common.DbDataReader pReader, int pColumnIndex) {

            if(pReader.IsDBNull(pColumnIndex)) {
                return null;
            }
            if(pReader.GetFieldType(pColumnIndex) == typeof(decimal)) {
                return decimal.ToInt64(pReader.GetDecimal(pColumnIndex));
            }
            else {
                return (Int64?)pReader.GetInt64(pColumnIndex);
            }
        }
    }

    public sealed class SumDecimal : ANumericFunction {

        private readonly AColumn mColumn;

        public SumDecimal(Column.DecimalColumn pColumn) {

            if(((object)pColumn) == null) {
                throw new NullReferenceException("pColumn cannot be null");
            }
            mColumn = pColumn;
        }

        public SumDecimal(Column.NDecimalColumn pColumn) {

            if(((object)pColumn) == null) {
                throw new NullReferenceException("pColumn cannot be null");
            }
            mColumn = pColumn;
        }

        public decimal? this[int pIndex, IResult pResult] {
            get {
                return (decimal?)pResult.GetValue(this, pIndex);
            }
        }
        public override string GetFunctionSql(ADatabase pDatabase, bool pUseAlias, Sql.Database.IAliasManager pAliasManager) {
            return "SUM(" + (pUseAlias ? pAliasManager.GetAlias(mColumn.Table) + "." : string.Empty) + mColumn.ColumnName + ")" + GetWindowFunctionSql(pUseAlias, pAliasManager);
        }
        public override object GetValue(ADatabase pDatabase, System.Data.Common.DbDataReader pReader, int pColumnIndex) {

            if(pReader.IsDBNull(pColumnIndex)) {
                return null;
            }
            return (decimal?)pReader.GetDecimal(pColumnIndex);
        }
    }

    public sealed class SumSmallInt : ANumericFunction {

        private readonly AColumn mColumn;

        public SumSmallInt(Column.SmallIntegerColumn pColumn) {

            if(((object)pColumn) == null) {
                throw new NullReferenceException("pColumn cannot be null");
            }
            mColumn = pColumn;
        }

        public SumSmallInt(Column.NSmallIntegerColumn pColumn) {

            if(((object)pColumn) == null) {
                throw new NullReferenceException("pColumn cannot be null");
            }
            mColumn = pColumn;
        }

        public Int16? this[int pIndex, IResult pResult] {
            get {
                return (Int16?)pResult.GetValue(this, pIndex);
            }
        }
        public override string GetFunctionSql(ADatabase pDatabase, bool pUseAlias, Sql.Database.IAliasManager pAliasManager) {
            return "SUM(" + (pUseAlias ? pAliasManager.GetAlias(mColumn.Table) + "." : string.Empty) + mColumn.ColumnName + ")" + GetWindowFunctionSql(pUseAlias, pAliasManager);
        }
        public override object GetValue(ADatabase pDatabase, System.Data.Common.DbDataReader pReader, int pColumnIndex) {

            if(pReader.IsDBNull(pColumnIndex)) {
                return null;
            }
            return (Int16?)pReader.GetInt16(pColumnIndex);
        }
    }

    public sealed class SumFloat : ANumericFunction {

        private readonly AColumn mColumn;

        public SumFloat(Column.FloatColumn pColumn) {

            if(((object)pColumn) == null) {
                throw new NullReferenceException("pColumn cannot be null");
            }
            mColumn = pColumn;
        }

        public SumFloat(Column.NFloatColumn pColumn) {

            if(((object)pColumn) == null) {
                throw new NullReferenceException("pColumn cannot be null");
            }
            mColumn = pColumn;
        }

        public float? this[int pIndex, IResult pResult] {
            get {
                return (float?)pResult.GetValue(this, pIndex);
            }
        }
        public override string GetFunctionSql(ADatabase pDatabase, bool pUseAlias, Sql.Database.IAliasManager pAliasManager) {
            return "SUM(" + (pUseAlias ? pAliasManager.GetAlias(mColumn.Table) + "." : string.Empty) + mColumn.ColumnName + ")" + GetWindowFunctionSql(pUseAlias, pAliasManager);
        }
        public override object GetValue(ADatabase pDatabase, System.Data.Common.DbDataReader pReader, int pColumnIndex) {

            if(pReader.IsDBNull(pColumnIndex)) {
                return null;
            }
            return (float?)pReader.GetFloat(pColumnIndex);
        }
    }

    public sealed class SumDouble : ANumericFunction {

        private readonly AColumn mColumn;

        public SumDouble(Column.DoubleColumn pColumn) {

            if(((object)pColumn) == null) {
                throw new NullReferenceException("pColumn cannot be null");
            }
            mColumn = pColumn;
        }

        public SumDouble(Column.NDoubleColumn pColumn) {

            if(((object)pColumn) == null) {
                throw new NullReferenceException("pColumn cannot be null");
            }
            mColumn = pColumn;
        }

        public double? this[int pIndex, IResult pResult] {
            get {
                return (double?)pResult.GetValue(this, pIndex);
            }
        }
        public override string GetFunctionSql(ADatabase pDatabase, bool pUseAlias, Sql.Database.IAliasManager pAliasManager) {
            return "SUM(" + (pUseAlias ? pAliasManager.GetAlias(mColumn.Table) + "." : string.Empty) + mColumn.ColumnName + ")" + GetWindowFunctionSql(pUseAlias, pAliasManager);
        }
        public override object GetValue(ADatabase pDatabase, System.Data.Common.DbDataReader pReader, int pColumnIndex) {

            if(pReader.IsDBNull(pColumnIndex)) {
                return null;
            }
            return (double?)pReader.GetDouble(pColumnIndex);
        }
    }
}