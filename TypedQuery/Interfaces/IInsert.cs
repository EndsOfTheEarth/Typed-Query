
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

namespace Sql.Interfaces {

	public interface IInsert {

		IInsertSet Set(Column.SmallIntegerColumn pColumn, Int16 pValue);
		IInsertSet Set(Column.NSmallIntegerColumn pColumn, Int16? pValue);
		IInsertSet Set(Column.IntegerColumn pColumn, int pValue);
		IInsertSet Set(Column.NIntegerColumn pColumn, int? pValue);		
		IInsertSet Set(Column.BigIntegerColumn pColumn, Int64 pValue);
		IInsertSet Set(Column.NBigIntegerColumn pColumn, Int64? pValue);		
		IInsertSet Set(Column.StringColumn pColumn, string pValue);
		IInsertSet Set(Column.DecimalColumn pColumn, decimal pValue);
		IInsertSet Set(Column.NDecimalColumn pColumn, decimal? pValue);
		IInsertSet Set(Column.DateTimeColumn pColumn, DateTime pValue);
		IInsertSet Set(Column.DateTimeColumn pColumn, Function.CurrentDateTime pValue);
		IInsertSet Set(Column.NDateTimeColumn pColumn, DateTime? pValue);
		IInsertSet Set(Column.NDateTimeColumn pColumn, Function.CurrentDateTime pValue);
		
		IInsertSet Set(Column.DateTime2Column pColumn, DateTime pValue);
		IInsertSet Set(Column.DateTime2Column pColumn, Function.CurrentDateTime pValue);
		IInsertSet Set(Column.NDateTime2Column pColumn, DateTime? pValue);
		IInsertSet Set(Column.NDateTime2Column pColumn, Function.CurrentDateTime pValue);
		
		IInsertSet Set(Column.DateTimeOffsetColumn pColumn, DateTimeOffset pValue);
		IInsertSet Set(Column.DateTimeOffsetColumn pColumn, Function.CurrentDateTimeOffset pValue);
		
		IInsertSet Set(Column.NDateTimeOffsetColumn pColumn, DateTimeOffset? pValue);
		IInsertSet Set(Column.NDateTimeOffsetColumn pColumn, Function.CurrentDateTimeOffset pValue);
		
		IInsertSet Set(Column.BoolColumn pColumn, bool pValue);
		IInsertSet Set(Column.NBoolColumn pColumn, bool? pValue);
		IInsertSet Set(Column.GuidColumn pColumn, Guid pValue);
		IInsertSet Set(Column.NGuidColumn pColumn, Guid? pValue);
		IInsertSet Set(Column.BinaryColumn pColumn, byte[] pValue);
		IInsertSet Set(Column.NBinaryColumn pColumn, byte[] pValue);
		IInsertSet Set<ENUM>(Column.EnumColumn<ENUM> pColumn, ENUM pValue);
		IInsertSet Set(AColumn pColumn, Sql.Function.CustomSql pValue);
		
		IInsertSet Set<TABLE>(Column.GuidKeyColumn<TABLE> pColumn, Guid pValue) where TABLE : Sql.ATable;
		IInsertSet Set<TABLE>(Column.NGuidKeyColumn<TABLE> pColumn, Guid? pValue) where TABLE : Sql.ATable;
		IInsertSet Set<TABLE>(Column.SmallIntegerKeyColumn<TABLE> pColumn, Int16 pValue) where TABLE : Sql.ATable;
		IInsertSet Set<TABLE>(Column.NSmallIntegerKeyColumn<TABLE> pColumn, Int16? pValue) where TABLE : Sql.ATable;
		IInsertSet Set<TABLE>(Column.IntegerKeyColumn<TABLE> pColumn, int pValue) where TABLE : Sql.ATable;
		IInsertSet Set<TABLE>(Column.NIntegerKeyColumn<TABLE> pColumn, int? pValue) where TABLE : Sql.ATable;
		IInsertSet Set<TABLE>(Column.BigIntegerKeyColumn<TABLE> pColumn, Int64 pValue) where TABLE : Sql.ATable;
		IInsertSet Set<TABLE>(Column.NBigIntegerKeyColumn<TABLE> pColumn, Int64? pValue) where TABLE : Sql.ATable;
		IInsertSet Set<TABLE>(Column.StringKeyColumn<TABLE> pColumn, string pValue) where TABLE : Sql.ATable;
		
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

	public interface IInsertSet : IInsertUseParams {
		
