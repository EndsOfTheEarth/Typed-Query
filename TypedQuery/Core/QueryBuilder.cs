
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
using System.Data.SqlClient;
using Sql.Interfaces;
using System.Diagnostics;

namespace Sql.Core {

    internal class QueryBuilder : IDistinct, ITop, IFromInto, IFrom, IJoin, IWhere, IGroupBy, IHaving, IOrderBy, IAppend, IUseParams, ITimeout, IExecute {

        private QueryBuilder? mUnionQuery;
        private UnionType mUnionType;
        private readonly ISelectable[] mColumns;
        private bool mDistinct;
        private int? mTopRows = null;
        private ATable? mIntoTable;
        private ATable? mFromTable;
        private string[]? mFromHints;
        private List<Join> mJoinList = new List<Join>();
        private Condition? mWhereCondition;
        private ISelectable[]? mGroupByColumns;
        private IOrderByColumn[]? mOrderByColumns;
        private Condition? mHavingCondition;
        private string? mCustomSql;
        private bool? mUseParameters = null;
        private int? mTimeout = null;

        #region Properties

        public QueryBuilder? UnionQuery {
            get { return mUnionQuery; }
            private set { mUnionQuery = value; }
        }
        public UnionType UnionType {
            get { return mUnionType; }
            private set { mUnionType = value; }
        }
        public ISelectable[] SelectColumns {
            get { return mColumns; }
        }
        public bool IsDistinct {
            get { return mDistinct; }
        }
        public int? TopRows {
            get { return mTopRows; }
        }
        public ATable? IntoTable {
            get { return mIntoTable; }
        }
        public ATable? FromTable {
            get { return mFromTable; }
        }
        public string[]? FromHints {
            get { return mFromHints; }
        }
        public List<Join> JoinList {
            get { return mJoinList; }
        }
        public Condition? WhereCondition {
            get { return mWhereCondition; }
        }
        public ISelectable[]? GroupByColumns {
            get { return mGroupByColumns; }
        }
        public IOrderByColumn[]? OrderByColumns {
            get { return mOrderByColumns; }
        }
        public Condition? HavingCondition {
            get { return mHavingCondition; }
        }
        public string? CustomSql {
            get { return mCustomSql; }
        }
        #endregion

        internal QueryBuilder(ISelectable[] pColumns) {

            if(pColumns == null || pColumns.Length == 0) {
                throw new Exception($"{ nameof(pColumns) } cannot be null or empty");
            }
            mColumns = pColumns;
        }

        public IList<ATable> GetAllTables() {

            List<ATable> tables = new List<ATable>();

            if(mFromTable == null) {
                throw new NullReferenceException($"{ nameof(mFromTable) } is null. A table has not been added to this query.");
            }
            tables.Add(mFromTable);

            foreach(Join join in mJoinList) {
                tables.Add(join.Table);
            }
            return tables;
        }

        public ITop Distinct {
            get {
                mDistinct = true;
                return this;
            }
        }

        public IFromInto Top(int pRows) {

            if(pRows < 0) {
                throw new Exception($"{ nameof(pRows) } must be >= 0. pRows = { pRows.ToString() }");
            }
            mTopRows = pRows;
            return this;
        }

        public IFrom Into(ATable pTable) {

            if(pTable == null) {
                throw new NullReferenceException($"{ nameof(pTable) } cannot be null");
            }
            mIntoTable = pTable;
            return this;
        }
        public IJoin From(ATable pFromTable, params string[] pHints) {

            if(pFromTable == null) {
                throw new NullReferenceException($"{ nameof(pFromTable) } cannot be null");
            }
            mFromTable = pFromTable;
            mFromHints = pHints != null ? pHints : new string[] { };
            return this;
        }

        public IJoin Join(ATable pTable, Condition pCondition, params string[] pHints) {

            if(pTable == null) {
                throw new NullReferenceException($"{ nameof(pTable) } cannot be null");
            }
            if(pCondition == null) {
                throw new NullReferenceException($"{ nameof(pCondition) } cannot be null");
            }
            mJoinList.Add(new Join(JoinType.JOIN, pTable, pCondition, pHints));
            return this;
        }

        public IJoin JoinIf(bool pIncludeJoin, ATable pTable, Condition pCondition, params string[] pHints) {

            if(pIncludeJoin) {
                Join(pTable, pCondition, pHints);
            }
            return this;
        }

        public IJoin LeftJoin(ATable pTable, Condition pCondition, params string[] pHints) {

            if(pTable == null) {
                throw new NullReferenceException($"{ nameof(pTable) } cannot be null");
            }
            if(pCondition == null) {
                throw new NullReferenceException($"{ nameof(pCondition) } cannot be null");
            }
            mJoinList.Add(new Join(JoinType.LEFT, pTable, pCondition, pHints));
            return this;
        }

        public IJoin LeftJoinIf(bool pIncludeJoin, ATable pTable, Condition pCondition, params string[] pHints) {

            if(pIncludeJoin) {
                LeftJoin(pTable, pCondition, pHints);
            }
            return this;
        }

