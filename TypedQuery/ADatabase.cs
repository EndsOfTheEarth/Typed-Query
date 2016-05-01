
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
using Sql.Interfaces;

namespace Sql {

	/// <summary>
	/// Abstract database. Represents a database. Provides methods to create connections.
	/// </summary>
	public abstract class ADatabase {

		private readonly string mDatabaseName;
		private readonly DatabaseType mDatabaseType;
		
		protected ADatabase(string pDatabaseName, DatabaseType pDatabaseType) {
			
			if(string.IsNullOrWhiteSpace(pDatabaseName))
				throw new Exception($"{nameof(pDatabaseName)} cannot be null or empty");
			
			if(!DatabaseType.IsDefined(typeof(DatabaseType), pDatabaseType))	//Check for incorrect enum value e.g. being cast from an int value like zero.
				throw new Exception($"Unknown {nameof(pDatabaseType)}. Value = {pDatabaseType.ToString()}");
			
			mDatabaseName = pDatabaseName;
			mDatabaseType = pDatabaseType;
		}

		/// <summary>
		/// Returns database connection string
		/// </summary>
		protected abstract string ConnectionString { get; }

		/// <summary>
		/// Returns connection to database.
		/// 
		/// The parameter pCanBeReadonly indicates that the connection is allowed to be readonly. This is can be used as a security feature.
		/// </summary>
		/// <param name="pCanBeReadonly">If true then the returned connection is allowed to be readonly</param>
		/// <returns></returns>
		/// 
		public abstract System.Data.Common.DbConnection GetConnection(bool pCanBeReadonly);
		
		/// <summary>
		/// Name of database
		/// </summary>
		public string DatabaseName {
			get { return mDatabaseName; }
		}
		/// <summary>
		/// Database type
		/// </summary>
		public DatabaseType DatabaseType {
			get { return mDatabaseType; }
		}
	}

	/// <summary>
	/// Database type
	/// </summary>
	public enum DatabaseType {
		/// <summary>
		/// PostgreSql database
		/// </summary>
		PostgreSql = 1,
		
		/// <summary>
		/// Sql Server database
		/// </summary>
		Mssql = 2
	}
}