
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

namespace Sql.Tables.Earthquake {

	public sealed class Table : Sql.ATable {

		public static readonly Table INSTANCE = new Table();

		public readonly Sql.Column.IntegerColumn CUSP_ID;
		public readonly Sql.Column.DecimalColumn LAT;
		public readonly Sql.Column.DecimalColumn LONG;
		public readonly Sql.Column.IntegerColumn NZMGE;
		public readonly Sql.Column.IntegerColumn NZMGN;
		public readonly Sql.Column.DateTimeColumn DateTime;
		public readonly Sql.Column.DecimalColumn MAG;
		public readonly Sql.Column.DecimalColumn DEPTH;

		public Table() : base("Earthquake", "", false, typeof(Row)) {

			CUSP_ID = new Sql.Column.IntegerColumn(this, "earCUSP_ID", true);
			LAT = new Sql.Column.DecimalColumn(this, "earLAT", false);
			LONG = new Sql.Column.DecimalColumn(this, "earLONG", false);
			NZMGE = new Sql.Column.IntegerColumn(this, "earNZMGE", false);
			NZMGN = new Sql.Column.IntegerColumn(this, "earNZMGN", false);
			DateTime = new Sql.Column.DateTimeColumn(this, "earDateTime", false);
			MAG = new Sql.Column.DecimalColumn(this, "earMAG", false);
			DEPTH = new Sql.Column.DecimalColumn(this, "earDEPTH", false);

			AddColumns(CUSP_ID,LAT,LONG,NZMGE,NZMGN,DateTime,MAG,DEPTH);
		}

		public Row this[int pIndex, Sql.IResult pQueryResult]{
			get { return (Row)pQueryResult.GetRow(this, pIndex); }
		}
	}

	public sealed class Row : Sql.ARow {

		private new Table Tbl {
			get { return (Table)base.Tbl; }
		}

		public Row() : base(Table.INSTANCE) {
		}

		public int CUSP_ID {
			get { return Tbl.CUSP_ID.ValueOf(this); }
			set { Tbl.CUSP_ID.SetValue(this, value); }
		}

		public decimal LAT {
			get { return Tbl.LAT.ValueOf(this); }
			set { Tbl.LAT.SetValue(this, value); }
		}

		public decimal LONG {
			get { return Tbl.LONG.ValueOf(this); }
			set { Tbl.LONG.SetValue(this, value); }
		}

		public int NZMGE {
			get { return Tbl.NZMGE.ValueOf(this); }
			set { Tbl.NZMGE.SetValue(this, value); }
		}

		public int NZMGN {
			get { return Tbl.NZMGN.ValueOf(this); }
			set { Tbl.NZMGN.SetValue(this, value); }
		}

		public DateTime DateTime {
			get { return Tbl.DateTime.ValueOf(this); }
			set { Tbl.DateTime.SetValue(this, value); }
		}

		public decimal MAG {
			get { return Tbl.MAG.ValueOf(this); }
			set { Tbl.MAG.SetValue(this, value); }
		}

		public decimal DEPTH {
			get { return Tbl.DEPTH.ValueOf(this); }
			set { Tbl.DEPTH.SetValue(this, value); }
		}
	}
}