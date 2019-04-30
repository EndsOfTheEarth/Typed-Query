
/*
 * 
 * Copyright (C) 2009-2019 JFo.nz
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
using Npgsql;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Sql {
	
	public class DB : Sql.ADatabase {

		public readonly static Sql.ADatabase TestDB = new DB();

		//private DB() : base("Application", Sql.DatabaseType.Mssql) {
		private DB()
			: base("Application", Sql.DatabaseType.PostgreSql) {
		}
		
		static DB() {
			Sql.Settings.QueryExecuting += new Sql.Settings.QueryExecutingDelegate(Sql_Settings_QueryExecutingDelegate);
			Sql.Settings.QueryPerformed += new Sql.Settings.QueryPerformedDelegate(Settings_QueryPerformed);
		}

		public static void Sql_Settings_QueryExecutingDelegate(ADatabase pDatabase, string pSql, QueryType pQueryType, Nullable<DateTime> pStart, System.Data.IsolationLevel pIsolationLevel, Nullable<ulong> pTransactionId) {
			
			Assert.IsNotNull(pDatabase);
			Assert.IsNotNull(pSql);
			Assert.IsNotNull(pQueryType);
			Assert.IsNotNull(pIsolationLevel);
		}
		
		public static void Settings_QueryPerformed(ADatabase pDatabase, string pSql, int pRows, QueryType pQueryType, DateTime? pStart, DateTime? pEnd, Exception pException, System.Data.IsolationLevel pIsolationLevel, int? pResultSize, ulong? pTransactionId) {
			
			Assert.IsNotNull(pDatabase);
			Assert.IsNotNull(pSql);
			Assert.IsNotNull(pQueryType);
			Assert.IsNotNull(pIsolationLevel);
		}

		private string ConnectionString {
			get {
				
				if(DatabaseType == DatabaseType.Mssql)
					return "server=localhost\\SQLEXPRESS;Trusted_Connection=yes;database=TQ_Test;connection timeout=30";    //user id=;password=;
                else if(DatabaseType == DatabaseType.PostgreSql)
					return "Server=127.0.0.1;Port=5432;Database=TQ_Test;User Id=postgres;Password=1;";
				throw new Exception("Unknown database type: " + DatabaseType.ToString());
			}
		}
		public override System.Data.Common.DbConnection GetConnection(bool pCanBeReadOnly) {
			
			lock(this) {
				if(DatabaseType == DatabaseType.Mssql) {
					SqlConnection connection = new SqlConnection(ConnectionString);
					connection.Open();
					return connection;
				}
				else if(DatabaseType == DatabaseType.PostgreSql) {
					Npgsql.NpgsqlConnection connection = new Npgsql.NpgsqlConnection(ConnectionString);
					connection.Open();
					return connection;
				}
				throw new Exception("Unknown database type: " + DatabaseType.ToString());
			}
		}
	}
}