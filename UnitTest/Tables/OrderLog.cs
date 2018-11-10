using Sql.Types;
using System;
using System.Collections.Generic;

namespace Sql.Tables.OrderLog {

	public sealed class Table : Sql.ATable {

		public static readonly Table INSTANCE = new Table();

		public Sql.Column.GuidColumn Key { get; private set; }
		public Sql.Column.GuidKeyColumn<Person.Table> PersonKey { get; private set; }
		public Sql.Column.StringColumn Item_ { get; private set; }

		public Table() : base("OrderLog", "", false, typeof(Row)) {

			Key = new Sql.Column.GuidColumn(this, "ordKey", true);
			PersonKey = new Sql.Column.GuidKeyColumn<Sql.Tables.Person.Table>(this, "ordPersonKey", false);
			Item_ = new Sql.Column.StringColumn(this, "ordItem", false, 100);

			AddColumns(Key,PersonKey,Item_);
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

		public Guid Key {
			get { return Tbl.Key.ValueOf(this); }
			set { Tbl.Key.SetValue(this, value); }
		}

		public GuidKey<Person.Table> PersonKey {
			get { return Tbl.PersonKey.ValueOf(this); }
			set { Tbl.PersonKey.SetValue(this, value); }
		}

		public string Item {
			get { return Tbl.Item_.ValueOf(this); }
			set { Tbl.Item_.SetValue(this, value); }
		}
	}
}