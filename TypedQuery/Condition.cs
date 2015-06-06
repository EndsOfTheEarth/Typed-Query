
/*
 * 
 * Copyright (C) 2009-2015 JFo.nz
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

namespace Sql {
	
	/// <summary>
	/// Condition. Used to create query conditions. e.g. fieldA > 0 and fieldB > 0
	/// </summary>
	public abstract class Condition {

		private readonly object mLeft;
		private readonly Operator mOperator;
		private readonly object mRight;
		private readonly System.Data.DbType mRightDbType;

		internal object Left {
			get { return mLeft; }
		}
		internal Operator Operator {
			get { return mOperator; }
		}
		internal object Right {
			get { return mRight; }
		}
		internal System.Data.DbType RightDbType {
			get { return mRightDbType; }
		}

		internal Condition(object pLeft, Operator pOperator, object pRight) {
			mLeft = pLeft;
			mOperator = pOperator;
			mRight = pRight;
			
			if(pLeft is AColumn && !(pRight is AColumn || pRight is Condition))	//If right side is an actual value
				mRightDbType = ((AColumn) mLeft).DbType;
		}

		/// <summary>
		/// AND condition operator
		/// </summary>
		/// <param name="pConditionA"></param>
		/// <param name="pConditionB"></param>
		/// <returns></returns>
		public static Condition operator &(Condition pConditionA, Condition pConditionB) {
			return new AndCondition(pConditionA, pConditionB);
		}
		
		/// <summary>
		/// OR condition operator
		/// </summary>
		/// <param name="pConditionA"></param>
		/// <param name="pConditionB"></param>
		/// <returns></returns>
		public static Condition operator |(Condition pConditionA, Condition pConditionB) {
			return new OrCondition(pConditionA, pConditionB);
		}
		
		/// <summary>
		/// AND condition operator. Can also use the C# operator &amp;
		/// </summary>
		/// <param name="pConditionB"></param>
		/// <returns></returns>
		public Condition And(Condition pConditionB) {
			return new AndCondition(this, pConditionB);
		}
		
		/// <summary>
		/// AND condition operator.
		/// </summary>
		/// <param name="pConditionB"></param>
		/// <returns></returns>
		public static Condition And(Condition pConditionA, Condition pConditionB) {
			
			if(pConditionA != null)
				return new AndCondition(pConditionA, pConditionB);
			else
				return pConditionB;
		}
		
		/// <summary>
		/// OR condition operator. Can also use the C# operator '|'.
		/// </summary>
		/// <param name="pConditionB"></param>
		/// <returns></returns>
		public Condition Or(Condition pConditionB) {
			return new OrCondition(this, pConditionB);
		}
		
		/// <summary>
		/// OR condition operator.
		/// </summary>
		/// <param name="pConditionB"></param>
		/// <returns></returns>
		public static Condition Or(Condition pConditionA, Condition pConditionB) {
			
			if(pConditionA != null)
				return new OrCondition(pConditionA, pConditionB);
			else
				return pConditionB;
		}
		
		/// <summary>
		/// AND condition operator. The parameter pIncludeCondition determines if this condition is actually used.
		/// This method if useful for creating dynamic conditions without the need of an 'if' statement.
		/// </summary>
		/// <param name="pIncludeCondition">AND condition is only used if true.</param>
		/// <param name="pConditionB"></param>
		/// <returns></returns>
		public Condition AndIf(bool pIncludeCondition, Condition pConditionB) {
			return pIncludeCondition ? new AndCondition(this, pConditionB) : this;
		}
		
		/// <summary>
		/// OR condition operator. The parameter pIncludeCondition determines if this condition is actually used.
		/// This method if useful for creating dynamic conditions without the need of an 'if' statement.
		/// </summary>
		/// <param name="pIncludeCondition">AND condition is only used if true.</param>
		/// <param name="pConditionB"></param>
		/// <returns></returns>
		public Condition OrIf(bool pIncludeCondition, Condition pConditionB) {
			return pIncludeCondition ? new OrCondition(this, pConditionB) : this;
		}
		
		#region Hide Members
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
		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public new Type GetType(){
			return base.GetType();
		}
		#endregion
	}
	
	
	public class ColumnCondition : Condition {
		
		internal ColumnCondition(AColumn pLeft, Operator pOperator, AColumn pRight) : base(pLeft, pOperator, pRight) {
			
			if(pLeft == null)
				throw new NullReferenceException("pLeft column cannot be null");
			
			if(pRight == null)
				throw new NullReferenceException("pRight column cannot be null");
			
		}
		
		internal ColumnCondition(AColumn pLeft, Operator pOperator, object pRight) : base(pLeft, pOperator, pRight) {
			
			if(pLeft == null)
				throw new NullReferenceException("pLeft column cannot be null");
			
			if(pRight == null)
				throw new NullReferenceException("pRight value cannot be null");
		}
		
		internal ColumnCondition(Interfaces.IFunction pLeft, Operator pOperator, object pRight) : base(pLeft, pOperator, pRight) {
			
			if(pLeft == null)
				throw new NullReferenceException("pLeft cannot be null");
			
			if(pRight == null)
				throw new NullReferenceException("pRight cannot be null");
		}
		
		internal ColumnCondition(INumericCondition pLeft, Operator pOperator, object pRight) : base(pLeft, pOperator, pRight) {
			
			if(pLeft == null)
				throw new NullReferenceException("pLeft cannot be null");
			
			if(pRight == null)
				throw new NullReferenceException("pRight cannot be null");
		}
	}
	
	public class NestedQueryCondition : Condition {
		
		internal NestedQueryCondition(AColumn pLeft, Operator pOperator, Interfaces.IExecute pQuery) : base(pLeft, pOperator, pQuery) {
			
			if(pLeft == null)
				throw new NullReferenceException("pLeft cannot be null");
			
			if(pQuery == null)
				throw new NullReferenceException("pQuery cannot be null");
		}
	}
	
	public class InCondition<T> : Condition {
		
		internal InCondition(AColumn pLeft, IList<T> pList) : base(pLeft, Operator.IN, pList) {
			
			if(pLeft == null)
				throw new NullReferenceException("pLeft cannot be null");
			
			if(pList == null)
				throw new NullReferenceException("pList cannot be null");
			
			if(pList.Count == 0)
				throw new Exception("pList cannot be empty");
			
			foreach(T value in pList){
			
				if(value == null)
					throw new NullReferenceException("A value in pList is null. This is not allowed.");
			}
			
		}
		internal InCondition(AColumn pLeft, T[] pList) : base(pLeft, Operator.IN, pList) {
			
			if(pLeft == null)
				throw new NullReferenceException("pLeft cannot be null");
			
			if(pList == null)
				throw new NullReferenceException("pList cannot be null");
			
			if(pList.Length == 0)
				throw new Exception("pList cannot be empty");
			
			foreach(T value in pList){
			
				if(value == null)
					throw new NullReferenceException("A value in pList is null. This is not allowed.");
			}
		}
		
		internal InCondition(Interfaces.IFunction pLeft, IList<T> pList) : base(pLeft, Operator.IN, pList) {
			
			if(pLeft == null)
				throw new NullReferenceException("pLeft cannot be null");
			
			if(pList == null)
				throw new NullReferenceException("pList cannot be null");
			
			if(pList.Count == 0)
				throw new Exception("pList cannot be empty");
			
			foreach(T value in pList){
			
				if(value == null)
					throw new NullReferenceException("A value in pList is null. This is not allowed.");
			}
		}
		
		internal InCondition(Interfaces.IFunction pLeft, T[] pList) : base(pLeft, Operator.IN, pList) {
			
			if(pLeft == null)
				throw new NullReferenceException("pLeft cannot be null");
			
			if(pList == null)
				throw new NullReferenceException("pList cannot be null");
			
			if(pList.Length == 0)
				throw new Exception("pList cannot be empty");
			
			foreach(T value in pList){
			
				if(value == null)
					throw new NullReferenceException("A value in pList is null. This is not allowed.");
			}
		}		
	}
	
	public class NotInCondition<T> : Condition {
		
		internal NotInCondition(AColumn pLeft, IList<T> pList) : base(pLeft, Operator.NOT_IN, pList) {
			
			if(pLeft == null)
				throw new NullReferenceException("pLeft cannot be null");
			
			if(pList == null)
				throw new NullReferenceException("pList cannot be null");
			
			if(pList.Count == 0)
				throw new Exception("pList cannot be empty");
			
			foreach(T value in pList){
			
				if(value == null)
					throw new NullReferenceException("A value in pList is null. This is not allowed.");
			}
		}
		
		internal NotInCondition(AColumn pLeft, T[] pList) : base(pLeft, Operator.NOT_IN, pList) {
			
			if(pLeft == null)
				throw new NullReferenceException("pLeft cannot be null");
			
			if(pList == null)
				throw new NullReferenceException("pList cannot be null");
			
			if(pList.Length == 0)
				throw new Exception("pList cannot be empty");
			
			foreach(T value in pList){
			
				if(value == null)
					throw new NullReferenceException("A value in pList is null. This is not allowed.");
			}
		}
		
		internal NotInCondition(Interfaces.IFunction pLeft, IList<T> pList) : base(pLeft, Operator.NOT_IN, pList) {
			
			if(pLeft == null)
				throw new NullReferenceException("pLeft cannot be null");
			
			if(pList == null)
				throw new NullReferenceException("pList cannot be null");
			
			if(pList.Count == 0)
				throw new Exception("pList cannot be empty");
			
			foreach(T value in pList){
			
				if(value == null)
					throw new NullReferenceException("A value in pList is null. This is not allowed.");
			}
		}
		
		internal NotInCondition(Interfaces.IFunction pLeft, T[] pList) : base(pLeft, Operator.NOT_IN, pList) {
			
			if(pLeft == null)
				throw new NullReferenceException("pLeft cannot be null");
			
			if(pList == null)
				throw new NullReferenceException("pList cannot be null");
			
			if(pList.Length == 0)
				throw new Exception("pList cannot be empty");
			
			foreach(T value in pList){
			
				if(value == null)
					throw new NullReferenceException("A value in pList is null. This is not allowed.");
			}
		}
	}
	
	public class IsNullCondition : Condition {
			
		public IsNullCondition(ISelectable pSelectable) : base(pSelectable, Operator.IS_NULL, null) {
			
			if(pSelectable == null)
				throw new NullReferenceException("pSelectable cannot be null");
		}
	}
	
	public class IsNotNullCondition : Condition {
			
		public IsNotNullCondition(ISelectable pSelectable) : base(pSelectable, Operator.IS_NOT_NULL, null) {
			
			if(pSelectable == null)
				throw new NullReferenceException("pSelectable cannot be null");
		}
	}
	
	public class AndCondition : Condition {
		
		public AndCondition(Condition pConditionA, Condition pConditionB) : base(pConditionA, Operator.AND, pConditionB){
			
			if(pConditionA == null)
				throw new NullReferenceException("pConditionA cannot be null");
			
			if(pConditionB == null)
				throw new NullReferenceException("pConditionB cannot be null");
		}
	}
	
	public class OrCondition : Condition {
		
		public OrCondition(Condition pConditionA, Condition pConditionB) : base(pConditionA, Operator.OR, pConditionB){
			
			if(pConditionA == null)
				throw new NullReferenceException("pConditionA cannot be null");
			
			if(pConditionB == null)
				throw new NullReferenceException("pConditionB cannot be null");
		}
	}
	
	public interface INumericCondition {
		
		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		object Left { get; }
		
		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		NumericOperator Operator { get; }
		
		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		object Right { get; }
	}
	
	public class NumericCondition<COLUMN, NCOLUMN, TYPE> : INumericCondition{
		
		private readonly object mLeft;
		private readonly NumericOperator mOperator;
		private readonly object mRight;

		public object Left {
			get { return mLeft; }
		}
		public NumericOperator Operator {
			get { return mOperator; }
		}
		public object Right {
			get { return mRight; }
		}

		internal NumericCondition(object pLeft, NumericOperator pOperator, object pRight) {
			mLeft = pLeft;
			mOperator = pOperator;
			mRight = pRight;
		}
		
		public static Condition operator ==(NumericCondition<COLUMN, NCOLUMN, TYPE> pColumnA, COLUMN pColumnB) {
			return new ColumnCondition(pColumnA, Sql.Operator.EQUALS, pColumnB);
		}
		public static Condition operator ==(NumericCondition<COLUMN, NCOLUMN, TYPE> pColumnA, NCOLUMN pColumnB) {
			return new ColumnCondition(pColumnA, Sql.Operator.EQUALS, pColumnB);
		}
		public static Condition operator ==(NumericCondition<COLUMN, NCOLUMN, TYPE> pColumnA, TYPE pValue) {
			return new ColumnCondition(pColumnA, Sql.Operator.EQUALS, pValue);
		}

		public static Condition operator !=(NumericCondition<COLUMN, NCOLUMN, TYPE> pColumnA, COLUMN pColumnB) {
			return new ColumnCondition(pColumnA, Sql.Operator.NOT_EQUALS, pColumnB);
		}
		public static Condition operator !=(NumericCondition<COLUMN, NCOLUMN, TYPE> pColumnA, NCOLUMN pColumnB) {
			return new ColumnCondition(pColumnA, Sql.Operator.NOT_EQUALS, pColumnB);
		}
		public static Condition operator !=(NumericCondition<COLUMN, NCOLUMN, TYPE> pColumnA, TYPE pValue) {
			return new ColumnCondition(pColumnA, Sql.Operator.NOT_EQUALS, pValue);
		}

		public static Condition operator >(NumericCondition<COLUMN, NCOLUMN, TYPE> pColumnA, COLUMN pColumnB) {
			return new ColumnCondition(pColumnA, Sql.Operator.GREATER_THAN, pColumnB);
		}
		public static Condition operator >(NumericCondition<COLUMN, NCOLUMN, TYPE> pColumnA, NCOLUMN pColumnB) {
			return new ColumnCondition(pColumnA, Sql.Operator.GREATER_THAN, pColumnB);
		}
		public static Condition operator >(NumericCondition<COLUMN, NCOLUMN, TYPE> pColumnA, TYPE pValue) {
			return new ColumnCondition(pColumnA, Sql.Operator.GREATER_THAN, pValue);
		}

		public static Condition operator >=(NumericCondition<COLUMN, NCOLUMN, TYPE> pColumnA, COLUMN pColumnB) {
			return new ColumnCondition(pColumnA, Sql.Operator.GREATER_THAN_OR_EQUAL, pColumnB);
		}
		public static Condition operator >=(NumericCondition<COLUMN, NCOLUMN, TYPE> pColumnA, NCOLUMN pColumnB) {
			return new ColumnCondition(pColumnA, Sql.Operator.GREATER_THAN_OR_EQUAL, pColumnB);
		}
		public static Condition operator >=(NumericCondition<COLUMN, NCOLUMN, TYPE> pColumnA, TYPE pValue) {
			return new ColumnCondition(pColumnA, Sql.Operator.GREATER_THAN_OR_EQUAL, pValue);
		}

		public static Condition operator <(NumericCondition<COLUMN, NCOLUMN, TYPE> pColumnA, COLUMN pColumnB) {
			return new ColumnCondition(pColumnA, Sql.Operator.LESS_THAN, pColumnB);
		}
		public static Condition operator <(NumericCondition<COLUMN, NCOLUMN, TYPE> pColumnA, NCOLUMN pColumnB) {
			return new ColumnCondition(pColumnA, Sql.Operator.LESS_THAN, pColumnB);
		}
		public static Condition operator <(NumericCondition<COLUMN, NCOLUMN, TYPE> pColumnA, TYPE pValue) {
			return new ColumnCondition(pColumnA, Sql.Operator.LESS_THAN, pValue);
		}

		public static Condition operator <=(NumericCondition<COLUMN, NCOLUMN, TYPE> pColumnA, COLUMN pColumnB) {
			return new ColumnCondition(pColumnA, Sql.Operator.LESS_THAN_OR_EQUAL, pColumnB);
		}
		public static Condition operator <=(NumericCondition<COLUMN, NCOLUMN, TYPE> pColumnA, NCOLUMN pColumnB) {
			return new ColumnCondition(pColumnA, Sql.Operator.LESS_THAN_OR_EQUAL, pColumnB);
		}
		public static Condition operator <=(NumericCondition<COLUMN, NCOLUMN, TYPE> pColumnA, TYPE pValue) {
			return new ColumnCondition(pColumnA, Sql.Operator.LESS_THAN_OR_EQUAL, pValue);
		}
		
		public static NumericCondition<COLUMN, NCOLUMN, TYPE> operator +(NumericCondition<COLUMN, NCOLUMN, TYPE> pColumnA, COLUMN pColumnB) {
			return new NumericCondition<COLUMN, NCOLUMN, TYPE>(pColumnA, NumericOperator.ADD, pColumnB);
		}
		public static NumericCondition<COLUMN, NCOLUMN, TYPE> operator +(NumericCondition<COLUMN, NCOLUMN, TYPE> pColumnA, NCOLUMN pColumnB) {
			return new NumericCondition<COLUMN, NCOLUMN, TYPE>(pColumnA, NumericOperator.ADD, pColumnB);
		}
		public static NumericCondition<COLUMN, NCOLUMN, TYPE> operator +(NumericCondition<COLUMN, NCOLUMN, TYPE> pColumnA, TYPE pValue) {
			return new NumericCondition<COLUMN, NCOLUMN, TYPE>(pColumnA, NumericOperator.ADD, pValue);
		}

		public static NumericCondition<COLUMN, NCOLUMN, TYPE> operator -(NumericCondition<COLUMN, NCOLUMN, TYPE> pColumnA, COLUMN pColumnB) {
			return new NumericCondition<COLUMN, NCOLUMN, TYPE>(pColumnA, NumericOperator.SUBTRACT, pColumnB);
		}
		public static NumericCondition<COLUMN, NCOLUMN, TYPE> operator -(NumericCondition<COLUMN, NCOLUMN, TYPE> pColumnA, NCOLUMN pColumnB) {
			return new NumericCondition<COLUMN, NCOLUMN, TYPE>(pColumnA, NumericOperator.SUBTRACT, pColumnB);
		}
		public static NumericCondition<COLUMN, NCOLUMN, TYPE> operator -(NumericCondition<COLUMN, NCOLUMN, TYPE> pColumnA, TYPE pValue) {
			return new NumericCondition<COLUMN, NCOLUMN, TYPE>(pColumnA, NumericOperator.SUBTRACT, pValue);
		}

		public static NumericCondition<COLUMN, NCOLUMN, TYPE> operator /(NumericCondition<COLUMN, NCOLUMN, TYPE> pColumnA, COLUMN pColumnB) {
			return new NumericCondition<COLUMN, NCOLUMN, TYPE>(pColumnA, NumericOperator.DIVIDE, pColumnB);
		}
		public static NumericCondition<COLUMN, NCOLUMN, TYPE> operator /(NumericCondition<COLUMN, NCOLUMN, TYPE> pColumnA, NCOLUMN pColumnB) {
			return new NumericCondition<COLUMN, NCOLUMN, TYPE>(pColumnA, NumericOperator.DIVIDE, pColumnB);
		}
		public static NumericCondition<COLUMN, NCOLUMN, TYPE> operator /(NumericCondition<COLUMN, NCOLUMN, TYPE> pColumnA, TYPE pValue) {
			return new NumericCondition<COLUMN, NCOLUMN, TYPE>(pColumnA, NumericOperator.DIVIDE, pValue);
		}

		public static NumericCondition<COLUMN, NCOLUMN, TYPE> operator *(NumericCondition<COLUMN, NCOLUMN, TYPE> pColumnA, COLUMN pColumnB) {
			return new NumericCondition<COLUMN, NCOLUMN, TYPE>(pColumnA, NumericOperator.MULTIPLY, pColumnB);
		}
		public static NumericCondition<COLUMN, NCOLUMN, TYPE> operator *(NumericCondition<COLUMN, NCOLUMN, TYPE> pColumnA, NCOLUMN pColumnB) {
			return new NumericCondition<COLUMN, NCOLUMN, TYPE>(pColumnA, NumericOperator.MULTIPLY, pColumnB);
		}
		public static NumericCondition<COLUMN, NCOLUMN, TYPE> operator *(NumericCondition<COLUMN, NCOLUMN, TYPE> pColumnA, TYPE pValue) {
			return new NumericCondition<COLUMN, NCOLUMN, TYPE>(pColumnA, NumericOperator.MULTIPLY, pValue);
		}
		public static NumericCondition<COLUMN, NCOLUMN, TYPE> operator *(TYPE pValue, NumericCondition<COLUMN, NCOLUMN, TYPE> pColumnA) {
			return new NumericCondition<COLUMN, NCOLUMN, TYPE>(pColumnA, NumericOperator.MULTIPLY, pValue);
		}
		
		#region Hide Members
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
		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public new Type GetType(){
			return base.GetType();
		}
		#endregion
	}
	
	public enum NumericOperator {
		ADD,
		SUBTRACT,
		DIVIDE,
		MULTIPLY,
		MODULO
	}
	
	internal enum Operator {
		EQUALS,
		NOT_EQUALS,
		GREATER_THAN,
		GREATER_THAN_OR_EQUAL,
		LESS_THAN,
		LESS_THAN_OR_EQUAL,
		AND,
		OR,
		IN,
		NOT_IN,
		LIKE,
		NOT_LIKE,
		IS_NULL,
		IS_NOT_NULL
	}
}