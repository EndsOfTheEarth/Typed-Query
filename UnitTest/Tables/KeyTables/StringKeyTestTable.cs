using System;
using Sql.Types;
using Sql.Column;

namespace Sql.Tables.KeyTables.StringKeyTestTable {

    public sealed class Table : Sql.ATable {

        public static readonly Table Instance = new Table();

        public StringKeyColumn<Table> Id { get; private set; }

        public Table() : base("stringkeytesttable", "", false, typeof(Row)) {

            Id = new StringKeyColumn<Table>(this, "id", true, 100);

            AddColumns(Id);
        }

        public Row GetRow(int pIndex, Sql.IResult pResult) {
            return (Row)pResult.GetRow(this, pIndex);
        }
    }

    public sealed class Row : Sql.ARow {

        private new Table Tbl {
            get { return (Table)base.Tbl; }
        }

        public Row() : base(Table.Instance) {
        }

        public StringKey<Table> Id {
            get { return Tbl.Id.ValueOf(this); }
            set { Tbl.Id.SetValue(this, value); }
        }
    }
}