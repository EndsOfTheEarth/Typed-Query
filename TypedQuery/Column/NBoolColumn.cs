
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

	/// <summary>
	/// Nullable boolean column
	/// </summary>
	public class NBoolColumn : AColumn {

		public NBoolColumn(ATable pTable, string pColumnName)
			: base(pTable, pColumnName, false, true) {
		}
		public NBoolColumn(ATable pTable, string pColumnName, bool pIsPrimaryKey)
			: base(pTable, pColumnName, pIsPrimaryKey, true) {
		}

		public static Condition operator ==(NBoolColumn pColumnA, BoolColumn pColumnB) {
			return new ColumnCondition(pColumnA, Sql.Operator.EQUALS, pColumnB);
		}
		public static Condition operator ==(NBoolColumn pColumnA, NBoolColumn pColumnB) {
			return new ColumnCondition(pColumnA, Sql.Operator.EQUALS, pColumnB);
		}
		public static Condition operator ==(NBoolColumn pColumnA, bool pValue) {
			return new ColumnCondition(pColumnA, Sql.Operator.EQUALS, pValue);
		}

		public static Condition operator !=(NBoolColumn pColumnA, BoolColumn pColumnB) {
			return new ColumnCondition(pColumnA, Sql.Operator.NOT_EQUALS, pColumnB);
		}
		public static Condition operator !=(NBoolColumn pColumnA, NBoolColumn pColumnB) {
			return new ColumnCondition(pColumnA, Sql.Operator.NOT_EQUALS, pColumnB);
		}
		public static Condition operator !=(NBoolColumn pColumnA, bool pValue) {
			return new ColumnCondition(pColumnA, Sql.Operator.NOT_EQUALS, pValue);
		}
		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public override object GetValue(ADatabase pDatabase, System.Data.Common.DbDataReader pReader, int pColumnIndex) {
			
			if (pReader.IsDBNull(pColumnIndex))
				return null;
			
			Type dataType = pReader.GetFieldType(pColumnIndex);
			
			if(dataType != typeof(bool) && dataType != typeof(bool?))
				throw new Exception("Row column data is not of the correct type. Expected bool or bool? value instead got '" + dataType.ToString() + "'. This probably means that the database and table column data types are not matching. Please run the definition tester to check table columns are of the correct type. Table: '" + Table.TableName + "' Column: '" + ColumnName + "'");
			
			return (bool?)pReader.GetBoolean(pColumnIndex);
		}
		public bool? ValueOf(ARow pRow) {
			return (bool?)pRow.GetValue(this);
		}
		public void SetValue(ARow pRow, bool? pValue) {
			pRow.SetValue(this, pValue);
		}
		
		internal override void TestSetValue(ARow pRow, object pValue) {
			SetValue(pRow, (bool?) pValue);
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
			get { return System.Data.DbType.Boolean; }
		}
		public override object GetDefaultType(){
			return null;
		}
	}
}