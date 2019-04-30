
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

namespace Sql.Tables.BinaryTable {

	public sealed class Table : Sql.ATable {

		public static readonly Table INSTANCE = new Table();

		public readonly Sql.Column.BinaryColumn BinaryValue;
		public readonly Sql.Column.NBinaryColumn NBinaryValue;

		public Table() : base("BinaryTable", "", false, typeof(Row)) {

			BinaryValue = new Sql.Column.BinaryColumn(this, "BinaryValue", false);
			NBinaryValue = new Sql.Column.NBinaryColumn(this, "NBinaryValue", false);

			AddColumns(BinaryValue,NBinaryValue);
		}

		public Row this[int pIndex, Sql.IResult pQueryResult]{
			get { return (Row)pQueryResult.GetRow(this, pIndex); }
		}

//		protected override Sql.ARow LoadRow(IList<Sql.ISelectable> pSelectColumns, System.Data.Common.DbDataReader pReader) {
//			return new Row(this, pSelectColumns, pReader);
//		}
	}

	public sealed class Row : Sql.ARow {

		private new Table Tbl {
			get { return (Table)base.Tbl; }
		}

		public Row() : base(Table.INSTANCE) {
		}

//		internal Row(Table pTable, IList<Sql.ISelectable> pSelectColumns, System.Data.Common.DbDataReader pReader)
//			: base(pTable, pSelectColumns, pReader) {
//			mTable = pTable;
//		}

		public byte[] BinaryValue {
			get { return Tbl.BinaryValue.ValueOf(this); }
			set { Tbl.BinaryValue.SetValue(this, value); }
		}

		public byte[] NBinaryValue {
			get { return Tbl.NBinaryValue.ValueOf(this); }
			set { Tbl.NBinaryValue.SetValue(this, value); }
		}
	}
}