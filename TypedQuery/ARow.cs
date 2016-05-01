
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

namespace Sql {

	/// <summary>
	/// Abstract row
	/// </summary>
	public abstract class ARow {

		private ATable mTable;

		protected virtual ATable Tbl {
			get { return mTable; }
		}

		public ATable ParentTable {
			get { return mTable; }
		}

		/// <summary>
		/// Holds the committed row data
		/// </summary>
		private object[] mRowData;

		/// <summary>
		/// Holds the current row data that is not yet committed to the database
		/// </summary>
		private object[] mCurrentData;

		/// <summary>
		/// Holds the row data that has been updated to database but not yet committed.
		/// This is used to help with commit and rollback actions.
		/// </summary>
		private object[] mPersistedData;

		/// <summary>
		/// Place holder object used to indicate if a value has not been set or selected.
		/// </summary>
		private static readonly NotSet NOT_SET = new NotSet();

		/// <summary>
		/// Place holder class
		/// </summary>
		private class NotSet {
		}

		private RowStateEnum mPreviousRowState;

		private RowStateEnum mRowState;

		public RowStateEnum RowState {
			get { return mRowState; }
		}

		public enum RowStateEnum {
			/// <summary>
			/// Row Exists in table
			/// </summary>
			Exists,
			/// <summary>
			/// Row is yet to be inserting into table
			/// </summary>
			AddPending,
			/// <summary>
			/// Row has been inserted into table but not yet committed
			/// </summary>
			AddPerformedNotYetCommitted,
			/// <summary>
			/// Row is flaged to be deleted but still exists in database
			/// </summary>
			DeletePending,
			/// <summary>
			/// Row has been deleted from table but has not yet been committed
			/// </summary>
			DeletePerformedNotYetCommitted,
			/// <summary>
			/// Row has been deleted from table and has been committed
			/// </summary>
			DeletedAndCommitted
		}

		private bool mIsInit = false;

		protected ARow(ATable pTable) {

			if(pTable == null)
				throw new NullReferenceException($"{nameof(pTable)} cannot be null");

			mTable = pTable;
			mPreviousRowState = RowStateEnum.AddPending;
			mRowState = RowStateEnum.AddPending;
		}

		/// <summary>
		/// Initialises row data. This is called either when the row is loaded from a select query or when a property is accessed on a new row.
		/// </summary>
		private void Init() {

			if(!mIsInit) {

				mIsInit = true;

				mPreviousRowState = RowStateEnum.AddPending;
				mRowState = RowStateEnum.AddPending;

				mRowData = new object[mTable.Columns.Count];
				mCurrentData = new object[mTable.Columns.Count];
				mPersistedData = new object[mTable.Columns.Count];

				for(int index = 0; index < mTable.Columns.Count; index++) {
					mCurrentData[index] = NOT_SET;
					mPersistedData[index] = NOT_SET;
				}
			}
		}

		/// <summary>
		/// Loads row from select query
		/// </summary>
		/// <param name="pTable"></param>
		/// <param name="pSelectColumns"></param>
		/// <param name="pReader"></param>
		internal void LoadFromQuery(ADatabase pDatabase, ATable pTable, IList<ISelectable> pSelectColumns, System.Data.Common.DbDataReader pReader) {

			if(pDatabase == null)
				throw new NullReferenceException($"{nameof(pDatabase)} cannot be null");

			if(pTable == null)
				throw new NullReferenceException($"{nameof(pTable)} cannot be null");

			if(mIsInit)
				throw new Exception("Row is already initialised");

			mIsInit = true;

			mTable = pTable;
			mRowData = new object[mTable.Columns.Count];
			mCurrentData = new object[mTable.Columns.Count];
			mPersistedData = new object[mTable.Columns.Count];

			for(int index = 0; index < mTable.Columns.Count; index++) {

				AColumn column = mTable.Columns[index];
				int columnIndex = pSelectColumns.IndexOf(column);

				if(columnIndex != -1)
					mRowData[index] = !pReader.IsDBNull(columnIndex) ? column.GetValue(pDatabase, pReader, columnIndex) : null;
				else
					mRowData[index] = NOT_SET;

				mCurrentData[index] = mRowData[index];
				mPersistedData[index] = NOT_SET;
			}
			mPreviousRowState = RowStateEnum.Exists;
			mRowState = RowStateEnum.Exists;
		}

		/// <summary>
		/// Returns true if all the columns selected on this row returned null when loaded from the database. This is used to determine if a row returned null from a left join.
		/// </summary>
		/// <returns></returns>
		public bool IsRowNull() {

			bool isRowNull = true;

			for(int index = 0; index < mTable.Columns.Count; index++) {
				object value = mRowData[index];
				if(value != NOT_SET && value != null) {
					isRowNull = false;
					break;
				}
			}
			return isRowNull;
		}

