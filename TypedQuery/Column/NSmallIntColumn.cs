﻿
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

namespace Sql.Column {

    public class NSmallIntegerColumn : ANumericColumn {

        public NSmallIntegerColumn(ATable pTable, string pColumnName)
            : base(pTable, pColumnName, false, true) {
        }
        public NSmallIntegerColumn(ATable pTable, string pColumnName, bool pIsPrimaryKey)
            : base(pTable, pColumnName, pIsPrimaryKey, true) {
        }

        public static Condition operator ==(NSmallIntegerColumn pColumnA, NSmallIntegerColumn pColumnB) {
            return new ColumnCondition(pColumnA, Sql.Operator.EQUALS, pColumnB);
        }
        public static Condition operator ==(NSmallIntegerColumn pColumnA, SmallIntegerColumn pColumnB) {
            return new ColumnCondition(pColumnA, Sql.Operator.EQUALS, pColumnB);
        }
        public static Condition operator ==(NSmallIntegerColumn pColumnA, Int16 pValue) {
            return new ColumnCondition(pColumnA, Sql.Operator.EQUALS, pValue);
        }

        public static Condition operator !=(NSmallIntegerColumn pColumnA, NSmallIntegerColumn pColumnB) {
            return new ColumnCondition(pColumnA, Sql.Operator.NOT_EQUALS, pColumnB);
        }
        public static Condition operator !=(NSmallIntegerColumn pColumnA, SmallIntegerColumn pColumnB) {
            return new ColumnCondition(pColumnA, Sql.Operator.NOT_EQUALS, pColumnB);
        }
        public static Condition operator !=(NSmallIntegerColumn pColumnA, Int16 pValue) {
            return new ColumnCondition(pColumnA, Sql.Operator.NOT_EQUALS, pValue);
        }

        public static Condition operator >(NSmallIntegerColumn pColumnA, NSmallIntegerColumn pColumnB) {
            return new ColumnCondition(pColumnA, Sql.Operator.GREATER_THAN, pColumnB);
        }
        public static Condition operator >(NSmallIntegerColumn pColumnA, SmallIntegerColumn pColumnB) {
            return new ColumnCondition(pColumnA, Sql.Operator.GREATER_THAN, pColumnB);
        }
        public static Condition operator >(NSmallIntegerColumn pColumnA, Int16 pValue) {
            return new ColumnCondition(pColumnA, Sql.Operator.GREATER_THAN, pValue);
        }

        public static Condition operator >=(NSmallIntegerColumn pColumnA, NSmallIntegerColumn pColumnB) {
            return new ColumnCondition(pColumnA, Sql.Operator.GREATER_THAN_OR_EQUAL, pColumnB);
        }
        public static Condition operator >=(NSmallIntegerColumn pColumnA, SmallIntegerColumn pColumnB) {
            return new ColumnCondition(pColumnA, Sql.Operator.GREATER_THAN_OR_EQUAL, pColumnB);
        }
        public static Condition operator >=(NSmallIntegerColumn pColumnA, Int16 pValue) {
            return new ColumnCondition(pColumnA, Sql.Operator.GREATER_THAN_OR_EQUAL, pValue);
        }

        public static Condition operator <(NSmallIntegerColumn pColumnA, NSmallIntegerColumn pColumnB) {
            return new ColumnCondition(pColumnA, Sql.Operator.LESS_THAN, pColumnB);
        }
        public static Condition operator <(NSmallIntegerColumn pColumnA, SmallIntegerColumn pColumnB) {
            return new ColumnCondition(pColumnA, Sql.Operator.LESS_THAN, pColumnB);
        }
        public static Condition operator <(NSmallIntegerColumn pColumnA, Int16 pValue) {
            return new ColumnCondition(pColumnA, Sql.Operator.LESS_THAN, pValue);
        }

