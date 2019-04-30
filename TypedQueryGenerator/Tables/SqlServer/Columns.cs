
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

namespace SqlServer.Columns {

    public sealed class Table : Sql.ATable {

        public readonly static Table Instance = new Table();

        public Sql.Column.StringColumn Column_Name { get; private set; }
        public Sql.Column.StringColumn Data_Type { get; private set; }
        public Sql.Column.StringColumn Is_Nullable { get; private set; }
        public Sql.Column.StringColumn Table_Name { get; private set; }
        public Sql.Column.StringColumn Table_Schema { get; private set; }
        public Sql.Column.StringColumn Table_Catalog { get; private set; }
        public Sql.Column.IntegerColumn Ordinal_Position { get; private set; }
        public Sql.Column.NIntegerColumn Character_Maximum_Length { get; private set; }

        public Table()
            : base("columns", "information_schema", false, typeof(Row)) {

            Column_Name = new Sql.Column.StringColumn(this, "Column_Name", false, int.MaxValue);
            Data_Type = new Sql.Column.StringColumn(this, "Data_Type", false, int.MaxValue);
            Is_Nullable = new Sql.Column.StringColumn(this, "Is_Nullable", false, int.MaxValue);
            Table_Name = new Sql.Column.StringColumn(this, "Table_Name", false, int.MaxValue);
            Table_Schema = new Sql.Column.StringColumn(this, "Table_Schema", false, int.MaxValue);
            Table_Catalog = new Sql.Column.StringColumn(this, "Table_Catalog", false, int.MaxValue);
            Ordinal_Position = new Sql.Column.IntegerColumn(this, "Ordinal_Position", false);
            Character_Maximum_Length = new Sql.Column.NIntegerColumn(this, "CHARACTER_MAXIMUM_LENGTH", false);

            AddColumns(Column_Name, Data_Type, Is_Nullable, Table_Name, Table_Schema, Table_Catalog, Ordinal_Position, Character_Maximum_Length);
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

        public string Column_Name {
            get { return Tbl.Column_Name.ValueOf(this); }
            set { Tbl.Column_Name.SetValue(this, value); }
        }

        public string Data_Type {
            get { return Tbl.Data_Type.ValueOf(this); }
            set { Tbl.Data_Type.SetValue(this, value); }
        }

        public string Is_Nullable {
            get { return Tbl.Is_Nullable.ValueOf(this); }
            set { Tbl.Is_Nullable.SetValue(this, value); }
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

        public int Ordinal_Position {
            get { return Tbl.Ordinal_Position.ValueOf(this); }
            set { Tbl.Ordinal_Position.SetValue(this, value); }
        }
        public int? Character_Maximum_Length {
            get { return Tbl.Character_Maximum_Length.ValueOf(this); }
            set { Tbl.Character_Maximum_Length.SetValue(this, value); }
        }
    }
}