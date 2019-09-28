
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

    public sealed class AvgInteger : ANumericFunction {

        private readonly AColumn mColumn;

        public AvgInteger(Column.IntegerColumn pColumn) {

            if(((object)pColumn) == null) {
                throw new NullReferenceException("pColumn cannot be null");
            }
            mColumn = pColumn;
        }

        public AvgInteger(Column.NIntegerColumn pColumn) {

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
            return "AVG(" + (pUseAlias ? pAliasManager.GetAlias(mColumn.Table) + "." : string.Empty) + mColumn.ColumnName + ")" + GetWindowFunctionSql(pUseAlias, pAliasManager);
        }
        public override object? GetValue(ADatabase pDatabase, System.Data.Common.DbDataReader pReader, int pColumnIndex) {
            return GetValueAsDecimal(pReader, pColumnIndex);
        }
    }

    public sealed class AvgBigInteger : ANumericFunction {

        private readonly AColumn mColumn;

        public AvgBigInteger(Column.BigIntegerColumn pColumn) {

            if(((object)pColumn) == null) {
                throw new NullReferenceException("pColumn cannot be null");
            }
            mColumn = pColumn;
        }

        public AvgBigInteger(Column.NBigIntegerColumn pColumn) {

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
            return "AVG(" + (pUseAlias ? pAliasManager.GetAlias(mColumn.Table) + "." : string.Empty) + mColumn.ColumnName + ")";
        }
        public override object? GetValue(ADatabase pDatabase, System.Data.Common.DbDataReader pReader, int pColumnIndex) {
            return GetValueAsDecimal(pReader, pColumnIndex);
        }
    }

    public sealed class AvgDecimal : ANumericFunction {

        private readonly AColumn mColumn;

        public AvgDecimal(Column.DecimalColumn pColumn) {

            if(((object)pColumn) == null) {
                throw new NullReferenceException("pColumn cannot be null");
            }
            mColumn = pColumn;
        }

        public AvgDecimal(Column.NDecimalColumn pColumn) {

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
            return "AVG(" + (pUseAlias ? pAliasManager.GetAlias(mColumn.Table) + "." : string.Empty) + mColumn.ColumnName + ")";
        }
        public override object? GetValue(ADatabase pDatabase, System.Data.Common.DbDataReader pReader, int pColumnIndex) {
            return GetValueAsDecimal(pReader, pColumnIndex);
        }
    }

    public sealed class AvgSmallInt : ANumericFunction {

        private readonly AColumn mColumn;

        public AvgSmallInt(Column.SmallIntegerColumn pColumn) {

            if(((object)pColumn) == null) {
                throw new NullReferenceException("pColumn cannot be null");
            }
            mColumn = pColumn;
        }

        public AvgSmallInt(Column.NSmallIntegerColumn pColumn) {

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
            return "AVG(" + (pUseAlias ? pAliasManager.GetAlias(mColumn.Table) + "." : string.Empty) + mColumn.ColumnName + ")";
        }
        public override object? GetValue(ADatabase pDatabase, System.Data.Common.DbDataReader pReader, int pColumnIndex) {
            return GetValueAsDecimal(pReader, pColumnIndex);
        }
    }

    public sealed class AvgFloat : ANumericFunction {

        private readonly AColumn mColumn;

        public AvgFloat(Column.FloatColumn pColumn) {

            if(((object)pColumn) == null) {
                throw new NullReferenceException("pColumn cannot be null");
            }
            mColumn = pColumn;
        }

        public AvgFloat(Column.NFloatColumn pColumn) {

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
            return "AVG(" + (pUseAlias ? pAliasManager.GetAlias(mColumn.Table) + "." : string.Empty) + mColumn.ColumnName + ")";
        }
        public override object? GetValue(ADatabase pDatabase, System.Data.Common.DbDataReader pReader, int pColumnIndex) {
            return GetValueAsDouble(pReader, pColumnIndex);
        }
    }

    public sealed class AvgDouble : ANumericFunction {

        private readonly AColumn mColumn;

        public AvgDouble(Column.DoubleColumn pColumn) {

            if(((object)pColumn) == null) {
                throw new NullReferenceException("pColumn cannot be null");
            }
            mColumn = pColumn;
        }

        public AvgDouble(Column.NDoubleColumn pColumn) {

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
            return "AVG(" + (pUseAlias ? pAliasManager.GetAlias(mColumn.Table) + "." : string.Empty) + mColumn.ColumnName + ")";
        }
        public override object? GetValue(ADatabase pDatabase, System.Data.Common.DbDataReader pReader, int pColumnIndex) {
            return GetValueAsDouble(pReader, pColumnIndex);
        }
    }
}