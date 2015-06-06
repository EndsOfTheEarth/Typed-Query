
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
using System.Text;

namespace SqlServer.Views {

	public sealed class Table : Sql.ATable {

		public readonly static Table Instance = new Table(SqlServerDatabase.Instance);

		public Sql.Column.StringColumn Table_Name { get; private set; }
		public Sql.Column.StringColumn Table_Schema { get; private set; }
		public Sql.Column.StringColumn Table_Catalog { get; private set; }

		//public Table()
		//	: this(SqlServerDatabase.Instance) {

		//}

		public Table(Sql.ADatabase pDatabase)
			: base(pDatabase, "Views", "information_schema", true, typeof(Row)) {

				Table_Name = new Sql.Column.StringColumn(this, "table_name", false, int.MaxValue);
				Table_Schema = new Sql.Column.StringColumn(this, "table_schema", false, int.MaxValue);
				Table_Catalog = new Sql.Column.StringColumn(this, "table_catalog", false, int.MaxValue);

				AddColumns(Table_Name, Table_Schema, Table_Catalog);
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

		public string Table_Name {
			get { return Tbl.Table_Name.ValueOf(this); }
			set { Tbl.Table_Name.SetValue(this, value); }
		}

		public string Table_Schema {
			get { return Tbl.Table_Schema.ValueOf(this); }
			set { Tbl.Table_Schema.SetValue(this, value); }
		}

		public string Table_Catalog {
			get { return Tbl.Table_Catalog.ValueOf(this); }
			set { Tbl.Table_Catalog.SetValue(this, value); }
		}
	}
}