        public IJoin RightJoin(ATable pTable, Condition pCondition, params string[] pHints) {

            if(pTable == null) {
                throw new NullReferenceException($"{ nameof(pTable) } cannot be null");
            }
            if(pCondition == null) {
                throw new NullReferenceException($"{ nameof(pCondition) } cannot be null");
            }
            mJoinList.Add(new Join(JoinType.RIGHT, pTable, pCondition, pHints));
            return this;
        }

        public IJoin RightJoinIf(bool pIncludeJoin, ATable pTable, Condition pCondition, params string[] pHints) {

            if(pIncludeJoin) {
                RightJoin(pTable, pCondition, pHints);
            }
            return this;
        }

        public IGroupBy Where(Condition pCondition) {
            mWhereCondition = pCondition;
            return this;
        }

        public IHaving GroupBy(params ISelectable[] pColumns) {

            if(pColumns == null) {
                throw new NullReferenceException($"{ nameof(pColumns) } cannot be null");
            }
            if(pColumns.Length == 0) {
                throw new Exception($"{ nameof(pColumns) } must be > 0 in length");
            }
            mGroupByColumns = pColumns;
            return this;
        }

        public IOrderBy Having(Condition pCondition) {

            if(pCondition == null) {
                throw new NullReferenceException($"{ nameof(pCondition) } cannot be null");
            }
            mHavingCondition = pCondition;
            return this;
        }

        public IAppend OrderBy(params IOrderByColumn[] pOrderByColumns) {

            if(pOrderByColumns == null) {
                throw new NullReferenceException($"{ nameof(pOrderByColumns) } cannot be null");
            }
            if(pOrderByColumns.Length == 0) {
                throw new Exception($"{ nameof(pOrderByColumns) } must be > 0 in length");
            }
            mOrderByColumns = pOrderByColumns;
            return this;
        }

        public string GetSql(ADatabase pDatabase) {

            if(pDatabase == null) {
                throw new NullReferenceException($"{ nameof(pDatabase) } cannot be null");
            }
            return Database.GenertateSql.GetSelectQuery(pDatabase, this, null);
        }

        private string GetSql(ADatabase pDatabase, Core.Parameters? pParameters) {

            if(pDatabase == null) {
                throw new NullReferenceException($"{ nameof(pDatabase) } cannot be null");
            }
            return Database.GenertateSql.GetSelectQuery(pDatabase, this, pParameters);
        }

        public IUseParams Append(string pCustomSql) {
            mCustomSql = pCustomSql;
            return this;
        }

        public ITimeout UseParameters(bool pUseParameters) {
            mUseParameters = pUseParameters;
            return this;
        }

        public IExecute Timeout(int pSeconds) {

            if(pSeconds < 0) {
                throw new Exception($"{ nameof(pSeconds) } must be >=. Current value = { pSeconds.ToString() }");
            }
            mTimeout = pSeconds;
            return this;
        }

        public IResult Execute(ADatabase pDatabase) {

            if(pDatabase == null) {
                throw new NullReferenceException($"{ nameof(pDatabase) } cannot be null");
            }
            return ExecuteQuery(pDatabase, null);
        }

        public IResult ExecuteUncommitted(ADatabase pDatabase) {

            if(pDatabase == null) {
                throw new NullReferenceException($"{ nameof(pDatabase) } cannot be null");
            }

            using(Transaction transaction = new Transaction(pDatabase, System.Data.IsolationLevel.ReadUncommitted)) {
                IResult result = Execute(transaction);
                transaction.Commit();
                return result;
            }
        }

        public IResult Execute(Transaction pTransaction) {

            if(pTransaction == null) {
                throw new NullReferenceException($"{ nameof(pTransaction) } cannot be null");
            }
            return ExecuteQuery(pTransaction.Database, pTransaction);
        }

