
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
using System.Data.SqlClient;
using System.Data;

namespace Sql.Tables.SP {
	
	public class StoredProc : Sql.ATable {
		
		public static readonly StoredProc INSTANCE = new StoredProc();
		
		public StoredProc() : base("BooleanTable", "", false, typeof(Row)) {
			
		}
	}
	
	public sealed class Row : Sql.ARow {

		private new StoredProc Tbl {
			get { return (StoredProc)base.Tbl; }
		}

		public Row() : base(StoredProc.INSTANCE) {
			
		}
	}
}