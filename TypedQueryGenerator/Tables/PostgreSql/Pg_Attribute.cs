
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

namespace Postgresql.pg_attribute {

	[Sql.TableAttribute("")]
	public sealed class Table : Sql.ATable {

		public static readonly Table Instance = new Table(PgDatabase.Instance);

		[Sql.ColumnAttribute("")]
		public Sql.Column.BigIntegerColumn Relid { get; private set; }

		[Sql.ColumnAttribute("")]
		public Sql.Column.StringColumn Name { get; private set; }

		[Sql.ColumnAttribute("")]
		public Sql.Column.BigIntegerColumn Typid { get; private set; }

		[Sql.ColumnAttribute("")]
		public Sql.Column.IntegerColumn Stattarget { get; private set; }

		[Sql.ColumnAttribute("")]
		public Sql.Column.SmallIntegerColumn Len { get; private set; }

		[Sql.ColumnAttribute("")]
		public Sql.Column.SmallIntegerColumn Num { get; private set; }

		[Sql.ColumnAttribute("")]
		public Sql.Column.IntegerColumn Ndims { get; private set; }

		[Sql.ColumnAttribute("")]
		public Sql.Column.IntegerColumn Cacheoff { get; private set; }

		[Sql.ColumnAttribute("")]
		public Sql.Column.IntegerColumn Typmod { get; private set; }

		[Sql.ColumnAttribute("")]
		public Sql.Column.BoolColumn Byval { get; private set; }

		[Sql.ColumnAttribute("")]
		public Sql.Column.StringColumn Storage { get; private set; }

		[Sql.ColumnAttribute("")]
		public Sql.Column.StringColumn Align { get; private set; }

		[Sql.ColumnAttribute("")]
		public Sql.Column.BoolColumn Notnull { get; private set; }

		[Sql.ColumnAttribute("")]
		public Sql.Column.BoolColumn Hasdef { get; private set; }

		[Sql.ColumnAttribute("")]
		public Sql.Column.BoolColumn Isdropped { get; private set; }

		[Sql.ColumnAttribute("")]
		public Sql.Column.BoolColumn Islocal { get; private set; }

		[Sql.ColumnAttribute("")]
		public Sql.Column.IntegerColumn Inhcount { get; private set; }

		[Sql.ColumnAttribute("")]
		public Sql.Column.BigIntegerColumn Collation { get; private set; }

		//[Sql.ColumnAttribute("")]
		//public UNKNOWN_COLUMN_TYPE Acl { get; private set; }

		//[Sql.ColumnAttribute("")]
		//public UNKNOWN_COLUMN_TYPE Options { get; private set; }

		public Table(Sql.ADatabase pDatabase)
			: base(pDatabase, "pg_attribute", "pg_catalog", false, typeof(Row)) {

			Relid = new Sql.Column.BigIntegerColumn(this, "attrelid", false);
			Name = new Sql.Column.StringColumn(this, "attname", false, int.MaxValue);
			Typid = new Sql.Column.BigIntegerColumn(this, "atttypid", false);
			Stattarget = new Sql.Column.IntegerColumn(this, "attstattarget", false);
			Len = new Sql.Column.SmallIntegerColumn(this, "attlen", false);
			Num = new Sql.Column.SmallIntegerColumn(this, "attnum", false);
			Ndims = new Sql.Column.IntegerColumn(this, "attndims", false);
			Cacheoff = new Sql.Column.IntegerColumn(this, "attcacheoff", false);
			Typmod = new Sql.Column.IntegerColumn(this, "atttypmod", false);
			Byval = new Sql.Column.BoolColumn(this, "attbyval", false);
			Storage = new Sql.Column.StringColumn(this, "attstorage", false, int.MaxValue);
			Align = new Sql.Column.StringColumn(this, "attalign", false, int.MaxValue);
			Notnull = new Sql.Column.BoolColumn(this, "attnotnull", false);
			Hasdef = new Sql.Column.BoolColumn(this, "atthasdef", false);
			Isdropped = new Sql.Column.BoolColumn(this, "attisdropped", false);
			Islocal = new Sql.Column.BoolColumn(this, "attislocal", false);
			Inhcount = new Sql.Column.IntegerColumn(this, "attinhcount", false);
			Collation = new Sql.Column.BigIntegerColumn(this, "attcollation", false);
			//Acl = new UNKNOWN_COLUMN_TYPE(this, "attacl", false);
			//Options = new UNKNOWN_COLUMN_TYPE(this, "attoptions", false);

			//, Acl, Options
			AddColumns(Relid, Name, Typid, Stattarget, Len, Num, Ndims, Cacheoff, Typmod, Byval, Storage, Align, Notnull, Hasdef, Isdropped, Islocal, Inhcount, Collation);
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

		public long Relid {
			get { return Tbl.Relid.ValueOf(this); }
			set { Tbl.Relid.SetValue(this, value); }
		}

		public string Name {
			get { return Tbl.Name.ValueOf(this); }
			set { Tbl.Name.SetValue(this, value); }
		}

		public long Typid {
			get { return Tbl.Typid.ValueOf(this); }
			set { Tbl.Typid.SetValue(this, value); }
		}

		public int Stattarget {
			get { return Tbl.Stattarget.ValueOf(this); }
			set { Tbl.Stattarget.SetValue(this, value); }
		}

		public short Len {
			get { return Tbl.Len.ValueOf(this); }
			set { Tbl.Len.SetValue(this, value); }
		}

		public short Num {
			get { return Tbl.Num.ValueOf(this); }
			set { Tbl.Num.SetValue(this, value); }
		}

		public int Ndims {
			get { return Tbl.Ndims.ValueOf(this); }
			set { Tbl.Ndims.SetValue(this, value); }
		}

		public int Cacheoff {
			get { return Tbl.Cacheoff.ValueOf(this); }
			set { Tbl.Cacheoff.SetValue(this, value); }
		}

		public int Typmod {
			get { return Tbl.Typmod.ValueOf(this); }
			set { Tbl.Typmod.SetValue(this, value); }
		}

		public bool Byval {
			get { return Tbl.Byval.ValueOf(this); }
			set { Tbl.Byval.SetValue(this, value); }
		}

		public string Storage {
			get { return Tbl.Storage.ValueOf(this); }
			set { Tbl.Storage.SetValue(this, value); }
		}

		public string Align {
			get { return Tbl.Align.ValueOf(this); }
			set { Tbl.Align.SetValue(this, value); }
		}

		public bool Notnull {
			get { return Tbl.Notnull.ValueOf(this); }
			set { Tbl.Notnull.SetValue(this, value); }
		}

		public bool Hasdef {
			get { return Tbl.Hasdef.ValueOf(this); }
			set { Tbl.Hasdef.SetValue(this, value); }
		}

		public bool Isdropped {
			get { return Tbl.Isdropped.ValueOf(this); }
			set { Tbl.Isdropped.SetValue(this, value); }
		}

		public bool Islocal {
			get { return Tbl.Islocal.ValueOf(this); }
			set { Tbl.Islocal.SetValue(this, value); }
		}

		public int Inhcount {
			get { return Tbl.Inhcount.ValueOf(this); }
			set { Tbl.Inhcount.SetValue(this, value); }
		}

		public long Collation {
			get { return Tbl.Collation.ValueOf(this); }
			set { Tbl.Collation.SetValue(this, value); }
		}

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