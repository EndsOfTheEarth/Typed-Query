
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
using System.Data.SqlClient;
using System.Data;
using Sql.Interfaces;

namespace Sql {

	public static class Query {

		/// <summary>
		/// Select query
		/// </summary>
		/// <param name="pField">Column, table or function to select. Cannot be null.</param>
		/// <param name="pFields">Column, table or function to select</param>
		/// <returns></returns>
		public static IDistinct Select(ISelectableColumns pField, params ISelectableColumns[] pFields) {
			
			if(pField == null)
				throw new NullReferenceException($"{nameof(pField)} cannot be null");
			
			List<ISelectable> selectList = new List<ISelectable>();
			selectList.AddRange(pField.SelectableColumns);
			
			foreach(ISelectableColumns selectableFields in pFields)
				selectList.AddRange(selectableFields.SelectableColumns);
			
			return new Core.QueryBuilder(selectList.ToArray());
		}
		
		/// <summary>
		/// Insert query
		/// </summary>
		/// <param name="pTable">Insert into table</param>
		/// <returns></returns>
		public static IInsert Insert(ATable pTable) {
			return new Core.InsertBuilder(pTable);
		}
		
		/// <summary>
		/// Insert select query
		/// </summary>
		/// <param name="pTable"></param>
		/// <returns></returns>
		public static IInsertSelect InsertSelect(ATable pTable) {
			return new Core.InsertSelectBuilder(pTable);
		}
		
		/// <summary>
		/// Update query
		/// </summary>
		/// <param name="pTable">Update table</param>
		/// <returns></returns>
		public static IUpdate Update(ATable pTable) {
			return new Core.UpdateBuilder(pTable);
		}
		
		/// <summary>
		/// Delete query
		/// </summary>
		/// <param name="pTable">Delete table</param>
		/// <returns></returns>
		public static IDelete Delete(ATable pTable) {
			return new Core.DeleteBuilder(pTable);
		}

		/// <summary>
		/// Truncate table query
		/// </summary>
		/// <param name="pTable">Table to be truncated</param>
		/// <returns></returns>
		public static ITruncateTimeout Truncate(ATable pTable) {
			return new Core.TruncateBuilder(pTable);
		}
		
		/// <summary>
		/// Gets sql to execute stored procedure
		/// </summary>
		/// <param name="pSP"></param>
		/// <param name="pTransaction"></param>
		/// <param name="pParameters"></param>
		/// <returns></returns>
		public static string GetSpSql(ATable pSP, Transaction pTransaction, params object[] pParameters) {
			
			if (pSP == null)
				throw new NullReferenceException($"{nameof(pSP)} cannot be null");

			if (pTransaction.Database.DatabaseType != DatabaseType.Mssql)
				throw new Exception($"Stored procedures are only implemented for DatabaseType.Mssql. Not {pTransaction.Database.DatabaseType.ToString()}");

			Sql.Database.IAliasManager aliasManager = new Sql.Database.AliasManager();
			return Sql.Database.SqlServer.GenerateSql.GetStoreProcedureQuery(pTransaction.Database, pSP, null, pParameters, aliasManager);
		}
		
