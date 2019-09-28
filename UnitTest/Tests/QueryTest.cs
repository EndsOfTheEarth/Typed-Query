
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
	public class QueryTest {

		[TestMethod]
		public void Test_01() {

			Tables.GuidTable.Table table = Tables.GuidTable.Table.INSTANCE;

			using(Transaction transaction = new Transaction(DB.TestDB)) {
				Query.Delete(table).NoWhereCondition.Execute(transaction);
				transaction.Commit();
			}

			IResult result = Sql.Query.Select(table).From(table).Execute(DB.TestDB);
			Assert.AreEqual(0, result.Count);

			bool exceptionThrown = false;

			using(Transaction transaction = new Transaction(DB.TestDB)) {

				Tables.GuidTable.Row row = new Sql.Tables.GuidTable.Row();
				row.Id = Guid.NewGuid();

				row.Delete();

				try {
					//You cannot call update on a deleted row that doesn't exist in the database
					row.Update(transaction);
				}
				catch {
					exceptionThrown = true;
				}
				transaction.Commit();
			}

			Assert.AreEqual(true, exceptionThrown);
		}

		[TestMethod]
		public void Test_02() {

			Tables.GuidTable.Table table = Tables.GuidTable.Table.INSTANCE;

			using(Transaction transaction = new Transaction(DB.TestDB)) {
				Query.Delete(table).NoWhereCondition.Execute(transaction);
				transaction.Commit();
			}

			IResult result = Sql.Query.Select(table).From(table).Execute(DB.TestDB);
			Assert.AreEqual(0, result.Count);

			using(Transaction transaction = new Transaction(DB.TestDB)) {

				Tables.GuidTable.Row row = new Sql.Tables.GuidTable.Row();
				row.Id = Guid.NewGuid();

				row.Delete();
				try {
					row.Id = Guid.NewGuid();
				}
				catch(Exception e) {
					if(e.Message.StartsWith("Cannot set columns data when row is deleted"))
						return;
				}
				Assert.IsTrue(false);
			}
		}

		[TestMethod]
		public void Test_03() {

			Tables.GuidTable.Table table = Tables.GuidTable.Table.INSTANCE;

			using(Transaction transaction = new Transaction(DB.TestDB)) {
				Query.Delete(table).NoWhereCondition.Execute(transaction);
				transaction.Commit();
			}

			IResult result = Sql.Query.Select(table).From(table).Execute(DB.TestDB);
			Assert.AreEqual(0, result.Count);

			const int rows = 10;

			using(Transaction transaction = new Transaction(DB.TestDB)) {

				for(int index = 0; index < rows; index++) {
					Tables.GuidTable.Row row = new Sql.Tables.GuidTable.Row();
					row.Id = Guid.NewGuid();

					row.Update(transaction);
				}
				transaction.Commit();
			}

			Function.Count count = new Sql.Function.Count();

			result = Sql.Query.Select(table, count).From(table).GroupBy(table.Id).Execute(DB.TestDB);
			Assert.AreEqual(rows, result.Count);

			for(int index = 0; index < result.Count; index++)
				Assert.AreEqual(1, count[index, result]!.Value);
		}
	}
}