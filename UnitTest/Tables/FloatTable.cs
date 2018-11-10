
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

namespace Sql.Tables.FloatTable {

	public sealed class Table : Sql.ATable {

		public static readonly Table INSTANCE = new Table();

		public readonly Sql.Column.GuidColumn Id;
		public readonly Sql.Column.FloatColumn Value;
		public readonly Sql.Column.NFloatColumn NValue;

		public Table() : base("FloatTable", "", false, typeof(Row)) {

			Id = new Sql.Column.GuidColumn(this, "Id", true);
			Value = new Sql.Column.FloatColumn(this, "Value", false);
			NValue = new Sql.Column.NFloatColumn(this, "NValue", false);

			AddColumns(Id,Value,NValue);
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

		public Guid Id {
			get { return Tbl.Id.ValueOf(this); }
			set { Tbl.Id.SetValue(this, value); }
		}

		public float Value {
			get { return Tbl.Value.ValueOf(this); }
			set { Tbl.Value.SetValue(this, value); }
		}

		public float? NValue {
			get { return Tbl.NValue.ValueOf(this); }
			set { Tbl.NValue.SetValue(this, value); }
		}
	}
}