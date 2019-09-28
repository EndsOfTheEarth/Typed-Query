
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

namespace Sql {

    public static class Settings {

        /// <summary>
        /// Default value to turn query parameters on or off.
        /// </summary>
        public static bool UseParameters { get; set; }

        /// <summary>
        /// When updating ARow records an exception is thrown if the original row data has changed on the database. If set to false this feature is turned off.
        /// </summary>
        public static bool UseConcurrenyChecking { get; set; }

        /// <summary>
        /// Default query timeout setting in seconds. Default value is 30 seconds.
        /// </summary>
        public static int DefaultTimeout { get; set; }

        /// <summary>
        /// If true this causes code to stop on a break point when a select query is executed in debug mode. This is a debugging feature.
        /// </summary>
        public static bool BreakOnSelectQuery { get; set; }

        /// <summary>
        /// If true this causes code to stop on a break point when a insert query is executed in debug mode. This is a debugging feature.
        /// </summary>
        public static bool BreakOnInsertQuery { get; set; }

        /// <summary>
        /// If true this causes code to stop on a break point when a insert select query is executed in debug mode. This is a debugging feature.
        /// </summary>
        public static bool BreakOnInsertSelectQuery { get; set; }

        /// <summary>
        /// If true this causes code to stop on a break point when a update query is executed in debug mode. This is a debugging feature.
        /// </summary>
        public static bool BreakOnUpdateQuery { get; set; }

        /// <summary>
        /// If true this causes code to stop on a break point when a delete query is executed in debug mode. This is a debugging feature.
        /// </summary>
        public static bool BreakOnDeleteQuery { get; set; }

        /// <summary>
        /// If true this causes code to stop on a break point when a truncate query is executed in debug mode. This is a debugging feature.
        /// </summary>
        public static bool BreakOnTruncateQuery { get; set; }

        static Settings() {
            UseParameters = true;
            UseConcurrenyChecking = false;
            DefaultTimeout = 30;
            ReturnResultSize = true;
            BreakOnSelectQuery = false;
            BreakOnInsertQuery = false;
            BreakOnInsertSelectQuery = false;
            BreakOnUpdateQuery = false;
            BreakOnDeleteQuery = false;
            BreakOnTruncateQuery = false;
        }

        /// <summary>
        /// When using the QueryPerformed event if true then the result size is passed else it isn't. This setting is here for performance reasons.
        /// </summary>
        public static bool ReturnResultSize { get; set; }


        public delegate void QueryExecutingDelegate(ADatabase pDatabase, string pSql, QueryType pQueryType, DateTime? pStart, System.Data.IsolationLevel pIsolationLevel, ulong? pTransactionId);

        /// <summary>
        /// Event fired when a query begins execution
        /// </summary>
        public static event QueryExecutingDelegate? QueryExecuting;

        internal static void FireQueryExecutingEvent(ADatabase pDatabase, string pSql, QueryType pQueryType, DateTime? pStart, System.Data.IsolationLevel pIsolationLevel, ulong? pTransactionId) {
            try {

                if(QueryExecuting != null) {
                    QueryExecuting(pDatabase, pSql, pQueryType, pStart, pIsolationLevel, pTransactionId);
                }
            }
            catch { }
        }


        public delegate void QueryPerformedDelegate(ADatabase pDatabase, string pSql, int pRows, QueryType pQueryType, DateTime? pStart, DateTime? pEnd, Exception? pException, System.Data.IsolationLevel pIsolationLevel, int? pResultSize, ulong? pTransactionId);

        /// <summary>
        /// Event fired when a query completes execution or throws an exception
        /// </summary>
        public static event QueryPerformedDelegate? QueryPerformed;

        internal static void FireQueryPerformedEvent(ADatabase pDatabase, string pSql, int pRows, QueryType pQueryType, DateTime? pStart, DateTime? pEnd, Exception? pException, System.Data.IsolationLevel pIsolationLevel, IResult? pResult, ulong? pTransactionId) {
            try {

                if(QueryPerformed != null) {
                    int? resultSize = ReturnResultSize && pResult != null ? (int?)pResult.GetDataSetSizeInBytes() : null;
                    QueryPerformed(pDatabase, pSql, pRows, pQueryType, pStart, pEnd, pException, pIsolationLevel, resultSize, pTransactionId);
                }
            }
            catch { }
        }
    }

    public enum QueryType {
        Select,
        Insert,
        Update,
        Delete,
        PlainText,
        StoredProc,
        Truncate
    }
}