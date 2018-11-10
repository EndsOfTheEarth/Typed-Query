
/*
 * 
 * Copyright (C) 2009-2016 JFo.nz
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
using System.Collections.Generic;

namespace Postgresql.Pg_Class {

	public sealed class Table : Sql.ATable {

		public readonly static Table Instance = new Table();

		public Sql.Column.BigIntegerColumn Oid { get; private set; }
		public Sql.Column.StringColumn Name { get; private set; }
		//public UNKNOWN_COLUMN_TYPE Namespace { get; private set; }
		//public UNKNOWN_COLUMN_TYPE Type { get; private set; }
		//public UNKNOWN_COLUMN_TYPE Oftype { get; private set; }
		//public UNKNOWN_COLUMN_TYPE Owner { get; private set; }
		//public UNKNOWN_COLUMN_TYPE Am { get; private set; }
		//public UNKNOWN_COLUMN_TYPE Filenode { get; private set; }
		//public UNKNOWN_COLUMN_TYPE Tablespace { get; private set; }
		//public Sql.Column.IntegerColumn Pages { get; private set; }
		//public Sql.Column.FloatColumn Tuples { get; private set; }
		//public UNKNOWN_COLUMN_TYPE Toastrelid { get; private set; }
		//public UNKNOWN_COLUMN_TYPE Toastidxid { get; private set; }
		//public Sql.Column.BoolColumn Hasindex { get; private set; }
		//public Sql.Column.BoolColumn Isshared { get; private set; }
		//public Sql.Column.StringColumn Persistence { get; private set; }
		public Sql.Column.StringColumn Kind { get; private set; }
		//public Sql.Column.SmallIntegerColumn Natts { get; private set; }
		//public Sql.Column.SmallIntegerColumn Checks { get; private set; }
		//public Sql.Column.BoolColumn Hasoids { get; private set; }
		//public Sql.Column.BoolColumn Haspkey { get; private set; }
		//public Sql.Column.BoolColumn Hasrules { get; private set; }
		//public Sql.Column.BoolColumn Hastriggers { get; private set; }
		//public Sql.Column.BoolColumn Hassubclass { get; private set; }
		//public UNKNOWN_COLUMN_TYPE Frozenxid { get; private set; }
		//public UNKNOWN_COLUMN_TYPE Acl { get; private set; }
		//public UNKNOWN_COLUMN_TYPE Options { get; private set; }

		public Table()
			: base("pg_class", "pg_catalog", false, typeof(Row)) {

				Oid = new Sql.Column.BigIntegerColumn(this, "oid");
				Name = new Sql.Column.StringColumn(this, "relname", false, int.MaxValue);
			//Namespace = new UNKNOWN_COLUMN_TYPE(this, "relnamespace", false);
			//Type = new UNKNOWN_COLUMN_TYPE(this, "reltype", false);
			//Oftype = new UNKNOWN_COLUMN_TYPE(this, "reloftype", false);
			//Owner = new UNKNOWN_COLUMN_TYPE(this, "relowner", false);
			//Am = new UNKNOWN_COLUMN_TYPE(this, "relam", false);
			//Filenode = new UNKNOWN_COLUMN_TYPE(this, "relfilenode", false);
			//Tablespace = new UNKNOWN_COLUMN_TYPE(this, "reltablespace", false);
			//Pages = new Sql.Column.IntegerColumn(this, "relpages", false);
			//Tuples = new Sql.Column.FloatColumn(this, "reltuples", false);
			//Toastrelid = new UNKNOWN_COLUMN_TYPE(this, "reltoastrelid", false);
			//Toastidxid = new UNKNOWN_COLUMN_TYPE(this, "reltoastidxid", false);
			//Hasindex = new Sql.Column.BoolColumn(this, "relhasindex", false);
			//Isshared = new Sql.Column.BoolColumn(this, "relisshared", false);
			//Persistence = new Sql.Column.StringColumn(this, "relpersistence", false);
				Kind = new Sql.Column.StringColumn(this, "relkind", false, int.MaxValue);
			//Natts = new Sql.Column.SmallIntegerColumn(this, "relnatts", false);
			//Checks = new Sql.Column.SmallIntegerColumn(this, "relchecks", false);
			//Hasoids = new Sql.Column.BoolColumn(this, "relhasoids", false);
			//Haspkey = new Sql.Column.BoolColumn(this, "relhaspkey", false);
			//Hasrules = new Sql.Column.BoolColumn(this, "relhasrules", false);
			//Hastriggers = new Sql.Column.BoolColumn(this, "relhastriggers", false);
			//Hassubclass = new Sql.Column.BoolColumn(this, "relhassubclass", false);
			//Frozenxid = new UNKNOWN_COLUMN_TYPE(this, "relfrozenxid", false);
			//Acl = new UNKNOWN_COLUMN_TYPE(this, "relacl", false);
			//Options = new UNKNOWN_COLUMN_TYPE(this, "reloptions", false);

			//, Namespace, Type, Oftype, Owner, Am, Filenode, Tablespace, Pages, Tuples, Toastrelid, Toastidxid, Hasindex, Isshared, Persistence
			//, Natts, Checks, Hasoids, Haspkey, Hasrules, Hastriggers, Hassubclass, Frozenxid, Acl, Options
			AddColumns(Oid, Name, Kind);
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

		public long Oid {
			get { return Tbl.Oid.ValueOf(this); }
			set { Tbl.Oid.SetValue(this, value); }
		}

		public string Name {
			get { return Tbl.Name.ValueOf(this); }
			set { Tbl.Name.SetValue(this, value); }
		}

		//public UNKNOWN_COLUMN_TYPE Namespace {
		//	get { return Tbl.Namespace.ValueOf(this); }
		//	set { Tbl.Namespace.SetValue(this, value); }
		//}

		//public UNKNOWN_COLUMN_TYPE Type {
		//	get { return Tbl.Type.ValueOf(this); }
		//	set { Tbl.Type.SetValue(this, value); }
		//}

		//public UNKNOWN_COLUMN_TYPE Oftype {
		//	get { return Tbl.Oftype.ValueOf(this); }
		//	set { Tbl.Oftype.SetValue(this, value); }
		//}

		//public UNKNOWN_COLUMN_TYPE Owner {
		//	get { return Tbl.Owner.ValueOf(this); }
		//	set { Tbl.Owner.SetValue(this, value); }
		//}

		//public UNKNOWN_COLUMN_TYPE Am {
		//	get { return Tbl.Am.ValueOf(this); }
		//	set { Tbl.Am.SetValue(this, value); }
		//}

		//public UNKNOWN_COLUMN_TYPE Filenode {
		//	get { return Tbl.Filenode.ValueOf(this); }
		//	set { Tbl.Filenode.SetValue(this, value); }
		//}

		//public UNKNOWN_COLUMN_TYPE Tablespace {
		//	get { return Tbl.Tablespace.ValueOf(this); }
		//	set { Tbl.Tablespace.SetValue(this, value); }
		//}

		//public int Pages {
		//	get { return Tbl.Pages.ValueOf(this); }
		//	set { Tbl.Pages.SetValue(this, value); }
		//}

		//public float Tuples {
		//	get { return Tbl.Tuples.ValueOf(this); }
		//	set { Tbl.Tuples.SetValue(this, value); }
		//}

		//public UNKNOWN_COLUMN_TYPE Toastrelid {
		//	get { return Tbl.Toastrelid.ValueOf(this); }
		//	set { Tbl.Toastrelid.SetValue(this, value); }
		//}

		//public UNKNOWN_COLUMN_TYPE Toastidxid {
		//	get { return Tbl.Toastidxid.ValueOf(this); }
		//	set { Tbl.Toastidxid.SetValue(this, value); }
		//}

		//public bool Hasindex {
		//	get { return Tbl.Hasindex.ValueOf(this); }
		//	set { Tbl.Hasindex.SetValue(this, value); }
		//}

		//public bool Isshared {
		//	get { return Tbl.Isshared.ValueOf(this); }
		//	set { Tbl.Isshared.SetValue(this, value); }
		//}

		//public string Persistence {
		//	get { return Tbl.Persistence.ValueOf(this); }
		//	set { Tbl.Persistence.SetValue(this, value); }
		//}

		public string Kind {
			get { return Tbl.Kind.ValueOf(this); }
			set { Tbl.Kind.SetValue(this, value); }
		}

		//public short Natts {
		//	get { return Tbl.Natts.ValueOf(this); }
		//	set { Tbl.Natts.SetValue(this, value); }
		//}

		//public short Checks {
		//	get { return Tbl.Checks.ValueOf(this); }
		//	set { Tbl.Checks.SetValue(this, value); }
		//}

		//public bool Hasoids {
		//	get { return Tbl.Hasoids.ValueOf(this); }
		//	set { Tbl.Hasoids.SetValue(this, value); }
		//}

		//public bool Haspkey {
		//	get { return Tbl.Haspkey.ValueOf(this); }
		//	set { Tbl.Haspkey.SetValue(this, value); }
		//}

		//public bool Hasrules {
		//	get { return Tbl.Hasrules.ValueOf(this); }
		//	set { Tbl.Hasrules.SetValue(this, value); }
		//}

		//public bool Hastriggers {
		//	get { return Tbl.Hastriggers.ValueOf(this); }
		//	set { Tbl.Hastriggers.SetValue(this, value); }
		//}

		//public bool Hassubclass {
		//	get { return Tbl.Hassubclass.ValueOf(this); }
		//	set { Tbl.Hassubclass.SetValue(this, value); }
		//}

		//public UNKNOWN_COLUMN_TYPE Frozenxid {
		//	get { return Tbl.Frozenxid.ValueOf(this); }
		//	set { Tbl.Frozenxid.SetValue(this, value); }
		//}

		//public UNKNOWN_COLUMN_TYPE Acl {
		//	get { return Tbl.Acl.ValueOf(this); }
		//	set { Tbl.Acl.SetValue(this, value); }
		//}

		//public UNKNOWN_COLUMN_TYPE Options {
		//	get { return Tbl.Options.ValueOf(this); }
		//	set { Tbl.Options.SetValue(this, value); }
		//}
	}
}