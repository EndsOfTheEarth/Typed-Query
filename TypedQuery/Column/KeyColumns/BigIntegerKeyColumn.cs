
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

using Sql.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sql.Column {

	/// <summary>
	/// Big Integer Column
	/// This column maps to 8 byte integer fields, Int64, Long, Int8
	/// </summary>
	public class BigIntegerKeyColumn<TABLE> : ANumericColumn {
		
		public BigIntegerKeyColumn(ATable pTable, string pColumnName)
			: base(pTable, pColumnName, false, false) {
		}
		public BigIntegerKeyColumn(ATable pTable, string pColumnName, bool pIsPrimaryKey)
			: base(pTable, pColumnName, pIsPrimaryKey, false) {
		}
		public BigIntegerKeyColumn(ATable pTable, string pColumnName, bool pIsPrimaryKey, bool pIsAutoId)
			: base(pTable, pColumnName, pIsPrimaryKey, false) {
			IsAutoId = pIsAutoId;
		}
		
		public static Condition operator ==(BigIntegerKeyColumn<TABLE> pColumnA, BigIntegerKeyColumn<TABLE> pColumnB) {
			return new ColumnCondition(pColumnA, Sql.Operator.EQUALS, pColumnB);
		}
		public static Condition operator ==(BigIntegerKeyColumn<TABLE> pColumnA, NBigIntegerKeyColumn<TABLE> pColumnB) {
			return new ColumnCondition(pColumnA, Sql.Operator.EQUALS, pColumnB);
		}
		public static Condition operator ==(BigIntegerKeyColumn<TABLE> pColumnA, Int64Key<TABLE> pValue) {
			return new ColumnCondition(pColumnA, Sql.Operator.EQUALS, pValue);
		}

		public static Condition operator !=(BigIntegerKeyColumn<TABLE> pColumnA, BigIntegerKeyColumn<TABLE> pColumnB) {
			return new ColumnCondition(pColumnA, Sql.Operator.NOT_EQUALS, pColumnB);
		}
		public static Condition operator !=(BigIntegerKeyColumn<TABLE> pColumnA, NBigIntegerKeyColumn<TABLE> pColumnB) {
			return new ColumnCondition(pColumnA, Sql.Operator.NOT_EQUALS, pColumnB);
		}
		public static Condition operator !=(BigIntegerKeyColumn<TABLE> pColumnA, Int64Key<TABLE> pValue) {
			return new ColumnCondition(pColumnA, Sql.Operator.NOT_EQUALS, pValue);
		}

		public static Condition operator >(BigIntegerKeyColumn<TABLE> pColumnA, BigIntegerKeyColumn<TABLE> pColumnB) {
			return new ColumnCondition(pColumnA, Sql.Operator.GREATER_THAN, pColumnB);
		}
		public static Condition operator >(BigIntegerKeyColumn<TABLE> pColumnA, NBigIntegerKeyColumn<TABLE> pColumnB) {
			return new ColumnCondition(pColumnA, Sql.Operator.GREATER_THAN, pColumnB);
		}
		public static Condition operator >(BigIntegerKeyColumn<TABLE> pColumnA, Int64Key<TABLE> pValue) {
			return new ColumnCondition(pColumnA, Sql.Operator.GREATER_THAN, pValue);
		}

		public static Condition operator >=(BigIntegerKeyColumn<TABLE> pColumnA, BigIntegerKeyColumn<TABLE> pColumnB) {
			return new ColumnCondition(pColumnA, Sql.Operator.GREATER_THAN_OR_EQUAL, pColumnB);
		}
		public static Condition operator >=(BigIntegerKeyColumn<TABLE> pColumnA, NBigIntegerKeyColumn<TABLE> pColumnB) {
			return new ColumnCondition(pColumnA, Sql.Operator.GREATER_THAN_OR_EQUAL, pColumnB);
		}
		public static Condition operator >=(BigIntegerKeyColumn<TABLE> pColumnA, Int64Key<TABLE> pValue) {
			return new ColumnCondition(pColumnA, Sql.Operator.GREATER_THAN_OR_EQUAL, pValue);
		}

		public static Condition operator <(BigIntegerKeyColumn<TABLE> pColumnA, BigIntegerKeyColumn<TABLE> pColumnB) {
			return new ColumnCondition(pColumnA, Sql.Operator.LESS_THAN, pColumnB);
		}
		public static Condition operator <(BigIntegerKeyColumn<TABLE> pColumnA, NBigIntegerKeyColumn<TABLE> pColumnB) {
			return new ColumnCondition(pColumnA, Sql.Operator.LESS_THAN, pColumnB);
		}
		public static Condition operator <(BigIntegerKeyColumn<TABLE> pColumnA, Int64Key<TABLE> pValue) {
			return new ColumnCondition(pColumnA, Sql.Operator.LESS_THAN, pValue);
		}

		public static Condition operator <=(BigIntegerKeyColumn<TABLE> pColumnA, BigIntegerKeyColumn<TABLE> pColumnB) {
			return new ColumnCondition(pColumnA, Sql.Operator.LESS_THAN_OR_EQUAL, pColumnB);
		}
		public static Condition operator <=(BigIntegerKeyColumn<TABLE> pColumnA, NBigIntegerKeyColumn<TABLE> pColumnB) {
			return new ColumnCondition(pColumnA, Sql.Operator.LESS_THAN_OR_EQUAL, pColumnB);
		}
		public static Condition operator <=(BigIntegerKeyColumn<TABLE> pColumnA, Int64Key<TABLE> pValue) {
			return new ColumnCondition(pColumnA, Sql.Operator.LESS_THAN_OR_EQUAL, pValue);
		}
		
		public static NumericCondition<BigIntegerKeyColumn<TABLE>, NBigIntegerKeyColumn<TABLE>, Int64> operator +(BigIntegerKeyColumn<TABLE> pColumnA, BigIntegerKeyColumn<TABLE> pColumnB) {
			return new NumericCondition<BigIntegerKeyColumn<TABLE>, NBigIntegerKeyColumn<TABLE>, Int64>(pColumnA, NumericOperator.ADD, pColumnB);
		}		
		public static NumericCondition<BigIntegerKeyColumn<TABLE>, NBigIntegerKeyColumn<TABLE>, Int64> operator +(BigIntegerKeyColumn<TABLE> pColumnA, NBigIntegerKeyColumn<TABLE> pColumnB) {
			return new NumericCondition<BigIntegerKeyColumn<TABLE>, NBigIntegerKeyColumn<TABLE>, Int64>(pColumnA, NumericOperator.ADD, pColumnB);
		}		
		public static NumericCondition<BigIntegerKeyColumn<TABLE>, NBigIntegerKeyColumn<TABLE>, Int64> operator +(BigIntegerKeyColumn<TABLE> pColumnA, Int64Key<TABLE> pValue) {
			return new NumericCondition<BigIntegerKeyColumn<TABLE>, NBigIntegerKeyColumn<TABLE>, Int64>(pColumnA, NumericOperator.ADD, pValue);
		}		
		
		public static NumericCondition<BigIntegerKeyColumn<TABLE>, NBigIntegerKeyColumn<TABLE>, Int64> operator -(BigIntegerKeyColumn<TABLE> pColumnA, BigIntegerKeyColumn<TABLE> pColumnB) {
			return new NumericCondition<BigIntegerKeyColumn<TABLE>, NBigIntegerKeyColumn<TABLE>, Int64>(pColumnA, NumericOperator.SUBTRACT, pColumnB);
		}		
		public static NumericCondition<BigIntegerKeyColumn<TABLE>, NBigIntegerKeyColumn<TABLE>, Int64> operator -(BigIntegerKeyColumn<TABLE> pColumnA, NBigIntegerKeyColumn<TABLE> pColumnB) {
			return new NumericCondition<BigIntegerKeyColumn<TABLE>, NBigIntegerKeyColumn<TABLE>, Int64>(pColumnA, NumericOperator.SUBTRACT, pColumnB);
		}		
		public static NumericCondition<BigIntegerKeyColumn<TABLE>, NBigIntegerKeyColumn<TABLE>, Int64> operator -(BigIntegerKeyColumn<TABLE> pColumnA, Int64Key<TABLE> pValue) {
			return new NumericCondition<BigIntegerKeyColumn<TABLE>, NBigIntegerKeyColumn<TABLE>, Int64>(pColumnA, NumericOperator.SUBTRACT, pValue);
		}
		
		public static NumericCondition<BigIntegerKeyColumn<TABLE>, NBigIntegerKeyColumn<TABLE>, Int64> operator /(BigIntegerKeyColumn<TABLE> pColumnA, BigIntegerKeyColumn<TABLE> pColumnB) {
			return new NumericCondition<BigIntegerKeyColumn<TABLE>, NBigIntegerKeyColumn<TABLE>, Int64>(pColumnA, NumericOperator.DIVIDE, pColumnB);
		}		
		public static NumericCondition<BigIntegerKeyColumn<TABLE>, NBigIntegerKeyColumn<TABLE>, Int64> operator /(BigIntegerKeyColumn<TABLE> pColumnA, NBigIntegerKeyColumn<TABLE> pColumnB) {
			return new NumericCondition<BigIntegerKeyColumn<TABLE>, NBigIntegerKeyColumn<TABLE>, Int64>(pColumnA, NumericOperator.DIVIDE, pColumnB);
		}		
		public static NumericCondition<BigIntegerKeyColumn<TABLE>, NBigIntegerKeyColumn<TABLE>, Int64> operator /(BigIntegerKeyColumn<TABLE> pColumnA, Int64Key<TABLE> pValue) {
			return new NumericCondition<BigIntegerKeyColumn<TABLE>, NBigIntegerKeyColumn<TABLE>, Int64>(pColumnA, NumericOperator.DIVIDE, pValue);
		}		
		
		public static NumericCondition<BigIntegerKeyColumn<TABLE>, NBigIntegerKeyColumn<TABLE>, Int64> operator *(BigIntegerKeyColumn<TABLE> pColumnA, BigIntegerKeyColumn<TABLE> pColumnB) {
			return new NumericCondition<BigIntegerKeyColumn<TABLE>, NBigIntegerKeyColumn<TABLE>, Int64>(pColumnA, NumericOperator.MULTIPLY, pColumnB);
		}		
		public static NumericCondition<BigIntegerKeyColumn<TABLE>, NBigIntegerKeyColumn<TABLE>, Int64> operator *(BigIntegerKeyColumn<TABLE> pColumnA, NBigIntegerKeyColumn<TABLE> pColumnB) {
			return new NumericCondition<BigIntegerKeyColumn<TABLE>, NBigIntegerKeyColumn<TABLE>, Int64>(pColumnA, NumericOperator.MULTIPLY, pColumnB);
		}		
		public static NumericCondition<BigIntegerKeyColumn<TABLE>, NBigIntegerKeyColumn<TABLE>, Int64> operator *(BigIntegerKeyColumn<TABLE> pColumnA, Int64Key<TABLE> pValue) {
			return new NumericCondition<BigIntegerKeyColumn<TABLE>, NBigIntegerKeyColumn<TABLE>, Int64>(pColumnA, NumericOperator.MULTIPLY, pValue);
		}
		
		public static NumericCondition<BigIntegerKeyColumn<TABLE>, NBigIntegerKeyColumn<TABLE>, Int64> operator %(BigIntegerKeyColumn<TABLE> pColumnA, BigIntegerKeyColumn<TABLE> pColumnB) {
			return new NumericCondition<BigIntegerKeyColumn<TABLE>, NBigIntegerKeyColumn<TABLE>, Int64>(pColumnA, NumericOperator.MODULO, pColumnB);
		}		
		public static NumericCondition<BigIntegerKeyColumn<TABLE>, NBigIntegerKeyColumn<TABLE>, Int64> operator %(BigIntegerKeyColumn<TABLE> pColumnA, NBigIntegerKeyColumn<TABLE> pColumnB) {
			return new NumericCondition<BigIntegerKeyColumn<TABLE>, NBigIntegerKeyColumn<TABLE>, Int64>(pColumnA, NumericOperator.MODULO, pColumnB);
		}		
		public static NumericCondition<BigIntegerKeyColumn<TABLE>, NBigIntegerKeyColumn<TABLE>, Int64> operator %(BigIntegerKeyColumn<TABLE> pColumnA, Int64Key<TABLE> pValue) {
			return new NumericCondition<BigIntegerKeyColumn<TABLE>, NBigIntegerKeyColumn<TABLE>, Int64>(pColumnA, NumericOperator.MODULO, pValue);
		}
		
		public Condition In(IList<Int64Key<TABLE>> pIntegerList) {
			return new InCondition<Int64Key<TABLE>>(this, pIntegerList);
		}
		public Condition NotIn(IList<Int64Key<TABLE>> pIntegerList) {
			return new NotInCondition<Int64Key<TABLE>>(this, pIntegerList);
		}

		public Condition In(Interfaces.IExecute pNestedQuery) {
			return new NestedQueryCondition(this, Sql.Operator.IN, pNestedQuery);
		}
		public Condition NotIn(Interfaces.IExecute pNestedQuery) {
			return new NestedQueryCondition(this, Sql.Operator.NOT_IN, pNestedQuery);
		}

		public Condition In(params Int64Key<TABLE>[] pValues) {
			return new InCondition<Int64Key<TABLE>>(this, pValues);
		}
		public Condition NotIn(params Int64Key<TABLE>[] pValues) {
			return new NotInCondition<Int64Key<TABLE>>(this, pValues);
		}
		
		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public override object GetValue(ADatabase pDatabase, System.Data.Common.DbDataReader pReader, int pColumnIndex) {
			
			Type dataType = pReader.GetFieldType(pColumnIndex);
			
			Int64 intValue;
			
			if(IsAutoId) {

				if(dataType != typeof(Int64) && dataType != typeof(decimal)) {
					throw new Exception($"Row column data is not of the correct type. Expected Int64 or decimal value instead got '{ dataType.ToString() }'. This probably means that the database and table column data types are not matching. Please run the definition tester to check table columns are of the correct type. Table: '{Table.TableName}' Column: '{ColumnName}'");
				}
				
				object value = pReader.GetValue(pColumnIndex);
				
				if(value is decimal)	//Queries like SELECT @@IDENTITY return decimals on integer columns so we need to handle this case. Not the best solution.
					intValue = (Int64) (decimal)value;
				else
					intValue = (Int64) value;	//Should give a cast exception if not an int
			}
			else {

				if(dataType != typeof(Int64)) {
					throw new Exception($"Row column data is not of the correct type. Expected Int64 value instead got '{ dataType.ToString() }'. This probably means that the database and table column data types are not matching. Please run the definition tester to check table columns are of the correct type. Table: '{ Table.TableName }' Column: '{ ColumnName }'");
				}
				
				intValue = pReader.GetInt64(pColumnIndex);
			}
			return intValue;
		}
		public Int64Key<TABLE> ValueOf(ARow pRow) {
			return (Int64Key<TABLE>)pRow.GetValue(this);
		}
		public void SetValue(ARow pRow, Int64Key<TABLE> pValue) {
			pRow.SetValue(this, pValue);
		}
		
		internal override void TestSetValue(ARow pRow, object pValue) {
			SetValue(pRow, (Int64Key<TABLE>) pValue);
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
			get { return System.Data.DbType.Int64; }
		}
		public override object GetDefaultType(){
			return new Int64Key<TABLE>(0);
		}
	}
}