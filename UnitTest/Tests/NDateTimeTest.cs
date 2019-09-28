
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
using System.Collections.Generic;

using Sql.Tables;

namespace Sql.Tests {

	[TestClass]
	public class NDateTimeTest {

		[TestInitialize()]
		public void Init() {

			using(Transaction transaction = new Transaction(DB.TestDB)) {

				Tables.NDateTimeTable.Table table = Tables.NDateTimeTable.Table.INSTANCE;

				Query.Delete(table).NoWhereCondition.Execute(transaction);

				transaction.Commit();
			}
		}

		private readonly DateTime mDateTime1 = new DateTime(2010, 12, 31, 23, 59, 59, 10);
		private readonly DateTime mDateTime2 = new DateTime(2011, 10, 31, 23, 59, 59, 25);

		[TestMethod]
		public void Test_01() {

			Tables.NDateTimeTable.Table table = Tables.NDateTimeTable.Table.INSTANCE;

			using(Transaction transaction = new Transaction(DB.TestDB)) {

				IResult insertResult = Query.Insert(table)
					.Set(table.Id, Guid.NewGuid())
					.Set(table.Dt, mDateTime1)
					.Execute(transaction);

				Assert.AreEqual(1, insertResult.RowsEffected);
				transaction.Commit();
			}

			IResult result = Query.Select(table.Dt).From(table).Where(table.Dt == mDateTime1).Execute(DB.TestDB);
			Assert.AreEqual(1, result.Count);
			Assert.AreEqual(mDateTime1, table[0, result].Dt);

			result = Query.Select(table.Dt).From(table).Where(table.Dt != mDateTime1).Execute(DB.TestDB);
			Assert.AreEqual(0, result.Count);

			DateTime before = mDateTime1.AddMilliseconds(-100);
			DateTime after = mDateTime1.AddMilliseconds(100);

			result = Query.Select(table.Dt).From(table).Where(table.Dt > before & table.Dt < after).Execute(DB.TestDB);
			Assert.AreEqual(1, result.Count);
			Assert.AreEqual(mDateTime1, table[0, result].Dt);

			result = Query.Select(table.Dt).From(table).Where(table.Dt >= mDateTime1 & table.Dt <= mDateTime1).Execute(DB.TestDB);
			Assert.AreEqual(1, result.Count);
			Assert.AreEqual(mDateTime1, table[0, result].Dt);
		}

		[TestMethod]
		public void Test_02() {

			Tables.NDateTimeTable.Table table = Tables.NDateTimeTable.Table.INSTANCE;

			using(Transaction transaction = new Transaction(DB.TestDB)) {

				IResult insertResult = Query.Insert(table)
					.Set(table.Id, Guid.NewGuid())
					.Set(table.Dt, mDateTime1)
					.Execute(transaction);

				Assert.AreEqual(1, insertResult.RowsEffected);

				insertResult = Query.Insert(table)
					.Set(table.Id, Guid.NewGuid())
					.Set(table.Dt, mDateTime2)
					.Execute(transaction);

				Assert.AreEqual(1, insertResult.RowsEffected);
				transaction.Commit();
			}

			IResult result = Query.Select(table.Dt).From(table).Where(table.Dt == mDateTime1 | table.Dt == mDateTime2).Execute(DB.TestDB);
			Assert.AreEqual(2, result.Count);

			Tables.NDateTimeTable.Table table2 = new Tables.NDateTimeTable.Table();

			result = Query.Select(table.Id, table2.Dt)
				.From(table)
				.Join(table2, table.Dt == table2.Dt)
				.Execute(DB.TestDB);

			Assert.AreEqual(2, result.Count);
		}

		[TestMethod]
		public void Test_03() {

			Tables.NDateTimeTable.Table table = Tables.NDateTimeTable.Table.INSTANCE;

			using(Transaction transaction = new Transaction(DB.TestDB)) {

				Tables.NDateTimeTable.Row row1 = new Sql.Tables.NDateTimeTable.Row();
				row1.Id = Guid.NewGuid();
				row1.Dt = mDateTime1;

				Tables.NDateTimeTable.Row row2 = new Sql.Tables.NDateTimeTable.Row();
				row2.Id = Guid.NewGuid();
				row2.Dt = mDateTime2;

				row1.Update(transaction);
				row2.Update(transaction);

				transaction.Commit();
			}

			IResult result = Query.Select(table.Dt).From(table).Where(table.Dt == mDateTime1 | table.Dt == mDateTime2).Execute(DB.TestDB);
			Assert.AreEqual(2, result.Count);

			result = Query.Select(table.Id)
				.From(table)
				.Where(table.Dt.In(mDateTime1, mDateTime2))
				.Execute(DB.TestDB);

			Assert.AreEqual(2, result.Count);

			List<DateTime> list = new List<DateTime>();
			list.Add(mDateTime1);
			list.Add(mDateTime2);

			result = Query.Select(table.Id)
				.From(table)
				.Where(table.Dt.In(list))
				.Execute(DB.TestDB);

			Assert.AreEqual(2, result.Count);
		}

		[TestMethod]
		public void Test_04() {

			Tables.NDateTimeTable.Table table = Tables.NDateTimeTable.Table.INSTANCE;

			using(Transaction transaction = new Transaction(DB.TestDB)) {

				IResult insertResult = Query.Insert(table)
					.Set(table.Id, Guid.NewGuid())
					.Set(table.Dt, (DateTime?)null)
					.Execute(transaction);

				Assert.AreEqual(1, insertResult.RowsEffected);
				transaction.Commit();
			}

			IResult result = Query.Select(table.Dt).From(table).Where(table.Dt.IsNull).Execute(DB.TestDB);
			Assert.AreEqual(1, result.Count);
			Assert.IsNull(table[0, result].Dt);

			result = Query.Select(table.Dt).From(table).Where(table.Dt.IsNotNull).Execute(DB.TestDB);
			Assert.AreEqual(0, result.Count);
		}

