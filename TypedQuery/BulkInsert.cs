
using System;
using System.Text;
using System.Collections.Generic;
using Sql.Interfaces;
using Sql.Core;

namespace Sql {
	
	public class BulkInsert {
		
		private List<InsertBuilder> mValues = new List<InsertBuilder>();
		private int? mTimeout = null;
		
		public int? Timeout {
			get { return mTimeout; }
			set {
				
				if(value != null && value <= 0)
					throw new Exception("Timeout can not be <= 0. Value = " + value.ToString());
				
				mTimeout = value;
			}
		}
		public BulkInsert() {
			
		}
		
		public void AddValues(IInsertSet pInsertValues) {
			
			if(pInsertValues == null)
				throw new NullReferenceException("pInsertValues can not be null");
			
			mValues.Add((InsertBuilder)pInsertValues);
		}
		
		public int Execute(Transaction pTransaction) {
					
			if(pTransaction == null)
				throw new NullReferenceException("pTransaction can not be null");
			
			System.Data.Common.DbConnection connection = null;
			
			string sql = string.Empty;
			DateTime? start = null;
			DateTime? end = null;
			
			try {
				
				connection = pTransaction.GetOrSetConnection(pTransaction.Database);

				using(System.Data.Common.DbCommand command = Transaction.CreateCommand(connection, pTransaction)){
	
					sql = Database.GenertateSql.GetBulkInsertQuery(pTransaction.Database, mValues);
					
					command.CommandText = sql;
					command.CommandType = System.Data.CommandType.Text;
					command.CommandTimeout = mTimeout != null ? mTimeout.Value : Settings.DefaultTimeout;
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