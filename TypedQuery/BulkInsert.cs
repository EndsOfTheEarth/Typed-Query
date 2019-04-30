
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

                if(value != null && value <= 0) {
                    throw new Exception($"Timeout cannot be <= 0. Value = {value.ToString()}");
                }
                mTimeout = value;
            }
        }
        public BulkInsert() {

        }

        public void AddValues(IInsertSet pInsertValues) {

            if(pInsertValues == null) {
                throw new NullReferenceException($"{nameof(pInsertValues)} cannot be null");
            }
            mValues.Add((InsertBuilder)pInsertValues);
        }

        public int Execute(Transaction pTransaction) {

            if(pTransaction == null) {
                throw new NullReferenceException($"{nameof(pTransaction)} cannot be null");
            }

            System.Data.Common.DbConnection connection = null;

            string sql = string.Empty;
            DateTime? start = null;
            DateTime? end = null;

            try {

                connection = pTransaction.GetOrSetConnection(pTransaction.Database);

                using(System.Data.Common.DbCommand command = Transaction.CreateCommand(connection, pTransaction)) {

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
            catch(Exception e) {

                if(connection != null && connection.State != System.Data.ConnectionState.Closed) {
                    connection.Close();
                }
                Settings.FireQueryPerformedEvent(pTransaction.Database, sql, 0, QueryType.Insert, start, end, e, pTransaction.IsolationLevel, null, pTransaction.Id);
                throw;
            }
        }
    }
}