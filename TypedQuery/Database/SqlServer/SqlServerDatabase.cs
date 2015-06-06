
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
using System.Data.SqlClient;

namespace Sql.Database.SqlServer {
	
	public sealed class SqlServerDatabase : Sql.ADatabase {
		
		private readonly string mConnectionString;
		
		public SqlServerDatabase(String pDbTitle, string pServerName, string pDbName, string pUserName, string pPassword, bool pEncrypt) : base(pDbTitle, DatabaseType.Mssql) {
			
			if(string.IsNullOrEmpty(pServerName))
				throw new Exception("pServerName cannot be null or empty");
			
			if(string.IsNullOrEmpty(pDbName))
				throw new Exception("pDbName cannot be null or empty");
			
			if(string.IsNullOrEmpty(pUserName))
				throw new Exception("pUserName cannot be null or empty");
			
			if(string.IsNullOrEmpty(pPassword))
				throw new Exception("pPassword cannot be null or empty");
			
			SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
			
			builder.DataSource = pServerName;
			builder.InitialCatalog = pDbName;
			builder.UserID = pUserName;
			builder.Password = pPassword;
			builder.Encrypt = pEncrypt;
			
			mConnectionString = builder.ConnectionString;
		}
		
		protected override string ConnectionString {
			get { return mConnectionString; }
		}
		
		public string GetConnectionString() {
			return mConnectionString;
		}
		
		public override System.Data.Common.DbConnection GetConnection(bool pCanBeReadonly) {
			SqlConnection connection = new SqlConnection(ConnectionString);
			connection.Open();
			return connection;
		}
	}
}