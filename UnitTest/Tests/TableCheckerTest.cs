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
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using Sql.Tables;
using TypedQuery.Logic;

namespace Sql.Tests {
	
	[TestClass]
	public class TableCheckerTest {
		
		[TestInitialize()]
		public void Init() {
			
		}
		
		[TestMethod]
		public void Test_01() {			
			
			List<Sql.ARow> rows = new List<Sql.ARow>();
			
			rows.Add(new Tables.BigIntTable.Row());
			rows.Add(new Sql.Tables.AutoTable.Row());
			
			rows.Add(new Sql.Tables.BinaryTable.Row());
			rows.Add(new Sql.Tables.BooleanTable.Row());
			rows.Add(new Sql.Tables.DateTime2Table.Row());
			rows.Add(new Sql.Tables.DateTimeOffsetTable.Row());
			rows.Add(new Sql.Tables.DateTimeTable.Row());
			rows.Add(new Sql.Tables.DecimalTable.Row());
			rows.Add(new Sql.Tables.DoubleTable.Row());
			rows.Add(new Sql.Tables.Earthquake.Row());
			rows.Add(new Sql.Tables.EnumTable.Row());
			rows.Add(new Sql.Tables.FloatTable.Row());
			rows.Add(new Sql.Tables.GuidTable.Row());
			rows.Add(new Sql.Tables.IntTable.Row());
			rows.Add(new Sql.Tables.NDateTime2Table.Row());
			rows.Add(new Sql.Tables.NDateTimeTable.Row());
			rows.Add(new Sql.Tables.NDecimalTable.Row());
			rows.Add(new Sql.Tables.NIntTable.Row());
			rows.Add(new Sql.Tables.SmallIntTable.Row());
			rows.Add(new Sql.Tables.StringTable.Row());
			
			Sql.ADatabase database = DB.TestDB;
			
			StringBuilder output = new StringBuilder();
			
			foreach(Sql.ARow row in rows) {

				List<ValidationError> errors = DefinitionChecker.CheckTable(row, DB.TestDB);

                foreach(ValidationError error in errors) {
                    output.Append(error.Message).Append(System.Environment.NewLine);
                }
			}
			
			string finalResult = output.ToString();
			return;
		}
	}
}