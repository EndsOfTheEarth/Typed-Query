
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

namespace Sql.Tests {

	[TestClass]
	public class InsertSelectTest {

		[TestInitialize()]
		public void Init() {

			using(Transaction transaction = new Transaction(DB.TestDB)) {

				Tables.BigIntTable.Table table = Tables.BigIntTable.Table.INSTANCE;

				Query.Delete(table).NoWhereCondition.Execute(transaction);

				transaction.Commit();
			}
		}

		[TestMethod]
		public void Test_01() {

			using(Transaction transaction = new Transaction(DB.TestDB)) {

				Tables.BigIntTable.Row row = new Sql.Tables.BigIntTable.Row();
				row.IntValue = 25;
				row.Update(transaction);

				transaction.Commit();
			}

			Tables.BigIntTable.Table table = Tables.BigIntTable.Table.INSTANCE;

			using(Transaction transaction = new Transaction(DB.TestDB)) {
				Sql.Query.InsertSelect(table).Columns(table.IntValue).Query(Sql.Query.Select(table.IntValue).From(table)).Execute(transaction);
				transaction.Commit();
			}

			IResult result = Sql.Query.Select(table.IntValue)
				.From(table)
				.Execute(DB.TestDB);

			Assert.AreEqual(2, result.Count);
			Assert.AreEqual(25, table[0, result].IntValue);
			Assert.AreEqual(25, table[1, result].IntValue);

			using(Transaction transaction = new Transaction(DB.TestDB)) {
				Sql.Query.InsertSelect(table).Columns(table.IntValue).Query(Sql.Query.Select(table.IntValue).From(table)).Execute(transaction);
				transaction.Commit();
			}

			result = Sql.Query.Select(table.IntValue)
				.From(table)
				.Execute(DB.TestDB);

			Assert.AreEqual(4, result.Count);

			using(Transaction transaction = new Transaction(DB.TestDB)) {

				Sql.Query.InsertSelect(table).Columns(table.IntValue)
					.Query(
						Sql.Query.Select(table.IntValue)
						.From(table)
						.UnionAll(table.IntValue)
						.From(table)
					).Execute(transaction);

				transaction.Commit();
			}

			result = Sql.Query.Select(table.IntValue)
				.From(table)
				.Execute(DB.TestDB);
			Assert.AreEqual(12, result.Count);
		}

		[TestMethod]
		public void ParametersTurnedOff() {

			Settings.UseParameters = false;

			try {
				Test_01();
			}
			finally {
				Settings.UseParameters = true;
			}
		}
	}
}