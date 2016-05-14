
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

namespace Sql {

	/// <summary>
	/// Abstract column
	/// </summary>
	public abstract class AColumn : IOrderByColumn, ISelectable, ISelectableColumns {

		private readonly ATable mTable;
		private readonly string mColumnName;
		private readonly bool mIsPrimaryKey;
		private bool mIsAutoId;
		private readonly bool mAllowsNulls;

		/// <summary>
		/// Abstract column
		/// </summary>
		/// <param name="pTable">Name of table in database</param>
		/// <param name="pColumnName">Name of column in table</param>
		/// <param name="pIsPrimaryKey">true if column is part of its tables primary key</param>
		protected AColumn(ATable pTable, string pColumnName, bool pIsPrimaryKey, bool pAllowsNulls) {
			
			if (pTable == null)
				throw new NullReferenceException($"{nameof(pTable)} cannot be null");

			if (string.IsNullOrWhiteSpace(pColumnName))
				throw new ArgumentException($"{nameof(pColumnName)} cannot be null or empty");

			mTable = pTable;
			mColumnName = pColumnName;
			mIsPrimaryKey = pIsPrimaryKey;
			mAllowsNulls = pAllowsNulls;
		}
		/// <summary>
		/// Table that column belongs to.
		/// </summary>
		public ATable Table {
			get { return mTable; }
		}
		
		/// <summary>
		/// Name of column in table
		/// </summary>
		public string ColumnName {
			get { return mColumnName; }
		}
		
		/// <summary>
		/// Returns true if column is part of its tables primary key.
		/// </summary>
		public bool IsPrimaryKey {
			get { return mIsPrimaryKey; }
		}
		
		/// <summary>
		/// Returns true if column allows nulls
		/// </summary>
		public bool AllowsNulls {
			get { return mAllowsNulls; }
		}
		
		/// <summary>
		/// Returns true is column is an auto id field
		/// </summary>
		public bool IsAutoId {
			get { return mIsAutoId; }
			protected set { mIsAutoId = value; }
		}
		
		/// <summary>
		/// Returns the default type. For example an integer column would return 0. Or a boolean column would return false
		/// </summary>
		/// <returns></returns>
		public abstract object GetDefaultType();
		
		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public ISelectable[] SelectableColumns {
			get{ return new ISelectable[]{ this }; }
		}

		/// <summary>
		/// System data type this column maps to
		/// </summary>
		public abstract System.Data.DbType DbType { get; }
		
		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public abstract object GetValue(ADatabase pDatabase, System.Data.Common.DbDataReader pReader, int pColumnIndex);

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
		/// Default ordering
		/// </summary>
		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public OrderByColumn GetOrderByColumn {
			get { return new OrderByColumn(this, OrderBy.Default); }
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
		
		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public new Type GetType(){
			return base.GetType();
		}
		
		internal abstract void TestSetValue(ARow pRow, object pValue);
		internal abstract object TestGetValue(ARow pRow);
	}
	
	public interface IColumnLength {
	
		int MaxLength { get; }
	}
	
	/// <summary>
	/// Abstract numeric column
	/// Used to idetify table columns that are numeric
	/// </summary>
	public abstract class ANumericColumn : AColumn {
	
		protected ANumericColumn(ATable pTable, string pColumnName, bool pIsPrimaryKey, bool pAllowsNulls) : base(pTable, pColumnName, pIsPrimaryKey, pAllowsNulls){
			
		}
	}

	public interface IOrderByColumn {
		OrderByColumn GetOrderByColumn { get; }
	}
	public sealed class OrderByColumn : IOrderByColumn {

		private readonly ISelectable mColumn;
		private readonly OrderBy mOrderBy;

		internal OrderByColumn(ISelectable pColumn, OrderBy pOrderBy) {
			
			if(pColumn == null)
				throw new NullReferenceException($"{nameof(pColumn)} cannot be null");
			
			mColumn = pColumn;
			mOrderBy = pOrderBy;
		}
		public ISelectable Column {
			get { return mColumn; }
		}
		internal OrderBy OrderBy {
			get { return mOrderBy; }
		}
		public OrderByColumn GetOrderByColumn {
			get { return this; }
		}
	}
	
	/// <summary>
	/// Query order by
	/// </summary>
	internal enum OrderBy {
		/// <summary>
		/// Ascending
		/// </summary>
		ASC,
		
		/// <summary>
		/// Descending
		/// </summary>
		DESC,
		
		/// <summary>
		/// Default
		/// </summary>
		Default
	}
}