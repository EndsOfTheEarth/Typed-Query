
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

namespace Sql.Interfaces {
	
	public interface IUpdate {

		IUpdateSet Set<COLUMN>(COLUMN pColumn, COLUMN pValue) where COLUMN : AColumn;
		
		IUpdateSet Set(Column.SmallIntegerColumn pColumn, Int16 pValue);
		IUpdateSet Set(Column.NSmallIntegerColumn pColumn, Int16? pValue);
		IUpdateSet Set(Column.IntegerColumn pColumn, int pValue);
		IUpdateSet Set(Column.NIntegerColumn pColumn, int? pValue);		
		IUpdateSet Set(Column.BigIntegerColumn pColumn, Int64 pValue);
		IUpdateSet Set(Column.NBigIntegerColumn pColumn, Int64? pValue);		
		IUpdateSet Set(Column.StringColumn pColumn, string pValue);
		IUpdateSet Set(Column.DecimalColumn pColumn, decimal pValue);
		IUpdateSet Set(Column.NDecimalColumn pColumn, decimal? pValue);		
		IUpdateSet Set(Column.DateTimeColumn pColumn, DateTime pValue);
		IUpdateSet Set(Column.DateTimeColumn pColumn, Function.CurrentDateTime pValue);		
		IUpdateSet Set(Column.NDateTimeColumn pColumn, DateTime? pValue);
		IUpdateSet Set(Column.NDateTimeColumn pColumn, Function.CurrentDateTime pValue);
		
		IUpdateSet Set(Column.DateTime2Column pColumn, DateTime pValue);
		IUpdateSet Set(Column.DateTime2Column pColumn, Function.CurrentDateTime pValue);		
		IUpdateSet Set(Column.NDateTime2Column pColumn, DateTime? pValue);
		IUpdateSet Set(Column.NDateTime2Column pColumn, Function.CurrentDateTime pValue);
		
		IUpdateSet Set(Column.DateTimeOffsetColumn pColumn, DateTimeOffset pValue);
		IUpdateSet Set(Column.DateTimeOffsetColumn pColumn, Function.CurrentDateTimeOffset pValue);		
		IUpdateSet Set(Column.NDateTimeOffsetColumn pColumn, DateTimeOffset? pValue);
		IUpdateSet Set(Column.NDateTimeOffsetColumn pColumn, Function.CurrentDateTimeOffset pValue);
		
		IUpdateSet Set(Column.BoolColumn pColumn, bool pValue);
		IUpdateSet Set(Column.NBoolColumn pColumn, bool? pValue);
		IUpdateSet Set(Column.GuidColumn pColumn, Guid pValue);
		IUpdateSet Set(Column.NGuidColumn pColumn, Guid? pValue);
		IUpdateSet Set(Column.BinaryColumn pColumn, byte[] pValue);
		IUpdateSet Set(Column.NBinaryColumn pColumn, byte[] pValue);
		
		IUpdateSet Set<TABLE>(Column.GuidKeyColumn<TABLE> pColumn, GuidKey<TABLE> pValue);
		IUpdateSet Set<TABLE>(Column.NGuidKeyColumn<TABLE> pColumn, GuidKey<TABLE> pValue);
		IUpdateSet Set<TABLE>(Column.NGuidKeyColumn<TABLE> pColumn, GuidKey<TABLE>? pValue);
		IUpdateSet Set<TABLE>(Column.SmallIntegerKeyColumn<TABLE> pColumn, Int16Key<TABLE> pValue);
		IUpdateSet Set<TABLE>(Column.NSmallIntegerKeyColumn<TABLE> pColumn, Int16Key<TABLE> pValue);
		IUpdateSet Set<TABLE>(Column.NSmallIntegerKeyColumn<TABLE> pColumn, Int16Key<TABLE>? pValue);
		IUpdateSet Set<TABLE>(Column.IntegerKeyColumn<TABLE> pColumn, Int32Key<TABLE> pValue);
		IUpdateSet Set<TABLE>(Column.NIntegerKeyColumn<TABLE> pColumn, Int32Key<TABLE> pValue);
		IUpdateSet Set<TABLE>(Column.NIntegerKeyColumn<TABLE> pColumn, Int32Key<TABLE>? pValue);
		IUpdateSet Set<TABLE>(Column.BigIntegerKeyColumn<TABLE> pColumn, Int64Key<TABLE> pValue);
		IUpdateSet Set<TABLE>(Column.NBigIntegerKeyColumn<TABLE> pColumn, Int64Key<TABLE> pValue);
		IUpdateSet Set<TABLE>(Column.NBigIntegerKeyColumn<TABLE> pColumn, Int64Key<TABLE>? pValue);
		IUpdateSet Set<TABLE>(Column.StringKeyColumn<TABLE> pColumn, StringKey<TABLE> pValue);
		IUpdateSet Set<TABLE>(Column.StringKeyColumn<TABLE> pColumn, StringKey<TABLE>? pValue);

