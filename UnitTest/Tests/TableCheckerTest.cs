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
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using Sql.Tables;

namespace Sql.Tests {
	
	[TestClass]
	public class TableCheckerTest {
		
		[TestInitialize()]
		public void Init() {
			
		}
		
		[TestMethod]
		public void Test_01() {			
			
			List<Type> tables = new List<Type>();
			
			tables.Add(typeof(Tables.BigIntTable.Table));
			tables.Add(typeof(Sql.Tables.AutoTable.Table));
			
			tables.Add(typeof(Sql.Tables.BinaryTable.Table));
			tables.Add(typeof(Sql.Tables.BooleanTable.Table));
			tables.Add(typeof(Sql.Tables.DateTime2Table.Table));
			tables.Add(typeof(Sql.Tables.DateTimeOffsetTable.Table));
			tables.Add(typeof(Sql.Tables.DateTimeTable.Table));
			tables.Add(typeof(Sql.Tables.DecimalTable.Table));
			tables.Add(typeof(Sql.Tables.DoubleTable.Table));
			tables.Add(typeof(Sql.Tables.Earthquake.Table));
			tables.Add(typeof(Sql.Tables.EnumTable.Table));
			tables.Add(typeof(Sql.Tables.FloatTable.Table));
			tables.Add(typeof(Sql.Tables.GuidTable.Table));
			tables.Add(typeof(Sql.Tables.IntTable.Table));
			tables.Add(typeof(Sql.Tables.NDateTime2Table.Table));
			tables.Add(typeof(Sql.Tables.NDateTimeTable.Table));
			tables.Add(typeof(Sql.Tables.NDecimalTable.Table));
			tables.Add(typeof(Sql.Tables.NIntTable.Table));
			tables.Add(typeof(Sql.Tables.SmallIntTable.Table));
			tables.Add(typeof(Sql.Tables.StringTable.Table));
			
			Sql.ADatabase database = DB.TestDB;
			
			StringBuilder output = new StringBuilder();
			
			foreach(Type tableType in tables) {
				string result = Sql.Database.CheckDefinitions.CheckTable(tableType, database);
				output.Append(result).Append(System.Environment.NewLine);
			}
			
			string finalResult = output.ToString();
			return;
		}
	}
}