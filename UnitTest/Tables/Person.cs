using System;
using System.Collections.Generic;

namespace Sql.Tables.Person {

	public sealed class Table : Sql.ATable {

		public static readonly Table INSTANCE = new Table();

		public Sql.Column.GuidKeyColumn<Table> Key { get; private set; }
		public Sql.Column.StringColumn FirstName { get; private set; }
		public Sql.Column.StringColumn Surname { get; private set; }

		public Table() : base(DB.TestDB, "Person", "", false, typeof(Row)) {

			Key = new Sql.Column.GuidKeyColumn<Table>(this, "perKey", true);
			FirstName = new Sql.Column.StringColumn(this, "perFirstName", false, 100);
			Surname = new Sql.Column.StringColumn(this, "perSurname", false, 100);

			AddColumns(Key,FirstName,Surname);
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

		public string FirstName {
			get { return Tbl.FirstName.ValueOf(this); }
			set { Tbl.FirstName.SetValue(this, value); }
		}

		public string Surname {
			get { return Tbl.Surname.ValueOf(this); }
			set { Tbl.Surname.SetValue(this, value); }
		}
	}
}