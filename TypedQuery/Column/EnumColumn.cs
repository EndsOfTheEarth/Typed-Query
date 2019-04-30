
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

    public interface IEnumColumn {  //Used as a marked by the definition checker to get around the generic type 

        Type GetEnumType();
    }

    public class EnumColumn<ENUM> : AColumn, IEnumColumn {

        public EnumColumn(ATable pTable, string pColumnName) : base(pTable, pColumnName, false, false) {

            if(!typeof(ENUM).IsEnum) {
                throw new ArgumentException($"<{nameof(ENUM)}> must be an enum type");
            }
        }
        public EnumColumn(ATable pTable, string pColumnName, bool pIsPrimaryKey) : base(pTable, pColumnName, pIsPrimaryKey, false) {

            if(!typeof(ENUM).IsEnum) {
                throw new ArgumentException($"<{nameof(ENUM)}> must be an enum type");
            }
        }

        public Type GetEnumType() {
            return typeof(ENUM);
        }

        public static Condition operator ==(EnumColumn<ENUM> pColumnA, EnumColumn<ENUM> pColumnB) {
            return new ColumnCondition(pColumnA, Sql.Operator.EQUALS, pColumnB);
        }
        public static Condition operator ==(EnumColumn<ENUM> pColumnA, ENUM pValue) {
            return new ColumnCondition(pColumnA, Sql.Operator.EQUALS, pValue);
        }
        public static Condition operator !=(EnumColumn<ENUM> pColumnA, EnumColumn<ENUM> pColumnB) {
            return new ColumnCondition(pColumnA, Sql.Operator.NOT_EQUALS, pColumnB);
        }
        public static Condition operator !=(EnumColumn<ENUM> pColumnA, ENUM pValue) {
            return new ColumnCondition(pColumnA, Sql.Operator.NOT_EQUALS, pValue);
        }

        public Condition In(List<ENUM> pList) {
            return new InCondition<ENUM>(this, pList);
        }
        public Condition NotIn(List<ENUM> pList) {
            return new NotInCondition<ENUM>(this, pList);
        }

        public Condition In(Interfaces.IExecute pNestedQuery) {
            return new NestedQueryCondition(this, Sql.Operator.IN, pNestedQuery);
        }
        public Condition NotIn(Interfaces.IExecute pNestedQuery) {
            return new NestedQueryCondition(this, Sql.Operator.NOT_IN, pNestedQuery);
        }

        public Condition In(params ENUM[] pValues) {
            return new InCondition<ENUM>(this, pValues);
        }
        public Condition NotIn(params ENUM[] pValues) {
            return new NotInCondition<ENUM>(this, pValues);
        }
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        public override object GetValue(ADatabase pDatabase, System.Data.Common.DbDataReader pReader, int pColumnIndex) {

            Type dataType = pReader.GetFieldType(pColumnIndex);

            if(dataType != typeof(byte) && dataType != typeof(Int16) && dataType != typeof(Int32)) {
                throw new Exception($"Row column data is not of the correct type. Expected byte or Int16 or Int32 value instead got '{ dataType.ToString() }'. This probably means that the database and table column data types are not matching. Please run the definition tester to check table columns are of the correct type. Table: '{ Table.TableName }' Column: '{ ColumnName }'");
            }

            object value = pReader.GetValue(pColumnIndex);

            if(value is byte) {
                return (int)((byte)value);
            }
            else if(value is Int16) {
                return (int)((Int16)value);
            }
            return (int)value;
        }
        public ENUM ValueOf(ARow pRow) {
            return (ENUM)pRow.GetValue(this);
        }
        public void SetValue(ARow pRow, ENUM pValue) {
            pRow.SetValue(this, pValue);
        }

        internal override void TestSetValue(ARow pRow, object pValue) {
            SetValue(pRow, (ENUM)pValue);
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
            get { return System.Data.DbType.Int16; }
        }
        public override object GetDefaultType() {
            return (int)0;
        }
    }
}