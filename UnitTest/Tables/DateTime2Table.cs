
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

using Sql.Tables;

namespace Sql.Tables.DateTime2Table {

	public sealed class Table : Sql.ATable {

		public static readonly Table INSTANCE = new Table();

		public readonly Sql.Column.GuidColumn Id;
		public readonly Sql.Column.DateTime2Column Dt;

		public Table() : base(DB.TestDB, "DateTimeTable2", "", false, typeof(Row)) {

			Id = new Sql.Column.GuidColumn(this, "dtId", true);
			Dt = new Sql.Column.DateTime2Column(this, "dtDt", false);

			AddColumns(Id,Dt);
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
	
		public DateTime Dt {
			get { return Tbl.Dt.ValueOf(this); }
			set { Tbl.Dt.SetValue(this, value); }
		}	
	}
}