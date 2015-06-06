
/*
 * 
 * Copyright (C) 2009-2015 JFo.nz
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
	
	public class StringKeyColumn<TABLE> : AColumn, Sql.IColumnLength where TABLE : ATable {

		public int MaxLength { get; private set; }
		
		public StringKeyColumn(ATable pTable, string pColumnName, int pMaxLength)
			: base(pTable, pColumnName, false, false) {
			
			if(pMaxLength <= 0)
				throw new Exception("pMaxLength must be >= 1");
			
			MaxLength = pMaxLength;
		}
		public StringKeyColumn(ATable pTable, string pColumnName, bool pIsPrimaryKey, int pMaxLength)
			: base(pTable, pColumnName, pIsPrimaryKey, false) {
			
			if(pMaxLength <= 0)
				throw new Exception("pMaxLength must be >= 1");
			
			MaxLength = pMaxLength;
		}

		public static Condition operator ==(StringKeyColumn<TABLE> pColumnA, StringKeyColumn<TABLE> pColumnB) {
			
			if(((object)pColumnB) == null)
				throw new NullReferenceException("pColumnB cannot be null");
			
			return new ColumnCondition(pColumnA, Sql.Operator.EQUALS, pColumnB);
		}
		
		public static Condition operator !=(StringKeyColumn<TABLE> pColumnA, StringKeyColumn<TABLE> pColumnB) {
			
			if(((object)pColumnB) == null)
				throw new NullReferenceException("pColumnB cannot be null");
			
			return new ColumnCondition(pColumnA, Sql.Operator.NOT_EQUALS, pColumnB);
		}
		
		public static Condition operator ==(StringKeyColumn<TABLE> pColumnA, string pValue) {
			
			if(pValue == null)
				throw new NullReferenceException("pValue cannot be null when using the == operator. Use .IsNull() method if a null condition is required. 'StringKeyColumn = null' is an undefined condition in sql so this library disallows it.");
			
			return new ColumnCondition(pColumnA, Sql.Operator.EQUALS, pValue);
		}
		
		public static Condition operator !=(StringKeyColumn<TABLE> pColumnA, string pValue) {
			
			if(pValue == null)
				throw new NullReferenceException("pValue cannot be null when using the != operator. Use .IsNull() method if a null condition is required. 'StringKeyColumn != null' is an undefined condition in sql so this library disallows it.");
			
			return new ColumnCondition(pColumnA, Sql.Operator.NOT_EQUALS, pValue);
		}

		public Condition Like(string pValue) {
			
			if(pValue == null)
				throw new NullReferenceException("pValue cannot be null when using the 'like' operator. 'StringKeyColumn like null' is an undefined condition in sql so this library disallows it.");
			
			return new ColumnCondition(this, Operator.LIKE, pValue);
		}

//		public Condition ILike(string pValue) {
//			return new ColumnCondition(this, Operator.ILIKE, pValue);
//		}

		public Condition NotLike(string pValue) {
			
			if(pValue == null)
				throw new NullReferenceException("pValue cannot be null when using the 'not like' operator. 'StringKeyColumn not like null' is an undefined condition in sql so this library disallows it.");
			
			return new ColumnCondition(this, Operator.NOT_LIKE, pValue);
		}

//		public Condition NotILike(string pValue) {
//			return new ColumnCondition(this, Operator.NOT_ILIKE, pValue);
//		}

		public Condition In(List<string> pList) {
			
			foreach(string value in pList) {
				if(value == null)
					throw new NullReferenceException("A value in pList is null. 'StringKeyColumn IN (null)' is an undefined condition in sql so this library disallows it.");
			}
				
			return new InCondition<string>(this, pList);
		}
		public Condition NotIn(List<string> pList) {
			
			foreach(string value in pList) {
				if(value == null)
					throw new NullReferenceException("A value in pList is null. 'StringKeyColumn NOT IN (null)' is an undefined condition in sql so this library disallows it.");
			}
			
			return new NotInCondition<string>(this, pList);
		}

		public Condition In(Interfaces.IExecute pNestedQuery) {
			return new NestedQueryCondition(this, Sql.Operator.IN, pNestedQuery);
		}
		public Condition NotIn(Interfaces.IExecute pNestedQuery) {
			return new NestedQueryCondition(this, Sql.Operator.NOT_IN, pNestedQuery);
		}

		public Condition In(params string[] pValues) {
			
			if(pValues == null)
				throw new NullReferenceException("pValues cannot be null");
			
			foreach(string value in pValues) {
				if(value == null)
					throw new NullReferenceException("A value in pValues is null. 'StringKeyColumn IN (null)' is an undefined condition in sql so this library disallows it.");
			}
			
			return new InCondition<string>(this, pValues);
		}
		public Condition NotIn(params string[] pValues) {
			
			if(pValues == null)
				throw new NullReferenceException("pValues cannot be null");
			
			foreach(string value in pValues) {
				if(value == null)
					throw new NullReferenceException("A value in pValues is null. 'StringKeyColumn NOT IN (null)' is an undefined condition in sql so this library disallows it.");
			}
			
			return new NotInCondition<string>(this, pValues);
		}
		
		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public override object GetValue(ADatabase pDatabase, System.Data.Common.DbDataReader pReader, int pColumnIndex) {
			
			Type dataType = pReader.GetFieldType(pColumnIndex);
		
			if(dataType != typeof(string))
				throw new Exception("Row column data is not of the correct type. Expected string value instead got '" + dataType.ToString() + "'. This probably means that the database and table column data types are not matching. Please run the definition tester to check table columns are of the correct type. Table: '" + Table.TableName + "' Column: '" + ColumnName + "'");
			
			return pReader.GetString(pColumnIndex);
		}
		public string ValueOf(ARow pRow) {
			return (string)pRow.GetValue(this);
		}
		public void SetValue(ARow pRow, string pValue) {
			
			if(pValue != null && pValue.Length > MaxLength)
				throw new Exception("string value is too long. Max Length = " + MaxLength.ToString() + ". Actual length = " + pValue.Length.ToString() + ". Table: " + Table.ToString() + ", Column = " + ColumnName);
			
			pRow.SetValue(this, pValue);
		}
		
		internal override void TestSetValue(ARow pRow, object pValue) {
			SetValue(pRow, (string) pValue);
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
			get { return System.Data.DbType.String; }
		}
		public override object GetDefaultType(){
			return null;
		}
	}
}