		/// <summary>
		/// Returns value of column in this row.
		/// </summary>
		/// <param name="pColumn"></param>
		/// <returns></returns>
		internal object GetValue(AColumn pColumn) {

			if(!mIsInit)
				Init();

			int index = mTable.Columns.IndexOf(pColumn);

			if(index == -1)
				throw new Exception($"Column does not exist in table class. Table: {mTable.TableName}, column name: {pColumn.ColumnName}");

			object value = mCurrentData[index];

			if(value == NOT_SET) {

				if(pColumn.IsAutoId) {
					if(mRowState == RowStateEnum.AddPending)
						throw new Exception($"Auto id on column '{pColumn.ColumnName}' has not been set. Row probably hasn't been persisted to database");
					else
						throw new Exception($"Column '{pColumn.ColumnName}' is not set on row. This probably means it was not used in the select query");
				}

				if(mRowState == RowStateEnum.AddPending)
					value = pColumn.GetDefaultType();
				else
					throw new Exception($"Column '{pColumn.ColumnName}' is not set on row. This probably means it was not used in the select query");
			}
			if(mRowState == RowStateEnum.DeletePending || mRowState == RowStateEnum.DeletePerformedNotYetCommitted)
				throw new Exception($"Cannot access a columns data when is deleted. Table: {mTable.TableName}, column name: {pColumn.ColumnName}");

			return value;
		}

		/// <summary>
		/// Sets column value on row.
		/// </summary>
		/// <param name="pColumn"></param>
		/// <param name="pValue"></param>
		internal void SetValue(AColumn pColumn, object pValue) {

			if(!mIsInit)
				Init();

			int index = mTable.Columns.IndexOf(pColumn);

			if(index == -1)
				throw new Exception($"Column does not exist in table class. Table: {mTable.TableName}, column name: {pColumn.ColumnName}");

			if(mRowState == RowStateEnum.DeletePending || mRowState == RowStateEnum.DeletePerformedNotYetCommitted || mRowState == RowStateEnum.DeletedAndCommitted)
				throw new Exception($"Cannot set columns data when row is deleted. Table: {mTable.TableName}, column name: {pColumn.ColumnName}");

			mCurrentData[index] = pValue;
		}

		/// <summary>
		/// Flags row to be deleted from database. Row is deleted from database when the Update(...) method is called.
		/// </summary>
		public void Delete() {

			if(mRowState == RowStateEnum.AddPerformedNotYetCommitted)
				throw new Exception("You cannot delete a row that has been inserted but not yet committed");

			if(mRowState == RowStateEnum.DeletePending || mRowState == RowStateEnum.DeletePerformedNotYetCommitted || mRowState == RowStateEnum.DeletedAndCommitted)
				return;

			if(mRowState == RowStateEnum.AddPending)
				mRowState = RowStateEnum.DeletedAndCommitted;
			else
				mRowState = RowStateEnum.DeletePending;
		}

		/// <summary>
		/// Stores the transaction used to call Update() row with. This is used for checked the row isn't being updated to different transactions e.t.c.
		/// </summary>
		private Transaction mUpdateTransaction;

		/// <summary>
		/// Updates row changes to database. Adds new rows, updates existing rows with changes and deletes rows that are marked to be deleted.
		/// If row has any auto id columns defined those values are loaded from database after an insert.
		/// When updating rows there are two ways that the row is identified. The first is using the concurrency setting on the table or the global setting 'Settings.UseConcurrenyChecking'. (Note the table overrides the global setting).
		/// When this is ture the row is found by comparing all column values. If the row can't be found (i.e. Another process has altered the row) then a concurreny exception is thrown.
		/// If Settings.UseConcurrenyChecking == false then the row is found by looking up primary key values. If the row doesn't have a primary key defined an exception is thrown.
		/// </summary>
		/// <param name="pTransaction"></param>
		public void Update(Transaction pTransaction) {
			Update(pTransaction, false);
		}

