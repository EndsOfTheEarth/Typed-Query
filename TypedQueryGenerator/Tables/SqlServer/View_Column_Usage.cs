
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

namespace SqlServer.View_Column_Usage {

	public sealed class Table : Sql.ATable {

		public readonly static Table Instance = new Table(SqlServerDatabase.Instance);

		public Sql.Column.StringColumn View_Name { get; private set; }
		public Sql.Column.StringColumn Column_Name { get; private set; }
		public Sql.Column.StringColumn View_Catalog { get; private set; }
		public Sql.Column.StringColumn View_Schema { get; private set; }
		public Sql.Column.StringColumn Table_Catalog { get; private set; }
		public Sql.Column.StringColumn Table_Schema { get; private set; }
		public Sql.Column.StringColumn Table_Name { get; private set; }

		//public Table()
		//	: this(SqlServerDatabase.Instance) {

		//}

		public Table(Sql.ADatabase pDatabase)
			: base(pDatabase, "View_Column_Usage", "information_schema", true, typeof(Row)) {

				View_Name = new Sql.Column.StringColumn(this, "view_name", false, int.MaxValue);
				Column_Name = new Sql.Column.StringColumn(this, "column_name", false, int.MaxValue);
				View_Catalog = new Sql.Column.StringColumn(this, "view_catalog", false, int.MaxValue);
				View_Schema = new Sql.Column.StringColumn(this, "view_schema", false, int.MaxValue);
				Table_Catalog = new Sql.Column.StringColumn(this, "table_catalog", false, int.MaxValue);
				Table_Schema = new Sql.Column.StringColumn(this, "table_schema", false, int.MaxValue);
				Table_Name = new Sql.Column.StringColumn(this, "table_name", false, int.MaxValue);

				AddColumns(View_Name, Column_Name, View_Catalog, View_Schema, Table_Catalog, Table_Schema, Table_Name);
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

		public string View_Name {
			get { return Tbl.View_Name.ValueOf(this); }
			set { Tbl.View_Name.SetValue(this, value); }
		}

		public string Column_Name {
			get { return Tbl.Column_Name.ValueOf(this); }
			set { Tbl.Column_Name.SetValue(this, value); }
		}

		public string View_Catalog {
			get { return Tbl.View_Catalog.ValueOf(this); }
			set { Tbl.View_Catalog.SetValue(this, value); }
		}

		public string View_Schema {
			get { return Tbl.View_Schema.ValueOf(this); }
			set { Tbl.View_Schema.SetValue(this, value); }
		}

		public string Table_Catalog {
			get { return Tbl.Table_Catalog.ValueOf(this); }
			set { Tbl.Table_Catalog.SetValue(this, value); }
		}

		public string Table_Schema {
			get { return Tbl.Table_Schema.ValueOf(this); }
			set { Tbl.Table_Schema.SetValue(this, value); }
		}

		public string Table_Name {
			get { return Tbl.Table_Name.ValueOf(this); }
			set { Tbl.Table_Name.SetValue(this, value); }
		}
	}
}