        public static Condition operator <=(NSmallIntegerColumn pColumnA, NSmallIntegerColumn pColumnB) {
            return new ColumnCondition(pColumnA, Sql.Operator.LESS_THAN_OR_EQUAL, pColumnB);
        }
        public static Condition operator <=(NSmallIntegerColumn pColumnA, SmallIntegerColumn pColumnB) {
            return new ColumnCondition(pColumnA, Sql.Operator.LESS_THAN_OR_EQUAL, pColumnB);
        }
        public static Condition operator <=(NSmallIntegerColumn pColumnA, Int16 pValue) {
            return new ColumnCondition(pColumnA, Sql.Operator.LESS_THAN_OR_EQUAL, pValue);
        }

        public static NumericCondition<SmallIntegerColumn, NSmallIntegerColumn, Int16> operator +(NSmallIntegerColumn pColumnA, NSmallIntegerColumn pColumnB) {
            return new NumericCondition<SmallIntegerColumn, NSmallIntegerColumn, Int16>(pColumnA, NumericOperator.ADD, pColumnB);
        }
        public static NumericCondition<SmallIntegerColumn, NSmallIntegerColumn, Int16> operator +(NSmallIntegerColumn pColumnA, SmallIntegerColumn pColumnB) {
            return new NumericCondition<SmallIntegerColumn, NSmallIntegerColumn, Int16>(pColumnA, NumericOperator.ADD, pColumnB);
        }
        public static NumericCondition<SmallIntegerColumn, NSmallIntegerColumn, Int16> operator +(NSmallIntegerColumn pColumnA, Int16 pValue) {
            return new NumericCondition<SmallIntegerColumn, NSmallIntegerColumn, Int16>(pColumnA, NumericOperator.ADD, pValue);
        }

        public static NumericCondition<SmallIntegerColumn, NSmallIntegerColumn, Int16> operator -(NSmallIntegerColumn pColumnA, NSmallIntegerColumn pColumnB) {
            return new NumericCondition<SmallIntegerColumn, NSmallIntegerColumn, Int16>(pColumnA, NumericOperator.SUBTRACT, pColumnB);
        }
        public static NumericCondition<SmallIntegerColumn, NSmallIntegerColumn, Int16> operator -(NSmallIntegerColumn pColumnA, SmallIntegerColumn pColumnB) {
            return new NumericCondition<SmallIntegerColumn, NSmallIntegerColumn, Int16>(pColumnA, NumericOperator.SUBTRACT, pColumnB);
        }
        public static NumericCondition<SmallIntegerColumn, NSmallIntegerColumn, Int16> operator -(NSmallIntegerColumn pColumnA, Int16 pValue) {
            return new NumericCondition<SmallIntegerColumn, NSmallIntegerColumn, Int16>(pColumnA, NumericOperator.SUBTRACT, pValue);
        }

        public static NumericCondition<SmallIntegerColumn, NSmallIntegerColumn, Int16> operator /(NSmallIntegerColumn pColumnA, NSmallIntegerColumn pColumnB) {
            return new NumericCondition<SmallIntegerColumn, NSmallIntegerColumn, Int16>(pColumnA, NumericOperator.DIVIDE, pColumnB);
        }
        public static NumericCondition<SmallIntegerColumn, NSmallIntegerColumn, Int16> operator /(NSmallIntegerColumn pColumnA, SmallIntegerColumn pColumnB) {
            return new NumericCondition<SmallIntegerColumn, NSmallIntegerColumn, Int16>(pColumnA, NumericOperator.DIVIDE, pColumnB);
        }
        public static NumericCondition<SmallIntegerColumn, NSmallIntegerColumn, Int16> operator /(NSmallIntegerColumn pColumnA, Int16 pValue) {
            return new NumericCondition<SmallIntegerColumn, NSmallIntegerColumn, Int16>(pColumnA, NumericOperator.DIVIDE, pValue);
        }

