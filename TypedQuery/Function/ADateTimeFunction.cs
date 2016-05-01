
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

	public abstract class ADateTimeFunction : Interfaces.IFunction {
		
		public ISelectable[] SelectableColumns {
			get{ return new ISelectable[]{ this }; }
		}

		public static Condition operator ==(ADateTimeFunction pColumnA, DateTime pValue) {
			return new ColumnCondition(pColumnA, Sql.Operator.EQUALS, pValue);
		}
		public static Condition operator ==(ADateTimeFunction pColumnA, ADateTimeFunction pColumnB) {
			return new ColumnCondition(pColumnA, Sql.Operator.EQUALS, pColumnB);
		}

		public static Condition operator !=(ADateTimeFunction pColumnA, DateTime pValue) {
			return new ColumnCondition(pColumnA, Sql.Operator.NOT_EQUALS, pValue);
		}
		public static Condition operator !=(ADateTimeFunction pColumnA, ADateTimeFunction pColumnB) {
			return new ColumnCondition(pColumnA, Sql.Operator.NOT_EQUALS, pColumnB);
		}

		public static Condition operator >(ADateTimeFunction pColumnA, DateTime pValue) {
			return new ColumnCondition(pColumnA, Sql.Operator.GREATER_THAN, pValue);
		}
		public static Condition operator >(ADateTimeFunction pColumnA, ADateTimeFunction pColumnB) {
			return new ColumnCondition(pColumnA, Sql.Operator.GREATER_THAN, pColumnB);
		}

		public static Condition operator >=(ADateTimeFunction pColumnA, DateTime pValue) {
			return new ColumnCondition(pColumnA, Sql.Operator.GREATER_THAN_OR_EQUAL, pValue);
		}
		public static Condition operator >=(ADateTimeFunction pColumnA, ADateTimeFunction pColumnB) {
			return new ColumnCondition(pColumnA, Sql.Operator.GREATER_THAN_OR_EQUAL, pColumnB);
		}

		public static Condition operator <(ADateTimeFunction pColumnA, DateTime pValue) {
			return new ColumnCondition(pColumnA, Sql.Operator.LESS_THAN, pValue);
		}
		public static Condition operator <(ADateTimeFunction pColumnA, ADateTimeFunction pColumnB) {
			return new ColumnCondition(pColumnA, Sql.Operator.LESS_THAN, pColumnB);
		}

		public static Condition operator <=(ADateTimeFunction pColumnA, DateTime pValue) {
			return new ColumnCondition(pColumnA, Sql.Operator.LESS_THAN_OR_EQUAL, pValue);
		}
		public static Condition operator <=(ADateTimeFunction pColumnA, ADateTimeFunction pColumnB) {
			return new ColumnCondition(pColumnA, Sql.Operator.LESS_THAN_OR_EQUAL, pColumnB);
		}

		public Condition In(IList<DateTime> pList) {
			return new InCondition<DateTime>(this, pList);
		}
		public Condition NotIn(IList<DateTime> pList) {
			return new NotInCondition<DateTime>(this, pList);
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
			get { return new OrderByColumn(this, OrderBy.ASC); }
		}
		
		/// <summary>
		/// Order Descending
		/// </summary>
		public OrderByColumn DESC {
			get { return new OrderByColumn(this, OrderBy.DESC); }
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
			get { return new OrderByColumn(this, OrderBy.Default); }
		}
	}
}
