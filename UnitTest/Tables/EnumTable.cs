
/*
 * 
 * Copyright (C) 2009-2015 JFo.nz
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
using System.Data;
using System.Collections.Generic;
using Sql;

namespace Sql.Tables.EnumTable {

	public enum EnumTypes {
		A = 1,
		B = 2,
		C = 3
	}
	
	[Sql.TableAttribute("Enum 'table' description")]
	public sealed class Table : Sql.ATable {

		public static readonly Table INSTANCE = new Table();

		[Sql.ColumnAttribute("Enum column description")]
		public readonly Sql.Column.EnumColumn<EnumTypes> EnumValue;

		public Table() : base(DB.TestDB, "EnumTable", "", false, typeof(Row)) {

			EnumValue = new Sql.Column.EnumColumn<EnumTypes>(this, "EnumValue", false);

			AddColumns(EnumValue);
		}

		public Row this[int pIndex, Sql.IResult pQueryResult]{
			get { return (Row)pQueryResult.GetRow(this, pIndex); }
		}

//		protected override Sql.ARow LoadRow(IList<Sql.ISelectable> pSelectColumns, System.Data.Common.DbDataReader pReader) {
//			return new Row(this, pSelectColumns, pReader);
//		}
	}

	public sealed class Row : Sql.ARow {

		private new Table Tbl {
			get { return (Table)base.Tbl; }
		}

		public Row() : base(Table.INSTANCE) {
			
		}

//		internal Row(Table pTable, IList<Sql.ISelectable> pSelectColumns, System.Data.Common.DbDataReader pReader)
//			: base(pTable, pSelectColumns, pReader) {
//			mTable = pTable;
//		}

		public EnumTypes EnumValue {
			get { return Tbl.EnumValue.ValueOf(this); }
			set { Tbl.EnumValue.SetValue(this, value); }
		}	
	}
}