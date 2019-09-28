
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
using Sql.Interfaces;
using System.Diagnostics;

namespace Sql.Core {

    internal class TruncateBuilder : ITruncateTimeout, ITrucateExecute {

        private readonly ATable mTable;
        private int? mTimeout;

        internal TruncateBuilder(ATable pTable) {

            if(pTable == null) {
                throw new NullReferenceException($"{ nameof(pTable) } cannot be null");
            }
            mTable = pTable;
        }

        internal ATable Table {
            get { return mTable; }
        }

        public ITrucateExecute Timeout(int pTimeout) {

            if(pTimeout < 0) {
                throw new Exception($"{ nameof(pTimeout) } cannot be less than 0. Value = { pTimeout.ToString() }");
            }
            mTimeout = pTimeout;
            return this;
        }

        public string GetSql(Sql.ADatabase pDatabase) {

            if(pDatabase == null) {
                throw new NullReferenceException($"{ nameof(pDatabase) } cannot be null or empty");
            }
            return Database.GenertateSql.GetTruncateQuery(pDatabase, mTable);
        }
        public int Execute(Transaction pTransaction) {

            if(pTransaction == null) {
                throw new NullReferenceException($"{ nameof(pTransaction) } cannot be null");
            }

            if(Sql.Settings.BreakOnTruncateQuery) {

                if(Debugger.IsAttached) {
                    Debugger.Break();
                }
            }

            string sql = GetSql(pTransaction.Database);

            DateTime? start = null;
            DateTime? end = null;

            System.Data.Common.DbConnection? connection = null;

            try {

                connection = pTransaction.GetOrSetConnection(pTransaction.Database);

                using(System.Data.Common.DbCommand command = Transaction.CreateCommand(connection, pTransaction)) {

                    command.CommandText = sql;

                    command.CommandType = System.Data.CommandType.Text;
                    command.CommandTimeout = mTimeout != null ? mTimeout.Value : Settings.DefaultTimeout;

                    command.Transaction = pTransaction.GetOrSetDbTransaction(pTransaction.Database);

                    start = DateTime.Now;
                    Settings.FireQueryExecutingEvent(pTransaction.Database, sql, QueryType.Truncate, start, pTransaction.IsolationLevel, pTransaction.Id);

                    int returnValue = command.ExecuteNonQuery();

                    end = DateTime.Now;
                    Settings.FireQueryPerformedEvent(pTransaction.Database, sql, returnValue, QueryType.Truncate, start, end, null, pTransaction.IsolationLevel, null, pTransaction.Id);

                    return returnValue;
                }
            }
            catch(Exception e) {

                if(connection != null && connection.State != System.Data.ConnectionState.Closed) {
                    connection.Close();
                }
                Settings.FireQueryPerformedEvent(pTransaction.Database, sql, 0, QueryType.Truncate, start, end, e, pTransaction.IsolationLevel, null, pTransaction.Id);
                throw;
            }
        }
    }
}