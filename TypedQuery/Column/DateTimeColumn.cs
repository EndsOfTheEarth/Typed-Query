﻿
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

namespace Sql.Column {

    /// <summary>
    /// Datetime column
    /// </summary>
    public class DateTimeColumn : AColumn {

        public DateTimeColumn(ATable pTable, string pColumnName)
            : base(pTable, pColumnName, false, false) {
        }
        public DateTimeColumn(ATable pTable, string pColumnName, bool pIsPrimaryKey)
            : base(pTable, pColumnName, pIsPrimaryKey, false) {
        }

        public static Condition operator ==(DateTimeColumn pColumnA, DateTimeColumn pColumnB) {
            return new ColumnCondition(pColumnA, Sql.Operator.EQUALS, pColumnB);
        }
        public static Condition operator ==(DateTimeColumn pColumnA, NDateTimeColumn pColumnB) {
            return new ColumnCondition(pColumnA, Sql.Operator.EQUALS, pColumnB);
        }
        public static Condition operator ==(DateTimeColumn pColumnA, DateTime pValue) {
            return new ColumnCondition(pColumnA, Sql.Operator.EQUALS, pValue);
        }

        public static Condition operator !=(DateTimeColumn pColumnA, DateTimeColumn pColumnB) {
            return new ColumnCondition(pColumnA, Sql.Operator.NOT_EQUALS, pColumnB);
        }
        public static Condition operator !=(DateTimeColumn pColumnA, NDateTimeColumn pColumnB) {
            return new ColumnCondition(pColumnA, Sql.Operator.NOT_EQUALS, pColumnB);
        }
        public static Condition operator !=(DateTimeColumn pColumnA, DateTime pValue) {
            return new ColumnCondition(pColumnA, Sql.Operator.NOT_EQUALS, pValue);
        }

        public static Condition operator >(DateTimeColumn pColumnA, DateTimeColumn pColumnB) {
            return new ColumnCondition(pColumnA, Sql.Operator.GREATER_THAN, pColumnB);
        }
        public static Condition operator >(DateTimeColumn pColumnA, NDateTimeColumn pColumnB) {
            return new ColumnCondition(pColumnA, Sql.Operator.GREATER_THAN, pColumnB);
        }
        public static Condition operator >(DateTimeColumn pColumnA, DateTime pValue) {
            return new ColumnCondition(pColumnA, Sql.Operator.GREATER_THAN, pValue);
        }

        public static Condition operator >=(DateTimeColumn pColumnA, DateTimeColumn pColumnB) {
            return new ColumnCondition(pColumnA, Sql.Operator.GREATER_THAN_OR_EQUAL, pColumnB);
        }
        public static Condition operator >=(DateTimeColumn pColumnA, NDateTimeColumn pColumnB) {
            return new ColumnCondition(pColumnA, Sql.Operator.GREATER_THAN_OR_EQUAL, pColumnB);
        }
        public static Condition operator >=(DateTimeColumn pColumnA, DateTime pValue) {
            return new ColumnCondition(pColumnA, Sql.Operator.GREATER_THAN_OR_EQUAL, pValue);
        }

        public static Condition operator <(DateTimeColumn pColumnA, DateTimeColumn pColumnB) {
            return new ColumnCondition(pColumnA, Sql.Operator.LESS_THAN, pColumnB);
        }
        public static Condition operator <(DateTimeColumn pColumnA, NDateTimeColumn pColumnB) {
            return new ColumnCondition(pColumnA, Sql.Operator.LESS_THAN, pColumnB);
        }
        public static Condition operator <(DateTimeColumn pColumnA, DateTime pValue) {
            return new ColumnCondition(pColumnA, Sql.Operator.LESS_THAN, pValue);
        }

        public static Condition operator <=(DateTimeColumn pColumnA, DateTimeColumn pColumnB) {
            return new ColumnCondition(pColumnA, Sql.Operator.LESS_THAN_OR_EQUAL, pColumnB);
        }
        public static Condition operator <=(DateTimeColumn pColumnA, NDateTimeColumn pColumnB) {
            return new ColumnCondition(pColumnA, Sql.Operator.LESS_THAN_OR_EQUAL, pColumnB);
        }
        public static Condition operator <=(DateTimeColumn pColumnA, DateTime pValue) {
            return new ColumnCondition(pColumnA, Sql.Operator.LESS_THAN_OR_EQUAL, pValue);
        }

        public Condition In(IList<DateTime> pList) {
            return new InCondition<DateTime>(this, pList);
        }
        public Condition NotIn(IList<DateTime> pList) {
            return new NotInCondition<DateTime>(this, pList);
        }

        public Condition In(Interfaces.IExecute pNestedQuery) {
            return new NestedQueryCondition(this, Sql.Operator.IN, pNestedQuery);
        }
        public Condition NotIn(Interfaces.IExecute pNestedQuery) {
            return new NestedQueryCondition(this, Sql.Operator.NOT_IN, pNestedQuery);
        }

        public Condition In(params DateTime[] pValues) {
            return new InCondition<DateTime>(this, pValues);
        }
        public Condition NotIn(params DateTime[] pValues) {
            return new NotInCondition<DateTime>(this, pValues);
        }
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        public override object GetValue(ADatabase pDatabase, System.Data.Common.DbDataReader pReader, int pColumnIndex) {

            Type dataType = pReader.GetFieldType(pColumnIndex);

            if(dataType != typeof(DateTime)) {
                throw new Exception($"Row column data is not of the correct type. Expected DateTime value instead got '{ dataType.ToString() }'. This probably means that the database and table column data types are not matching. Please run the definition tester to check table columns are of the correct type. Table: '{ Table.TableName }' Column: '{ ColumnName }'");
            }

            return pReader.GetDateTime(pColumnIndex);
        }
        public DateTime ValueOf(ARow pRow) {
            return (DateTime)pRow.GetValue(this)!;
        }
        public void SetValue(ARow pRow, DateTime pValue) {
            pRow.SetValue(this, pValue);
        }

        internal override void TestSetValue(ARow pRow, object? pValue) {
            SetValue(pRow, (DateTime)pValue!);
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
            get { return System.Data.DbType.DateTime; }
        }
        public override object? GetDefaultType() {
            return DateTime.MinValue;
        }
    }
}