        public static NumericCondition<SmallIntegerColumn, NSmallIntegerColumn, Int16> operator *(NSmallIntegerColumn pColumnA, NSmallIntegerColumn pColumnB) {
            return new NumericCondition<SmallIntegerColumn, NSmallIntegerColumn, Int16>(pColumnA, NumericOperator.MULTIPLY, pColumnB);
        }
        public static NumericCondition<SmallIntegerColumn, NSmallIntegerColumn, Int16> operator *(NSmallIntegerColumn pColumnA, SmallIntegerColumn pColumnB) {
            return new NumericCondition<SmallIntegerColumn, NSmallIntegerColumn, Int16>(pColumnA, NumericOperator.MULTIPLY, pColumnB);
        }
        public static NumericCondition<SmallIntegerColumn, NSmallIntegerColumn, Int16> operator *(NSmallIntegerColumn pColumnA, Int16 pValue) {
            return new NumericCondition<SmallIntegerColumn, NSmallIntegerColumn, Int16>(pColumnA, NumericOperator.MULTIPLY, pValue);
        }

        public static NumericCondition<SmallIntegerColumn, NSmallIntegerColumn, Int16> operator %(NSmallIntegerColumn pColumnA, NSmallIntegerColumn pColumnB) {
            return new NumericCondition<SmallIntegerColumn, NSmallIntegerColumn, Int16>(pColumnA, NumericOperator.MODULO, pColumnB);
        }
        public static NumericCondition<SmallIntegerColumn, NSmallIntegerColumn, Int16> operator %(NSmallIntegerColumn pColumnA, SmallIntegerColumn pColumnB) {
            return new NumericCondition<SmallIntegerColumn, NSmallIntegerColumn, Int16>(pColumnA, NumericOperator.MODULO, pColumnB);
        }
        public static NumericCondition<SmallIntegerColumn, NSmallIntegerColumn, Int16> operator %(NSmallIntegerColumn pColumnA, Int16 pValue) {
            return new NumericCondition<SmallIntegerColumn, NSmallIntegerColumn, Int16>(pColumnA, NumericOperator.MODULO, pValue);
        }

        public Condition In(IList<Int16> pIntegerList) {
            return new InCondition<Int16>(this, pIntegerList);
        }
        public Condition NotIn(IList<Int16> pIntegerList) {
            return new NotInCondition<Int16>(this, pIntegerList);
        }

        public Condition In(Interfaces.IExecute pNestedQuery) {
            return new NestedQueryCondition(this, Sql.Operator.IN, pNestedQuery);
        }
        public Condition NotIn(Interfaces.IExecute pNestedQuery) {
            return new NestedQueryCondition(this, Sql.Operator.NOT_IN, pNestedQuery);
        }

        public Condition In(params Int16[] pValues) {
            return new InCondition<Int16>(this, pValues);
        }
        public Condition NotIn(params Int16[] pValues) {
            return new NotInCondition<Int16>(this, pValues);
        }

        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        public override object? GetValue(ADatabase pDatabase, System.Data.Common.DbDataReader pReader, int pColumnIndex) {

            if(pReader.IsDBNull(pColumnIndex)) {
                return null;
            }

            Type dataType = pReader.GetFieldType(pColumnIndex);

            if(dataType != typeof(Int16) && dataType != typeof(Int16?)) {
                throw new Exception($"Row column data is not of the correct type. Expected Int16 or Int16? value instead got '{ dataType.ToString() }'. This probably means that the database and table column data types are not matching. Please run the definition tester to check table columns are of the correct type. Table: '{ Table.TableName }' Column: '{ ColumnName }'");
            }

            return pReader.GetInt16(pColumnIndex);
        }
        public Int16? ValueOf(ARow pRow) {
            return (Int16?)pRow.GetValue(this);
        }
        public void SetValue(ARow pRow, Int16? pValue) {
            pRow.SetValue(this, pValue);
        }

        internal override void TestSetValue(ARow pRow, object? pValue) {
            SetValue(pRow, (Int16?)pValue);
        }
        internal override object? TestGetValue(ARow pRow) {
            return ValueOf(pRow);
        }

        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        public override int GetHashCode() {
            return base.GetHashCode();
        }
        public override bool Equals(object? obj) {
            return base.Equals(obj);
        }
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        public override string? ToString() {
            return base.ToString();
        }
        public override System.Data.DbType DbType {
            get { return System.Data.DbType.Int16; }
        }
        public override object? GetDefaultType() {
            return null;
        }
    }
}