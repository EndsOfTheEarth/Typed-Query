
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
using System.Collections.Generic;
using System.Text;

namespace Sql.Function {

	public abstract class ANumericFunction : Interfaces.IFunction, Interfaces.IWindowFunction {
		
		public ISelectable[] SelectableColumns {
			get{ return new ISelectable[]{ this }; }
		}

		public static Condition operator ==(ANumericFunction pColumnA, int pValue) {
			return new ColumnCondition(pColumnA, Sql.Operator.EQUALS, pValue);
		}
		public static Condition operator ==(ANumericFunction pColumnA, Int64 pValue) {
			return new ColumnCondition(pColumnA, Sql.Operator.EQUALS, pValue);
		}
		public static Condition operator ==(ANumericFunction pColumnA, decimal pValue) {
			return new ColumnCondition(pColumnA, Sql.Operator.EQUALS, pValue);
		}
		public static Condition operator ==(ANumericFunction pColumnA, short pValue) {
			return new ColumnCondition(pColumnA, Sql.Operator.EQUALS, pValue);
		}
		public static Condition operator ==(ANumericFunction pColumnA, float pValue) {
			return new ColumnCondition(pColumnA, Sql.Operator.EQUALS, pValue);
		}
		public static Condition operator ==(ANumericFunction pColumnA, double pValue) {
			return new ColumnCondition(pColumnA, Sql.Operator.EQUALS, pValue);
		}		
		public static Condition operator ==(ANumericFunction pColumnA, ANumericFunction pColumnB) {
			return new ColumnCondition(pColumnA, Sql.Operator.EQUALS, pColumnB);
		}
		public static Condition operator ==(ANumericFunction pColumnA, ANumericColumn pColumnB) {
			return new ColumnCondition(pColumnA, Sql.Operator.EQUALS, pColumnB);
		}

		public static Condition operator !=(ANumericFunction pColumnA, int pValue) {
			return new ColumnCondition(pColumnA, Sql.Operator.NOT_EQUALS, pValue);
		}
		public static Condition operator !=(ANumericFunction pColumnA, Int64 pValue) {
			return new ColumnCondition(pColumnA, Sql.Operator.NOT_EQUALS, pValue);
		}
		public static Condition operator !=(ANumericFunction pColumnA, decimal pValue) {
			return new ColumnCondition(pColumnA, Sql.Operator.NOT_EQUALS, pValue);
		}		
		public static Condition operator !=(ANumericFunction pColumnA, short pValue) {
			return new ColumnCondition(pColumnA, Sql.Operator.NOT_EQUALS, pValue);
		}
		public static Condition operator !=(ANumericFunction pColumnA, float pValue) {
			return new ColumnCondition(pColumnA, Sql.Operator.NOT_EQUALS, pValue);
		}
		public static Condition operator !=(ANumericFunction pColumnA, double pValue) {
			return new ColumnCondition(pColumnA, Sql.Operator.NOT_EQUALS, pValue);
		}		
		public static Condition operator !=(ANumericFunction pColumnA, ANumericFunction pColumnB) {
			return new ColumnCondition(pColumnA, Sql.Operator.NOT_EQUALS, pColumnB);
		}
		public static Condition operator !=(ANumericFunction pColumnA, ANumericColumn pColumnB) {
			return new ColumnCondition(pColumnA, Sql.Operator.NOT_EQUALS, pColumnB);
		}

		public static Condition operator >(ANumericFunction pColumnA, int pValue) {
			return new ColumnCondition(pColumnA, Sql.Operator.GREATER_THAN, pValue);
		}
		public static Condition operator >(ANumericFunction pColumnA, Int64 pValue) {
			return new ColumnCondition(pColumnA, Sql.Operator.GREATER_THAN, pValue);
		}
		public static Condition operator >(ANumericFunction pColumnA, decimal pValue) {
			return new ColumnCondition(pColumnA, Sql.Operator.GREATER_THAN, pValue);
		}		
		public static Condition operator >(ANumericFunction pColumnA, short pValue) {
			return new ColumnCondition(pColumnA, Sql.Operator.GREATER_THAN, pValue);
		}
		public static Condition operator >(ANumericFunction pColumnA, float pValue) {
			return new ColumnCondition(pColumnA, Sql.Operator.GREATER_THAN, pValue);
		}
		public static Condition operator >(ANumericFunction pColumnA, double pValue) {
			return new ColumnCondition(pColumnA, Sql.Operator.GREATER_THAN, pValue);
		}		
		public static Condition operator >(ANumericFunction pColumnA, ANumericFunction pColumnB) {
			return new ColumnCondition(pColumnA, Sql.Operator.GREATER_THAN, pColumnB);
		}
		public static Condition operator >(ANumericFunction pColumnA, ANumericColumn pColumnB) {
			return new ColumnCondition(pColumnA, Sql.Operator.GREATER_THAN, pColumnB);
		}		

