
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
using System.Data.SqlClient;

namespace SqlServer {

	public class SqlServerDatabase : Sql.ADatabase {

		public readonly static SqlServerDatabase Instance = new SqlServerDatabase("abc");

		private string mConnectionString;

		public SqlServerDatabase(string pConnectionString) : base("<<db>>", Sql.DatabaseType.Mssql) {

			if(string.IsNullOrEmpty(pConnectionString))
				throw new Exception("pConnectionString cannot be null or empty");

			mConnectionString = pConnectionString;
		}

		public void SetConnectionString(string pConnectionString) {
			mConnectionString = pConnectionString;
		}

		public override System.Data.Common.DbConnection GetConnection(bool pCanBeReadonly) {
			SqlConnection connection = new SqlConnection(mConnectionString);
			connection.Open();
			return connection;
		}
	}
}