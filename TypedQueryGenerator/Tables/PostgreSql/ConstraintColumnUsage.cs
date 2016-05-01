
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

namespace Postgresql.ConstraintColumnUsage {

	public sealed class Table : Sql.ATable {

		public readonly static Table Instance = new Table();

		public Sql.Column.StringColumn Table_catalog { get; private set; }
		public Sql.Column.StringColumn Table_schema { get; private set; }
		public Sql.Column.StringColumn Table_name { get; private set; }
		public Sql.Column.StringColumn Column_name { get; private set; }
		public Sql.Column.StringColumn Constraint_catalog { get; private set; }
		public Sql.Column.StringColumn Constraint_schema { get; private set; }
		public Sql.Column.StringColumn Constraint_name { get; private set; }

		public Table()
			: base(PgDatabase.Instance, "constraint_column_usage", "information_schema", true, typeof(Row)) {

				Table_catalog = new Sql.Column.StringColumn(this, "table_catalog", false, int.MaxValue);
				Table_schema = new Sql.Column.StringColumn(this, "table_schema", false, int.MaxValue);
				Table_name = new Sql.Column.StringColumn(this, "table_name", false, int.MaxValue);
				Column_name = new Sql.Column.StringColumn(this, "column_name", false, int.MaxValue);
				Constraint_catalog = new Sql.Column.StringColumn(this, "constraint_catalog", false, int.MaxValue);
				Constraint_schema = new Sql.Column.StringColumn(this, "constraint_schema", false, int.MaxValue);
				Constraint_name = new Sql.Column.StringColumn(this, "constraint_name", false, int.MaxValue);

			AddColumns(Table_catalog, Table_schema, Table_name, Column_name, Constraint_catalog, Constraint_schema, Constraint_name);
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

		public string Table_catalog {
			get { return Tbl.Table_catalog.ValueOf(this); }
		}

		public string Table_schema {
			get { return Tbl.Table_schema.ValueOf(this); }
		}

		public string Table_name {
			get { return Tbl.Table_name.ValueOf(this); }
		}

		public string Column_name {
			get { return Tbl.Column_name.ValueOf(this); }
		}

		public string Constraint_catalog {
			get { return Tbl.Constraint_catalog.ValueOf(this); }
		}

		public string Constraint_schema {
			get { return Tbl.Constraint_schema.ValueOf(this); }
		}

		public string Constraint_name {
			get { return Tbl.Constraint_name.ValueOf(this); }
		}
	}
}