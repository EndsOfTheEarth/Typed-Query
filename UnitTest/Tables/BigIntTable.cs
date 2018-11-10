
/*
 * 
 * Copyright (C) 2009-2016 JFo.nz
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

namespace Sql.Tables.BigIntTable {

	public sealed class Table : Sql.ATable {

		public static readonly Table INSTANCE = new Table();

		public readonly Sql.Column.BigIntegerColumn Id;
		public readonly Sql.Column.NBigIntegerColumn IntValue;

		public Table() : base("BigIntTable", "", false, typeof(Row)) {

			Id = new Sql.Column.BigIntegerColumn(this, "Id", true, true);
			IntValue = new Sql.Column.NBigIntegerColumn(this, "IntValue", false);

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

		public Int64 Id {
			get { return Tbl.Id.ValueOf(this); }
		}

		public Int64? IntValue {
			get { return Tbl.IntValue.ValueOf(this); }
			set { Tbl.IntValue.SetValue(this, value); }
		}
	}
}