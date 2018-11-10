using Sql.Types;
using System;
using System.Collections.Generic;

namespace Sql.Tables.OrderLogId {

	public sealed class Table : Sql.ATable {

		public static readonly Table INSTANCE = new Table();

		public Sql.Column.IntegerColumn Id { get; private set; }
		public Sql.Column.IntegerKeyColumn<PersonId.Table> PersonId { get; private set; }
		public Sql.Column.StringColumn Item_ { get; private set; }

		public Table() : base(DB.TestDB, "OrderLogId", "", false, typeof(Row)) {

			Id = new Sql.Column.IntegerColumn(this, "ordId", true, true);
			PersonId = new Sql.Column.IntegerKeyColumn<PersonId.Table>(this, "ordPersonId", false);
			Item_ = new Sql.Column.StringColumn(this, "ordItem", false, 100);

			AddColumns(Id,PersonId,Item_);
		}

		public Row this[int pIndex, Sql.IResult pResult]{
			get { return (Row)pResult.GetRow(this, pIndex); }
		}
	}

	public sealed class Row : Sql.ARow {

		private new Table Tbl {
			get { return (Table)base.Tbl; }
		}

		public Row() : base(Table.INSTANCE) {
		}

		public int Id {
			get { return Tbl.Id.ValueOf(this); }
		}

		public Int32Key<PersonId.Table> PersonId {
			get { return Tbl.PersonId.ValueOf(this); }
			set { Tbl.PersonId.SetValue(this, value); }
		}

		public string Item {
			get { return Tbl.Item_.ValueOf(this); }
			set { Tbl.Item_.SetValue(this, value); }
		}
	}
}