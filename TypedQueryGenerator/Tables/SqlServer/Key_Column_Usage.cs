
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
using System.Text;

namespace SqlServer.Key_Column_Usage {

	public sealed class Table : Sql.ATable {

		public readonly static Table Instance = new Table(SqlServerDatabase.Instance);

		public Sql.Column.StringColumn Constraint_Name { get; private set; }

		public Sql.Column.StringColumn Constraint_Catalog { get; private set; }
		public Sql.Column.StringColumn Constraint_Schema { get; private set; }

		public Sql.Column.StringColumn Column_Name { get; private set; }
		public Sql.Column.IntegerColumn Ordinal_Position { get; private set; }
		public Sql.Column.StringColumn Table_Name { get; private set; }
		public Sql.Column.StringColumn Table_Schema { get; private set; }

		//public Table()
		//	: this(SqlServerDatabase.Instance) {

		//}
		public Table(Sql.ADatabase pDatabase)
			: base(pDatabase, "Key_Column_Usage", "information_schema", true, typeof(Row)) {

				Constraint_Name = new Sql.Column.StringColumn(this, "Constraint_Name", false, int.MaxValue);
				Constraint_Catalog = new Sql.Column.StringColumn(this, "constraint_catalog", false, int.MaxValue);
				Constraint_Schema = new Sql.Column.StringColumn(this, "constraint_schema", false, int.MaxValue);
				Column_Name = new Sql.Column.StringColumn(this, "Column_Name", false, int.MaxValue);
				Ordinal_Position = new Sql.Column.IntegerColumn(this, "Ordinal_Position", false);
				Table_Name = new Sql.Column.StringColumn(this, "Table_Name", false, int.MaxValue);
				Table_Schema = new Sql.Column.StringColumn(this, "Table_Schema", false, int.MaxValue);

				AddColumns(Constraint_Name, Constraint_Catalog, Constraint_Schema, Column_Name, Ordinal_Position, Table_Name, Table_Schema);
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

		public string Constraint_Name {
			get { return Tbl.Constraint_Name.ValueOf(this); }
			set { Tbl.Constraint_Name.SetValue(this, value); }
		}

		public string Constraint_Catalog {
			get { return Tbl.Constraint_Catalog.ValueOf(this); }
			set { Tbl.Constraint_Catalog.SetValue(this, value); }
		}

		public string Constraint_Schema {
			get { return Tbl.Constraint_Schema.ValueOf(this); }
			set { Tbl.Constraint_Schema.SetValue(this, value); }
		}

		public string Column_Name {
			get { return Tbl.Column_Name.ValueOf(this); }
			set { Tbl.Column_Name.SetValue(this, value); }
		}

		public int Ordinal_Position {
			get { return Tbl.Ordinal_Position.ValueOf(this); }
			set { Tbl.Ordinal_Position.SetValue(this, value); }
		}

		public string Table_Name {
			get { return Tbl.Table_Name.ValueOf(this); }
			set { Tbl.Table_Name.SetValue(this, value); }
		}

		public string Table_Schema {
			get { return Tbl.Table_Schema.ValueOf(this); }
			set { Tbl.Table_Schema.SetValue(this, value); }
		}
	}
}