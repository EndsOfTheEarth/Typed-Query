
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
using Sql.Function;

namespace Sql.Tables.AutoTable {

	public sealed class Table : Sql.ATable {

		public static readonly Table INSTANCE = new Table();

		[Sql.ColumnAttribute("Description")]
		public readonly Sql.Column.IntegerColumn Id;		
		
		public readonly Sql.Column.StringColumn Text;

		public Table() : base(DB.TestDB, "AutoTable", "", false, typeof(Row)) {
			
			Id = new Sql.Column.IntegerColumn(this, "Id", true, true);			
			Text = new Sql.Column.StringColumn(this, "Text", false, 255);

			AddColumns(Id,Text);
		}

		public Row this[int pIndex, Sql.IResult pQueryResult]{
			get { return (Row)pQueryResult.GetRow(this, pIndex); }
		}
//
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

		public int Id {
			get { return Tbl.Id.ValueOf(this); }
		}

		public string Text {
			get { return Tbl.Text.ValueOf(this); }
			set { Tbl.Text.SetValue(this, value); }
		}
	}
}