		#region Hide Members
		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		bool Equals(object pObject);
		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		int GetHashCode();
		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		Type GetType();
		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		string ToString();
		#endregion
	}

	public interface IUpdateSet : IUpdateJoin {
		
		IUpdateSet Set<COLUMN>(COLUMN pColumn, COLUMN pValue) where COLUMN : AColumn;
		
		IUpdateSet Set(Column.SmallIntegerColumn pColumn, Int16 pValue);
		IUpdateSet Set(Column.NSmallIntegerColumn pColumn, Int16? pValue);
		IUpdateSet Set(Column.IntegerColumn pColumn, int pValue);
		IUpdateSet Set(Column.NIntegerColumn pColumn, int? pValue);
		IUpdateSet Set(Column.BigIntegerColumn pColumn, Int64 pValue);
		IUpdateSet Set(Column.NBigIntegerColumn pColumn, Int64? pValue);
		IUpdateSet Set(Column.StringColumn pColumn, string pValue);
		IUpdateSet Set(Column.DecimalColumn pColumn, decimal pValue);
		IUpdateSet Set(Column.NDecimalColumn pColumn, decimal? pValue);		
		IUpdateSet Set(Column.DateTimeColumn pColumn, DateTime pValue);
		IUpdateSet Set(Column.DateTimeColumn pColumn, Function.CurrentDateTime pValue);
		IUpdateSet Set(Column.NDateTimeColumn pColumn, DateTime? pValue);
		IUpdateSet Set(Column.NDateTimeColumn pColumn, Function.CurrentDateTime pValue);
		
		IUpdateSet Set(Column.DateTime2Column pColumn, DateTime pValue);
		IUpdateSet Set(Column.DateTime2Column pColumn, Function.CurrentDateTime pValue);		
		IUpdateSet Set(Column.NDateTime2Column pColumn, DateTime? pValue);
		IUpdateSet Set(Column.NDateTime2Column pColumn, Function.CurrentDateTime pValue);
		
		IUpdateSet Set(Column.DateTimeOffsetColumn pColumn, DateTimeOffset pValue);
		IUpdateSet Set(Column.DateTimeOffsetColumn pColumn, Function.CurrentDateTimeOffset pValue);		
		IUpdateSet Set(Column.NDateTimeOffsetColumn pColumn, DateTimeOffset? pValue);
		IUpdateSet Set(Column.NDateTimeOffsetColumn pColumn, Function.CurrentDateTimeOffset pValue);
		
		IUpdateSet Set(Column.BoolColumn pColumn, bool pValue);
		IUpdateSet Set(Column.NBoolColumn pColumn, bool? pValue);
		IUpdateSet Set(Column.GuidColumn pColumn, Guid pValue);
		IUpdateSet Set(Column.NGuidColumn pColumn, Guid? pValue);
		IUpdateSet Set(Column.BinaryColumn pColumn, byte[] pValue);
		IUpdateSet Set(Column.NBinaryColumn pColumn, byte[] pValue);
		