		public static Condition operator >=(ANumericFunction pColumnA, int pValue) {
			return new ColumnCondition(pColumnA, Sql.Operator.GREATER_THAN_OR_EQUAL, pValue);
		}
		public static Condition operator >=(ANumericFunction pColumnA, Int64 pValue) {
			return new ColumnCondition(pColumnA, Sql.Operator.GREATER_THAN_OR_EQUAL, pValue);
		}
		public static Condition operator >=(ANumericFunction pColumnA, decimal pValue) {
			return new ColumnCondition(pColumnA, Sql.Operator.GREATER_THAN_OR_EQUAL, pValue);
		}		
		public static Condition operator >=(ANumericFunction pColumnA, short pValue) {
			return new ColumnCondition(pColumnA, Sql.Operator.GREATER_THAN_OR_EQUAL, pValue);
		}
		public static Condition operator >=(ANumericFunction pColumnA, float pValue) {
			return new ColumnCondition(pColumnA, Sql.Operator.GREATER_THAN_OR_EQUAL, pValue);
		}
		public static Condition operator >=(ANumericFunction pColumnA, double pValue) {
			return new ColumnCondition(pColumnA, Sql.Operator.GREATER_THAN_OR_EQUAL, pValue);
		}		
		public static Condition operator >=(ANumericFunction pColumnA, ANumericFunction pColumnB) {
			return new ColumnCondition(pColumnA, Sql.Operator.GREATER_THAN_OR_EQUAL, pColumnB);
		}
		public static Condition operator >=(ANumericFunction pColumnA, ANumericColumn pColumnB) {
			return new ColumnCondition(pColumnA, Sql.Operator.GREATER_THAN_OR_EQUAL, pColumnB);
		}

		public static Condition operator <(ANumericFunction pColumnA, int pValue) {
			return new ColumnCondition(pColumnA, Sql.Operator.LESS_THAN, pValue);
		}
		public static Condition operator <(ANumericFunction pColumnA, Int64 pValue) {
			return new ColumnCondition(pColumnA, Sql.Operator.LESS_THAN, pValue);
		}
		public static Condition operator <(ANumericFunction pColumnA, decimal pValue) {
			return new ColumnCondition(pColumnA, Sql.Operator.LESS_THAN, pValue);
		}		
		public static Condition operator <(ANumericFunction pColumnA, short pValue) {
			return new ColumnCondition(pColumnA, Sql.Operator.LESS_THAN, pValue);
		}
		public static Condition operator <(ANumericFunction pColumnA, float pValue) {
			return new ColumnCondition(pColumnA, Sql.Operator.LESS_THAN, pValue);
		}
		public static Condition operator <(ANumericFunction pColumnA, double pValue) {
			return new ColumnCondition(pColumnA, Sql.Operator.LESS_THAN, pValue);
		}		
		public static Condition operator <(ANumericFunction pColumnA, ANumericFunction pColumnB) {
			return new ColumnCondition(pColumnA, Sql.Operator.LESS_THAN, pColumnB);
		}
		public static Condition operator <(ANumericFunction pColumnA, ANumericColumn pColumnB) {
			return new ColumnCondition(pColumnA, Sql.Operator.LESS_THAN, pColumnB);
		}

		public static Condition operator <=(ANumericFunction pColumnA, int pValue) {
			return new ColumnCondition(pColumnA, Sql.Operator.LESS_THAN_OR_EQUAL, pValue);
		}
		public static Condition operator <=(ANumericFunction pColumnA, Int64 pValue) {
			return new ColumnCondition(pColumnA, Sql.Operator.LESS_THAN_OR_EQUAL, pValue);
		}
		public static Condition operator <=(ANumericFunction pColumnA, decimal pValue) {
			return new ColumnCondition(pColumnA, Sql.Operator.LESS_THAN_OR_EQUAL, pValue);
		}		
		public static Condition operator <=(ANumericFunction pColumnA, short pValue) {
			return new ColumnCondition(pColumnA, Sql.Operator.LESS_THAN_OR_EQUAL, pValue);
		}
		public static Condition operator <=(ANumericFunction pColumnA, float pValue) {
			return new ColumnCondition(pColumnA, Sql.Operator.LESS_THAN_OR_EQUAL, pValue);
		}
		public static Condition operator <=(ANumericFunction pColumnA, double pValue) {
			return new ColumnCondition(pColumnA, Sql.Operator.LESS_THAN_OR_EQUAL, pValue);
		}		
		public static Condition operator <=(ANumericFunction pColumnA, ANumericFunction pColumnB) {
			return new ColumnCondition(pColumnA, Sql.Operator.LESS_THAN_OR_EQUAL, pColumnB);
		}
		public static Condition operator <=(ANumericFunction pColumnA, ANumericColumn pColumnB) {
			return new ColumnCondition(pColumnA, Sql.Operator.LESS_THAN_OR_EQUAL, pColumnB);
		}

