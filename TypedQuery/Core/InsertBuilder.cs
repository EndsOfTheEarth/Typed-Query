
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

	internal class InsertBuilder : IInsert, IInsertSet, IInsertUseParams, IInsertTimeout, IInsertExecute {
		
		private readonly ATable mTable;
		private readonly IList<SetValue> mSetValueList = new List<SetValue>();
		private bool? mUseParameters = null;
		private int? mTimeout = null;
		
		internal bool? UseParams {
			get { return mUseParameters; }
		}
		
		public ATable Table {
			get { return mTable; }
		}

		public IList<SetValue> SetValueList {
			get { return mSetValueList; }
		}
		
		public AColumn[] ReturnColumns { get; private set; }

		public InsertBuilder(ATable pTable) {

			if(pTable == null) {
				throw new NullReferenceException($"{nameof(pTable)} cannot be null");
			}
			
			mTable = pTable;
		}

		internal IInsertSet SetInternal(Sql.AColumn pColumn, object pValue) {
			mSetValueList.Add(new SetValue(pColumn, pValue));
			return this;
		}

		public IInsertSet Set(Sql.Column.SmallIntegerColumn pColumn, Int16 pValue) {
			mSetValueList.Add(new SetValue(pColumn, pValue));
			return this;
		}
		
		public IInsertSet Set(Sql.Column.NSmallIntegerColumn pColumn, Int16? pValue) {
			mSetValueList.Add(new SetValue(pColumn, pValue));
			return this;
		}
		
		public IInsertSet Set(Sql.Column.IntegerColumn pColumn, int pValue) {
			mSetValueList.Add(new SetValue(pColumn, pValue));
			return this;
		}
		
		public IInsertSet Set(Sql.Column.NIntegerColumn pColumn, int? pValue) {
			mSetValueList.Add(new SetValue(pColumn, pValue));
			return this;
		}
		
		public IInsertSet Set(Sql.Column.BigIntegerColumn pColumn, Int64 pValue) {
			mSetValueList.Add(new SetValue(pColumn, pValue));
			return this;
		}
		
		public IInsertSet Set(Sql.Column.NBigIntegerColumn pColumn, Int64? pValue) {
			mSetValueList.Add(new SetValue(pColumn, pValue));
			return this;
		}
		
		public IInsertSet Set(Sql.Column.StringColumn pColumn, string pValue) {

			if(pValue != null && pValue.Length > pColumn.MaxLength) {
				throw new Exception($"{pColumn.ColumnName} column string value is too long. Max length = { pColumn.MaxLength.ToString() }. Actual length = { pValue.Length.ToString() }. Table = { pColumn.Table.TableName }");
			}
			
			mSetValueList.Add(new SetValue(pColumn, pValue));
			return this;
		}

		public IInsertSet Set(Sql.Column.DecimalColumn pColumn, decimal pValue) {
			mSetValueList.Add(new SetValue(pColumn, pValue));
			return this;
		}

		public IInsertSet Set(Sql.Column.NDecimalColumn pColumn, decimal? pValue) {
			mSetValueList.Add(new SetValue(pColumn, pValue));
			return this;
		}
		
		public IInsertSet Set(Sql.Column.DateTimeColumn pColumn, DateTime pValue) {
			mSetValueList.Add(new SetValue(pColumn, pValue));
			return this;
		}
		
		public IInsertSet Set(Sql.Column.DateTimeColumn pColumn, Function.CurrentDateTime pValue) {
			mSetValueList.Add(new SetValue(pColumn, pValue));
			return this;
		}
		
		public IInsertSet Set(Sql.Column.NDateTimeColumn pColumn, DateTime? pValue) {
			mSetValueList.Add(new SetValue(pColumn, pValue));
			return this;
		}
		
		public IInsertSet Set(Sql.Column.NDateTimeColumn pColumn, Function.CurrentDateTime pValue) {
			mSetValueList.Add(new SetValue(pColumn, pValue));
			return this;
		}
		
		public IInsertSet Set(Sql.Column.DateTime2Column pColumn, DateTime pValue) {
			mSetValueList.Add(new SetValue(pColumn, pValue));
			return this;
		}
		
		public IInsertSet Set(Sql.Column.DateTime2Column pColumn, Function.CurrentDateTime pValue) {
			mSetValueList.Add(new SetValue(pColumn, pValue));
			return this;
		}
		
		public IInsertSet Set(Sql.Column.NDateTime2Column pColumn, DateTime? pValue) {
			mSetValueList.Add(new SetValue(pColumn, pValue));
			return this;
		}
		
		public IInsertSet Set(Sql.Column.NDateTime2Column pColumn, Function.CurrentDateTime pValue) {
			mSetValueList.Add(new SetValue(pColumn, pValue));
			return this;
		}
		
		public IInsertSet Set(Sql.Column.DateTimeOffsetColumn pColumn, DateTimeOffset pValue) {
			mSetValueList.Add(new SetValue(pColumn, pValue));
			return this;
		}
		
		public IInsertSet Set(Sql.Column.DateTimeOffsetColumn pColumn, Function.CurrentDateTimeOffset pValue) {
			mSetValueList.Add(new SetValue(pColumn, pValue));
			return this;
		}
		
		public IInsertSet Set(Sql.Column.NDateTimeOffsetColumn pColumn, DateTimeOffset? pValue) {
			mSetValueList.Add(new SetValue(pColumn, pValue));
			return this;
		}
		
		public IInsertSet Set(Sql.Column.NDateTimeOffsetColumn pColumn, Function.CurrentDateTimeOffset pValue) {
			mSetValueList.Add(new SetValue(pColumn, pValue));
			return this;
		}
		
		public IInsertSet Set(Sql.Column.BoolColumn pColumn, bool pValue) {
			mSetValueList.Add(new SetValue(pColumn, pValue));
			return this;
		}

		public IInsertSet Set(Sql.Column.NBoolColumn pColumn, bool? pValue) {
			mSetValueList.Add(new SetValue(pColumn, pValue));
			return this;
		}

		public IInsertSet Set(Sql.Column.GuidColumn pColumn, Guid pValue) {
			mSetValueList.Add(new SetValue(pColumn, pValue));
			return this;
		}

		public IInsertSet Set(Sql.Column.NGuidColumn pColumn, Guid? pValue) {
			mSetValueList.Add(new SetValue(pColumn, pValue));
			return this;
		}
		
		public IInsertSet Set(Sql.Column.BinaryColumn pColumn, byte[] pValue) {
			mSetValueList.Add(new SetValue(pColumn, pValue));
			return this;
		}
		
		public IInsertSet Set(Sql.Column.NBinaryColumn pColumn, byte[] pValue) {
			mSetValueList.Add(new SetValue(pColumn, pValue));
			return this;
		}
		
		public IInsertSet Set<ENUM>(Column.EnumColumn<ENUM> pColumn, ENUM pValue){
			mSetValueList.Add(new SetValue(pColumn, (int)(object)pValue));
			return this;
		}
		
		public IInsertSet Set(AColumn pColumn, Sql.Function.CustomSql pValue) {
			mSetValueList.Add(new SetValue(pColumn, pValue));
			return this;
		}

		public IInsertSet Set<TABLE>(Column.GuidKeyColumn<TABLE> pColumn, GuidKey<TABLE> pValue) {
			mSetValueList.Add(new SetValue(pColumn, pValue));
			return this;
		}
		
		public IInsertSet Set<TABLE>(Column.NGuidKeyColumn<TABLE> pColumn, GuidKey<TABLE> pValue) {
			mSetValueList.Add(new SetValue(pColumn, pValue));
			return this;
		}

		public IInsertSet Set<TABLE>(Column.NGuidKeyColumn<TABLE> pColumn, GuidKey<TABLE>? pValue) {
			mSetValueList.Add(new SetValue(pColumn, pValue));
			return this;
		}

		public IInsertSet Set<TABLE>(Column.SmallIntegerKeyColumn<TABLE> pColumn, Int16Key<TABLE> pValue) {
			mSetValueList.Add(new SetValue(pColumn, pValue));
			return this;
		}
		
		public IInsertSet Set<TABLE>(Column.NSmallIntegerKeyColumn<TABLE> pColumn, Int16Key<TABLE> pValue) {
			mSetValueList.Add(new SetValue(pColumn, pValue));
			return this;
		}

		public IInsertSet Set<TABLE>(Column.NSmallIntegerKeyColumn<TABLE> pColumn, Int16Key<TABLE>? pValue) {
			mSetValueList.Add(new SetValue(pColumn, pValue));
			return this;
		}

		public IInsertSet Set<TABLE>(Column.IntegerKeyColumn<TABLE> pColumn, Int32Key<TABLE> pValue) {
			mSetValueList.Add(new SetValue(pColumn, pValue));
			return this;
		}
		
		public IInsertSet Set<TABLE>(Column.NIntegerKeyColumn<TABLE> pColumn, Int32Key<TABLE> pValue) {
			mSetValueList.Add(new SetValue(pColumn, pValue));
			return this;
		}

		public IInsertSet Set<TABLE>(Column.NIntegerKeyColumn<TABLE> pColumn, Int32Key<TABLE>? pValue) {
			mSetValueList.Add(new SetValue(pColumn, pValue));
			return this;
		}

		public IInsertSet Set<TABLE>(Column.BigIntegerKeyColumn<TABLE> pColumn, Int64Key<TABLE> pValue) {
			mSetValueList.Add(new SetValue(pColumn, pValue));
			return this;
		}
		
		public IInsertSet Set<TABLE>(Column.NBigIntegerKeyColumn<TABLE> pColumn, Int64Key<TABLE> pValue) {
			mSetValueList.Add(new SetValue(pColumn, pValue));
			return this;
		}

		public IInsertSet Set<TABLE>(Column.NBigIntegerKeyColumn<TABLE> pColumn, Int64Key<TABLE>? pValue) {
			mSetValueList.Add(new SetValue(pColumn, pValue));
			return this;
		}

		public IInsertSet Set<TABLE>(Column.StringKeyColumn<TABLE> pColumn, StringKey<TABLE> pValue) {

			if(pValue != null && pValue.Value.Length > pColumn.MaxLength) {
				throw new Exception($"{ pColumn.ColumnName } column string value is too long. Max length = { pColumn.MaxLength.ToString() }. Actual length = { pValue.Value.Length.ToString() }. Table = { pColumn.Table.TableName }");
			}
			
			mSetValueList.Add(new SetValue(pColumn, pValue));
			return this;
		}

		public IInsertSet Set<TABLE>(Column.StringKeyColumn<TABLE> pColumn, StringKey<TABLE>? pValue) {

			if(pValue != null && pValue.Value.Value.Length > pColumn.MaxLength) {
				throw new Exception($"{ pColumn.ColumnName } column string value is too long. Max length = { pColumn.MaxLength.ToString() }. Actual length = { pValue.Value.Value.Length.ToString() }. Table = { pColumn.Table.TableName }");
			}

			mSetValueList.Add(new SetValue(pColumn, pValue));
			return this;
		}

		public IInsertTimeout UseParameters(bool pUseParameters) {
			mUseParameters = pUseParameters;
			return this;
		}
		
		public IInsertExecute Timeout(int pSeconds) {

			if(pSeconds < 0) {
				throw new Exception($"{nameof(pSeconds)} must be >=. Current value = { pSeconds.ToString() }");
			}
			mTimeout = pSeconds;
			return this;
		}
		
		public IInsertExecute Returning(params AColumn[] pColumns) {

			if(pColumns == null) {
				throw new NullReferenceException($"{ nameof(pColumns)} cannot be null");
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
			
			return Database.GenertateSql.GetInsertQuery(pDatabase, this, null);
		}

		private string GetSql(ADatabase pDatabase, Core.Parameters pParameters) {

			if(pDatabase == null) {
				throw new NullReferenceException($"{ nameof(pDatabase) } cannot be null");
			}
			
			return Database.GenertateSql.GetInsertQuery(pDatabase, this, pParameters);
		}

		public IResult Execute(Transaction pTransaction) {

			if(pTransaction == null) {
				throw new NullReferenceException($"{ nameof(pTransaction) } cannot be null");
			}
			
			if(Sql.Settings.BreakOnInsertQuery) {
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
					Settings.FireQueryExecutingEvent(pTransaction.Database, sql, QueryType.Insert, start, pTransaction.IsolationLevel, pTransaction.Id);
	
					QueryResult result;
					
					if(ReturnColumns == null || ReturnColumns.Length == 0){
						int returnValue = command.ExecuteNonQuery();
						end = DateTime.Now;
						result = new QueryResult(returnValue, sql);
						Settings.FireQueryPerformedEvent(pTransaction.Database, sql, returnValue, QueryType.Insert, start, end, null, pTransaction.IsolationLevel, null, pTransaction.Id);
					}
					else{
						using(System.Data.Common.DbDataReader reader = command.ExecuteReader()) {
							end = DateTime.Now;
							result = new QueryResult(pTransaction.Database, ReturnColumns, reader, command.CommandText);
							Settings.FireQueryPerformedEvent(pTransaction.Database, sql, result.Count, QueryType.Insert, start, end, null, pTransaction.IsolationLevel, null, pTransaction.Id);
						}
					}
					return result;
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

	public class SetValue {

		private readonly AColumn mColumn;
		private readonly object mValue;

		public SetValue(AColumn pColumn, object pValue) {

			if(pColumn == null) {
				throw new NullReferenceException($"{ nameof(pColumn) } cannot be null");
			}
			mColumn = pColumn;
			mValue = pValue;
		}
		public AColumn Column {
			get { return mColumn; }
		}
		public object Value {
			get { return mValue; }
		}
	}
}