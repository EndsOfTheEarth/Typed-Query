
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
using System.Threading;

namespace Sql {

    /// <summary>
    /// Transaction - Used to execute queries inside a database transaction
    /// </summary>
    public sealed class Transaction : IDisposable {

        private readonly ulong mId;

        /// <summary>
        /// Transaction id. Used for debugging what transactions are being used to execute queries.
        /// </summary>
        public ulong Id {
            get { return mId; }
        }

        private readonly System.Data.IsolationLevel mIsolationLevel;
        private readonly ADatabase mDatabase;

        public ADatabase Database {
            get { return mDatabase; }
        }

        private System.Data.Common.DbConnection mConnection;
        private System.Data.Common.DbTransaction mDbTransaction;

        /// <summary>
        /// Isolation level of transaction
        /// </summary>
        public System.Data.IsolationLevel IsolationLevel {
            get { return mIsolationLevel; }
        }

        /// <summary>
        /// Actual ado database transaction object. Only set once transaction has been used.
        /// </summary>
        internal System.Data.Common.DbTransaction DbTransaction {
            get { return mDbTransaction; }
        }

        /// <summary>
        /// Creates a transaction with Read Committed isolation level
        /// </summary>
        public Transaction(ADatabase pDatabase) : this(pDatabase, System.Data.IsolationLevel.ReadCommitted, false) {

        }
        public Transaction(ADatabase pDatabase, System.Data.IsolationLevel pIsolationLevel) : this(pDatabase, pIsolationLevel, false) {

        }

        /// <summary>
        /// Creates a transaction
        /// </summary>
        /// <param name="pIsolationLevel"></param>
        /// <param name="pForceUseOnThread">Checks that on the current thread only this transaction is used for queries until either commit or rollback are called.</param>
        public Transaction(ADatabase pDatabase, System.Data.IsolationLevel pIsolationLevel, bool pForceUseOnThread) {

            if(pDatabase == null) {
                throw new NullReferenceException($"{nameof(pDatabase)} cannot be null");
            }

            mDatabase = pDatabase;
            mId = GetNextId();
            mIsolationLevel = pIsolationLevel;

            if(pForceUseOnThread) {
                RegisterForceThread(this);
            }
        }

        private readonly static object sLockObject = new ulong();

        /// <summary>
        /// Counter used to give each transaction a unique id mainly for debugging purposes
        /// </summary>
        private static ulong sCounter = 0;

        /// <summary>
        /// Gets a new transaction id
        /// </summary>
        /// <returns></returns>
        private static ulong GetNextId() {

            lock(sLockObject) {

                if(sCounter == ulong.MaxValue) {  //Should never come close to reaching this
                    sCounter = ulong.MinValue;
                }
                sCounter++;
                return sCounter;
            }
        }

        private static Dictionary<Thread, Transaction> mForceThreadDict = new Dictionary<Thread, Transaction>();
        private Thread mForceThread;

        private static void RegisterForceThread(Transaction pTransaction) {

            if(pTransaction == null) {
                throw new NullReferenceException($"{nameof(pTransaction)} cannot be null");
            }

            lock(mForceThreadDict) {

                if(mForceThreadDict.ContainsKey(Thread.CurrentThread)) {
                    throw new Exception("Cannot create transaction as this thread is being forced to use another transaction");
                }
                mForceThreadDict.Add(Thread.CurrentThread, pTransaction);
                pTransaction.mForceThread = Thread.CurrentThread;
            }
        }

        private static void CheckForceThread(Transaction pTransaction) {

            lock(mForceThreadDict) {

                if(mForceThreadDict.Count > 0 && mForceThreadDict.ContainsKey(Thread.CurrentThread) && (pTransaction == null || mForceThreadDict[Thread.CurrentThread].mId != pTransaction.mId)) {
                    throw new Exception("Cannot create connection on this thread as it is being forced to use another transaction");
                }
            }
        }

        private static void ReleaseForceThread(Transaction pTransaction) {

            if(pTransaction == null) {
                throw new NullReferenceException($"{nameof(pTransaction)} cannot be null");
            }

            lock(mForceThreadDict) {

                if(pTransaction.mForceThread != null) {
                    mForceThreadDict.Remove(pTransaction.mForceThread);
                }
            }
        }

        internal System.Data.Common.DbConnection GetOrSetConnection(ADatabase pDatabase) {

            if(pDatabase == null) {
                throw new NullReferenceException($"{nameof(pDatabase)} cannot be null");
            }

            if(mDatabase != null && mDatabase != pDatabase) {
                throw new Exception("Transaction connecting was opened using a different database class. All queries used within a transaction must have tables using the same ADatabase class.");
            }

            lock(this) {

                if(mConnection == null) {
                    mConnection = pDatabase.GetConnection(false);
                }
                return mConnection;
            }
        }
        internal System.Data.Common.DbTransaction GetOrSetDbTransaction(ADatabase pDatabase) {

            if(pDatabase == null) {
                throw new NullReferenceException($"{nameof(pDatabase)} cannot be null");
            }

            lock(this) {

                if(mDatabase != null && mDatabase != pDatabase) {
                    throw new Exception("Transaction connecting was opened using a different database class. All queries used within a transaction must have tables using the same ADatabase class.");
                }

                if(mConnection == null) {
                    GetOrSetConnection(pDatabase);
                }

                if(mDbTransaction == null) {
                    mDbTransaction = mConnection.BeginTransaction(mIsolationLevel);
                }
                return mDbTransaction;
            }
        }