		public Condition In(IList<int> pList) {
			return new InCondition<int>(this, pList);
		}
		public Condition NotIn(IList<int> pList) {
			return new NotInCondition<int>(this, pList);
		}

		public Condition In(IList<decimal> pList) {
			return new InCondition<decimal>(this, pList);
		}
		public Condition NotIn(IList<decimal> pList) {
			return new NotInCondition<decimal>(this, pList);
		}
		
		public Condition In(IList<short> pList) {
			return new InCondition<short>(this, pList);
		}
		public Condition NotIn(IList<short> pList) {
			return new NotInCondition<short>(this, pList);
		}
		
		public Condition In(IList<float> pList) {
			return new InCondition<float>(this, pList);
		}
		public Condition NotIn(IList<float> pList) {
			return new NotInCondition<float>(this, pList);
		}
		
		public Condition In(IList<double> pList) {
			return new InCondition<double>(this, pList);
		}
		public Condition NotIn(IList<double> pList) {
			return new NotInCondition<double>(this, pList);
		}
		public abstract string GetFunctionSql(ADatabase pDatabase, bool pUseAlias, Sql.Database.IAliasManager pAliasManager);
		public abstract object GetValue(ADatabase pDatabase, System.Data.Common.DbDataReader pReader, int pColumnIndex);

		public override int GetHashCode() {
			return base.GetHashCode();
		}
		public override bool Equals(object obj) {
			return base.Equals(obj);
		}
		public override string ToString() {
			return base.ToString();
		}
		
		/// <summary>
		/// Order ascending
		/// </summary>
		public OrderByColumn ASC {
			get { return new OrderByColumn(this, Sql.OrderBy.ASC); }
		}
		
		/// <summary>
		/// Order Descending
		/// </summary>
		public OrderByColumn DESC {
			get { return new OrderByColumn(this, Sql.OrderBy.DESC); }
		}
		
		/// <summary>
		/// IS NULL sql condition
		/// </summary>
		public Condition IsNull {
			get { return new IsNullCondition(this); }
		}
		
		/// <summary>
		///	IS NOT NULL sql condition
		/// </summary>
		public Condition IsNotNull {
			get { return new IsNotNullCondition(this); }
		}
		
		/// <summary>
		/// Default ordering
		/// </summary>
		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public OrderByColumn GetOrderByColumn {
			get { return new OrderByColumn(this, Sql.OrderBy.Default); }
		}
		
		protected decimal? GetValueAsDecimal(System.Data.Common.DbDataReader pReader, int pColumnIndex) {

			if (pReader.IsDBNull(pColumnIndex))
				return null;
			
			Type fieldType = pReader.GetFieldType(pColumnIndex);
			
			decimal? value;
			
			if (fieldType == typeof(Int64))
				value = (decimal?)Convert.ToDecimal(pReader.GetInt64(pColumnIndex));
			else if(fieldType == typeof(decimal))
				value = (decimal?)Convert.ToDecimal(pReader.GetDecimal(pColumnIndex));
			else if(fieldType == typeof(short))
				value = (decimal?)Convert.ToDecimal(pReader.GetInt16(pColumnIndex));
			else
				value = (decimal?)pReader.GetInt32(pColumnIndex);
			
			return value;
		}
		
		protected double? GetValueAsDouble(System.Data.Common.DbDataReader pReader, int pColumnIndex) {

			if (pReader.IsDBNull(pColumnIndex))
				return null;
			
			Type fieldType = pReader.GetFieldType(pColumnIndex);
			
			double? value;
			
			if (fieldType == typeof(float))
				value = (double?)Convert.ToDouble(pReader.GetFloat(pColumnIndex));
			else
				value = (double?)Convert.ToDouble(pReader.GetInt64(pColumnIndex));			
			
			return value;
		}
		
		private WindowFunction mWindowFunction;
		
		protected string GetWindowFunctionSql(bool pUseAlias, Sql.Database.IAliasManager pAliasManager) {
			return mWindowFunction != null ? mWindowFunction.GetSql(pUseAlias, pAliasManager) : string.Empty;
		}
		
		public ANumericFunction Over() {
			
			if(mWindowFunction == null)
				mWindowFunction = new WindowFunction();
			
			mWindowFunction.SetOverPartitionBy(null);
			return this;
		}
		public ANumericFunction OverPartitionBy(params AColumn[] pColumns) {
			
			if(mWindowFunction == null)
				mWindowFunction = new WindowFunction();
			
			mWindowFunction.SetOverPartitionBy(pColumns);
			return this;
		}
		public ANumericFunction OrderBy(params IOrderByColumn[] pOrderByColumns) {
			
			if(mWindowFunction == null)
				mWindowFunction = new WindowFunction();
			
			mWindowFunction.SetOrderBy(pOrderByColumns);
			return this;
		}
	}
}