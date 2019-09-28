
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

    /// <summary>
    /// Big Integer Column
    /// This column maps to 8 byte integer fields, float, Long, Int8
    /// </summary>
    public class FloatColumn : ANumericColumn {

        public FloatColumn(ATable pTable, string pColumnName)
            : base(pTable, pColumnName, false, false) {
        }
        public FloatColumn(ATable pTable, string pColumnName, bool pIsPrimaryKey)
            : base(pTable, pColumnName, pIsPrimaryKey, false) {
        }
        public FloatColumn(ATable pTable, string pColumnName, bool pIsPrimaryKey, bool pIsAutoId)
            : base(pTable, pColumnName, pIsPrimaryKey, false) {
            IsAutoId = pIsAutoId;
        }

        public static Condition operator ==(FloatColumn pColumnA, FloatColumn pColumnB) {
            return new ColumnCondition(pColumnA, Sql.Operator.EQUALS, pColumnB);
        }
        public static Condition operator ==(FloatColumn pColumnA, NFloatColumn pColumnB) {
            return new ColumnCondition(pColumnA, Sql.Operator.EQUALS, pColumnB);
        }
        public static Condition operator ==(FloatColumn pColumnA, float pValue) {
            return new ColumnCondition(pColumnA, Sql.Operator.EQUALS, pValue);
        }

        public static Condition operator !=(FloatColumn pColumnA, FloatColumn pColumnB) {
            return new ColumnCondition(pColumnA, Sql.Operator.NOT_EQUALS, pColumnB);
        }
        public static Condition operator !=(FloatColumn pColumnA, NFloatColumn pColumnB) {
            return new ColumnCondition(pColumnA, Sql.Operator.NOT_EQUALS, pColumnB);
        }
        public static Condition operator !=(FloatColumn pColumnA, float pValue) {
            return new ColumnCondition(pColumnA, Sql.Operator.NOT_EQUALS, pValue);
        }

        public static Condition operator >(FloatColumn pColumnA, FloatColumn pColumnB) {
            return new ColumnCondition(pColumnA, Sql.Operator.GREATER_THAN, pColumnB);
        }
        public static Condition operator >(FloatColumn pColumnA, NFloatColumn pColumnB) {
            return new ColumnCondition(pColumnA, Sql.Operator.GREATER_THAN, pColumnB);
        }
        public static Condition operator >(FloatColumn pColumnA, float pValue) {
            return new ColumnCondition(pColumnA, Sql.Operator.GREATER_THAN, pValue);
        }

        public static Condition operator >=(FloatColumn pColumnA, FloatColumn pColumnB) {
            return new ColumnCondition(pColumnA, Sql.Operator.GREATER_THAN_OR_EQUAL, pColumnB);
        }
        public static Condition operator >=(FloatColumn pColumnA, NFloatColumn pColumnB) {
            return new ColumnCondition(pColumnA, Sql.Operator.GREATER_THAN_OR_EQUAL, pColumnB);
        }
        public static Condition operator >=(FloatColumn pColumnA, float pValue) {
            return new ColumnCondition(pColumnA, Sql.Operator.GREATER_THAN_OR_EQUAL, pValue);
        }

        public static Condition operator <(FloatColumn pColumnA, FloatColumn pColumnB) {
            return new ColumnCondition(pColumnA, Sql.Operator.LESS_THAN, pColumnB);
        }
        public static Condition operator <(FloatColumn pColumnA, NFloatColumn pColumnB) {
            return new ColumnCondition(pColumnA, Sql.Operator.LESS_THAN, pColumnB);
        }
        public static Condition operator <(FloatColumn pColumnA, float pValue) {
            return new ColumnCondition(pColumnA, Sql.Operator.LESS_THAN, pValue);
        }

        public static Condition operator <=(FloatColumn pColumnA, FloatColumn pColumnB) {
            return new ColumnCondition(pColumnA, Sql.Operator.LESS_THAN_OR_EQUAL, pColumnB);
        }
        public static Condition operator <=(FloatColumn pColumnA, NFloatColumn pColumnB) {
            return new ColumnCondition(pColumnA, Sql.Operator.LESS_THAN_OR_EQUAL, pColumnB);
        }
        public static Condition operator <=(FloatColumn pColumnA, float pValue) {
            return new ColumnCondition(pColumnA, Sql.Operator.LESS_THAN_OR_EQUAL, pValue);
        }

        public static NumericCondition<FloatColumn, NFloatColumn, float> operator +(FloatColumn pColumnA, FloatColumn pColumnB) {
            return new NumericCondition<FloatColumn, NFloatColumn, float>(pColumnA, NumericOperator.ADD, pColumnB);
        }
        public static NumericCondition<FloatColumn, NFloatColumn, float> operator +(FloatColumn pColumnA, NFloatColumn pColumnB) {
            return new NumericCondition<FloatColumn, NFloatColumn, float>(pColumnA, NumericOperator.ADD, pColumnB);
        }
        public static NumericCondition<FloatColumn, NFloatColumn, float> operator +(FloatColumn pColumnA, float pValue) {
            return new NumericCondition<FloatColumn, NFloatColumn, float>(pColumnA, NumericOperator.ADD, pValue);
        }

        public static NumericCondition<FloatColumn, NFloatColumn, float> operator -(FloatColumn pColumnA, FloatColumn pColumnB) {
            return new NumericCondition<FloatColumn, NFloatColumn, float>(pColumnA, NumericOperator.SUBTRACT, pColumnB);
        }
        public static NumericCondition<FloatColumn, NFloatColumn, float> operator -(FloatColumn pColumnA, NFloatColumn pColumnB) {
            return new NumericCondition<FloatColumn, NFloatColumn, float>(pColumnA, NumericOperator.SUBTRACT, pColumnB);
        }
        public static NumericCondition<FloatColumn, NFloatColumn, float> operator -(FloatColumn pColumnA, float pValue) {
            return new NumericCondition<FloatColumn, NFloatColumn, float>(pColumnA, NumericOperator.SUBTRACT, pValue);
        }

        public static NumericCondition<FloatColumn, NFloatColumn, float> operator /(FloatColumn pColumnA, FloatColumn pColumnB) {
            return new NumericCondition<FloatColumn, NFloatColumn, float>(pColumnA, NumericOperator.DIVIDE, pColumnB);
        }
        public static NumericCondition<FloatColumn, NFloatColumn, float> operator /(FloatColumn pColumnA, NFloatColumn pColumnB) {
            return new NumericCondition<FloatColumn, NFloatColumn, float>(pColumnA, NumericOperator.DIVIDE, pColumnB);
        }
        public static NumericCondition<FloatColumn, NFloatColumn, float> operator /(FloatColumn pColumnA, float pValue) {
            return new NumericCondition<FloatColumn, NFloatColumn, float>(pColumnA, NumericOperator.DIVIDE, pValue);
        }

        public static NumericCondition<FloatColumn, NFloatColumn, float> operator *(FloatColumn pColumnA, FloatColumn pColumnB) {
            return new NumericCondition<FloatColumn, NFloatColumn, float>(pColumnA, NumericOperator.MULTIPLY, pColumnB);
        }
        public static NumericCondition<FloatColumn, NFloatColumn, float> operator *(FloatColumn pColumnA, NFloatColumn pColumnB) {
            return new NumericCondition<FloatColumn, NFloatColumn, float>(pColumnA, NumericOperator.MULTIPLY, pColumnB);
        }
        public static NumericCondition<FloatColumn, NFloatColumn, float> operator *(FloatColumn pColumnA, float pValue) {
            return new NumericCondition<FloatColumn, NFloatColumn, float>(pColumnA, NumericOperator.MULTIPLY, pValue);
        }

        public static NumericCondition<FloatColumn, NFloatColumn, float> operator %(FloatColumn pColumnA, FloatColumn pColumnB) {
            return new NumericCondition<FloatColumn, NFloatColumn, float>(pColumnA, NumericOperator.MODULO, pColumnB);
        }
        public static NumericCondition<FloatColumn, NFloatColumn, float> operator %(FloatColumn pColumnA, NFloatColumn pColumnB) {
            return new NumericCondition<FloatColumn, NFloatColumn, float>(pColumnA, NumericOperator.MODULO, pColumnB);
        }
        public static NumericCondition<FloatColumn, NFloatColumn, float> operator %(FloatColumn pColumnA, float pValue) {
            return new NumericCondition<FloatColumn, NFloatColumn, float>(pColumnA, NumericOperator.MODULO, pValue);
        }

        public Condition In(IList<float> pList) {
            return new InCondition<float>(this, pList);
        }
        public Condition NotIn(IList<float> pList) {
            return new NotInCondition<float>(this, pList);
        }

        public Condition In(Interfaces.IExecute pNestedQuery) {
            return new NestedQueryCondition(this, Sql.Operator.IN, pNestedQuery);
        }
        public Condition NotIn(Interfaces.IExecute pNestedQuery) {
            return new NestedQueryCondition(this, Sql.Operator.NOT_IN, pNestedQuery);
        }

        public Condition In(params float[] pValues) {
            return new InCondition<float>(this, pValues);
        }
        public Condition NotIn(params float[] pValues) {
            return new NotInCondition<float>(this, pValues);
        }

        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        public override object GetValue(ADatabase pDatabase, System.Data.Common.DbDataReader pReader, int pColumnIndex) {

            Type dataType = pReader.GetFieldType(pColumnIndex);

            if(dataType != typeof(float)) {
                throw new Exception($"Row column data is not of the correct type. Expected float value instead got '{ dataType.ToString() }'. This probably means that the database and table column data types are not matching. Please run the definition tester to check table columns are of the correct type. Table: '{ Table.TableName }' Column: '{ ColumnName }'");
            }

            return pReader.GetFloat(pColumnIndex);
        }
        public float ValueOf(ARow pRow) {
            return (float)pRow.GetValue(this)!;
        }
        public void SetValue(ARow pRow, float pValue) {
            pRow.SetValue(this, pValue);
        }

        internal override void TestSetValue(ARow pRow, object? pValue) {
            SetValue(pRow, (float)pValue!);
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
            get { return System.Data.DbType.Single; }
        }
        public override object? GetDefaultType() {
            return (float)0;
        }
    }
}