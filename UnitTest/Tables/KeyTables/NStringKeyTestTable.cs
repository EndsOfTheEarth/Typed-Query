using System;
using Sql.Types;
using Sql.Column;

namespace Sql.Tables.KeyTables.NStringKeyTestTable {

    public sealed class Table : Sql.ATable {

        public static readonly Table Instance = new Table();

        public NStringKeyColumn<Table> Id { get; private set; }

        public Table() : base("nstringkeytesttable", "public", false, typeof(Row)) {

            Id = new NStringKeyColumn<Table>(this, "id", true, 100);

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

        public StringKey<Table>? Id {
            get { return Tbl.Id.ValueOf(this); }
            set { Tbl.Id.SetValue(this, value); }
        }
    }
}