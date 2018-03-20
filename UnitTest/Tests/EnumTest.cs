
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
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Sql.Tables;

namespace Sql.Tests {

	[TestClass]
	public class EnumTest {

		[TestInitialize()]
		public void Init() {

			using(Transaction transaction = new Transaction(DB.TestDB)) {

				Tables.EnumTable.Table table = Tables.EnumTable.Table.INSTANCE;

				Query.Delete(table).NoWhereCondition.Execute(transaction);

				transaction.Commit();
			}
		}

		[TestMethod]
		public void Test_01() {

			Tables.EnumTable.Table table = Tables.EnumTable.Table.INSTANCE;

			using(Transaction transaction = new Transaction(DB.TestDB)) {
				Query.Insert(table).Set(table.EnumValue, Tables.EnumTable.EnumTypes.A).Execute(transaction);
				transaction.Commit();
			}

			IResult result = Query.Select(table.EnumValue).From(table).Where(table.EnumValue == Tables.EnumTable.EnumTypes.A).Execute(DB.TestDB);
			Assert.AreEqual(1, result.Count);
			Assert.AreEqual(Tables.EnumTable.EnumTypes.A, table[0, result].EnumValue);

			result = Query.Select(table.EnumValue).From(table).Where(table.EnumValue == Tables.EnumTable.EnumTypes.B).Execute(DB.TestDB);
			Assert.AreEqual(0, result.Count);

			result = Query.Select(table.EnumValue).From(table).Where(table.EnumValue != Tables.EnumTable.EnumTypes.A).Execute(DB.TestDB);
			Assert.AreEqual(0, result.Count);

			result = Query.Select(table.EnumValue).From(table).Where(table.EnumValue.In(Tables.EnumTable.EnumTypes.A, Tables.EnumTable.EnumTypes.B, Tables.EnumTable.EnumTypes.C)).Execute(DB.TestDB);
			Assert.AreEqual(1, result.Count);
			Assert.AreEqual(Tables.EnumTable.EnumTypes.A, table[0, result].EnumValue);
		}

		[TestMethod]
		public void ParametersTurnedOff() {

			Settings.UseParameters = false;

			try {
				Init();
				Test_01();
			}
			finally {
				Settings.UseParameters = true;
			}
		}

		[TestMethod]
		public void ParametersTurnedOn() {

			Settings.UseParameters = true;

			try {
				Init();
				Test_01();
			}
			finally {
				Settings.UseParameters = true;
			}
		}
	}
}