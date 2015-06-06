
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
using Sql.Interfaces;
using System.Diagnostics;

namespace Sql.Core {

	internal class InsertSelectBuilder : IInsertSelect, IInsertSelectQuery, IInsertSelectExecute {
		
		private readonly ATable mTable;
		private AColumn[] mColumns;
		private IExecute mSelectQuery;
		
		internal ATable Table {
			get { return mTable; }
		}
		internal AColumn[] InsertColumns {
			get { return mColumns; }
		}
		internal IExecute SelectQuery {
			get { return mSelectQuery; }
		}
		
		public InsertSelectBuilder(ATable pTable) {
			
			if(pTable == null)
				throw new NullReferenceException("pTable cannot be null");
			
			mTable = pTable;
		}
		
		public IInsertSelectQuery Columns(params AColumn[] pColumns) {
			
			if(pColumns == null)
				throw new NullReferenceException("pColumns cannot be null");
			
			if(pColumns.Length == 0)
				throw new Exception("pColumns cannot be empty");
			
			mColumns = pColumns;
			return this;
		}
		
		public IInsertSelectExecute Query(IExecute pSelectQuery) {
			
			if(pSelectQuery == null)
				throw new NullReferenceException("pSelectQuery cannot be null");
			
			mSelectQuery = pSelectQuery;
			return this;
		}
		
		public string GetSql(ADatabase pDatabase) {
			return Database.GenertateSql.GetInsertSelectQuery(pDatabase, this, null);
		}
		
		private string GetSql(ADatabase pDatabase, Core.Parameters pParameters) {			
			return Database.GenertateSql.GetInsertSelectQuery(pDatabase, this, pParameters);
		}
		
		public int Execute(Transaction pTransaction) {
			
			if (pTransaction == null)
				throw new NullReferenceException("pTransaction cannot be null");

			if(Sql.Settings.BreakOnInsertSelectQuery) {
				if(Debugger.IsAttached) {
					Debugger.Break();
				}
			}
			
			System.Data.Common.DbConnection connection = null;

			string sql = string.Empty;
			DateTime? start = null;
			DateTime? end = null;
			
			try {
				
				connection = pTransaction.GetOrSetConnection(pTransaction.Database);

				using(System.Data.Common.DbCommand command = Transaction.CreateCommand(connection, pTransaction)){

					Parameters parameters = Settings.UseParameters ? new Parameters(command) : null;
	
					sql = GetSql(pTransaction.Database, parameters);
					
					command.CommandText = sql;
					command.CommandType = System.Data.CommandType.Text;
					command.CommandTimeout = Settings.DefaultTimeout;
					command.Transaction = pTransaction.GetOrSetDbTransaction(pTransaction.Database);
					
					start = DateTime.Now;
					Settings.FireQueryExecutingEvent(pTransaction.Database, sql, QueryType.Insert, start, pTransaction.IsolationLevel, pTransaction.Id);
					int returnValue = command.ExecuteNonQuery();
	
					end = DateTime.Now;
	
					Settings.FireQueryPerformedEvent(pTransaction.Database, sql, returnValue, QueryType.Insert, start, end, null, pTransaction.IsolationLevel, null, pTransaction.Id);
	
					return returnValue;
				}
			}
			catch (Exception e) {
				if (connection != null && connection.State != System.Data.ConnectionState.Closed)
					connection.Close();
				Settings.FireQueryPerformedEvent(pTransaction.Database, sql, 0, QueryType.Insert, start, end, e, pTransaction.IsolationLevel, null, pTransaction.Id);
				throw;
			}
		}
	}
}