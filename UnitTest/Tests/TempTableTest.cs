
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
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Sql.Tests {

	[TestClass]
	public class TempTableTest {


		[TestInitialize()]
		public void Init() {

			using(Transaction transaction = new Transaction(DB.TestDB)) {

				Tables.StringTable.Table table = Tables.StringTable.Table.INSTANCE;

				Query.Delete(table).NoWhereCondition.Execute(transaction);

				transaction.Commit();
			}
		}

		[TestMethod]
		public void Test_01() {

			try {

				Tables.StringTable.Table strTable = Tables.StringTable.Table.INSTANCE;

				using(Sql.Transaction transaction = new Transaction(DB.TestDB)) {

					Sql.Query.Insert(strTable).Set(strTable.Str, "1").Execute(transaction);
					Sql.Query.Insert(strTable).Set(strTable.Str, "2").Execute(transaction);
					Sql.Query.Insert(strTable).Set(strTable.Str, "3").Execute(transaction);
					Sql.Query.Insert(strTable).Set(strTable.Str, "4").Execute(transaction);
					Sql.Query.Insert(strTable).Set(strTable.Str, "5").Execute(transaction);

					transaction.Commit();
				}

				using(Sql.Transaction transaction = new Transaction(DB.TestDB)) {

					Tables.TempStringTable.Table tempStrTable = Tables.TempStringTable.Table.INSTANCE;

					Sql.Query.Select(strTable.Str)
						.Into(tempStrTable)
						.From(strTable)
						.Execute(transaction);

					IResult result = Sql.Query.Select(tempStrTable.Str)
						.From(tempStrTable)
						.OrderBy(tempStrTable.Str)
						.Execute(transaction);

					Assert.AreEqual(5, result.Count);
					Assert.AreEqual("1", tempStrTable[0, result].Str);
					Assert.AreEqual("2", tempStrTable[1, result].Str);
					Assert.AreEqual("3", tempStrTable[2, result].Str);
					Assert.AreEqual("4", tempStrTable[3, result].Str);
					Assert.AreEqual("5", tempStrTable[4, result].Str);

					result = Sql.Query.Select(tempStrTable.Str)
						.From(strTable)
						.Join(tempStrTable, tempStrTable.Str == strTable.Str)
						.OrderBy(tempStrTable.Str)
						.Execute(transaction);

					Assert.AreEqual(5, result.Count);
					Assert.AreEqual("1", tempStrTable[0, result].Str);
					Assert.AreEqual("2", tempStrTable[1, result].Str);
					Assert.AreEqual("3", tempStrTable[2, result].Str);
					Assert.AreEqual("4", tempStrTable[3, result].Str);
					Assert.AreEqual("5", tempStrTable[4, result].Str);

					result = Sql.Query
						.Select(strTable.Str)
						.From(strTable)
						.Union(tempStrTable.Str)
						.From(tempStrTable)
						.OrderBy(tempStrTable.Str)
						.Execute(transaction);

					Assert.AreEqual(5, result.Count);
					Assert.AreEqual("1", tempStrTable[0, result].Str);
					Assert.AreEqual("2", tempStrTable[1, result].Str);
					Assert.AreEqual("3", tempStrTable[2, result].Str);
					Assert.AreEqual("4", tempStrTable[3, result].Str);
					Assert.AreEqual("5", tempStrTable[4, result].Str);
				}
			}
			catch(Exception e) {
				throw e;
			}
			return;
		}
	}
}