        private IResult ExecuteQuery(ADatabase pDatabase, Transaction? pTransaction) {

            if(pDatabase == null) {
                throw new NullReferenceException($"{ nameof(pDatabase) } cannot be null");
            }

            if(pTransaction != null && pTransaction.Database != pDatabase) {
                throw new ArgumentException($"{ nameof(pTransaction) } is using a different database connection than pDatabase.");
            }

            if(Sql.Settings.BreakOnSelectQuery) {

                if(Debugger.IsAttached) {
                    Debugger.Break();
                }
            }

            System.Data.Common.DbConnection? connection = null;

            string sql = string.Empty;
            DateTime? start = null;
            DateTime? end = null;

            System.Data.IsolationLevel isolationLevel = System.Data.IsolationLevel.Unspecified;

            try {

                connection = pTransaction != null ? pTransaction.GetOrSetConnection(pDatabase) : pDatabase.GetConnection(true);

                using(System.Data.Common.DbCommand command = Transaction.CreateCommand(connection, pTransaction)) {

                    Parameters? parameters;

                    if(mUseParameters != null) {
                        parameters = mUseParameters.Value ? new Parameters(command) : null;
                    }
                    else if(Settings.UseParameters) {
                        parameters = new Parameters(command);
                    }
                    else {
                        parameters = null;
                    }

                    sql = GetSql(pDatabase, parameters);

                    command.CommandText = sql;

                    command.CommandType = System.Data.CommandType.Text;
                    command.CommandTimeout = mTimeout != null ? mTimeout.Value : Settings.DefaultTimeout;

                    if(pTransaction != null) {
                        command.Transaction = pTransaction.GetOrSetDbTransaction(pDatabase);
                    }

                    if(command.Transaction != null) {
                        isolationLevel = command.Transaction.IsolationLevel;
                    }

                    start = DateTime.Now;

                    Settings.FireQueryExecutingEvent(pDatabase, sql, QueryType.Select, start, isolationLevel, (pTransaction != null ? (ulong?)pTransaction.Id : null));

                    using(System.Data.Common.DbDataReader reader = command.ExecuteReader()) {

                        end = DateTime.Now;

                        QueryResult result = new QueryResult(pDatabase, mColumns, reader, command.CommandText);

                        if(pTransaction == null) {
                            connection.Close();
                        }
                        Settings.FireQueryPerformedEvent(pDatabase, sql, result.Count, QueryType.Select, start, end, null, isolationLevel, result, (pTransaction != null ? (ulong?)pTransaction.Id : null));
                        return result;
                    }
                }
            }
            catch(Exception e) {

                if(connection != null && connection.State != System.Data.ConnectionState.Closed) {
                    connection.Close();
                }
                Settings.FireQueryPerformedEvent(pDatabase, sql, 0, QueryType.Select, start, end, e, isolationLevel, null, (pTransaction != null ? (ulong?)pTransaction.Id : null));
                throw;
            }
        }

        public IDistinct Union(ISelectableColumns pField, params ISelectableColumns[] pFields) {

            if(pField == null) {
                throw new NullReferenceException($"{ nameof(pField) } cannot be null");
            }

            List<ISelectable> selectList = new List<ISelectable>();
            selectList.AddRange(pField.SelectableColumns);

            foreach(ISelectableColumns selectableFields in pFields) {
                selectList.AddRange(selectableFields.SelectableColumns);
            }

            QueryBuilder queryBuilder = new QueryBuilder(selectList.ToArray());

            queryBuilder.UnionQuery = this;
            queryBuilder.UnionType = UnionType.UNION;

            return queryBuilder;
        }

        public IDistinct UnionAll(ISelectableColumns pField, params ISelectableColumns[] pFields) {

            if(pField == null) {
                throw new NullReferenceException($"{ nameof(pField) } cannot be null");
            }

            List<ISelectable> selectList = new List<ISelectable>();
            selectList.AddRange(pField.SelectableColumns);

            foreach(ISelectableColumns selectableFields in pFields) {
                selectList.AddRange(selectableFields.SelectableColumns);
            }

            QueryBuilder queryBuilder = new QueryBuilder(selectList.ToArray());

            queryBuilder.UnionQuery = this;
            queryBuilder.UnionType = UnionType.UNION_ALL;

            return queryBuilder;
        }

        public IDistinct Intersect(ISelectableColumns pField, params ISelectableColumns[] pFields) {

            if(pField == null) {
                throw new NullReferenceException($"{ nameof(pField) } cannot be null");
            }

            List<ISelectable> selectList = new List<ISelectable>();
            selectList.AddRange(pField.SelectableColumns);

            foreach(ISelectableColumns selectableFields in pFields) {
                selectList.AddRange(selectableFields.SelectableColumns);
            }

            QueryBuilder queryBuilder = new QueryBuilder(selectList.ToArray());

            queryBuilder.UnionQuery = this;
            queryBuilder.UnionType = UnionType.INTERSECT;

            return queryBuilder;
        }

        public IDistinct Except(ISelectableColumns pField, params ISelectableColumns[] pFields) {

            if(pField == null) {
                throw new NullReferenceException($"{ nameof(pField) } cannot be null");
            }

            List<ISelectable> selectList = new List<ISelectable>();
            selectList.AddRange(pField.SelectableColumns);

            foreach(ISelectableColumns selectableFields in pFields) {
                selectList.AddRange(selectableFields.SelectableColumns);
            }

            QueryBuilder queryBuilder = new QueryBuilder(selectList.ToArray());

            queryBuilder.UnionQuery = this;
            queryBuilder.UnionType = UnionType.EXCEPT;

            return queryBuilder;
        }
    }

    internal class Join {

        private readonly JoinType mJoinType;
        private readonly ATable mTable;
        private readonly Condition mCondition;
        private readonly string[] mHints;

        internal Join(JoinType pJoinType, ATable pTable, Condition pCondition, string[]? pHints) {
            mJoinType = pJoinType;
            mTable = pTable;
            mCondition = pCondition;
            mHints = pHints != null ? pHints : new string[] { };
        }

        internal JoinType JoinType {
            get { return mJoinType; }
        }
        internal ATable Table {
            get { return mTable; }
        }
        internal Condition Condition {
            get { return mCondition; }
        }
        internal string[] Hints {
            get { return mHints; }
        }
    }

    internal enum JoinType {
        JOIN,
        LEFT,
        RIGHT,
    }

    internal enum UnionType {
        UNION,
        UNION_ALL,
        INTERSECT,
        EXCEPT
    }
}