		[TestMethod]
		public void Test_05() {

			Tables.NDateTimeTable.Table table = Tables.NDateTimeTable.Table.INSTANCE;

			using(Transaction transaction = new Transaction(DB.TestDB)) {

				Tables.NDateTimeTable.Row row1 = new Sql.Tables.NDateTimeTable.Row();
				row1.Id = Guid.NewGuid();
				row1.Dt = mDateTime1;

				Tables.NDateTimeTable.Row row2 = new Sql.Tables.NDateTimeTable.Row();
				row2.Id = Guid.NewGuid();
				row2.Dt = mDateTime1;

				row1.Update(transaction);
				row2.Update(transaction);

				transaction.Commit();
			}

			IResult result = Query.Select(table.Dt).From(table).Where(table.Dt == mDateTime1).Execute(DB.TestDB);
			Assert.AreEqual(2, result.Count);

			result = Query.Select(table.Dt).Top(1).From(table).Where(table.Dt == mDateTime1).Execute(DB.TestDB);
			Assert.AreEqual(1, result.Count);

			result = Query.Select(table.Dt).Distinct.Top(1).From(table).Where(table.Dt == mDateTime1).Execute(DB.TestDB);
			Assert.AreEqual(1, result.Count);

			Function.Count count = new Sql.Function.Count();

			result = Query.Select(table.Dt, count)
				.From(table)
				.GroupBy(table.Dt)
				.Execute(DB.TestDB);

			Assert.AreEqual(1, result.Count);
			Assert.AreEqual(mDateTime1, table[0, result].Dt);
			Assert.AreEqual(2, count[0, result]!.Value);

			result = Query.Select(table.Dt, count)
				.From(table)
				.GroupBy(table.Dt)
				.Having(count > 1)
				.Execute(DB.TestDB);

			Assert.AreEqual(1, result.Count);
			Assert.AreEqual(mDateTime1, table[0, result].Dt);
			Assert.AreEqual(2, count[0, result]!.Value);

			result = Query.Select(table.Dt, count)
				.From(table)
				.GroupBy(table.Dt)
				.Having(count > 2)
				.Execute(DB.TestDB);

			Assert.AreEqual(0, result.Count);
		}

		[TestMethod]
		public void Test_06() {

			Tables.NDateTimeTable.Table table = Tables.NDateTimeTable.Table.INSTANCE;

			using(Transaction transaction = new Transaction(DB.TestDB)) {

				Tables.NDateTimeTable.Row row1 = new Sql.Tables.NDateTimeTable.Row();
				row1.Id = Guid.NewGuid();
				row1.Dt = mDateTime1;

				Tables.NDateTimeTable.Row row2 = new Sql.Tables.NDateTimeTable.Row();
				row2.Id = Guid.NewGuid();
				row2.Dt = mDateTime2;

				row1.Update(transaction);
				row2.Update(transaction);

				transaction.Commit();
			}

			IResult result = Query.Select(table).From(table).Execute(DB.TestDB);
			Assert.AreEqual(2, result.Count);

			result = Query.Select(table).From(table).Where(table.Dt.In(mDateTime1, mDateTime2)).Execute(DB.TestDB);
			Assert.AreEqual(2, result.Count);

			using(Transaction transaction = new Transaction(DB.TestDB)) {

				for(int index = 0; index < result.Count; index++) {
					Tables.NDateTimeTable.Row row = table[index, result];

					row.Dt = DateTime.Now;
					row.Update(transaction);
				}
				transaction.Commit();
			}
			result = Query.Select(table).From(table).Where(table.Dt.NotIn(mDateTime1, mDateTime2)).Execute(DB.TestDB);
			Assert.AreEqual(2, result.Count);

			using(Transaction transaction = new Transaction(DB.TestDB)) {

				for(int index = 0; index < result.Count; index++) {
					Tables.NDateTimeTable.Row row = table[index, result];
					row.Delete();
					row.Update(transaction);
				}
				transaction.Commit();
			}
			result = Query.Select(table).From(table).Execute(DB.TestDB);
			Assert.AreEqual(0, result.Count);
		}

		[TestMethod]
		public void Test_07() {

			Tables.NDateTimeTable.Table table = Tables.NDateTimeTable.Table.INSTANCE;

			using(Transaction transaction = new Transaction(DB.TestDB)) {

				Sql.Query.Insert(table)
					.Set(table.Id, Guid.NewGuid())
					.Set(table.Dt, new Function.CurrentDateTime(DB.TestDB.DatabaseType))
					.Execute(transaction);

				transaction.Commit();
			}

			IResult result = Query.Select(table.Dt).From(table).Execute(DB.TestDB);
			Assert.AreEqual(1, result.Count);

			using(Transaction transaction = new Transaction(DB.TestDB)) {

				Sql.Query.Update(table)
					.Set(table.Dt, new Function.CurrentDateTime(DB.TestDB.DatabaseType))
					.NoWhereCondition()
					.Execute(transaction);

				transaction.Commit();
			}
		}

		[TestMethod]
		public void TestsWithoutParameters() {

			Settings.UseParameters = false;

			try {
				Test_01();
				Init();
				Test_02();
				Init();
				Test_03();
				Init();
				Test_04();
				Init();
				Test_05();
				Init();
				Test_06();
				Init();
				Test_07();
			}
			finally {
				Settings.UseParameters = true;
			}
		}
	}
}