		IUpdateSet Set<TABLE>(Column.GuidKeyColumn<TABLE> pColumn, GuidKey<TABLE> pValue);
		IUpdateSet Set<TABLE>(Column.NGuidKeyColumn<TABLE> pColumn, GuidKey<TABLE> pValue);
		IUpdateSet Set<TABLE>(Column.NGuidKeyColumn<TABLE> pColumn, GuidKey<TABLE>? pValue);
		IUpdateSet Set<TABLE>(Column.SmallIntegerKeyColumn<TABLE> pColumn, Int16Key<TABLE> pValue);
		IUpdateSet Set<TABLE>(Column.NSmallIntegerKeyColumn<TABLE> pColumn, Int16Key<TABLE> pValue);
		IUpdateSet Set<TABLE>(Column.NSmallIntegerKeyColumn<TABLE> pColumn, Int16Key<TABLE>? pValue);
		IUpdateSet Set<TABLE>(Column.IntegerKeyColumn<TABLE> pColumn, Int32Key<TABLE> pValue);
		IUpdateSet Set<TABLE>(Column.NIntegerKeyColumn<TABLE> pColumn, Int32Key<TABLE> pValue);
		IUpdateSet Set<TABLE>(Column.NIntegerKeyColumn<TABLE> pColumn, Int32Key<TABLE>? pValue);
		IUpdateSet Set<TABLE>(Column.BigIntegerKeyColumn<TABLE> pColumn, Int64Key<TABLE> pValue);
		IUpdateSet Set<TABLE>(Column.NBigIntegerKeyColumn<TABLE> pColumn, Int64Key<TABLE> pValue);
		IUpdateSet Set<TABLE>(Column.NBigIntegerKeyColumn<TABLE> pColumn, Int64Key<TABLE>? pValue);
		IUpdateSet Set<TABLE>(Column.StringKeyColumn<TABLE> pColumn, StringKey<TABLE> pValue);
		IUpdateSet Set<TABLE>(Column.StringKeyColumn<TABLE> pColumn, StringKey<TABLE>? pValue);
	}

	public interface IUpdateJoin : IUpdateWhere {
		IUpdateJoin Join(ATable pTable, Condition pCondition);
	}

	public interface IUpdateWhere {
		IUpdateUseParams NoWhereCondition();
		IUpdateUseParams Where(Condition pCondition);
		
		#region Hide Members
		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		bool Equals(object pObject);
		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		int GetHashCode();
		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		Type GetType();
		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		string ToString();
		#endregion
	}
	
	public interface IUpdateUseParams : IUpdateTimeout {
		
		/// <summary>
		/// Force the query to use parameters or not. If not set then the default is used from Sql.Settings.UseParameters
		/// </summary>
		/// <param name="pUseParameters"></param>
		/// <returns></returns>
		IUpdateTimeout UseParameters(bool pUseParameters);
		
		#region Hide Members
		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		new bool Equals(object pObject);
		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		new int GetHashCode();
		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		new Type GetType();
		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		new string ToString();
		#endregion
	}
	
	public interface IUpdateTimeout : IUpdateReturning {
		
		/// <summary>
		/// Set query timeout. Overrides the default in Settings.DefaultTimeout
		/// </summary>
		/// <param name="pSeconds"></param>
		/// <returns></returns>
		IUpdateExecute Timeout(int pSeconds);
		
		#region Hide Members
		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		new bool Equals(object pObject);
		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		new int GetHashCode();
		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		new Type GetType();
		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		new string ToString();
		#endregion
	}

	public interface IUpdateReturning : IUpdateExecute {
		
		IUpdateExecute Returning(params AColumn[] pColumns);
		
		#region Hide Members
		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		new bool Equals(object pObject);
		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		new int GetHashCode();
		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		new Type GetType();
		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		new string ToString();
		#endregion
	}
	public interface IUpdateExecute {
		
		string GetSql(ADatabase pDatabase);
		IResult Execute(Transaction pTransaction);
		
		#region Hide Members
		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		bool Equals(object pObject);
		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		int GetHashCode();
		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		Type GetType();
		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		string ToString();
		#endregion
	}
}