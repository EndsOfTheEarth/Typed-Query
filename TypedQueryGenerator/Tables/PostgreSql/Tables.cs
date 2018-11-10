
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

namespace Postgresql.Tables {

	public sealed class Table : Sql.ATable {

		public readonly static Table Instance = new Table(PgDatabase.Instance);

		public Sql.Column.StringColumn Table_catalog { get; private set; }
		public Sql.Column.StringColumn Table_schema { get; private set; }
		public Sql.Column.StringColumn Table_name { get; private set; }
		public Sql.Column.StringColumn Table_type { get; private set; }
		public Sql.Column.StringColumn Self_referencing_column_name { get; private set; }
		public Sql.Column.StringColumn Reference_generation { get; private set; }
		public Sql.Column.StringColumn User_defined_type_catalog { get; private set; }
		public Sql.Column.StringColumn User_defined_type_schema { get; private set; }
		public Sql.Column.StringColumn User_defined_type_name { get; private set; }
		public Sql.Column.StringColumn Is_insertable_into { get; private set; }
		public Sql.Column.StringColumn Is_typed { get; private set; }
		public Sql.Column.StringColumn Commit_action { get; private set; }

		public Table(Sql.ADatabase pDatabase)
			: base(pDatabase, "tables", "information_schema", true, typeof(Row)) {

				Table_catalog = new Sql.Column.StringColumn(this, "table_catalog", false, int.MaxValue);
				Table_schema = new Sql.Column.StringColumn(this, "table_schema", false, int.MaxValue);
				Table_name = new Sql.Column.StringColumn(this, "table_name", false, int.MaxValue);
				Table_type = new Sql.Column.StringColumn(this, "table_type", false, int.MaxValue);
				Self_referencing_column_name = new Sql.Column.StringColumn(this, "self_referencing_column_name", false, int.MaxValue);
				Reference_generation = new Sql.Column.StringColumn(this, "reference_generation", false, int.MaxValue);
				User_defined_type_catalog = new Sql.Column.StringColumn(this, "user_defined_type_catalog", false, int.MaxValue);
				User_defined_type_schema = new Sql.Column.StringColumn(this, "user_defined_type_schema", false, int.MaxValue);
				User_defined_type_name = new Sql.Column.StringColumn(this, "user_defined_type_name", false, int.MaxValue);
				Is_insertable_into = new Sql.Column.StringColumn(this, "is_insertable_into", false, int.MaxValue);
				Is_typed = new Sql.Column.StringColumn(this, "is_typed", false, int.MaxValue);
				Commit_action = new Sql.Column.StringColumn(this, "commit_action", false, int.MaxValue);

			AddColumns(Table_catalog, Table_schema, Table_name, Table_type, Self_referencing_column_name, Reference_generation, User_defined_type_catalog, User_defined_type_schema, User_defined_type_name, Is_insertable_into, Is_typed, Commit_action);
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

		public string Table_type {
			get { return Tbl.Table_type.ValueOf(this); }
		}

		public string Self_referencing_column_name {
			get { return Tbl.Self_referencing_column_name.ValueOf(this); }
		}

		public string Reference_generation {
			get { return Tbl.Reference_generation.ValueOf(this); }
		}

		public string User_defined_type_catalog {
			get { return Tbl.User_defined_type_catalog.ValueOf(this); }
		}

		public string User_defined_type_schema {
			get { return Tbl.User_defined_type_schema.ValueOf(this); }
		}

		public string User_defined_type_name {
			get { return Tbl.User_defined_type_name.ValueOf(this); }
		}

		public string Is_insertable_into {
			get { return Tbl.Is_insertable_into.ValueOf(this); }
		}

		public string Is_typed {
			get { return Tbl.Is_typed.ValueOf(this); }
		}

		public string Commit_action {
			get { return Tbl.Commit_action.ValueOf(this); }
		}
	}
}