		/// <summary>
		/// Executes stored procedure.
		/// </summary>
		/// <param name="pSP">Stored procedure</param>
		/// <param name="pTransaction">Transaction</param>
		/// <param name="pParameters">Parameters being passed to stored procedure</param>
		/// <returns></returns>
		public static IResult ExecuteSP(ATable pSP, Transaction pTransaction, params object[] pParameters) {

			if (pSP == null)
				throw new NullReferenceException($"{nameof(pSP)} cannot be null");
			
			if(pTransaction == null)
				throw new NullReferenceException($"{nameof(pTransaction)} cannot be null");

			if (pTransaction.Database.DatabaseType != DatabaseType.Mssql)
				throw new Exception($"Stored procedures are only implemented for DatabaseType.Mssql. Not {pTransaction.Database.DatabaseType.ToString()}");

			System.Data.Common.DbConnection connection = null;
			
			string sql = string.Empty;
			DateTime? start = null;
			DateTime? end = null;

			try {

				connection = pTransaction.GetOrSetConnection(pTransaction.Database);

				using(System.Data.Common.DbCommand command = Transaction.CreateCommand(connection, pTransaction)) {

					Core.Parameters parameters = Settings.UseParameters ? new Core.Parameters(command) : null;

					sql = Database.GenertateSql.GetStoreProcedureQuery(pTransaction.Database, pSP, parameters, pParameters);
					
					command.CommandText = sql;
					command.CommandType = System.Data.CommandType.Text;

					if (pTransaction != null)
						command.Transaction = pTransaction.GetOrSetDbTransaction(pTransaction.Database);

					start = DateTime.Now;
					Settings.FireQueryExecutingEvent(pTransaction.Database, sql, QueryType.StoredProc, start, pTransaction.IsolationLevel, pTransaction.Id);
					
					using(System.Data.Common.DbDataReader reader = command.ExecuteReader()) {

						end = DateTime.Now;

						IList<ISelectable> selectList = new List<ISelectable>(pSP.Columns.Count);

						foreach (AColumn column in pSP.Columns)
							selectList.Add(column);

						Core.QueryResult result = new Core.QueryResult(pTransaction.Database, selectList, reader, command.CommandText);

						if (pTransaction == null)
							connection.Close();

						Settings.FireQueryPerformedEvent(pTransaction.Database, sql, result.Count, QueryType.StoredProc, start, end, null, pTransaction.IsolationLevel, result, pTransaction.Id);

						return result;
					}
				}
			}
			catch (Exception e) {
				if (connection != null && connection.State != System.Data.ConnectionState.Closed)
					connection.Close();				
				Settings.FireQueryPerformedEvent(pTransaction.Database, sql, 0, QueryType.StoredProc, start, end, e, pTransaction.IsolationLevel, null, pTransaction.Id);
				throw;
			}
		}

		/// <summary>
		/// Executes non query in pSql
		/// </summary>
		/// <param name="pSql">Plain text sql query to be executed</param>
		/// <param name="pDatabase">Database to execute query on</param>
		/// <param name="pTransaction">Transaction to execute query in</param>
		/// <returns></returns>
		public static int ExecuteNonQuery(string pSql, ADatabase pDatabase, Transaction pTransaction) {

			if (string.IsNullOrWhiteSpace(pSql))
				throw new Exception($"{nameof(pSql)} cannot be null or empty");

			if (pDatabase == null)
				throw new NullReferenceException($"{nameof(pDatabase)} cannot be null");

			if (pTransaction == null)
				throw new NullReferenceException($"{nameof(pTransaction)} cannot be null");

			System.Data.Common.DbConnection connection = null;
			
			DateTime? start = null;
			DateTime? end = null;
			
			try {

				connection = pTransaction.GetOrSetConnection(pDatabase);

				using(System.Data.Common.DbCommand command = Transaction.CreateCommand(connection, pTransaction)) {

					command.CommandText = pSql;
					command.CommandType = System.Data.CommandType.Text;

					if (pTransaction != null)
						command.Transaction = pTransaction.GetOrSetDbTransaction(pDatabase);

					start = DateTime.Now;
					Settings.FireQueryExecutingEvent(pDatabase, pSql, QueryType.PlainText, start, pTransaction.IsolationLevel, pTransaction.Id);

					int rowsEffected = command.ExecuteNonQuery();

					end = DateTime.Now;

					Settings.FireQueryPerformedEvent(pDatabase, pSql, rowsEffected, QueryType.PlainText, start, end, null, pTransaction.IsolationLevel, null, pTransaction.Id);

					return rowsEffected;
				}
			}
			catch (Exception e) {
				if (connection != null && connection.State != System.Data.ConnectionState.Closed)
					connection.Close();
				Settings.FireQueryPerformedEvent(pDatabase, pSql, 0, QueryType.PlainText, start, end, e, pTransaction.IsolationLevel, null, pTransaction.Id);
				throw;
			}
		}
	}
}