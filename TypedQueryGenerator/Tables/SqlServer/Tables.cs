
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

namespace SqlServer.Tables {

    public sealed class Table : Sql.ATable {

        public readonly static Table Instance = new Table();

        public Sql.Column.StringColumn Table_catalog { get; private set; }
        public Sql.Column.StringColumn Table_schema { get; private set; }
        public Sql.Column.StringColumn Table_name { get; private set; }
        public Sql.Column.StringColumn Table_Type { get; private set; }

        public Table()
            : base("tables", "information_schema", false, typeof(Row)) {

            Table_catalog = new Sql.Column.StringColumn(this, "table_catalog", false, int.MaxValue);
            Table_schema = new Sql.Column.StringColumn(this, "table_schema", false, int.MaxValue);
            Table_name = new Sql.Column.StringColumn(this, "table_name", false, int.MaxValue);
            Table_Type = new Sql.Column.StringColumn(this, "table_type", false, int.MaxValue);

            AddColumns(Table_catalog, Table_schema, Table_name, Table_Type);
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
            set { Tbl.Table_catalog.SetValue(this, value); }
        }

        public string Table_schema {
            get { return Tbl.Table_schema.ValueOf(this); }
            set { Tbl.Table_schema.SetValue(this, value); }
        }

        public string Table_name {
            get { return Tbl.Table_name.ValueOf(this); }
            set { Tbl.Table_name.SetValue(this, value); }
        }
        public string Table_Type {
            get { return Tbl.Table_Type.ValueOf(this); }
            set { Tbl.Table_Type.SetValue(this, value); }
        }
    }
}