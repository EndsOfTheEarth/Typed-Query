
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

namespace Postgresql.Pg_Description {

	public sealed class Table : Sql.ATable {

		public readonly static Table Instance = new Table();

		public Sql.Column.BigIntegerColumn ObjOid { get; private set; }
		public Sql.Column.BigIntegerColumn ClassOid { get; private set; }
		public Sql.Column.SmallIntegerColumn ObjSubId { get; private set; }
		public Sql.Column.StringColumn Description { get; private set; }

		public Table()
			: base(PgDatabase.Instance, "pg_description", "pg_catalog", false, typeof(Row)) {

				ObjOid = new Sql.Column.BigIntegerColumn(this, "objoid", false);
				ClassOid = new Sql.Column.BigIntegerColumn(this, "classoid", false);
				ObjSubId = new Sql.Column.SmallIntegerColumn(this, "objsubid", false);
				Description = new Sql.Column.StringColumn(this, "description", false, int.MaxValue);

			AddColumns(ObjOid, ClassOid, ObjSubId, Description);
		}

		public Row this[int pIndex, Sql.IResult pResult] {
			get { return (Row)pResult.GetRow(this, pIndex); }
		}
	}

	public sealed class Row : Sql.ARow {

		private new Table Tbl {
			get { return (Table)base.Tbl; }
		}

		public Row()
			: base(Table.Instance) {
		}

		public long ObjOid {
			get { return Tbl.ObjOid.ValueOf(this); }
			set { Tbl.ObjOid.SetValue(this, value); }
		}

		public long ClassOid {
			get { return Tbl.ClassOid.ValueOf(this); }
			set { Tbl.ClassOid.SetValue(this, value); }
		}

		public short ObjSubId {
			get { return Tbl.ObjSubId.ValueOf(this); }
			set { Tbl.ObjSubId.SetValue(this, value); }
		}

		public string Description {
			get { return Tbl.Description.ValueOf(this); }
			set { Tbl.Description.SetValue(this, value); }
		}
	}
}