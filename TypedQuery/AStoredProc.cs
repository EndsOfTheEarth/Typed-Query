
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
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Collections.Generic;

namespace Sql {
	
	public abstract class AStoredProc : ATable {
		
		public AStoredProc(ADatabase pDefaultDatabase, string pProcName, string pSchema, Type pRowType) : base(pDefaultDatabase, pProcName, pSchema, false, pRowType) {
			
		}
		
		protected IResult ExecuteProcedure(Transaction pTransaction, params SqlParameter[] pSqlParams) {
			return ExecuteProcedure(pTransaction, null, pSqlParams);			
		}
		
		protected IResult ExecuteProcedure(Transaction pTransaction, int? pTimeout, params SqlParameter[] pSqlParams) {
			
			if(pTransaction == null)
				throw new NullReferenceException($"{nameof(pTransaction)} cannot be null");
			
			System.Data.Common.DbConnection connection = null;
			
			string sql = string.Empty;
			DateTime? start = null;
			DateTime? end = null;
			
			try {
				
				connection = pTransaction.GetOrSetConnection(pTransaction.Database);
				
				using(System.Data.Common.DbCommand command = Transaction.CreateCommand(connection, pTransaction)){
					
					command.CommandText = TableName;
					command.CommandType = CommandType.StoredProcedure;
					command.CommandTimeout = pTimeout != null ? pTimeout.Value : Settings.DefaultTimeout;
					command.Transaction = pTransaction.GetOrSetDbTransaction(pTransaction.Database);
					
					command.Parameters.AddRange(pSqlParams);
					
					sql = command.CommandText;
					
					start = DateTime.Now;
					Settings.FireQueryExecutingEvent(pTransaction.Database, sql, QueryType.StoredProc, start, pTransaction.IsolationLevel, pTransaction.Id);
					
					int returnValue = 0;
					IResult result;
					
					if(RowType != null) {
						using(DbDataReader reader = command.ExecuteReader()) {
							result = new Core.QueryResult(pTransaction.Database, SelectableColumns, reader, command.CommandText);
							returnValue = result.Count;
						}
					}
					else {
						returnValue = command.ExecuteNonQuery();
						result = new Sql.Core.QueryResult(returnValue, sql);
					}
					
					end = DateTime.Now;
	
					Settings.FireQueryPerformedEvent(pTransaction.Database, sql, returnValue, QueryType.StoredProc, start, end, null, pTransaction.IsolationLevel, null, pTransaction.Id);
					
					return result;
				}
			}
			catch (Exception e) {
				if (connection != null && connection.State != System.Data.ConnectionState.Closed)
					connection.Close();
				Settings.FireQueryPerformedEvent(pTransaction.Database, sql, 0, QueryType.StoredProc, start, end, e, pTransaction.IsolationLevel, null, pTransaction.Id);
				throw;
			}
		}
	}
}