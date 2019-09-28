
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
	public class NIntegerTest {

		[TestInitialize()]
		public void Init() {

			using(Transaction transaction = new Transaction(DB.TestDB)) {

				Query.Delete(Tables.NIntTable.Table.INSTANCE).NoWhereCondition.Execute(transaction);
				Query.Delete(Tables.IntTable.Table.INSTANCE).NoWhereCondition.Execute(transaction);

				transaction.Commit();
			}
		}

		private int mInt1 = 25;

		[TestMethod]
		public void Test_01() {

			using(Transaction transaction = new Transaction(DB.TestDB)) {

				Tables.NIntTable.Row row = new Sql.Tables.NIntTable.Row();

				row.Id = Guid.NewGuid();
				row.IntValue = mInt1;
				row.Update(transaction);

				transaction.Commit();
			}

			Tables.NIntTable.Table table = Tables.NIntTable.Table.INSTANCE;

			IResult result = Query.Select(table.IntValue).From(table).Execute(DB.TestDB);
			Assert.AreEqual(1, result.Count);
			Assert.AreEqual(mInt1, table[0, result].IntValue);

			result = Query.Select(table.IntValue).From(table).Where(table.IntValue == mInt1).Execute(DB.TestDB);
			Assert.AreEqual(1, result.Count);
			Assert.AreEqual(mInt1, table[0, result].IntValue);

			result = Query.Select(table.IntValue).From(table).Where(table.IntValue != mInt1).Execute(DB.TestDB);
			Assert.AreEqual(0, result.Count);

			result = Query.Select(table.IntValue).From(table).Where(table.IntValue > 0).Execute(DB.TestDB);
			Assert.AreEqual(1, result.Count);
			Assert.AreEqual(mInt1, table[0, result].IntValue);

			result = Query.Select(table.IntValue).From(table).Where(table.IntValue < 0).Execute(DB.TestDB);
			Assert.AreEqual(0, result.Count);

			result = Query.Select(table.IntValue).From(table).Where(table.IntValue <= mInt1).Execute(DB.TestDB);
			Assert.AreEqual(1, result.Count);
			Assert.AreEqual(mInt1, table[0, result].IntValue);

			result = Query.Select(table.IntValue).From(table).Where(table.IntValue >= mInt1).Execute(DB.TestDB);
			Assert.AreEqual(1, result.Count);
			Assert.AreEqual(mInt1, table[0, result].IntValue);

			result = Query.Select(table.IntValue).From(table).Where(table.IntValue <= (mInt1 - 1)).Execute(DB.TestDB);
			Assert.AreEqual(0, result.Count);

			result = Query.Select(table.IntValue).From(table).Where(table.IntValue >= (mInt1 + 1)).Execute(DB.TestDB);
			Assert.AreEqual(0, result.Count);

			result = Query.Select(table.IntValue).From(table).Where(table.IntValue.In(mInt1, 5)).Execute(DB.TestDB);
			Assert.AreEqual(1, result.Count);
			Assert.AreEqual(mInt1, table[0, result].IntValue);

			result = Query.Select(table.IntValue).From(table).Where(table.IntValue.NotIn(mInt1, 5)).Execute(DB.TestDB);
			Assert.AreEqual(0, result.Count);

			System.Collections.Generic.List<int> list = new System.Collections.Generic.List<int>();
			list.Add(mInt1);

			result = Query.Select(table.IntValue).From(table).Where(table.IntValue.In(list)).Execute(DB.TestDB);
			Assert.AreEqual(1, result.Count);
			Assert.AreEqual(mInt1, table[0, result].IntValue);

			result = Query.Select(table.IntValue).From(table).Where(table.IntValue.NotIn(list)).Execute(DB.TestDB);
			Assert.AreEqual(0, result.Count);

			Tables.NIntTable.Table table2 = new Sql.Tables.NIntTable.Table();

			result = Query.Select(table, table2)
				.From(table)
				.Join(table2, table.IntValue == table2.IntValue)
				.Execute(DB.TestDB);

			Assert.AreEqual(1, result.Count);
			Assert.AreEqual(mInt1, table[0, result].IntValue);
			Assert.AreEqual(mInt1, table2[0, result].IntValue);

			result = Query.Select(table, table2)
				.From(table)
				.Join(table2, table.IntValue != table2.IntValue)
				.Execute(DB.TestDB);

			Assert.AreEqual(0, result.Count);

			result = Query.Select(table, table2)
				.From(table)
				.Join(table2, table.IntValue >= table2.IntValue)
				.Execute(DB.TestDB);

			Assert.AreEqual(1, result.Count);
			Assert.AreEqual(mInt1, table[0, result].IntValue);
			Assert.AreEqual(mInt1, table2[0, result].IntValue);

			result = Query.Select(table, table2)
				.From(table)
				.Join(table2, table.IntValue <= table2.IntValue)
				.Execute(DB.TestDB);

			Assert.AreEqual(1, result.Count);
			Assert.AreEqual(mInt1, table[0, result].IntValue);
			Assert.AreEqual(mInt1, table2[0, result].IntValue);

			result = Query.Select(table, table2)
				.From(table)
				.Join(table2, table.IntValue < table2.IntValue)
				.Execute(DB.TestDB);

			Assert.AreEqual(0, result.Count);

			result = Query.Select(table, table2)
				.From(table)
				.Join(table2, table.IntValue > table2.IntValue)
				.Execute(DB.TestDB);

			Assert.AreEqual(0, result.Count);

			result = Query.Select(table)
				.From(table)
				.Where(table.IntValue.In(Query.Select(table2.IntValue).Distinct.From(table2)))
				.Execute(DB.TestDB);

			Assert.AreEqual(1, result.Count);
			Assert.AreEqual(mInt1, table[0, result].IntValue);

			result = Query.Select(table)
				.From(table)
				.Where(table.IntValue.NotIn(Query.Select(table2.IntValue).Distinct.From(table2)))
				.Execute(DB.TestDB);

			Assert.AreEqual(0, result.Count);
		}

		[TestMethod]
		public void Test_02() {

			using(Transaction transaction = new Transaction(DB.TestDB)) {

				Tables.NIntTable.Row row = new Sql.Tables.NIntTable.Row();

				row.Id = Guid.NewGuid();
				row.IntValue = null;
				row.Update(transaction);

				Tables.IntTable.Row intRow = new Sql.Tables.IntTable.Row();

				intRow.Id = Guid.NewGuid();
				intRow.IntValue = mInt1;
				intRow.Update(transaction);

				transaction.Commit();
			}

			Tables.NIntTable.Table table = Tables.NIntTable.Table.INSTANCE;

			IResult result = Query.Select(table.IntValue).From(table).Execute(DB.TestDB);
			Assert.AreEqual(1, result.Count);
			Assert.AreEqual(null, table[0, result].IntValue);

			result = Query.Select(table.IntValue).From(table).Where(table.IntValue.IsNull).Execute(DB.TestDB);
			Assert.AreEqual(1, result.Count);
			Assert.AreEqual(null, table[0, result].IntValue);

			result = Query.Select(table.IntValue).From(table).Where(table.IntValue.IsNotNull).Execute(DB.TestDB);
			Assert.AreEqual(0, result.Count);

			Tables.IntTable.Table decTable = Tables.IntTable.Table.INSTANCE;

			result = Query.Select(table, decTable)
				.From(table)
				.Join(decTable, table.IntValue == decTable.IntValue)
				.Execute(DB.TestDB);

			Assert.AreEqual(0, result.Count);

			result = Query.Select(table, decTable)
				.From(table)
				.Join(decTable, table.IntValue != decTable.IntValue)
				.Execute(DB.TestDB);

			Assert.AreEqual(0, result.Count);

			using(Transaction transaction = new Transaction(DB.TestDB)) {

				Query.Update(table).Set(table.IntValue, mInt1).NoWhereCondition().Execute(transaction);

				transaction.Commit();
			}

			result = Query.Select(table, decTable)
				.From(table)
				.Join(decTable, table.IntValue == decTable.IntValue)
				.Execute(DB.TestDB);

			Assert.AreEqual(1, result.Count);
			Assert.AreEqual(mInt1, table[0, result].IntValue);
			Assert.AreEqual(mInt1, decTable[0, result].IntValue);

			result = Query.Select(table, decTable)
				.From(table)
				.Join(decTable, decTable.IntValue == table.IntValue)
				.Execute(DB.TestDB);

			Assert.AreEqual(1, result.Count);
			Assert.AreEqual(mInt1, table[0, result].IntValue);
			Assert.AreEqual(mInt1, decTable[0, result].IntValue);

			result = Query.Select(table, decTable)
				.From(table)
				.Join(decTable, table.IntValue != decTable.IntValue)
				.Execute(DB.TestDB);

			Assert.AreEqual(0, result.Count);

			result = Query.Select(table, decTable)
				.From(table)
				.Join(decTable, decTable.IntValue != table.IntValue)
				.Execute(DB.TestDB);

			Assert.AreEqual(0, result.Count);

			result = Query.Select(table, decTable)
				.From(table)
				.Join(decTable, table.IntValue < decTable.IntValue)
				.Execute(DB.TestDB);

			Assert.AreEqual(0, result.Count);

			result = Query.Select(table, decTable)
				.From(table)
				.Join(decTable, table.IntValue > decTable.IntValue)
				.Execute(DB.TestDB);

			Assert.AreEqual(0, result.Count);

			result = Query.Select(table, decTable)
				.From(table)
				.Join(decTable, table.IntValue >= decTable.IntValue)
				.Execute(DB.TestDB);

			Assert.AreEqual(1, result.Count);
			Assert.AreEqual(mInt1, table[0, result].IntValue);
			Assert.AreEqual(mInt1, decTable[0, result].IntValue);

			result = Query.Select(table, decTable)
				.From(table)
				.Join(decTable, table.IntValue <= decTable.IntValue)
				.Execute(DB.TestDB);

			Assert.AreEqual(1, result.Count);
			Assert.AreEqual(mInt1, table[0, result].IntValue);
			Assert.AreEqual(mInt1, decTable[0, result].IntValue);

			result = Query.Select(table, decTable)
				.From(table)
				.Join(decTable, decTable.IntValue < table.IntValue)
				.Execute(DB.TestDB);

			Assert.AreEqual(0, result.Count);

			result = Query.Select(table, decTable)
				.From(table)
				.Join(decTable, decTable.IntValue > table.IntValue)
				.Execute(DB.TestDB);

			Assert.AreEqual(0, result.Count);

			result = Query.Select(table, decTable)
				.From(table)
				.Join(decTable, decTable.IntValue >= table.IntValue)
				.Execute(DB.TestDB);

			Assert.AreEqual(1, result.Count);
			Assert.AreEqual(mInt1, table[0, result].IntValue);
			Assert.AreEqual(mInt1, decTable[0, result].IntValue);

			result = Query.Select(table, decTable)
				.From(table)
				.Join(decTable, decTable.IntValue <= table.IntValue)
				.Execute(DB.TestDB);

			Assert.AreEqual(1, result.Count);
			Assert.AreEqual(mInt1, table[0, result].IntValue);
			Assert.AreEqual(mInt1, decTable[0, result].IntValue);
		}

		[TestMethod]
		public void Test_03() {

			using(Transaction transaction = new Transaction(DB.TestDB)) {

				Tables.NIntTable.Row row = new Sql.Tables.NIntTable.Row();

				row.Id = Guid.NewGuid();
				row.IntValue = 20;
				row.Update(transaction);

				row = new Sql.Tables.NIntTable.Row();
				row.Id = Guid.NewGuid();
				row.IntValue = 20;
				row.Update(transaction);

				row = new Sql.Tables.NIntTable.Row();
				row.Id = Guid.NewGuid();
				row.IntValue = 30;
				row.Update(transaction);

				transaction.Commit();
			}

			Tables.NIntTable.Table table = Tables.NIntTable.Table.INSTANCE;

			Sql.Function.Count count = new Sql.Function.Count(table.IntValue, true);

			IResult result = Sql.Query
				.Select(count)
				.From(table)
				.Execute(DB.TestDB);

			Assert.AreEqual(1, result.Count);
			Assert.AreEqual(2, count[0, result]!.Value);

			count = new Sql.Function.Count(table.Id, true);

			result = Sql.Query
				.Select(count)
				.From(table)
				.Execute(DB.TestDB);

			Assert.AreEqual(1, result.Count);
			Assert.AreEqual(3, count[0, result]!.Value);

			count = new Sql.Function.Count(table.IntValue, false);

			result = Sql.Query
				.Select(count)
				.From(table)
				.Execute(DB.TestDB);

			Assert.AreEqual(1, result.Count);
			Assert.AreEqual(3, count[0, result]!.Value);
		}

		[TestMethod]
		public void ParametersTurnedOff() {

			Settings.UseParameters = false;

			try {
				Test_01();
				Init();
				Test_02();
				Init();
				Test_03();
			}
			finally {
				Settings.UseParameters = true;
			}
		}
	}
}