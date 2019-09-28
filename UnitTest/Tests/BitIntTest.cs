
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
	public class BigIntegerTest {

		private bool mRunOnce = false;

		[TestInitialize()]
		public void Init() {

			using(Transaction transaction = new Transaction(DB.TestDB)) {

				Tables.BigIntTable.Table table = Tables.BigIntTable.Table.INSTANCE;

				Query.Delete(table).NoWhereCondition.Execute(transaction);

				if(!mRunOnce) {
					if(DB.TestDB.DatabaseType == DatabaseType.Mssql) {
						Query.ExecuteNonQuery("DBCC CHECKIDENT (" + table.TableName + ", RESEED, 1)", transaction);
					}
					else if(DB.TestDB.DatabaseType == DatabaseType.PostgreSql) {
						Query.ExecuteNonQuery("ALTER SEQUENCE auto_id_seq RESTART WITH 1;", transaction);
					}
					mRunOnce = true;
				}

				transaction.Commit();
			}
		}

		private Int64 mInt1 = 25;

		[TestMethod]
		public void Test_01() {

			Int64? a = null;

			a += 10;

			string? b = null;
			b += "abc";

			int? c = null;
			c += 10;

			using(Transaction transaction = new Transaction(DB.TestDB)) {

				Tables.BigIntTable.Row row = new Sql.Tables.BigIntTable.Row();

				row.IntValue = mInt1;       //+= tests the default values on a new row

				row.Update(transaction);

				transaction.Commit();
			}

			Tables.BigIntTable.Table table = Tables.BigIntTable.Table.INSTANCE;

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

			System.Collections.Generic.List<Int64> list = new System.Collections.Generic.List<Int64>();
			list.Add(mInt1);

			result = Query.Select(table.IntValue).From(table).Where(table.IntValue.In(list)).Execute(DB.TestDB);
			Assert.AreEqual(1, result.Count);
			Assert.AreEqual(mInt1, table[0, result].IntValue);

			result = Query.Select(table.IntValue).From(table).Where(table.IntValue.NotIn(list)).Execute(DB.TestDB);
			Assert.AreEqual(0, result.Count);

			Tables.BigIntTable.Table table2 = new Sql.Tables.BigIntTable.Table();

			result = Query.Select(table, table2)
				.From(table)
				.Join(table2, table.IntValue == table2.IntValue)
				.Execute(DB.TestDB);

			Assert.AreEqual(1, result.Count);
			Assert.AreEqual(mInt1, table[0, result].IntValue);

			//			Assert.IsTrue(table[0, result] == table2[0, result]);

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

			const int insertRows = 1000;

			using(Transaction transaction = new Transaction(DB.TestDB)) {

				for(int index = 0; index < insertRows; index++) {
					Tables.BigIntTable.Row row = new Sql.Tables.BigIntTable.Row();

					row.IntValue = 12345;
					row.Update(transaction);
				}
				transaction.Commit();
			}

			Tables.BigIntTable.Table table = Tables.BigIntTable.Table.INSTANCE;

			IResult result = Sql.Query.Select(table).From(table).Execute(DB.TestDB);

			int bytes = result.GetDataSetSizeInBytes();

			Assert.AreEqual(2 * (insertRows * 8), bytes);
		}

		[TestMethod]
		public void Test_03() {

			const int insertRows = 1000;

			decimal sumValue = 0;

			using(Transaction transaction = new Transaction(DB.TestDB)) {

				for(int index = 0; index < insertRows; index++) {
					Tables.BigIntTable.Row row = new Sql.Tables.BigIntTable.Row();
					row.IntValue = index;
					row.Update(transaction);
					sumValue += index;
				}
				transaction.Commit();
			}

			Tables.BigIntTable.Table table = Tables.BigIntTable.Table.INSTANCE;

			Sql.Function.MaxBigInteger max = new Sql.Function.MaxBigInteger(table.IntValue);
			Sql.Function.MinBigInteger min = new Sql.Function.MinBigInteger(table.IntValue);
			Sql.Function.SumBigInteger sum = new Sql.Function.SumBigInteger(table.IntValue);
			Sql.Function.AvgBigInteger avg = new Sql.Function.AvgBigInteger(table.IntValue);

			Sql.IResult result = Sql.Query
				.Select(max, min, sum, avg)
				.From(table)
				.ExecuteUncommitted(DB.TestDB);

			Assert.AreEqual(1, result.Count);
			Assert.AreEqual(insertRows - 1, max[0, result]!.Value);
			Assert.AreEqual(0, min[0, result]);
			Assert.IsTrue(sumValue == sum[0, result]);

			if(DB.TestDB.DatabaseType == DatabaseType.PostgreSql)
				Assert.AreEqual(sumValue / insertRows, avg[0, result]);
			else if(DB.TestDB.DatabaseType == DatabaseType.Mssql)   //Sql server AVG(...) function outputs a whole number where as postgreSql does not
				Assert.AreEqual(((int)sumValue) / insertRows, avg[0, result]);
			else
				Assert.IsTrue(false);
		}

		[TestMethod]
		public void Test_04() {

			using(Transaction transaction = new Transaction(DB.TestDB)) {
				Tables.BigIntTable.Row row = new Sql.Tables.BigIntTable.Row();
				row.IntValue = 10;
				row.Update(transaction);
				transaction.Commit();
			}

			Tables.BigIntTable.Table table = Tables.BigIntTable.Table.INSTANCE;

			Sql.Function.MaxBigInteger max = new Sql.Function.MaxBigInteger(table.IntValue);
			Sql.Function.MinBigInteger min = new Sql.Function.MinBigInteger(table.IntValue);
			Sql.Function.SumBigInteger sum = new Sql.Function.SumBigInteger(table.IntValue);
			Sql.Function.AvgBigInteger avg = new Sql.Function.AvgBigInteger(table.IntValue);

			Sql.IResult result = Sql.Query
				.Select(table)
				.From(table)
				.Where(table.IntValue % 1 == 0)
				.ExecuteUncommitted(DB.TestDB);

			Assert.AreEqual(1, result.Count);

			result = Sql.Query
				.Select(table)
				.From(table)
				.Where(table.IntValue % 2 == 0)
				.ExecuteUncommitted(DB.TestDB);

			Assert.AreEqual(1, result.Count);

			result = Sql.Query
				.Select(table)
				.From(table)
				.Where(table.IntValue % 3 == 1)
				.ExecuteUncommitted(DB.TestDB);

			Assert.AreEqual(1, result.Count);

			result = Sql.Query
				.Select(table)
				.From(table)
				.Where(table.IntValue % 3 == 0)
				.ExecuteUncommitted(DB.TestDB);

			Assert.AreEqual(0, result.Count);

			result = Sql.Query
				.Select(table)
				.From(table)
				.Where(table.IntValue % 4 == 2)
				.ExecuteUncommitted(DB.TestDB);

			Assert.AreEqual(1, result.Count);

			result = Sql.Query
				.Select(table)
				.From(table)
				.Where(table.IntValue % 5 == 0)
				.ExecuteUncommitted(DB.TestDB);

			Assert.AreEqual(1, result.Count);
		}

		[TestMethod]
		public void ParametersTurnedOff() {

			Settings.UseParameters = false;
			try {
				Init();
				Test_01();
				Init();
				Test_02();
				Init();
				Test_03();
				Init();
				Test_04();
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
				Init();
				Test_02();
				Init();
				Test_03();
				Init();
				Test_04();
			}
			finally {
				Settings.UseParameters = true;
			}
		}
	}
}