        internal static System.Data.Common.DbCommand CreateCommand(System.Data.Common.DbConnection pConnection, Transaction pTransaction) {

            if(pConnection == null) {
                throw new NullReferenceException($"{nameof(pConnection)} cannot be null");
            }

            CheckForceThread(pTransaction);

            lock(pConnection) {
                return pConnection.CreateCommand();
            }
        }

        public delegate void CommitPerformed();

        /// <summary>
        /// Fired when commit is performed on the transaction
        /// </summary>
        public event CommitPerformed CommitEvent = delegate { };

        /// <summary>
        /// Called when rollback is performed on transaction
        /// </summary>
        public delegate void RollbackPerformed();
        public event RollbackPerformed RollbackEvent = delegate { };

        private bool mRollbackOnly;

        /// <summary>
        /// Stop the transaction from being committed. If Commit() is called and exception will be thrown
        /// </summary>
        public void SetRollbackOnly() {
            mRollbackOnly = true;
        }

        private bool mHasBeenCommitted = false;

        /// <summary>
        /// Commits transaction
        /// </summary>
        public void Commit() {

            lock(this) {

                ReleaseForceThread(this);

                if(mHasBeenCommitted) {
                    throw new Exception("Transaction has already been committed");
                }

                mHasBeenCommitted = true;

                if(mHasBeenRolledBack) {
                    throw new Exception("Transaction has already been rolled back");
                }

                if(mDbTransaction == null) {
                    return;
                }

                if(mRollbackOnly) {
                    throw new Exception("Transaction has been set so that it can only be rolled back");
                }

                try {

                    using(System.Data.Common.DbConnection connection = mDbTransaction.Connection) {

                        mDbTransaction.Commit();

                        if(CommitEvent != null) {
                            CommitEvent();
                            CommitEvent = null;
                        }

                        if(IsolationLevel != System.Data.IsolationLevel.ReadCommitted && mDatabase.DatabaseType == DatabaseType.Mssql) {

                            using(System.Data.Common.DbCommand command = connection.CreateCommand()) {
                                command.CommandText = "SET TRANSACTION ISOLATION LEVEL READ COMMITTED";
                                command.ExecuteNonQuery();
                            }
                        }
                    }
                }
                catch {
                    System.Data.SqlClient.SqlConnection.ClearAllPools();
                    throw;
                }
                finally {
                    mDbTransaction = null;
                }
            }
        }

        private bool mHasBeenRolledBack = false;

        /// <summary>
        /// Rollback transaction
        /// </summary>
        public void Rollback() {

            ReleaseForceThread(this);

            if(mHasBeenRolledBack) {
                throw new Exception("Transaction has already been rolled back");
            }

            mHasBeenRolledBack = true;

            if(mDbTransaction == null) {

                if(RollbackEvent != null) {
                    RollbackEvent();
                    RollbackEvent = null;
                }
                return;
            }
            try {

                using(System.Data.Common.DbConnection connection = mDbTransaction.Connection) {

                    using(mDbTransaction) {

                        if(mDbTransaction.Connection != null && mDbTransaction.Connection.State != System.Data.ConnectionState.Closed && mDbTransaction.Connection.State != System.Data.ConnectionState.Broken) {
                            mDbTransaction.Rollback();
                        }
                    }

                    if(IsolationLevel != System.Data.IsolationLevel.ReadCommitted && mDatabase.DatabaseType == DatabaseType.Mssql) {

                        using(System.Data.Common.DbCommand command = connection.CreateCommand()) {
                            command.CommandText = "SET TRANSACTION ISOLATION LEVEL READ COMMITTED";
                            command.ExecuteNonQuery();
                        }
                    }
                }
            }
            catch {
                System.Data.SqlClient.SqlConnection.ClearAllPools();
                throw;
            }
            finally {
                try {
                    if(RollbackEvent != null) {
                        RollbackEvent();
                        RollbackEvent = null;
                    }
                }
                catch { }   //Need to handle
            }
        }

        public System.Data.ConnectionState ConnectionState {
            get {
                return mConnection != null ? mConnection.State : System.Data.ConnectionState.Closed;
            }
        }
        /// <summary>
        /// Rolls back transaction if not committed
        /// </summary>
        public void Dispose() {

            if(!mHasBeenCommitted && !mHasBeenRolledBack) {
                Rollback();
            }
        }
    }
}