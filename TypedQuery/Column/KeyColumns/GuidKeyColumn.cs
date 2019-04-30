
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

using Sql.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sql.Column {

    public class GuidKeyColumn<TABLE> : AColumn {

        public GuidKeyColumn(ATable pTable, string pColumnName)
            : base(pTable, pColumnName, false, false) {
        }
        public GuidKeyColumn(ATable pTable, string pColumnName, bool pIsPrimaryKey)
            : base(pTable, pColumnName, pIsPrimaryKey, false) {
        }

        public static Condition operator ==(GuidKeyColumn<TABLE> pColumnA, GuidKeyColumn<TABLE> pColumnB) {
            return new ColumnCondition(pColumnA, Sql.Operator.EQUALS, pColumnB);
        }
        public static Condition operator ==(GuidKeyColumn<TABLE> pColumnA, NGuidKeyColumn<TABLE> pColumnB) {
            return new ColumnCondition(pColumnA, Sql.Operator.EQUALS, pColumnB);
        }
        public static Condition operator ==(GuidKeyColumn<TABLE> pColumnA, GuidKey<TABLE> pValue) {
            return new ColumnCondition(pColumnA, Sql.Operator.EQUALS, pValue.Value);
        }

        public static Condition operator !=(GuidKeyColumn<TABLE> pColumnA, GuidKeyColumn<TABLE> pColumnB) {
            return new ColumnCondition(pColumnA, Sql.Operator.NOT_EQUALS, pColumnB);
        }
        public static Condition operator !=(GuidKeyColumn<TABLE> pColumnA, NGuidKeyColumn<TABLE> pColumnB) {
            return new ColumnCondition(pColumnA, Sql.Operator.NOT_EQUALS, pColumnB);
        }
        public static Condition operator !=(GuidKeyColumn<TABLE> pColumnA, GuidKey<TABLE> pValue) {
            return new ColumnCondition(pColumnA, Sql.Operator.NOT_EQUALS, pValue.Value);
        }

        public Condition In(List<GuidKey<TABLE>> pList) {

            List<Guid> list = new List<Guid>(pList.Count);

            foreach(GuidKey<TABLE> key in pList) {
                list.Add(key.Value);
            }
            return new InCondition<Guid>(this, list);
        }
        public Condition NotIn(List<GuidKey<TABLE>> pList) {

            List<Guid> list = new List<Guid>(pList.Count);

            foreach(GuidKey<TABLE> key in pList) {
                list.Add(key.Value);
            }
            return new NotInCondition<Guid>(this, list);
        }

        public Condition In(Interfaces.IExecute pNestedQuery) {
            return new NestedQueryCondition(this, Sql.Operator.IN, pNestedQuery);
        }
        public Condition NotIn(Interfaces.IExecute pNestedQuery) {
            return new NestedQueryCondition(this, Sql.Operator.NOT_IN, pNestedQuery);
        }

        public Condition In(params GuidKey<TABLE>[] pValues) {

            List<Guid> list = new List<Guid>(pValues.Length);

            foreach(GuidKey<TABLE> key in pValues) {
                list.Add(key.Value);
            }
            return new InCondition<Guid>(this, list);
        }
        public Condition NotIn(params GuidKey<TABLE>[] pValues) {

            List<Guid> list = new List<Guid>(pValues.Length);

            foreach(GuidKey<TABLE> key in pValues) {
                list.Add(key.Value);
            }
            return new NotInCondition<Guid>(this, list);
        }

        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        public override object GetValue(ADatabase pDatabase, System.Data.Common.DbDataReader pReader, int pColumnIndex) {

            Type dataType = pReader.GetFieldType(pColumnIndex);

            if(dataType != typeof(Guid)) {
                throw new Exception($"Row column data is not of the correct type. Expected Guid value instead got '{ dataType.ToString() }'. This probably means that the database and table column data types are not matching. Please run the definition tester to check table columns are of the correct type. Table: '{ Table.TableName }' Column: '{ ColumnName }'");
            }

            return pReader.GetGuid(pColumnIndex);
        }
        public GuidKey<TABLE> ValueOf(ARow pRow) {
            return new GuidKey<TABLE>((Guid)pRow.GetValue(this));
        }
        public void SetValue(ARow pRow, GuidKey<TABLE> pValue) {
            pRow.SetValue(this, pValue.Value);
        }

        internal override void TestSetValue(ARow pRow, object pValue) {
            SetValue(pRow, (GuidKey<TABLE>)pValue);
        }
        internal override object TestGetValue(ARow pRow) {
            return ValueOf(pRow);
        }

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
        public override System.Data.DbType DbType {
            get { return System.Data.DbType.Guid; }
        }
        public override object GetDefaultType() {
            return new GuidKey<TABLE>(Guid.Empty);
        }
    }
}