		/// <summary>
		/// Updates row changes to database. Adds new rows, updates existing rows with changes and deletes rows that are marked to be deleted.
		/// If row has any auto id columns defined those values are loaded from database after an insert.
		/// When updating rows there are two ways that the row is identified. The first is using the concurrency setting on the table or the global setting 'Settings.UseConcurrenyChecking'. (Note the table overrides the global setting).
		/// When this is ture the row is found by comparing all column values. If the row can't be found (i.e. Another process has altered the row) then a concurreny exception is thrown.
		/// If Settings.UseConcurrenyChecking == false then the row is found by looking up primary key values. If the row doesn't have a primary key defined an exception is thrown.
		/// </summary>
		/// <param name="pTransaction"></param>
		/// <param name="pDiscardRowAfterUpdate">If true the row is not updated internally when transaction is committed / rolled back.</param>
		public void Update(Transaction pTransaction, bool pDiscardRowAfterUpdate) {

			if(pTransaction == null)
				throw new NullReferenceException($"{nameof(pTransaction)} cannot be null");

			if(mRowState == RowStateEnum.DeletedAndCommitted)
				throw new Exception("Row doesn't exist in database. You cannot call update on row that doesn't exist in the database.");

			if(mRowState == RowStateEnum.DeletePerformedNotYetCommitted)
				throw new Exception("Row has already been deleted from the database and is waiting for transaction to be committed. You cannot update a deleted row more than once within a transaction.");

			if(mRowState == RowStateEnum.AddPerformedNotYetCommitted)
				throw new Exception("Cannot call Update(...) more than once on a row within the same trasaction");

			if(!mIsInit)
				Init();

			if(mUpdateTransaction == null) {
				mUpdateTransaction = pTransaction;
				if(!pDiscardRowAfterUpdate) {
					pTransaction.CommitEvent += new Transaction.CommitPerformed(Transaction_CommitEvent);
					pTransaction.RollbackEvent += new Transaction.RollbackPerformed(Transaction_RollbackEvent);
				}
			}
			else if(mUpdateTransaction != pTransaction)
				throw new Exception("row.Update() has been called more than once on this row with a different transaction instance than before.");

			if(mTable.Columns.Count == 0)
				throw new Exception("Table has no fields defined. Cannot update row.");

			if(mRowState == RowStateEnum.AddPending) {

				Core.InsertBuilder insert = new Core.InsertBuilder(mTable);

				List<Sql.AColumn> autoIdColumns = new List<Sql.AColumn>();

				for(int index = 0; index < mTable.Columns.Count; index++) {

					AColumn column = mTable.Columns[index];

					if(column.IsAutoId)
						autoIdColumns.Add(column);
					else {
						object value = mCurrentData[index];

						if(value == NOT_SET)
							value = null;

						insert.SetInternal(column, value);
						mPersistedData[index] = value;
					}
				}

				if(autoIdColumns.Count == 0) {
					insert.Execute(pTransaction);
				}
				else {
					IResult result = insert.Returning(autoIdColumns.ToArray()).Execute(pTransaction);

					if(autoIdColumns.Count > 1) //For now only one auto id field in a table is supported. Need to find how to cope with multipule auto id fields.
						throw new Exception("Only one auto id field is supported");

					for(int index = 0; index < autoIdColumns.Count; index++) {
						Sql.AColumn column = autoIdColumns[index];

						int colIndex = mTable.Columns.IndexOf(column);

						mCurrentData[colIndex] = result.GetRow(column.Table, 0).GetValue(column);
						mPersistedData[colIndex] = mCurrentData[colIndex];
					}
				}
				mRowState = RowStateEnum.AddPerformedNotYetCommitted;
			}
			else if(mRowState == RowStateEnum.DeletePending) {

				Core.DeleteBuilder delete = new Sql.Core.DeleteBuilder(mTable);

				Condition condition = null;

				bool useConcurrenyChecking = mTable.UseConcurrenyChecking != null ? mTable.UseConcurrenyChecking.Value : Settings.UseConcurrenyChecking;

				bool conditionCreated = false;

				for(int index = 0; index < mTable.Columns.Count; index++) {

					AColumn column = mTable.Columns[index];

					if(useConcurrenyChecking || column.IsPrimaryKey) {

						conditionCreated = true;

						object value = mPersistedData[index];

						if(value == NOT_SET)
							value = mRowData[index];

						if(condition == null) {
							condition = value != NOT_SET ? new ColumnCondition(column, Operator.EQUALS, value) : (Condition)new IsNullCondition(column);
						}
						else
							condition = condition & (value != NOT_SET ? new ColumnCondition(column, Operator.EQUALS, value) : (Condition)new IsNullCondition(column));
					}
				}

				if(!conditionCreated)
					throw new Exception("There are no primary keys set on row and use concurrency checking is turned off. Unable to delete.");

				delete.Where(condition);
				IResult result = delete.Execute(pTransaction);

				if(result.RowsEffected != 1)
					throw new Exception("Row not updated. Possible data concurrency issue.");

				mRowState = RowStateEnum.DeletePerformedNotYetCommitted;
			}
			else {  //Update

				bool rowChanged = false;

				for(int index = 0; index < mRowData.Length; index++) {

					if(mRowData[index] is NotSet)
						throw new Exception("Not all columns were loaded in row. Unable to update.");

					if(!(mPersistedData[index] == null && mCurrentData[index] == null)) {
						if((mPersistedData[index] == null && mCurrentData[index] != null) || (mPersistedData[index] != null && mCurrentData[index] == null) || (!mPersistedData[index].Equals(mCurrentData[index]))) {
							rowChanged = true;
							break;
						}
					}
				}

				if(rowChanged) {

					Core.UpdateBuilder update = new Core.UpdateBuilder(mTable);

					Condition condition = null;

					bool useConcurrenyChecking = mTable.UseConcurrenyChecking != null ? mTable.UseConcurrenyChecking.Value : Settings.UseConcurrenyChecking;

					bool conditionCreated = false;

					for(int index = 0; index < mTable.Columns.Count; index++) {

						AColumn column = mTable.Columns[index];

						if(!column.IsAutoId && mPersistedData[index] != mCurrentData[index])
							update.SetInternal(column, mCurrentData[index]);

						if(useConcurrenyChecking || column.IsPrimaryKey) {

							conditionCreated = true;

							object value = mPersistedData[index];   //When updating no columns should have the NOT_SET place holder set

							if(value == NOT_SET)
								value = mRowData[index];

							if(condition == null)
								condition = value != null ? new ColumnCondition(column, Operator.EQUALS, value) : (Condition)new IsNullCondition(column);
							else
								condition = condition & (value != null ? new ColumnCondition(column, Operator.EQUALS, value) : (Condition)new IsNullCondition(column));
						}
						mPersistedData[index] = mCurrentData[index];
					}

					if(!conditionCreated)
						throw new Exception("There are no primary keys set on row and use concurrency checking is turned off. Unable to update.");

					update.Where(condition);

					IResult result = update.Execute(pTransaction);

					if(result.RowsEffected != 1)
						throw new Exception("Row not updated. Possible data concurrency issue.");
				}
			}
		}

