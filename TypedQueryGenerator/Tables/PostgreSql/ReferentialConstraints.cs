
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

namespace Postgresql.ReferentialConstraints {

	public sealed class Table : Sql.ATable {

		public readonly static Table Instance = new Table();

		public Sql.Column.StringColumn Constraint_catalog { get; private set; }
		public Sql.Column.StringColumn Constraint_schema { get; private set; }
		public Sql.Column.StringColumn Constraint_name { get; private set; }
		public Sql.Column.StringColumn Unique_constraint_catalog { get; private set; }
		public Sql.Column.StringColumn Unique_constraint_schema { get; private set; }
		public Sql.Column.StringColumn Unique_constraint_name { get; private set; }
		public Sql.Column.StringColumn Match_option { get; private set; }
		public Sql.Column.StringColumn Update_rule { get; private set; }
		public Sql.Column.StringColumn Delete_rule { get; private set; }

		public Table()
			: base(PgDatabase.Instance, "referential_constraints", "information_schema", true, typeof(Row)) {

				Constraint_catalog = new Sql.Column.StringColumn(this, "constraint_catalog", false, int.MaxValue);
				Constraint_schema = new Sql.Column.StringColumn(this, "constraint_schema", false, int.MaxValue);
				Constraint_name = new Sql.Column.StringColumn(this, "constraint_name", false, int.MaxValue);
				Unique_constraint_catalog = new Sql.Column.StringColumn(this, "unique_constraint_catalog", false, int.MaxValue);
				Unique_constraint_schema = new Sql.Column.StringColumn(this, "unique_constraint_schema", false, int.MaxValue);
				Unique_constraint_name = new Sql.Column.StringColumn(this, "unique_constraint_name", false, int.MaxValue);
				Match_option = new Sql.Column.StringColumn(this, "match_option", false, int.MaxValue);
				Update_rule = new Sql.Column.StringColumn(this, "update_rule", false, int.MaxValue);
				Delete_rule = new Sql.Column.StringColumn(this, "delete_rule", false, int.MaxValue);

			AddColumns(Constraint_catalog, Constraint_schema, Constraint_name, Unique_constraint_catalog, Unique_constraint_schema, Unique_constraint_name, Match_option, Update_rule, Delete_rule);
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

		public string Constraint_catalog {
			get { return Tbl.Constraint_catalog.ValueOf(this); }
		}

		public string Constraint_schema {
			get { return Tbl.Constraint_schema.ValueOf(this); }
		}

		public string Constraint_name {
			get { return Tbl.Constraint_name.ValueOf(this); }
		}

		public string Unique_constraint_catalog {
			get { return Tbl.Unique_constraint_catalog.ValueOf(this); }
		}

		public string Unique_constraint_schema {
			get { return Tbl.Unique_constraint_schema.ValueOf(this); }
		}

		public string Unique_constraint_name {
			get { return Tbl.Unique_constraint_name.ValueOf(this); }
		}

		public string Match_option {
			get { return Tbl.Match_option.ValueOf(this); }
		}

		public string Update_rule {
			get { return Tbl.Update_rule.ValueOf(this); }
		}

		public string Delete_rule {
			get { return Tbl.Delete_rule.ValueOf(this); }
		}
	}
}