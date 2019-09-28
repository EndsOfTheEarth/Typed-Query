
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

namespace Sql.Core {

    internal class QueryResult : IResult {

        private readonly IDictionary<ATable, IList<ARow>> mTableRows = new Dictionary<ATable, IList<ARow>>();
        private readonly IDictionary<ISelectable, IList<object?>> mFunctionValues = new Dictionary<ISelectable, IList<object?>>();

        private readonly int mRows;
        public int RowsEffected { get; private set; }

        private readonly string mSqlQuery;

        internal QueryResult(int pRowsEffected, string pSqlQuery) {
            RowsEffected = pRowsEffected;
            mSqlQuery = !string.IsNullOrEmpty(pSqlQuery) ? pSqlQuery : string.Empty;
        }
        internal QueryResult(ADatabase pDatabase, IList<ISelectable> pSelectColumns, System.Data.Common.DbDataReader pReader, string pSqlQuery) {

            if(pDatabase == null) {
                throw new NullReferenceException($"{ nameof(pDatabase) } cannot be null");
            }

            mSqlQuery = !string.IsNullOrEmpty(pSqlQuery) ? pSqlQuery : string.Empty;

            List<ATable> tables = new List<ATable>();
            List<ISelectable> functions = new List<ISelectable>();

            for(int index = 0; index < pSelectColumns.Count; index++) {

                ISelectable column = pSelectColumns[index];

                if(column is AColumn) {

                    ATable table = ((AColumn)column).Table;

                    if(!tables.Contains(table)) {
                        tables.Add(table);
                    }
                }
                else {

                    if(!functions.Contains(column)) {
                        functions.Add(column);
                    }
                }
            }

            while(pReader.Read()) {

                for(int tablesIndex = 0; tablesIndex < tables.Count; tablesIndex++) {

                    ATable table = tables[tablesIndex];

                    IList<ARow> rows;

                    if(!mTableRows.ContainsKey(table)) {
                        rows = new List<ARow>();
                        mTableRows.Add(table, rows);
                    }
                    else {
                        rows = mTableRows[table];
                    }

                    ARow row;

                    try {
                        row = (ARow)table.RowType.GetConstructor(new Type[] { })!.Invoke(null);
                    }
                    catch(Exception e) {
                        throw new Exception("Failed to create a new instance of Row. This might be because there is no constructor on the row that has no parameters. Also see inner exception.", e);
                    }

                    row.LoadFromQuery(pDatabase, table, pSelectColumns, pReader);
                    rows.Add(row);
                }

                for(int functionIndex = 0; functionIndex < functions.Count; functionIndex++) {

                    ISelectable function = functions[functionIndex];

                    IList<object?> values;

                    if(!mFunctionValues.ContainsKey(function)) {
                        values = new List<object?>();
                        mFunctionValues.Add(function, values);
                    }
                    else {
                        values = mFunctionValues[function];
                    }
                    values.Add(function.GetValue(pDatabase, pReader, pSelectColumns.IndexOf(function)));
                }
                mRows++;
            }
            RowsEffected = mRows;
        }

        public int Count {
            get { return mRows; }
        }
        public string SqlQuery {
            get { return mSqlQuery; }
        }

        public ARow GetRow(ATable pTable, int pIndex) {

            if(pTable == null) {
                throw new NullReferenceException($"{ nameof(pTable) } cannot be null");
            }

            if(pIndex < 0) {
                throw new IndexOutOfRangeException($"{ nameof(pIndex) } must >= 0. pIndex == { pIndex.ToString() }");
            }

            if(!mTableRows.ContainsKey(pTable)) {
                throw new Exception($"Table instance of type '{ pTable.GetType() }' does not exist in result. Check that is was included in select portion of query");
            }
            return mTableRows[pTable][pIndex];
        }

        public object? GetValue(ISelectable pFunction, int pIndex) {

            if(pFunction == null) {
                throw new NullReferenceException($"{ nameof(pFunction) } cannot be null");
            }

            if(pIndex < 0) {
                throw new IndexOutOfRangeException($"{ nameof(pIndex) } must >= 0. pIndex == { pIndex.ToString() }");
            }
            return mFunctionValues[pFunction][pIndex];
        }

        public int GetDataSetSizeInBytes() {

            int bytes = 0;

            foreach(ATable table in mTableRows.Keys) {

                foreach(ARow row in mTableRows[table]) {
                    bytes += row.GetOrigRowDataSizeInBytes();
                }
            }

            foreach(ISelectable function in mFunctionValues.Keys) {

                foreach(object? value in mFunctionValues[function]) {
                    bytes += SqlHelper.GetAproxByteSizeOf(value);
                }
            }
            return bytes;
        }
    }
}