		/// <summary>
		/// Called when transaction is committed.
		/// </summary>
		private void Transaction_CommitEvent() {

			mUpdateTransaction = null;

			if(mRowState == RowStateEnum.AddPending || mRowState == RowStateEnum.DeletePending)
				throw new Exception("Unexpected state. Row State must not be AddPending or DeletePending during a transaction commit. mRowState = '" + mRowState.ToString() + "'");

			if(mRowState == RowStateEnum.DeletePerformedNotYetCommitted || mRowState == RowStateEnum.DeletedAndCommitted) {
				mRowData = null;
				mCurrentData = null;
				mPersistedData = null;
				mPreviousRowState = RowStateEnum.DeletedAndCommitted;
				mRowState = RowStateEnum.DeletedAndCommitted;
			}
			else {
				for(int index = 0; index < mRowData.Length; index++) {
					mRowData[index] = mPersistedData[index];
					if(mCurrentData[index] == NOT_SET)
						mCurrentData[index] = null;
					mPersistedData[index] = null;
				}
				mPreviousRowState = RowStateEnum.Exists;
				mRowState = RowStateEnum.Exists;
			}
		}

		/// <summary>
		/// Called when transaction is rolled back.
		/// </summary>
		private void Transaction_RollbackEvent() {

			mUpdateTransaction = null;

			for(int index = 0; index < mTable.Columns.Count; index++) {
				AColumn column = mTable.Columns[index];
				if(mPreviousRowState == RowStateEnum.AddPending && column.IsAutoId)
					mCurrentData[index] = NOT_SET;
				mPersistedData[index] = NOT_SET;
			}

			if(mRowState == RowStateEnum.AddPerformedNotYetCommitted)
				mRowState = RowStateEnum.AddPending;
			else if(mRowState == RowStateEnum.DeletePerformedNotYetCommitted) {

				//In the case where are row is added and deleted within the same transaction the row is set to deleted and committed state because it does not exist in the database
				if(mPreviousRowState == RowStateEnum.AddPending)
					mRowState = RowStateEnum.DeletedAndCommitted;
				else
					mRowState = RowStateEnum.DeletePending;
			}
			else if(mRowState == RowStateEnum.Exists) {
				//Do nothing
			}
			else
				throw new Exception("Unexpected Row State. mRowState = '" + mRowState.ToString() + "'");
		}

		/// <summary>
		/// Returns the rows data size from the original (Last committed) data size. (Not the changed data size).
		/// </summary>
		/// <returns></returns>
		internal int GetOrigRowDataSizeInBytes() {

			int bytes = 0;

			for(int index = 0; index < mRowData.Length; index++) {
				object value = mRowData[index];
				if(value != null && value != NOT_SET)
					bytes += SqlHelper.GetAproxByteSizeOf(value);
			}
			return bytes;
		}

		#region Hide Members
		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public override int GetHashCode() {
			return base.GetHashCode();
		}
		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public override bool Equals(object obj) {
			return base.Equals(obj);
		}
		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public override string ToString() {
			return base.ToString();
		}
		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public new Type GetType() {
			return base.GetType();
		}
		#endregion
	}
}