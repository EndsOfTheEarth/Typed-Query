﻿using System;
using Sql.Types;
using Sql.Column;

namespace Sql.Tables.KeyTables.Int32KeyTestTable {

    public sealed class Table : Sql.ATable {

        public static readonly Table Instance = new Table();

        public IntegerKeyColumn<Table> Id { get; private set; }

        public Table() : base("int32keytesttable", "", false, typeof(Row)) {

            Id = new IntegerKeyColumn<Table>(this, "id", true);

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

        public Int32Key<Table> Id {
            get { return Tbl.Id.ValueOf(this); }
            set { Tbl.Id.SetValue(this, value); }
        }
    }
}