
using System;
using Sql.Types;
using Sql.Column;

namespace Sql.Tables.KeyTables.NGuidKeyTestTable {

    public sealed class Table : Sql.ATable {

        public static readonly Table Instance = new Table();

        public NGuidKeyColumn<Table> Id { get; private set; }

        public Table() : base("nguidkeytesttable", "", false, typeof(Row)) {

            Id = new NGuidKeyColumn<Table>(this, "id", true);

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

        public GuidKey<Table>? Id {
            get { return Tbl.Id.ValueOf(this); }
            set { Tbl.Id.SetValue(this, value); }
        }
    }
}