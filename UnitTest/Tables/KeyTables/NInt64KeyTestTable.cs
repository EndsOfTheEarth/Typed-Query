using System;
using Sql.Types;
using Sql.Column;

namespace Sql.Tables.KeyTables.NInt64KeyTestTable {

    public sealed class Table : Sql.ATable {

        public static readonly Table Instance = new Table();

        public NBigIntegerKeyColumn<Table> Id { get; private set; }

        public Table() : base("nint64keytesttable", "", false, typeof(Row)) {

            Id = new NBigIntegerKeyColumn<Table>(this, "id", true);

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

        public Int64Key<Table>? Id {
            get { return Tbl.Id.ValueOf(this); }
            set { Tbl.Id.SetValue(this, value); }
        }
    }
}