
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

    /// <summary>
    /// Abstract table
    /// </summary>
    public abstract class ATable : ISelectableColumns {

        private readonly string mTableName;
        private readonly string mSchema;
        private readonly bool mIsView;
        private List<AColumn> mColumns = new List<AColumn>();
        private readonly Type mRowType;
        private readonly bool mIsTemporaryTable;
        private readonly bool? mUseConcurrenyChecking;

        /// <summary>
        /// Name of table in database.
        /// </summary>
        public string TableName {
            get { return mTableName; }
        }

        public string Schema {
            get { return mSchema; }
        }

        public bool IsView {
            get { return mIsView; }
        }

        /// <summary>
        /// Columns that belong to table
        /// </summary>
        public IList<AColumn> Columns {
            /* Return copy so calling code cannot change column list. Note column objects are immutable so no need to copy those. */
            get { return new List<AColumn>(mColumns.ToArray()); }
        }
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        public ISelectable[] SelectableColumns {
            get { return mColumns.ToArray(); }
        }
        internal Type RowType {
            get { return mRowType; }
        }

        /// <summary>
        /// Returns true if table is a temporary table.
        /// </summary>
        public bool IsTemporaryTable {
            get { return mIsTemporaryTable; }
        }

        /// <summary>
        /// If true row updates use all column values to find row in table. If false only the primary key columns are used to find row.
        /// If null then the gloabl setting Settings.UseConcurrenyChecking value is used.
        /// </summary>
        public bool? UseConcurrenyChecking {
            get { return mUseConcurrenyChecking; }
        }

        protected ATable(string pTableName, string pSchema, bool pIsView, Type pRowType) : this(pTableName, pSchema, pIsView, pRowType, false, null) {

        }
        protected ATable(string pTableName, string pSchema, bool pIsView, bool pUseConcurrenyChecking, Type pRowType) : this(pTableName, pSchema, pIsView, pRowType, false, pUseConcurrenyChecking) {

        }

        /// <summary>
        /// Creates a temporary table with an auto generated table name
        /// </summary>
        /// <param name="pRowType"></param>
        protected ATable(Type pRowType, string pTempTableName) {

            if(!pRowType.IsSubclassOf(typeof(ARow))) {
                throw new Exception($"{nameof(pRowType)} must be a subclass of Sql.ARow");
            }

            if(string.IsNullOrEmpty(pTempTableName)) {
                throw new ArgumentException($"{nameof(pTempTableName)} cannot be null or empty");
            }

            mRowType = pRowType;
            mSchema = string.Empty;
            mIsTemporaryTable = true;
            mTableName = pTempTableName;
            mUseConcurrenyChecking = null;
        }
        private ATable(string pTableName, string pSchema, bool pIsView, Type pRowType, bool pIsTemporaryTable, bool? pUseConcurrenyChecking) {

            if(string.IsNullOrWhiteSpace(pTableName)) {
                throw new Exception($"{nameof(pTableName)} cannot be null or empty");
            }

            if(pRowType == null) {
                throw new NullReferenceException($"{nameof(pRowType)} cannot be null");
            }

            if(!pRowType.IsSubclassOf(typeof(ARow))) {
                throw new Exception($"{nameof(pRowType)} must be a subclass of Sql.ARow");
            }

            if(pTableName.Contains("'")) {
                throw new Exception($"{nameof(pTableName)} cannot any single quote characters. Value = {pTableName}");
            }

            if(pSchema.Contains("'")) {
                throw new Exception($"{nameof(pSchema)} cannot any single quote characters. Value = {pTableName}");
            }

            mTableName = pTableName;
            mSchema = pSchema ?? string.Empty;
            mIsView = pIsView;
            mRowType = pRowType;
            mIsTemporaryTable = pIsTemporaryTable;
            mUseConcurrenyChecking = pUseConcurrenyChecking;
        }

        /// <summary>
        /// Sets columns on table
        /// </summary>
        /// <param name="pColumns"></param>
        protected void AddColumns(params AColumn[] pColumns) {

            if(pColumns == null || pColumns.Length == 0) {
                throw new Exception("pColumns cannot be null or empty");
            }
            if(mColumns.Count > 0) {
                throw new Exception("Columns are already set on table");
            }
            mColumns.AddRange(pColumns);
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