		IInsertSet Set(Column.SmallIntegerColumn pColumn, Int16 pValue);
		IInsertSet Set(Column.NSmallIntegerColumn pColumn, Int16? pValue);
		IInsertSet Set(Column.IntegerColumn pColumn, int pValue);
		IInsertSet Set(Column.NIntegerColumn pColumn, int? pValue);		
		IInsertSet Set(Column.BigIntegerColumn pColumn, Int64 pValue);
		IInsertSet Set(Column.NBigIntegerColumn pColumn, Int64? pValue);
		IInsertSet Set(Column.StringColumn pColumn, string pValue);
		IInsertSet Set(Column.DecimalColumn pColumn, decimal pValue);
		IInsertSet Set(Column.NDecimalColumn pColumn, decimal? pValue);
		IInsertSet Set(Column.DateTimeColumn pColumn, DateTime pValue);
		IInsertSet Set(Column.DateTimeColumn pColumn, Function.CurrentDateTime pValue);
		IInsertSet Set(Column.NDateTimeColumn pColumn, DateTime? pValue);
		IInsertSet Set(Column.NDateTimeColumn pColumn, Function.CurrentDateTime pValue);
		
		IInsertSet Set(Column.DateTime2Column pColumn, DateTime pValue);
		IInsertSet Set(Column.DateTime2Column pColumn, Function.CurrentDateTime pValue);
		IInsertSet Set(Column.NDateTime2Column pColumn, DateTime? pValue);
		IInsertSet Set(Column.NDateTime2Column pColumn, Function.CurrentDateTime pValue);
		
		IInsertSet Set(Column.DateTimeOffsetColumn pColumn, DateTimeOffset pValue);
		IInsertSet Set(Column.DateTimeOffsetColumn pColumn, Function.CurrentDateTimeOffset pValue);
		
		IInsertSet Set(Column.NDateTimeOffsetColumn pColumn, DateTimeOffset? pValue);
		IInsertSet Set(Column.NDateTimeOffsetColumn pColumn, Function.CurrentDateTimeOffset pValue);
		
		IInsertSet Set(Column.BoolColumn pColumn, bool pValue);
		IInsertSet Set(Column.NBoolColumn pColumn, bool? pValue);
		IInsertSet Set(Column.GuidColumn pColumn, Guid pValue);
		IInsertSet Set(Column.NGuidColumn pColumn, Guid? pValue);
		IInsertSet Set(Column.BinaryColumn pColumn, byte[] pValue);
		IInsertSet Set(Column.NBinaryColumn pColumn, byte[] pValue);
		IInsertSet Set<ENUM>(Column.EnumColumn<ENUM> pColumn, ENUM pValue);
		IInsertSet Set(AColumn pColumn, Sql.Function.CustomSql pValue);
		
		IInsertSet Set<TABLE>(Column.GuidKeyColumn<TABLE> pColumn, Guid pValue) where TABLE : Sql.ATable;
		IInsertSet Set<TABLE>(Column.NGuidKeyColumn<TABLE> pColumn, Guid? pValue) where TABLE : Sql.ATable;
		IInsertSet Set<TABLE>(Column.SmallIntegerKeyColumn<TABLE> pColumn, Int16 pValue) where TABLE : Sql.ATable;
		IInsertSet Set<TABLE>(Column.NSmallIntegerKeyColumn<TABLE> pColumn, Int16? pValue) where TABLE : Sql.ATable;
		IInsertSet Set<TABLE>(Column.IntegerKeyColumn<TABLE> pColumn, int pValue) where TABLE : Sql.ATable;
		IInsertSet Set<TABLE>(Column.NIntegerKeyColumn<TABLE> pColumn, int? pValue) where TABLE : Sql.ATable;
		IInsertSet Set<TABLE>(Column.BigIntegerKeyColumn<TABLE> pColumn, Int64 pValue) where TABLE : Sql.ATable;
		IInsertSet Set<TABLE>(Column.NBigIntegerKeyColumn<TABLE> pColumn, Int64? pValue) where TABLE : Sql.ATable;
		IInsertSet Set<TABLE>(Column.StringKeyColumn<TABLE> pColumn, string pValue) where TABLE : Sql.ATable;
	}
	
	public interface IInsertUseParams : IInsertTimeout {
		
		/// <summary>
		/// Force the query to use parameters or not. If not set then the default is used from Sql.Settings.UseParameters
		/// </summary>
		/// <param name="pUseParameters"></param>
		/// <returns></returns>
		IInsertTimeout UseParameters(bool pUseParameters);
		
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
	
	public interface IInsertTimeout : IReturning {
		
		/// <summary>
		/// Set query timeout. Overrides the default in Settings.DefaultTimeout
		/// </summary>
		/// <param name="pSeconds"></param>
		/// <returns></returns>
		IInsertExecute Timeout(int pSeconds);
		
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

	public interface IReturning : IInsertExecute {
		
		IInsertExecute Returning(params AColumn[] pColumns);
		
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
	public interface IInsertExecute {
		
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