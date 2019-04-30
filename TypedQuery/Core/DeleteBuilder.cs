
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
using System.Collections.Generic;
using System.Text;
using Sql.Interfaces;
using System.Diagnostics;

namespace Sql.Core {

    internal class DeleteBuilder : IDelete, IDeleteUseParams, IDeleteTimeout, IDeleteExecute {

        private readonly ATable mTable;
        private Condition mWhereCondition;
        private bool? mUseParameters = null;
        private int? mTimeout = null;

        public ATable Table {
            get { return mTable; }
        }

        public Condition WhereCondition {
            get { return mWhereCondition; }
        }

        public AColumn[] ReturnColumns { get; private set; }

        internal DeleteBuilder(ATable pTable) {

            if(pTable == null) {
                throw new NullReferenceException($"{nameof(pTable)} cannot be null");
            }
            mTable = pTable;
        }

        public IDeleteUseParams NoWhereCondition {
            get { return this; }
        }

        public IDeleteUseParams Where(Condition pCondition) {

            if(pCondition == null) {
                throw new NullReferenceException($"{nameof(pCondition)} cannot be null");
            }
            mWhereCondition = pCondition;
            return this;
        }

        public IDeleteTimeout UseParameters(bool pUseParameters) {
            mUseParameters = pUseParameters;
            return this;
        }

        public IDeleteReturning Timeout(int pSeconds) {

            if(pSeconds < 0) {
                throw new Exception($"{nameof(pSeconds)} must be >= 0. pSeconds = { pSeconds.ToString() }");
            }
            mTimeout = pSeconds;
            return this;
        }

        public IDeleteExecute Returning(params AColumn[] pColumns) {

            if(pColumns == null) {
                throw new NullReferenceException($"{nameof(pColumns)} cannot be null");
            }

            if(pColumns.Length == 0) {
                throw new Exception($"{nameof(pColumns)} cannot be empty");
            }
            ReturnColumns = pColumns;
            return this;
        }

        public string GetSql(ADatabase pDatabase) {

            if(pDatabase == null) {
                throw new NullReferenceException($"{nameof(pDatabase)} cannot be null");
            }

            return Database.GenertateSql.GetDeleteQuery(pDatabase, this, null);
        }
        private string GetSql(ADatabase pDatabase, Core.Parameters pParameters) {

            if(pDatabase == null) {
                throw new NullReferenceException($"{nameof(pDatabase)} cannot be null");
            }
            return Database.GenertateSql.GetDeleteQuery(pDatabase, this, pParameters);
        }

        public IResult Execute(Transaction pTransaction) {

            if(pTransaction == null) {
                throw new NullReferenceException($"{nameof(pTransaction)} cannot be null");
            }

            if(Sql.Settings.BreakOnDeleteQuery) {

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

                using(System.Data.Common.DbCommand command = Transaction.CreateCommand(connection, pTransaction)) {

                    Parameters parameters;

                    if(mUseParameters != null) {
                        parameters = mUseParameters.Value ? new Parameters(command) : null;
                    }
                    else if(Settings.UseParameters) {
                        parameters = new Parameters(command);
                    }
                    else {
                        parameters = null;
                    }

                    sql = GetSql(pTransaction.Database, parameters);

                    command.CommandText = sql;
                    command.CommandType = System.Data.CommandType.Text;
                    command.CommandTimeout = mTimeout != null ? mTimeout.Value : Settings.DefaultTimeout;
                    command.Transaction = pTransaction.GetOrSetDbTransaction(pTransaction.Database);

                    start = DateTime.Now;
                    Settings.FireQueryExecutingEvent(pTransaction.Database, sql, QueryType.Delete, start, pTransaction.IsolationLevel, pTransaction.Id);

                    QueryResult result;

                    if(ReturnColumns == null || ReturnColumns.Length == 0) {
                        int returnValue = command.ExecuteNonQuery();
                        end = DateTime.Now;
                        result = new QueryResult(returnValue, sql);
                        Settings.FireQueryPerformedEvent(pTransaction.Database, sql, returnValue, QueryType.Delete, start, end, null, pTransaction.IsolationLevel, null, pTransaction.Id);
                    }
                    else {
                        using(System.Data.Common.DbDataReader reader = command.ExecuteReader()) {
                            end = DateTime.Now;
                            result = new QueryResult(pTransaction.Database, ReturnColumns, reader, command.CommandText);
                            Settings.FireQueryPerformedEvent(pTransaction.Database, sql, result.Count, QueryType.Delete, start, end, null, pTransaction.IsolationLevel, null, pTransaction.Id);
                        }
                    }
                    return result;
                }
            }
            catch(Exception e) {

                if(connection != null && connection.State != System.Data.ConnectionState.Closed) {
                    connection.Close();
                }
                Settings.FireQueryPerformedEvent(pTransaction.Database, sql, 0, QueryType.Delete, start, end, e, pTransaction.IsolationLevel, null, pTransaction.Id);
                throw;
            }
        }
    }
}