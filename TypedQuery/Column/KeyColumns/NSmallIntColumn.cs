
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

using Sql.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sql.Column {

    public class NSmallIntegerKeyColumn<TABLE> : ANumericColumn {

        public NSmallIntegerKeyColumn(ATable pTable, string pColumnName)
            : base(pTable, pColumnName, false, true) {
        }
        public NSmallIntegerKeyColumn(ATable pTable, string pColumnName, bool pIsPrimaryKey)
            : base(pTable, pColumnName, pIsPrimaryKey, true) {
        }

        public static Condition operator ==(NSmallIntegerKeyColumn<TABLE> pColumnA, NSmallIntegerKeyColumn<TABLE> pColumnB) {
            return new ColumnCondition(pColumnA, Sql.Operator.EQUALS, pColumnB);
        }
        public static Condition operator ==(NSmallIntegerKeyColumn<TABLE> pColumnA, SmallIntegerKeyColumn<TABLE> pColumnB) {
            return new ColumnCondition(pColumnA, Sql.Operator.EQUALS, pColumnB);
        }
        public static Condition operator ==(NSmallIntegerKeyColumn<TABLE> pColumnA, Int16Key<TABLE> pValue) {
            return new ColumnCondition(pColumnA, Sql.Operator.EQUALS, pValue.Value);
        }

        public static Condition operator !=(NSmallIntegerKeyColumn<TABLE> pColumnA, NSmallIntegerKeyColumn<TABLE> pColumnB) {
            return new ColumnCondition(pColumnA, Sql.Operator.NOT_EQUALS, pColumnB);
        }
        public static Condition operator !=(NSmallIntegerKeyColumn<TABLE> pColumnA, SmallIntegerKeyColumn<TABLE> pColumnB) {
            return new ColumnCondition(pColumnA, Sql.Operator.NOT_EQUALS, pColumnB);
        }
        public static Condition operator !=(NSmallIntegerKeyColumn<TABLE> pColumnA, Int16Key<TABLE> pValue) {
            return new ColumnCondition(pColumnA, Sql.Operator.NOT_EQUALS, pValue.Value);
        }

        public static Condition operator >(NSmallIntegerKeyColumn<TABLE> pColumnA, NSmallIntegerKeyColumn<TABLE> pColumnB) {
            return new ColumnCondition(pColumnA, Sql.Operator.GREATER_THAN, pColumnB);
        }
        public static Condition operator >(NSmallIntegerKeyColumn<TABLE> pColumnA, SmallIntegerKeyColumn<TABLE> pColumnB) {
            return new ColumnCondition(pColumnA, Sql.Operator.GREATER_THAN, pColumnB);
        }
        public static Condition operator >(NSmallIntegerKeyColumn<TABLE> pColumnA, Int16Key<TABLE> pValue) {
            return new ColumnCondition(pColumnA, Sql.Operator.GREATER_THAN, pValue.Value);
        }

        public static Condition operator >=(NSmallIntegerKeyColumn<TABLE> pColumnA, NSmallIntegerKeyColumn<TABLE> pColumnB) {
            return new ColumnCondition(pColumnA, Sql.Operator.GREATER_THAN_OR_EQUAL, pColumnB);
        }
        public static Condition operator >=(NSmallIntegerKeyColumn<TABLE> pColumnA, SmallIntegerKeyColumn<TABLE> pColumnB) {
            return new ColumnCondition(pColumnA, Sql.Operator.GREATER_THAN_OR_EQUAL, pColumnB);
        }
        public static Condition operator >=(NSmallIntegerKeyColumn<TABLE> pColumnA, Int16Key<TABLE> pValue) {
            return new ColumnCondition(pColumnA, Sql.Operator.GREATER_THAN_OR_EQUAL, pValue.Value);
        }

        public static Condition operator <(NSmallIntegerKeyColumn<TABLE> pColumnA, NSmallIntegerKeyColumn<TABLE> pColumnB) {
            return new ColumnCondition(pColumnA, Sql.Operator.LESS_THAN, pColumnB);
        }
        public static Condition operator <(NSmallIntegerKeyColumn<TABLE> pColumnA, SmallIntegerKeyColumn<TABLE> pColumnB) {
            return new ColumnCondition(pColumnA, Sql.Operator.LESS_THAN, pColumnB);
        }
        public static Condition operator <(NSmallIntegerKeyColumn<TABLE> pColumnA, Int16Key<TABLE> pValue) {
            return new ColumnCondition(pColumnA, Sql.Operator.LESS_THAN, pValue.Value);
        }

        public static Condition operator <=(NSmallIntegerKeyColumn<TABLE> pColumnA, NSmallIntegerKeyColumn<TABLE> pColumnB) {
            return new ColumnCondition(pColumnA, Sql.Operator.LESS_THAN_OR_EQUAL, pColumnB);
        }
        public static Condition operator <=(NSmallIntegerKeyColumn<TABLE> pColumnA, SmallIntegerKeyColumn<TABLE> pColumnB) {
            return new ColumnCondition(pColumnA, Sql.Operator.LESS_THAN_OR_EQUAL, pColumnB);
        }
        public static Condition operator <=(NSmallIntegerKeyColumn<TABLE> pColumnA, Int16Key<TABLE> pValue) {
            return new ColumnCondition(pColumnA, Sql.Operator.LESS_THAN_OR_EQUAL, pValue.Value);
        }

        public static NumericCondition<SmallIntegerKeyColumn<TABLE>, NSmallIntegerKeyColumn<TABLE>, Int16> operator +(NSmallIntegerKeyColumn<TABLE> pColumnA, NSmallIntegerKeyColumn<TABLE> pColumnB) {
            return new NumericCondition<SmallIntegerKeyColumn<TABLE>, NSmallIntegerKeyColumn<TABLE>, Int16>(pColumnA, NumericOperator.ADD, pColumnB);
        }
        public static NumericCondition<SmallIntegerKeyColumn<TABLE>, NSmallIntegerKeyColumn<TABLE>, Int16> operator +(NSmallIntegerKeyColumn<TABLE> pColumnA, SmallIntegerKeyColumn<TABLE> pColumnB) {
            return new NumericCondition<SmallIntegerKeyColumn<TABLE>, NSmallIntegerKeyColumn<TABLE>, Int16>(pColumnA, NumericOperator.ADD, pColumnB);
        }
        public static NumericCondition<SmallIntegerKeyColumn<TABLE>, NSmallIntegerKeyColumn<TABLE>, Int16> operator +(NSmallIntegerKeyColumn<TABLE> pColumnA, Int16Key<TABLE> pValue) {
            return new NumericCondition<SmallIntegerKeyColumn<TABLE>, NSmallIntegerKeyColumn<TABLE>, Int16>(pColumnA, NumericOperator.ADD, pValue.Value);
        }

        public static NumericCondition<SmallIntegerKeyColumn<TABLE>, NSmallIntegerKeyColumn<TABLE>, Int16> operator -(NSmallIntegerKeyColumn<TABLE> pColumnA, NSmallIntegerKeyColumn<TABLE> pColumnB) {
            return new NumericCondition<SmallIntegerKeyColumn<TABLE>, NSmallIntegerKeyColumn<TABLE>, Int16>(pColumnA, NumericOperator.SUBTRACT, pColumnB);
        }
        public static NumericCondition<SmallIntegerKeyColumn<TABLE>, NSmallIntegerKeyColumn<TABLE>, Int16> operator -(NSmallIntegerKeyColumn<TABLE> pColumnA, SmallIntegerKeyColumn<TABLE> pColumnB) {
            return new NumericCondition<SmallIntegerKeyColumn<TABLE>, NSmallIntegerKeyColumn<TABLE>, Int16>(pColumnA, NumericOperator.SUBTRACT, pColumnB);
        }
        public static NumericCondition<SmallIntegerKeyColumn<TABLE>, NSmallIntegerKeyColumn<TABLE>, Int16> operator -(NSmallIntegerKeyColumn<TABLE> pColumnA, Int16Key<TABLE> pValue) {
            return new NumericCondition<SmallIntegerKeyColumn<TABLE>, NSmallIntegerKeyColumn<TABLE>, Int16>(pColumnA, NumericOperator.SUBTRACT, pValue.Value);
        }

        public static NumericCondition<SmallIntegerKeyColumn<TABLE>, NSmallIntegerKeyColumn<TABLE>, Int16> operator /(NSmallIntegerKeyColumn<TABLE> pColumnA, NSmallIntegerKeyColumn<TABLE> pColumnB) {
            return new NumericCondition<SmallIntegerKeyColumn<TABLE>, NSmallIntegerKeyColumn<TABLE>, Int16>(pColumnA, NumericOperator.DIVIDE, pColumnB);
        }
        public static NumericCondition<SmallIntegerKeyColumn<TABLE>, NSmallIntegerKeyColumn<TABLE>, Int16> operator /(NSmallIntegerKeyColumn<TABLE> pColumnA, SmallIntegerKeyColumn<TABLE> pColumnB) {
            return new NumericCondition<SmallIntegerKeyColumn<TABLE>, NSmallIntegerKeyColumn<TABLE>, Int16>(pColumnA, NumericOperator.DIVIDE, pColumnB);
        }
        public static NumericCondition<SmallIntegerKeyColumn<TABLE>, NSmallIntegerKeyColumn<TABLE>, Int16> operator /(NSmallIntegerKeyColumn<TABLE> pColumnA, Int16Key<TABLE> pValue) {
            return new NumericCondition<SmallIntegerKeyColumn<TABLE>, NSmallIntegerKeyColumn<TABLE>, Int16>(pColumnA, NumericOperator.DIVIDE, pValue.Value);
        }

        public static NumericCondition<SmallIntegerKeyColumn<TABLE>, NSmallIntegerKeyColumn<TABLE>, Int16> operator *(NSmallIntegerKeyColumn<TABLE> pColumnA, NSmallIntegerKeyColumn<TABLE> pColumnB) {
            return new NumericCondition<SmallIntegerKeyColumn<TABLE>, NSmallIntegerKeyColumn<TABLE>, Int16>(pColumnA, NumericOperator.MULTIPLY, pColumnB);
        }
        public static NumericCondition<SmallIntegerKeyColumn<TABLE>, NSmallIntegerKeyColumn<TABLE>, Int16> operator *(NSmallIntegerKeyColumn<TABLE> pColumnA, SmallIntegerKeyColumn<TABLE> pColumnB) {
            return new NumericCondition<SmallIntegerKeyColumn<TABLE>, NSmallIntegerKeyColumn<TABLE>, Int16>(pColumnA, NumericOperator.MULTIPLY, pColumnB);
        }
        public static NumericCondition<SmallIntegerKeyColumn<TABLE>, NSmallIntegerKeyColumn<TABLE>, Int16> operator *(NSmallIntegerKeyColumn<TABLE> pColumnA, Int16Key<TABLE> pValue) {
            return new NumericCondition<SmallIntegerKeyColumn<TABLE>, NSmallIntegerKeyColumn<TABLE>, Int16>(pColumnA, NumericOperator.MULTIPLY, pValue.Value);
        }

        public static NumericCondition<SmallIntegerKeyColumn<TABLE>, NSmallIntegerKeyColumn<TABLE>, Int16> operator %(NSmallIntegerKeyColumn<TABLE> pColumnA, NSmallIntegerKeyColumn<TABLE> pColumnB) {
            return new NumericCondition<SmallIntegerKeyColumn<TABLE>, NSmallIntegerKeyColumn<TABLE>, Int16>(pColumnA, NumericOperator.MODULO, pColumnB);
        }
        public static NumericCondition<SmallIntegerKeyColumn<TABLE>, NSmallIntegerKeyColumn<TABLE>, Int16> operator %(NSmallIntegerKeyColumn<TABLE> pColumnA, SmallIntegerKeyColumn<TABLE> pColumnB) {
            return new NumericCondition<SmallIntegerKeyColumn<TABLE>, NSmallIntegerKeyColumn<TABLE>, Int16>(pColumnA, NumericOperator.MODULO, pColumnB);
        }
        public static NumericCondition<SmallIntegerKeyColumn<TABLE>, NSmallIntegerKeyColumn<TABLE>, Int16> operator %(NSmallIntegerKeyColumn<TABLE> pColumnA, Int16Key<TABLE> pValue) {
            return new NumericCondition<SmallIntegerKeyColumn<TABLE>, NSmallIntegerKeyColumn<TABLE>, Int16>(pColumnA, NumericOperator.MODULO, pValue.Value);
        }

        public Condition In(IList<Int16Key<TABLE>> pIntegerList) {

            List<short> list = new List<short>(pIntegerList.Count);

            foreach(Int16Key<TABLE> value in pIntegerList) {
                list.Add(value.Value);
            }
            return new InCondition<short>(this, list);
        }
        public Condition NotIn(IList<Int16Key<TABLE>> pIntegerList) {

            List<short> list = new List<short>(pIntegerList.Count);

            foreach(Int16Key<TABLE> value in pIntegerList) {
                list.Add(value.Value);
            }
            return new NotInCondition<short>(this, list);
        }

        public Condition In(Interfaces.IExecute pNestedQuery) {
            return new NestedQueryCondition(this, Sql.Operator.IN, pNestedQuery);
        }
        public Condition NotIn(Interfaces.IExecute pNestedQuery) {
            return new NestedQueryCondition(this, Sql.Operator.NOT_IN, pNestedQuery);
        }

        public Condition In(params Int16Key<TABLE>[] pValues) {

            List<short> list = new List<short>(pValues.Length);

            foreach(Int16Key<TABLE> value in pValues) {
                list.Add(value.Value);
            }
            return new InCondition<short>(this, list);
        }
        public Condition NotIn(params Int16Key<TABLE>[] pValues) {

            List<short> list = new List<short>(pValues.Length);

            foreach(Int16Key<TABLE> value in pValues) {
                list.Add(value.Value);
            }
            return new NotInCondition<short>(this, list);
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
        public Int16Key<TABLE>? ValueOf(ARow pRow) {
            object? value = pRow.GetValue(this);
            return value != null ? new Int16Key<TABLE>((short)value) : (Int16Key<TABLE>?)null;
        }
        public void SetValue(ARow pRow, Int16Key<TABLE>? pValue) {
            pRow.SetValue(this, pValue != null ? (short?)pValue.Value : null);
        }

        internal override void TestSetValue(ARow pRow, object? pValue) {
            SetValue(pRow, (Int16Key<TABLE>?)pValue);
        }
        internal override object? TestGetValue(ARow pRow) {
            return ValueOf(pRow);
        }

        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        public override int GetHashCode() {
            return base.GetHashCode();
        }
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
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