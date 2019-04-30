
/*
 * 
 * Copyright (C) 2009-2019 JFo.nz
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

namespace SqlServer.Referential_Constraints {

    public sealed class Table : Sql.ATable {

        public readonly static Table Instance = new Table();

        public Sql.Column.StringColumn Constraint_Name { get; private set; }
        public Sql.Column.StringColumn Constraint_Catalog { get; private set; }
        public Sql.Column.StringColumn Constraint_Schema { get; private set; }
        public Sql.Column.StringColumn Unique_Constraint_Catalog { get; private set; }
        public Sql.Column.StringColumn Unique_Constraint_Schema { get; private set; }
        public Sql.Column.StringColumn Unique_Constraint_Name { get; private set; }

        public Table()
            : base("Referential_Constraints", "information_schema", false, typeof(Row)) {

            Constraint_Name = new Sql.Column.StringColumn(this, "constraint_name", false, int.MaxValue);
            Constraint_Catalog = new Sql.Column.StringColumn(this, "constraint_catalog", false, int.MaxValue);
            Constraint_Schema = new Sql.Column.StringColumn(this, "constraint_schema", false, int.MaxValue);
            Unique_Constraint_Catalog = new Sql.Column.StringColumn(this, "unique_constraint_catalog", false, int.MaxValue);
            Unique_Constraint_Schema = new Sql.Column.StringColumn(this, "unique_constraint_schema", false, int.MaxValue);
            Unique_Constraint_Name = new Sql.Column.StringColumn(this, "unique_constraint_name", false, int.MaxValue);

            AddColumns(Constraint_Name, Constraint_Catalog, Constraint_Schema, Unique_Constraint_Catalog, Unique_Constraint_Schema, Unique_Constraint_Name);
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

        public string Unique_Constraint_Catalog {
            get { return Tbl.Unique_Constraint_Catalog.ValueOf(this); }
            set { Tbl.Unique_Constraint_Catalog.SetValue(this, value); }
        }

        public string Unique_Constraint_Schema {
            get { return Tbl.Unique_Constraint_Schema.ValueOf(this); }
            set { Tbl.Unique_Constraint_Schema.SetValue(this, value); }
        }

        public string Unique_Constraint_Name {
            get { return Tbl.Unique_Constraint_Name.ValueOf(this); }
            set { Tbl.Unique_Constraint_Name.SetValue(this, value); }
        }
    }
}