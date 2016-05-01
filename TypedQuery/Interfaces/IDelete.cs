
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

	public interface IDelete {
		
		IDeleteUseParams NoWhereCondition { get; }
		IDeleteUseParams Where(Condition pCondition);
		
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

	public interface IDeleteUseParams : IDeleteTimeout {
		
		/// <summary>
		/// Force the query to use parameters or not. If not set then the default is used from Sql.Settings.UseParameters
		/// </summary>
		/// <param name="pUseParameters"></param>
		/// <returns></returns>
		IDeleteTimeout UseParameters(bool pUseParameters);
		
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
	
	public interface IDeleteTimeout : IDeleteReturning {
		
		/// <summary>
		/// Set query timeout. Overrides the default in Settings.DefaultTimeout
		/// </summary>
		/// <param name="pSeconds"></param>
		/// <returns></returns>
		IDeleteReturning Timeout(int pSeconds);
		
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
	
	public interface IDeleteReturning : IDeleteExecute {
		
		IDeleteExecute Returning(params AColumn[] pColumns);
		
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
	public interface IDeleteExecute {
		
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