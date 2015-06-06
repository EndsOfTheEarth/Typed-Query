
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

namespace Sql.Tables.IntTable {

	public sealed class Table : Sql.ATable {

		public static readonly Table INSTANCE = new Table();

		public readonly Sql.Column.GuidColumn Id;
		public readonly Sql.Column.IntegerColumn IntValue;

		public Table() : base(DB.TestDB, "IntTable", "", false, typeof(Row)) {

			Id = new Sql.Column.GuidColumn(this, "Id", true);
			IntValue = new Sql.Column.IntegerColumn(this, "IntValue", false);

			AddColumns(Id,IntValue);
		}

		public Row this[int pIndex, Sql.IResult pQueryResult]{
			get { return (Row)pQueryResult.GetRow(this, pIndex); }
		}
	}

	public sealed class Row : Sql.ARow {

		private new Table Tbl {
			get { return (Table)base.Tbl; }
		}

		public Row() : base(Table.INSTANCE) {
			
		}
		
		public Guid Id {
			get { return Tbl.Id.ValueOf(this); }
			set { Tbl.Id.SetValue(this, value); }
		}

		public int IntValue {
			get { return Tbl.IntValue.ValueOf(this); }
			set { Tbl.IntValue.SetValue(this, value); }
		}
	}
}