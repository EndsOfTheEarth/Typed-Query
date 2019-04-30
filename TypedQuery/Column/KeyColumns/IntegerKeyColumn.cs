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

using Sql.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sql.Column {

    public class IntegerKeyColumn<TABLE> : ANumericColumn {

        public IntegerKeyColumn(ATable pTable, string pColumnName)
            : base(pTable, pColumnName, false, false) {
        }
        public IntegerKeyColumn(ATable pTable, string pColumnName, bool pIsPrimaryKey)
            : base(pTable, pColumnName, pIsPrimaryKey, false) {
        }

        public IntegerKeyColumn(ATable pTable, string pColumnName, bool pIsPrimaryKey, bool pIsAutoId)
            : base(pTable, pColumnName, pIsPrimaryKey, false) {
            IsAutoId = pIsAutoId;
        }

        public static Condition operator ==(IntegerKeyColumn<TABLE> pColumnA, IntegerKeyColumn<TABLE> pColumnB) {
            return new ColumnCondition(pColumnA, Sql.Operator.EQUALS, pColumnB);
        }
        public static Condition operator ==(IntegerKeyColumn<TABLE> pColumnA, NIntegerKeyColumn<TABLE> pColumnB) {
            return new ColumnCondition(pColumnA, Sql.Operator.EQUALS, pColumnB);
        }
        public static Condition operator ==(IntegerKeyColumn<TABLE> pColumnA, Int32Key<TABLE> pValue) {
            return new ColumnCondition(pColumnA, Sql.Operator.EQUALS, pValue.Value);
        }

        public static Condition operator !=(IntegerKeyColumn<TABLE> pColumnA, IntegerKeyColumn<TABLE> pColumnB) {
            return new ColumnCondition(pColumnA, Sql.Operator.NOT_EQUALS, pColumnB);
        }
        public static Condition operator !=(IntegerKeyColumn<TABLE> pColumnA, NIntegerKeyColumn<TABLE> pColumnB) {
            return new ColumnCondition(pColumnA, Sql.Operator.NOT_EQUALS, pColumnB);
        }
        public static Condition operator !=(IntegerKeyColumn<TABLE> pColumnA, Int32Key<TABLE> pValue) {
            return new ColumnCondition(pColumnA, Sql.Operator.NOT_EQUALS, pValue.Value);
        }

        public static Condition operator >(IntegerKeyColumn<TABLE> pColumnA, IntegerKeyColumn<TABLE> pColumnB) {
            return new ColumnCondition(pColumnA, Sql.Operator.GREATER_THAN, pColumnB);
        }
        public static Condition operator >(IntegerKeyColumn<TABLE> pColumnA, NIntegerKeyColumn<TABLE> pColumnB) {
            return new ColumnCondition(pColumnA, Sql.Operator.GREATER_THAN, pColumnB);
        }
        public static Condition operator >(IntegerKeyColumn<TABLE> pColumnA, Int32Key<TABLE> pValue) {
            return new ColumnCondition(pColumnA, Sql.Operator.GREATER_THAN, pValue.Value);
        }

        public static Condition operator >=(IntegerKeyColumn<TABLE> pColumnA, IntegerKeyColumn<TABLE> pColumnB) {
            return new ColumnCondition(pColumnA, Sql.Operator.GREATER_THAN_OR_EQUAL, pColumnB);
        }
        public static Condition operator >=(IntegerKeyColumn<TABLE> pColumnA, NIntegerKeyColumn<TABLE> pColumnB) {
            return new ColumnCondition(pColumnA, Sql.Operator.GREATER_THAN_OR_EQUAL, pColumnB);
        }
        public static Condition operator >=(IntegerKeyColumn<TABLE> pColumnA, Int32Key<TABLE> pValue) {
            return new ColumnCondition(pColumnA, Sql.Operator.GREATER_THAN_OR_EQUAL, pValue.Value);
        }

        public static Condition operator <(IntegerKeyColumn<TABLE> pColumnA, IntegerKeyColumn<TABLE> pColumnB) {
            return new ColumnCondition(pColumnA, Sql.Operator.LESS_THAN, pColumnB);
        }
        public static Condition operator <(IntegerKeyColumn<TABLE> pColumnA, NIntegerKeyColumn<TABLE> pColumnB) {
            return new ColumnCondition(pColumnA, Sql.Operator.LESS_THAN, pColumnB);
        }
        public static Condition operator <(IntegerKeyColumn<TABLE> pColumnA, Int32Key<TABLE> pValue) {
            return new ColumnCondition(pColumnA, Sql.Operator.LESS_THAN, pValue.Value);
        }

        public static Condition operator <=(IntegerKeyColumn<TABLE> pColumnA, IntegerKeyColumn<TABLE> pColumnB) {
            return new ColumnCondition(pColumnA, Sql.Operator.LESS_THAN_OR_EQUAL, pColumnB);
        }
        public static Condition operator <=(IntegerKeyColumn<TABLE> pColumnA, NIntegerKeyColumn<TABLE> pColumnB) {
            return new ColumnCondition(pColumnA, Sql.Operator.LESS_THAN_OR_EQUAL, pColumnB);
        }
        public static Condition operator <=(IntegerKeyColumn<TABLE> pColumnA, Int32Key<TABLE> pValue) {
            return new ColumnCondition(pColumnA, Sql.Operator.LESS_THAN_OR_EQUAL, pValue.Value);
        }

        public static NumericCondition<IntegerKeyColumn<TABLE>, NIntegerKeyColumn<TABLE>, int> operator +(IntegerKeyColumn<TABLE> pColumnA, IntegerKeyColumn<TABLE> pColumnB) {
            return new NumericCondition<IntegerKeyColumn<TABLE>, NIntegerKeyColumn<TABLE>, int>(pColumnA, NumericOperator.ADD, pColumnB);
        }
        public static NumericCondition<IntegerKeyColumn<TABLE>, NIntegerKeyColumn<TABLE>, int> operator +(IntegerKeyColumn<TABLE> pColumnA, NIntegerKeyColumn<TABLE> pColumnB) {
            return new NumericCondition<IntegerKeyColumn<TABLE>, NIntegerKeyColumn<TABLE>, int>(pColumnA, NumericOperator.ADD, pColumnB);
        }
        public static NumericCondition<IntegerKeyColumn<TABLE>, NIntegerKeyColumn<TABLE>, int> operator +(IntegerKeyColumn<TABLE> pColumnA, Int32Key<TABLE> pValue) {
            return new NumericCondition<IntegerKeyColumn<TABLE>, NIntegerKeyColumn<TABLE>, int>(pColumnA, NumericOperator.ADD, pValue.Value);
        }

        public static NumericCondition<IntegerKeyColumn<TABLE>, NIntegerKeyColumn<TABLE>, int> operator -(IntegerKeyColumn<TABLE> pColumnA, IntegerKeyColumn<TABLE> pColumnB) {
            return new NumericCondition<IntegerKeyColumn<TABLE>, NIntegerKeyColumn<TABLE>, int>(pColumnA, NumericOperator.SUBTRACT, pColumnB);
        }
        public static NumericCondition<IntegerKeyColumn<TABLE>, NIntegerKeyColumn<TABLE>, int> operator -(IntegerKeyColumn<TABLE> pColumnA, NIntegerKeyColumn<TABLE> pColumnB) {
            return new NumericCondition<IntegerKeyColumn<TABLE>, NIntegerKeyColumn<TABLE>, int>(pColumnA, NumericOperator.SUBTRACT, pColumnB);
        }
        public static NumericCondition<IntegerKeyColumn<TABLE>, NIntegerKeyColumn<TABLE>, int> operator -(IntegerKeyColumn<TABLE> pColumnA, Int32Key<TABLE> pValue) {
            return new NumericCondition<IntegerKeyColumn<TABLE>, NIntegerKeyColumn<TABLE>, int>(pColumnA, NumericOperator.SUBTRACT, pValue.Value);
        }

        public static NumericCondition<IntegerKeyColumn<TABLE>, NIntegerKeyColumn<TABLE>, int> operator /(IntegerKeyColumn<TABLE> pColumnA, IntegerKeyColumn<TABLE> pColumnB) {
            return new NumericCondition<IntegerKeyColumn<TABLE>, NIntegerKeyColumn<TABLE>, int>(pColumnA, NumericOperator.DIVIDE, pColumnB);
        }
        public static NumericCondition<IntegerKeyColumn<TABLE>, NIntegerKeyColumn<TABLE>, int> operator /(IntegerKeyColumn<TABLE> pColumnA, NIntegerKeyColumn<TABLE> pColumnB) {
            return new NumericCondition<IntegerKeyColumn<TABLE>, NIntegerKeyColumn<TABLE>, int>(pColumnA, NumericOperator.DIVIDE, pColumnB);
        }
        public static NumericCondition<IntegerKeyColumn<TABLE>, NIntegerKeyColumn<TABLE>, int> operator /(IntegerKeyColumn<TABLE> pColumnA, Int32Key<TABLE> pValue) {
            return new NumericCondition<IntegerKeyColumn<TABLE>, NIntegerKeyColumn<TABLE>, int>(pColumnA, NumericOperator.DIVIDE, pValue.Value);
        }

        public static NumericCondition<IntegerKeyColumn<TABLE>, NIntegerKeyColumn<TABLE>, int> operator *(IntegerKeyColumn<TABLE> pColumnA, IntegerKeyColumn<TABLE> pColumnB) {
            return new NumericCondition<IntegerKeyColumn<TABLE>, NIntegerKeyColumn<TABLE>, int>(pColumnA, NumericOperator.MULTIPLY, pColumnB);
        }
        public static NumericCondition<IntegerKeyColumn<TABLE>, NIntegerKeyColumn<TABLE>, int> operator *(IntegerKeyColumn<TABLE> pColumnA, NIntegerKeyColumn<TABLE> pColumnB) {
            return new NumericCondition<IntegerKeyColumn<TABLE>, NIntegerKeyColumn<TABLE>, int>(pColumnA, NumericOperator.MULTIPLY, pColumnB);
        }
        public static NumericCondition<IntegerKeyColumn<TABLE>, NIntegerKeyColumn<TABLE>, int> operator *(IntegerKeyColumn<TABLE> pColumnA, Int32Key<TABLE> pValue) {
            return new NumericCondition<IntegerKeyColumn<TABLE>, NIntegerKeyColumn<TABLE>, int>(pColumnA, NumericOperator.MULTIPLY, pValue.Value);
        }

        public static NumericCondition<IntegerKeyColumn<TABLE>, NIntegerKeyColumn<TABLE>, int> operator %(IntegerKeyColumn<TABLE> pColumnA, IntegerKeyColumn<TABLE> pColumnB) {
            return new NumericCondition<IntegerKeyColumn<TABLE>, NIntegerKeyColumn<TABLE>, int>(pColumnA, NumericOperator.MODULO, pColumnB);
        }
        public static NumericCondition<IntegerKeyColumn<TABLE>, NIntegerKeyColumn<TABLE>, int> operator %(IntegerKeyColumn<TABLE> pColumnA, NIntegerKeyColumn<TABLE> pColumnB) {
            return new NumericCondition<IntegerKeyColumn<TABLE>, NIntegerKeyColumn<TABLE>, int>(pColumnA, NumericOperator.MODULO, pColumnB);
        }
        public static NumericCondition<IntegerKeyColumn<TABLE>, NIntegerKeyColumn<TABLE>, int> operator %(IntegerKeyColumn<TABLE> pColumnA, Int32Key<TABLE> pValue) {
            return new NumericCondition<IntegerKeyColumn<TABLE>, NIntegerKeyColumn<TABLE>, int>(pColumnA, NumericOperator.MODULO, pValue.Value);
        }

        public Condition In(IList<Int32Key<TABLE>> pIntegerList) {

            List<int> list = new List<int>(pIntegerList.Count);

            foreach(Int32Key<TABLE> value in pIntegerList) {
                list.Add(value.Value);
            }
            return new InCondition<int>(this, list);
        }
        public Condition NotIn(IList<Int32Key<TABLE>> pIntegerList) {

            List<int> list = new List<int>(pIntegerList.Count);

            foreach(Int32Key<TABLE> value in pIntegerList) {
                list.Add(value.Value);
            }
            return new NotInCondition<int>(this, list);
        }

        public Condition In(Interfaces.IExecute pNestedQuery) {
            return new NestedQueryCondition(this, Sql.Operator.IN, pNestedQuery);
        }
        public Condition NotIn(Interfaces.IExecute pNestedQuery) {
            return new NestedQueryCondition(this, Sql.Operator.NOT_IN, pNestedQuery);
        }

        public Condition In(params Int32Key<TABLE>[] pValues) {

            List<int> list = new List<int>(pValues.Length);

            foreach(Int32Key<TABLE> value in pValues) {
                list.Add(value.Value);
            }
            return new InCondition<int>(this, list);
        }
        public Condition NotIn(params Int32Key<TABLE>[] pValues) {

            List<int> list = new List<int>(pValues.Length);

            foreach(Int32Key<TABLE> value in pValues) {
                list.Add(value.Value);
            }
            return new NotInCondition<int>(this, list);
        }

        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        public override object GetValue(ADatabase pDatabase, System.Data.Common.DbDataReader pReader, int pColumnIndex) {

            Type dataType = pReader.GetFieldType(pColumnIndex);

            int intValue;

            if(IsAutoId) {

                if(dataType != typeof(Int32) && dataType != typeof(decimal)) {
                    throw new Exception($"Row column data is not of the correct type. Expected Int32 or decimal value instead got '{ dataType.ToString() }'. This probably means that the database and table column data types are not matching. Please run the definition tester to check table columns are of the correct type. Table: '{ Table.TableName }' Column: '{ ColumnName }'");
                }

                object value = pReader.GetValue(pColumnIndex);

                if(value is decimal) {  //Queries like SELECT @@IDENTITY return decimals on integer columns so we need to handle this case. Not the best solution.
                    intValue = (int)(decimal)value;
                }
                else {
                    intValue = (int)value;  //Should give a cast exception if not an int
                }
            }
            else {

                if(dataType != typeof(Int32)) {
                    throw new Exception($"Row column data is not of the correct type. Expected Int32 value instead got '{ dataType.ToString() }'. This probably means that the database and table column data types are not matching. Please run the definition tester to check table columns are of the correct type. Table: '{ Table.TableName }' Column: '{ ColumnName }'");
                }

                intValue = pReader.GetInt32(pColumnIndex);
            }
            return intValue;
        }
        public Int32Key<TABLE> ValueOf(ARow pRow) {
            return new Int32Key<TABLE>((int)pRow.GetValue(this));
        }
        public void SetValue(ARow pRow, Int32Key<TABLE> pValue) {
            pRow.SetValue(this, pValue.Value);
        }

        internal override void TestSetValue(ARow pRow, object pValue) {
            SetValue(pRow, (Int32Key<TABLE>)pValue);
        }
        internal override object TestGetValue(ARow pRow) {
            return ValueOf(pRow);
        }

        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        public override int GetHashCode() {
            return base.GetHashCode();
        }
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        public override bool Equals(object obj) {
            return base.Equals(obj);
        }
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        public override string ToString() {
            return base.ToString();
        }
        public override System.Data.DbType DbType {
            get { return System.Data.DbType.Int32; }
        }
        public override object GetDefaultType() {
            return new Int32Key<TABLE>(0);
        }
    }
}