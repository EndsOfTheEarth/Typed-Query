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

using Sql.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sql.Column {

    public class NStringKeyColumn<TABLE> : AColumn, Sql.IColumnLength {

        public int MaxLength { get; private set; }

        public NStringKeyColumn(ATable pTable, string pColumnName, int pMaxLength) : base(pTable, pColumnName, false, true) {

            if(pMaxLength <= 0) {
                throw new Exception($"{nameof(pMaxLength)} must be >= 1");
            }

            MaxLength = pMaxLength;
        }
        public NStringKeyColumn(ATable pTable, string pColumnName, bool pIsPrimaryKey, int pMaxLength) : base(pTable, pColumnName, pIsPrimaryKey, true) {

            if(pMaxLength <= 0) {
                throw new Exception($"{nameof(pMaxLength)} must be >= 1");
            }

            MaxLength = pMaxLength;
        }

        public static Condition operator ==(NStringKeyColumn<TABLE> pColumnA, NStringKeyColumn<TABLE> pColumnB) {

            if(((object)pColumnB) == null) {
                throw new NullReferenceException($"{nameof(pColumnB)} cannot be null");
            }

            return new ColumnCondition(pColumnA, Sql.Operator.EQUALS, pColumnB);
        }

        public static Condition operator !=(NStringKeyColumn<TABLE> pColumnA, NStringKeyColumn<TABLE> pColumnB) {

            if(((object)pColumnB) == null) {
                throw new NullReferenceException($"{nameof(pColumnB)} cannot be null");
            }

            return new ColumnCondition(pColumnA, Sql.Operator.NOT_EQUALS, pColumnB);
        }

        public static Condition operator ==(NStringKeyColumn<TABLE> pColumnA, StringKeyColumn<TABLE> pColumnB) {

            if(((object)pColumnB) == null) {
                throw new NullReferenceException($"{nameof(pColumnB)} cannot be null");
            }

            return new ColumnCondition(pColumnA, Sql.Operator.EQUALS, pColumnB);
        }

        public static Condition operator !=(NStringKeyColumn<TABLE> pColumnA, StringKeyColumn<TABLE> pColumnB) {

            if(((object)pColumnB) == null) {
                throw new NullReferenceException($"{nameof(pColumnB)} cannot be null");
            }

            return new ColumnCondition(pColumnA, Sql.Operator.NOT_EQUALS, pColumnB);
        }

        public static Condition operator ==(NStringKeyColumn<TABLE> pColumnA, StringKey<TABLE> pValue) {

            if(pValue == null) {
                throw new NullReferenceException($"{nameof(pValue)} cannot be null when using the == operator. Use .IsNull() method if a null condition is required. 'NStringKeyColumn = null' is an undefined condition in sql so this library disallows it.");
            }

            return new ColumnCondition(pColumnA, Sql.Operator.EQUALS, pValue.Value);
        }

        public static Condition operator !=(NStringKeyColumn<TABLE> pColumnA, StringKey<TABLE> pValue) {

            if(pValue == null) {
                throw new NullReferenceException($"{nameof(pValue)} cannot be null when using the != operator. Use .IsNull() method if a null condition is required. 'NStringKeyColumn != null' is an undefined condition in sql so this library disallows it.");
            }

            return new ColumnCondition(pColumnA, Sql.Operator.NOT_EQUALS, pValue.Value);
        }

        public Condition Like(StringKey<TABLE> pValue) {

            if(pValue == null) {
                throw new NullReferenceException($"{nameof(pValue)} cannot be null when using the 'like' operator. 'NStringKeyColumn like null' is an undefined condition in sql so this library disallows it.");
            }

            return new ColumnCondition(this, Operator.LIKE, pValue.Value);
        }

        public Condition NotLike(StringKey<TABLE> pValue) {

            if(pValue == null) {
                throw new NullReferenceException($"{nameof(pValue)} cannot be null when using the 'not like' operator. 'NStringKeyColumn not like null' is an undefined condition in sql so this library disallows it.");
            }

            return new ColumnCondition(this, Operator.NOT_LIKE, pValue.Value);
        }

        public Condition In(List<StringKey<TABLE>> pList) {

            List<string> list = new List<string>();

            foreach(StringKey<TABLE> value in pList) {
                if(value == null) {
                    throw new NullReferenceException($"A value in {nameof(pList)} is null. 'NStringKeyColumn IN (null)' is an undefined condition in sql so this library disallows it.");
                }
                list.Add(value.Value);
            }

            return new InCondition<string>(this, list);
        }
        public Condition NotIn(List<StringKey<TABLE>> pList) {

            List<string> list = new List<string>();

            foreach(StringKey<TABLE> value in pList) {

                if(value == null) {
                    throw new NullReferenceException($"A value in {nameof(pList)} is null. 'NStringKeyColumn NOT IN (null)' is an undefined condition in sql so this library disallows it.");
                }
                list.Add(value.Value);
            }

            return new NotInCondition<string>(this, list);
        }

        public Condition In(Interfaces.IExecute pNestedQuery) {
            return new NestedQueryCondition(this, Sql.Operator.IN, pNestedQuery);
        }
        public Condition NotIn(Interfaces.IExecute pNestedQuery) {
            return new NestedQueryCondition(this, Sql.Operator.NOT_IN, pNestedQuery);
        }

        public Condition In(params StringKey<TABLE>[] pValues) {

            if(pValues == null) {
                throw new NullReferenceException($"{nameof(pValues)} cannot be null");
            }

            List<string> list = new List<string>();

            foreach(StringKey<TABLE> value in pValues) {

                if(value == null) {
                    throw new NullReferenceException($"A value in {nameof(pValues)} is null. 'NStringKeyColumn IN (null)' is an undefined condition in sql so this library disallows it.");
                }
                list.Add(value.Value);
            }

            return new InCondition<string>(this, list);
        }
        public Condition NotIn(params StringKey<TABLE>[] pValues) {

            if(pValues == null) {
                throw new NullReferenceException($"{nameof(pValues)} cannot be null");
            }

            List<string> list = new List<string>();

            foreach(StringKey<TABLE> value in pValues) {

                if(value == null) {
                    throw new NullReferenceException($"A value in {nameof(pValues)} is null. 'NStringKeyColumn NOT IN (null)' is an undefined condition in sql so this library disallows it.");
                }
                list.Add(value.Value);
            }

            return new NotInCondition<string>(this, list);
        }

        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        public override object GetValue(ADatabase pDatabase, System.Data.Common.DbDataReader pReader, int pColumnIndex) {

            Type dataType = pReader.GetFieldType(pColumnIndex);

            if(dataType != typeof(string)) {
                throw new Exception($"Row column data is not of the correct type. Expected string value instead got '{ dataType.ToString() }'. This probably means that the database and table column data types are not matching. Please run the definition tester to check table columns are of the correct type. Table: '{ Table.TableName }' Column: '{ ColumnName }'");
            }
            return pReader.GetString(pColumnIndex);
        }
        public StringKey<TABLE>? ValueOf(ARow pRow) {
            object? value = pRow.GetValue(this);
            return value != null ? new StringKey<TABLE>((string)value) : (StringKey<TABLE>?)null;
        }
        public void SetValue(ARow pRow, StringKey<TABLE>? pValue) {

            if(pValue != null && pValue.Value != null && pValue.Value.Length > MaxLength) {
                throw new Exception($"string value is too long. Max Length = { MaxLength.ToString() }. Actual length = { pValue.Value.Length.ToString() }. Table: { Table.ToString() }, Column = { ColumnName }");
            }
            pRow.SetValue(this, pValue != null ? pValue.Value : null);
        }

        internal override void TestSetValue(ARow pRow, object? pValue) {
            SetValue(pRow, (StringKey<TABLE>?)pValue);
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
            get { return System.Data.DbType.String; }
        }
        public override object? GetDefaultType() {
            return new StringKey<TABLE>(string.Empty);
        }
    }
}