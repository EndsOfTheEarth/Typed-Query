
/*
 * 
 * Copyright (C) 2009-2019 JFo.nz
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

namespace Sql.Tables.TempStringTable {

	public sealed class Table : Sql.ATable {

		public static readonly Table INSTANCE = new Table("TempTable");

		public readonly Sql.Column.StringColumn Str;

		public Table(string pTempTableName) : base(typeof(Row), pTempTableName) {

			Str = new Sql.Column.StringColumn(this, "Str", true, 100);

			AddColumns(Str);
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
		public string Str {
			get { return Tbl.Str.ValueOf(this); }
			set { Tbl.Str.SetValue(this, value); }
		}	
	}
}