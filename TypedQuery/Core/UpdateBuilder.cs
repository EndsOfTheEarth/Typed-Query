
/*
 * 
 * Copyright (C) 2009-2016 JFo.nz
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
using Sql.Types;

namespace Sql.Core {

	internal class UpdateBuilder : IUpdate, IUpdateSet, IUpdateWhere, IUpdateJoin, IUpdateUseParams, IUpdateTimeout, IUpdateReturning, IUpdateExecute {

		private readonly ATable mTable;
		private readonly IList<SetValue> mSetValueList = new List<SetValue>();
		private readonly List<Join> mJoinList = new List<Join>();
		private Condition mWhereCondition;
		private bool? mUseParameters = null;
		private int? mTimeout = null;

		public ATable Table {
			get { return mTable; }
		}

		public IList<SetValue> SetValueList {
			get { return mSetValueList; }
		}
		public List<Join> JoinList {
			get { return mJoinList; }
		}
		public Condition WhereCondition {
			get { return mWhereCondition; }
		}
		public AColumn[] ReturnColumns { get; private set; }

		public UpdateBuilder(ATable pTable) {

			if(pTable == null) {
				throw new NullReferenceException($"{ nameof(pTable) } cannot be null");
			}

			mTable = pTable;
		}

		internal IUpdateSet SetInternal(AColumn pColumn, object pValue) {
			mSetValueList.Add(new SetValue(pColumn, pValue, true));
			return this;
		}

		public IUpdateSet Set<COLUMN>(COLUMN pColumnA, COLUMN pColumnB) where COLUMN : AColumn {

			if(pColumnB == null) {
				throw new NullReferenceException($"{ nameof(pColumnB) } cannot be null");
			}

			mSetValueList.Add(new SetValue(pColumnA, pColumnB, true));
			return this;
		}
		public IUpdateSet Set(Sql.Column.SmallIntegerColumn pColumn, Int16 pValue) {
			mSetValueList.Add(new SetValue(pColumn, pValue));
			return this;
		}

		public IUpdateSet Set(Sql.Column.NSmallIntegerColumn pColumn, Int16? pValue) {
			mSetValueList.Add(new SetValue(pColumn, pValue));
			return this;
		}

		public IUpdateSet Set(Sql.Column.IntegerColumn pColumn, int pValue) {
			mSetValueList.Add(new SetValue(pColumn, pValue));
			return this;
		}

		public IUpdateSet Set(Sql.Column.NIntegerColumn pColumn, int? pValue) {
			mSetValueList.Add(new SetValue(pColumn, pValue));
			return this;
		}

		public IUpdateSet Set(Sql.Column.BigIntegerColumn pColumn, Int64 pValue) {
			mSetValueList.Add(new SetValue(pColumn, pValue));
			return this;
		}

		public IUpdateSet Set(Sql.Column.NBigIntegerColumn pColumn, Int64? pValue) {
			mSetValueList.Add(new SetValue(pColumn, pValue));
			return this;
		}

		public IUpdateSet Set(Sql.Column.StringColumn pColumn, string pValue) {

			if(pValue != null && pValue.Length > pColumn.MaxLength) {
				throw new Exception($"{ pColumn.ColumnName } column string value is too long. Max length = { pColumn.MaxLength.ToString() }. Actual length = { pValue.Length.ToString() }. Table = { pColumn.Table.TableName }");
			}

			mSetValueList.Add(new SetValue(pColumn, pValue));
			return this;
		}

		public IUpdateSet Set(Sql.Column.DecimalColumn pColumn, decimal pValue) {
			mSetValueList.Add(new SetValue(pColumn, pValue));
			return this;
		}

		public IUpdateSet Set(Sql.Column.NDecimalColumn pColumn, decimal? pValue) {
			mSetValueList.Add(new SetValue(pColumn, pValue));
			return this;
		}

		public IUpdateSet Set(Sql.Column.DateTimeColumn pColumn, DateTime pValue) {
			mSetValueList.Add(new SetValue(pColumn, pValue));
			return this;
		}

		public IUpdateSet Set(Sql.Column.DateTimeColumn pColumn, Function.CurrentDateTime pValue) {
			mSetValueList.Add(new SetValue(pColumn, pValue));
			return this;
		}

		public IUpdateSet Set(Sql.Column.NDateTimeColumn pColumn, DateTime? pValue) {
			mSetValueList.Add(new SetValue(pColumn, pValue));
			return this;
		}

		public IUpdateSet Set(Sql.Column.NDateTimeColumn pColumn, Function.CurrentDateTime pValue) {
			mSetValueList.Add(new SetValue(pColumn, pValue));
			return this;
		}

		public IUpdateSet Set(Sql.Column.DateTime2Column pColumn, DateTime pValue) {
			mSetValueList.Add(new SetValue(pColumn, pValue));
			return this;
		}

		public IUpdateSet Set(Sql.Column.DateTime2Column pColumn, Function.CurrentDateTime pValue) {
			mSetValueList.Add(new SetValue(pColumn, pValue));
			return this;
		}

		public IUpdateSet Set(Sql.Column.NDateTime2Column pColumn, DateTime? pValue) {
			mSetValueList.Add(new SetValue(pColumn, pValue));
			return this;
		}

		public IUpdateSet Set(Sql.Column.NDateTime2Column pColumn, Function.CurrentDateTime pValue) {
			mSetValueList.Add(new SetValue(pColumn, pValue));
			return this;
		}

		public IUpdateSet Set(Sql.Column.DateTimeOffsetColumn pColumn, DateTimeOffset pValue) {
			mSetValueList.Add(new SetValue(pColumn, pValue));
			return this;
		}

		public IUpdateSet Set(Sql.Column.DateTimeOffsetColumn pColumn, Function.CurrentDateTimeOffset pValue) {
			mSetValueList.Add(new SetValue(pColumn, pValue));
			return this;
		}

		public IUpdateSet Set(Sql.Column.NDateTimeOffsetColumn pColumn, DateTimeOffset? pValue) {
			mSetValueList.Add(new SetValue(pColumn, pValue));
			return this;
		}

		public IUpdateSet Set(Sql.Column.NDateTimeOffsetColumn pColumn, Function.CurrentDateTimeOffset pValue) {
			mSetValueList.Add(new SetValue(pColumn, pValue));
			return this;
		}

		public IUpdateSet Set(Sql.Column.BoolColumn pColumn, bool pValue) {
			mSetValueList.Add(new SetValue(pColumn, pValue));
			return this;
		}

		public IUpdateSet Set(Sql.Column.NBoolColumn pColumn, bool? pValue) {
			mSetValueList.Add(new SetValue(pColumn, pValue));
			return this;
		}

		public IUpdateSet Set(Sql.Column.GuidColumn pColumn, Guid pValue) {
			mSetValueList.Add(new SetValue(pColumn, pValue));
			return this;
		}

		public IUpdateSet Set(Sql.Column.NGuidColumn pColumn, Guid? pValue) {
			mSetValueList.Add(new SetValue(pColumn, pValue));
			return this;
		}

		public IUpdateSet Set(Sql.Column.BinaryColumn pColumn, byte[] pValue) {
			mSetValueList.Add(new SetValue(pColumn, pValue));
			return this;
		}

		public IUpdateSet Set(Sql.Column.NBinaryColumn pColumn, byte[] pValue) {
			mSetValueList.Add(new SetValue(pColumn, pValue));
			return this;
		}

		public IUpdateSet Set<TABLE>(Column.GuidKeyColumn<TABLE> pColumn, GuidKey<TABLE> pValue) {
			mSetValueList.Add(new SetValue(pColumn, pValue.Value));
			return this;
		}

		public IUpdateSet Set<TABLE>(Column.NGuidKeyColumn<TABLE> pColumn, GuidKey<TABLE> pValue) {
			mSetValueList.Add(new SetValue(pColumn, pValue.Value));
			return this;
		}

		public IUpdateSet Set<TABLE>(Column.NGuidKeyColumn<TABLE> pColumn, GuidKey<TABLE>? pValue) {
			mSetValueList.Add(new SetValue(pColumn, pValue != null ? pValue.Value.Value : (Guid?)null));
			return this;
		}

		public IUpdateSet Set<TABLE>(Column.SmallIntegerKeyColumn<TABLE> pColumn, Int16Key<TABLE> pValue) {
			mSetValueList.Add(new SetValue(pColumn, pValue.Value));
			return this;
		}

		public IUpdateSet Set<TABLE>(Column.NSmallIntegerKeyColumn<TABLE> pColumn, Int16Key<TABLE> pValue) {
			mSetValueList.Add(new SetValue(pColumn, pValue.Value));
			return this;
		}

		public IUpdateSet Set<TABLE>(Column.NSmallIntegerKeyColumn<TABLE> pColumn, Int16Key<TABLE>? pValue) {
			mSetValueList.Add(new SetValue(pColumn, pValue != null ? pValue.Value.Value : (short?)null));
			return this;
		}

		public IUpdateSet Set<TABLE>(Column.IntegerKeyColumn<TABLE> pColumn, Int32Key<TABLE> pValue) {
			mSetValueList.Add(new SetValue(pColumn, pValue.Value));
			return this;
		}

		public IUpdateSet Set<TABLE>(Column.NIntegerKeyColumn<TABLE> pColumn, Int32Key<TABLE> pValue) {
			mSetValueList.Add(new SetValue(pColumn, pValue.Value));
			return this;
		}

		public IUpdateSet Set<TABLE>(Column.NIntegerKeyColumn<TABLE> pColumn, Int32Key<TABLE>? pValue) {
			mSetValueList.Add(new SetValue(pColumn, pValue != null ? pValue.Value.Value : (int?)null));
			return this;
		}

		public IUpdateSet Set<TABLE>(Column.BigIntegerKeyColumn<TABLE> pColumn, Int64Key<TABLE> pValue) {
			mSetValueList.Add(new SetValue(pColumn, pValue.Value));
			return this;
		}

		public IUpdateSet Set<TABLE>(Column.NBigIntegerKeyColumn<TABLE> pColumn, Int64Key<TABLE> pValue) {
			mSetValueList.Add(new SetValue(pColumn, pValue.Value));
			return this;
		}

		public IUpdateSet Set<TABLE>(Column.NBigIntegerKeyColumn<TABLE> pColumn, Int64Key<TABLE>? pValue) {
			mSetValueList.Add(new SetValue(pColumn, pValue != null ? pValue.Value.Value : (long?)null));
			return this;
		}

		public IUpdateSet Set<TABLE>(Column.StringKeyColumn<TABLE> pColumn, StringKey<TABLE> pValue) {

			if(pValue != null && pValue.Value.Length > pColumn.MaxLength) {
				throw new Exception($"{ pColumn.ColumnName } column string value is too long. Max length = { pColumn.MaxLength.ToString() }. Actual length = { pValue.Value.Length.ToString() }. Table = { pColumn.Table.TableName }");
			}

			mSetValueList.Add(new SetValue(pColumn, pValue.Value));
			return this;
		}

		public IUpdateSet Set<TABLE>(Column.NStringKeyColumn<TABLE> pColumn, StringKey<TABLE>? pValue) {

			if(pValue != null && pValue.Value.Value.Length > pColumn.MaxLength) {
				throw new Exception($"{ pColumn.ColumnName } column string value is too long. Max length = { pColumn.MaxLength.ToString() }. Actual length = { pValue.Value.Value.Length.ToString() }. Table = { pColumn.Table.TableName }");
			}

			mSetValueList.Add(new SetValue(pColumn, pValue != null ? pValue.Value.Value : null));
			return this;
		}

		public IUpdateJoin Join(ATable pTable, Condition pCondition) {
			if(pTable == null) {
				throw new NullReferenceException($"{ nameof(pTable) } cannot be null");
			}
			if(pCondition == null) {
				throw new NullReferenceException($"{ nameof(pCondition) } cannot be null");
			}
			mJoinList.Add(new Join(JoinType.JOIN, pTable, pCondition, null));
			return this;
		}

		public IUpdateUseParams NoWhereCondition() {
			return this;
		}

		public IUpdateUseParams Where(Condition pCondition) {
			if(pCondition == null) {
				throw new NullReferenceException($"{ nameof(pCondition) } cannot be null");
			}
			mWhereCondition = pCondition;
			return this;
		}

		public IUpdateTimeout UseParameters(bool pUseParameters) {
			mUseParameters = pUseParameters;
			return this;
		}

		public IUpdateExecute Timeout(int pSeconds) {
			if(pSeconds < 0) {
				throw new Exception($"{ nameof(pSeconds) } must be >= 0. Current value = { pSeconds.ToString() }");
			}
			mTimeout = pSeconds;
			return this;
		}

		public IUpdateExecute Returning(params AColumn[] pColumns) {

			if(pColumns == null) {
				throw new NullReferenceException($"{ nameof(pColumns) } cannot be null");
			}

			if(pColumns.Length == 0) {
				throw new Exception($"{ nameof(pColumns) } cannot be empty");
			}

			ReturnColumns = pColumns;

			return this;
		}

		public string GetSql(ADatabase pDatabase) {

			if(pDatabase == null) {
				throw new NullReferenceException($"{ nameof(pDatabase) } cannot be null");
			}

			return Database.GenertateSql.GetUpdateQuery(pDatabase, this, null);
		}
		private string GetSql(ADatabase pDatabase, Core.Parameters pParameters) {

			if(pDatabase == null) {
				throw new NullReferenceException($"{ nameof(pDatabase) } cannot be null");
			}

			return Database.GenertateSql.GetUpdateQuery(pDatabase, this, pParameters);
		}

		public IResult Execute(Transaction pTransaction) {

			if(pTransaction == null) {
				throw new NullReferenceException($"{ nameof(pTransaction) } cannot be null");
			}

			if(Sql.Settings.BreakOnUpdateQuery) {
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
					Settings.FireQueryExecutingEvent(pTransaction.Database, sql, QueryType.Update, start, pTransaction.IsolationLevel, pTransaction.Id);

					QueryResult result;

					if(ReturnColumns == null || ReturnColumns.Length == 0) {
						int returnValue = command.ExecuteNonQuery();
						end = DateTime.Now;
						result = new QueryResult(returnValue, sql);
						Settings.FireQueryPerformedEvent(pTransaction.Database, sql, returnValue, QueryType.Update, start, end, null, pTransaction.IsolationLevel, null, pTransaction.Id);
					}
					else {
						using(System.Data.Common.DbDataReader reader = command.ExecuteReader()) {
							end = DateTime.Now;
							result = new QueryResult(pTransaction.Database, ReturnColumns, reader, command.CommandText);
							Settings.FireQueryPerformedEvent(pTransaction.Database, sql, result.Count, QueryType.Update, start, end, null, pTransaction.IsolationLevel, null, pTransaction.Id);
						}
					}
					return result;
				}
			}
			catch(Exception e) {
				if(connection != null && connection.State != System.Data.ConnectionState.Closed) {
					connection.Close();
				}
				Settings.FireQueryPerformedEvent(pTransaction.Database, sql, 0, QueryType.Update, start, end, e, pTransaction.IsolationLevel, null, pTransaction.Id);
				throw;
			}
		}
	}
}