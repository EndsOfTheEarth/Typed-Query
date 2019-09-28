
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

    public class NGuidKeyColumn<TABLE> : AColumn {

        public NGuidKeyColumn(ATable pTable, string pColumnName)
            : base(pTable, pColumnName, false, true) {
        }
        public NGuidKeyColumn(ATable pTable, string pColumnName, bool pIsPrimaryKey)
            : base(pTable, pColumnName, pIsPrimaryKey, true) {
        }

        public static Condition operator ==(NGuidKeyColumn<TABLE> pColumnA, GuidKeyColumn<TABLE> pColumnB) {
            return new ColumnCondition(pColumnA, Sql.Operator.EQUALS, pColumnB);
        }
        public static Condition operator ==(NGuidKeyColumn<TABLE> pColumnA, NGuidKeyColumn<TABLE> pColumnB) {
            return new ColumnCondition(pColumnA, Sql.Operator.EQUALS, pColumnB);
        }
        public static Condition operator ==(NGuidKeyColumn<TABLE> pColumnA, GuidKey<TABLE> pValue) {
            return new ColumnCondition(pColumnA, Sql.Operator.EQUALS, pValue.Value);
        }

        public static Condition operator !=(NGuidKeyColumn<TABLE> pColumnA, GuidKeyColumn<TABLE> pColumnB) {
            return new ColumnCondition(pColumnA, Sql.Operator.NOT_EQUALS, pColumnB);
        }
        public static Condition operator !=(NGuidKeyColumn<TABLE> pColumnA, NGuidKeyColumn<TABLE> pColumnB) {
            return new ColumnCondition(pColumnA, Sql.Operator.NOT_EQUALS, pColumnB);
        }
        public static Condition operator !=(NGuidKeyColumn<TABLE> pColumnA, GuidKey<TABLE> pValue) {
            return new ColumnCondition(pColumnA, Sql.Operator.NOT_EQUALS, pValue.Value);
        }

        public Condition In(IList<GuidKey<TABLE>> pList) {

            List<Guid> list = new List<Guid>(pList.Count);

            foreach(GuidKey<TABLE> value in pList) {
                list.Add(value.Value);
            }
            return new InCondition<Guid>(this, list);
        }
        public Condition NotIn(IList<GuidKey<TABLE>> pList) {

            List<Guid> list = new List<Guid>(pList.Count);

            foreach(GuidKey<TABLE> value in pList) {
                list.Add(value.Value);
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

            foreach(GuidKey<TABLE> value in pValues) {
                list.Add(value.Value);
            }
            return new InCondition<Guid>(this, list);
        }
        public Condition NotIn(params GuidKey<TABLE>[] pValues) {

            List<Guid> list = new List<Guid>(pValues.Length);

            foreach(GuidKey<TABLE> value in pValues) {
                list.Add(value.Value);
            }
            return new NotInCondition<Guid>(this, list);
        }

        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        public override object? GetValue(ADatabase pDatabase, System.Data.Common.DbDataReader pReader, int pColumnIndex) {

            if(pReader.IsDBNull(pColumnIndex)) {
                return null;
            }

            Type dataType = pReader.GetFieldType(pColumnIndex);

            if(dataType != typeof(Guid) && dataType != typeof(Guid?)) {
                throw new Exception($"Row column data is not of the correct type. Expected Guid or Guid? value instead got '{ dataType.ToString() }'. This probably means that the database and table column data types are not matching. Please run the definition tester to check table columns are of the correct type. Table: '{ Table.TableName }' Column: '{ ColumnName }'");
            }

            return (Guid?)pReader.GetGuid(pColumnIndex);
        }
        public GuidKey<TABLE>? ValueOf(ARow pRow) {
            object? value = pRow.GetValue(this);
            return value != null ? new GuidKey<TABLE>((Guid)value) : (GuidKey<TABLE>?)null;
        }
        public void SetValue(ARow pRow, GuidKey<TABLE>? pValue) {
            pRow.SetValue(this, pValue != null ? pValue.Value.Value : (Guid?)null);
        }

        internal override void TestSetValue(ARow pRow, object? pValue) {
            SetValue(pRow, (GuidKey<TABLE>?)pValue);
        }
        internal override object? TestGetValue(ARow pRow) {
            return ValueOf(pRow);
        }

        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        public override int GetHashCode() {
            return base.GetHashCode();
        }
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        public override bool Equals(object? obj) {
            return base.Equals(obj);
        }
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        public override string? ToString() {
            return base.ToString();
        }
        public override System.Data.DbType DbType {
            get { return System.Data.DbType.Guid; }
        }
        public override object? GetDefaultType() {
            return null;
        }
    }
}