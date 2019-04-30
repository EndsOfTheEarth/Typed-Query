
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
using Sql.Function;

namespace Sql.Tests {

	[TestClass]
	public class FunctionTest {

		[TestInitialize()]
		public void Init() {

			using(Transaction transaction = new Transaction(DB.TestDB)) {

				Query.Delete(Tables.IntTable.Table.INSTANCE).NoWhereCondition.Execute(transaction);
				Query.Delete(Tables.DecimalTable.Table.INSTANCE).NoWhereCondition.Execute(transaction);
				Query.Delete(Tables.DateTimeTable.Table.INSTANCE).NoWhereCondition.Execute(transaction);

				transaction.Commit();
			}
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
				Init();
				Test_04();
				Init();
				Test_05();
			}
			finally {
				Settings.UseParameters = true;
			}
		}

		[TestMethod]
		public void Test_01() {

			const int count = 10;

			int totalSum = 0;

			using(Transaction transaction = new Transaction(DB.TestDB)) {

				for(int index = 0; index < count; index++) {

					Tables.IntTable.Row row = new Tables.IntTable.Row();

					row.Id = Guid.NewGuid();
					row.IntValue = index;
					row.Update(transaction);
					totalSum += index;
				}
				transaction.Commit();
			}

			Tables.IntTable.Table table = Tables.IntTable.Table.INSTANCE;

			Function.MaxInt max = new Sql.Function.MaxInt(table.IntValue);

			IResult result = Query.Select(max).From(table).Execute(DB.TestDB);

			Assert.AreEqual(1, result.Count);
			Assert.AreEqual(count - 1, max[0, result]);

			result = Query.Select(max).From(table).Where(table.IntValue < 7).Execute(DB.TestDB);

			Assert.AreEqual(1, result.Count);
			Assert.AreEqual(6, max[0, result]);

			Function.MinInteger min = new Function.MinInteger(table.IntValue);

			result = Query.Select(min).From(table).Execute(DB.TestDB);

			Assert.AreEqual(1, result.Count);
			Assert.AreEqual(0, min[0, result]);

			result = Query.Select(min).From(table).Where(table.IntValue > 4).Execute(DB.TestDB);

			Assert.AreEqual(1, result.Count);
			Assert.AreEqual(5, min[0, result]);

			Function.SumInteger sum = new Function.SumInteger(table.IntValue);

			result = Query.Select(sum).From(table).Execute(DB.TestDB);

			Assert.AreEqual(1, result.Count);
			Assert.AreEqual(totalSum, sum[0, result]);
		}

		[TestMethod]
		public void Test_02() {

			const decimal count = 10;

			decimal totalSum = 0;

			using(Transaction transaction = new Transaction(DB.TestDB)) {

				for(int index = 0; index < count; index++) {

					Tables.DecimalTable.Row row = new Tables.DecimalTable.Row();

					row.DecimalValue = index;
					row.Update(transaction);
					totalSum += index;
				}
				transaction.Commit();
			}

			Tables.DecimalTable.Table table = Tables.DecimalTable.Table.INSTANCE;

			Function.MaxDecimal max = new Sql.Function.MaxDecimal(table.DecimalValue);

			IResult result = Query.Select(max).From(table).Execute(DB.TestDB);

			Assert.AreEqual(1, result.Count);
			Assert.AreEqual(count - 1, max[0, result]);

			result = Query.Select(max).From(table).Where(table.DecimalValue < 7).Execute(DB.TestDB);

			Assert.AreEqual(1, result.Count);
			Assert.AreEqual(6, max[0, result]);

			Function.MinDecimal min = new Function.MinDecimal(table.DecimalValue);

			result = Query.Select(min).From(table).Execute(DB.TestDB);

			Assert.AreEqual(1, result.Count);
			Assert.AreEqual(0, min[0, result]);

			result = Query.Select(min).From(table).Where(table.DecimalValue > 4).Execute(DB.TestDB);

			Assert.AreEqual(1, result.Count);
			Assert.AreEqual(5, min[0, result]);

			Function.AvgDecimal sum = new Function.AvgDecimal(table.DecimalValue);

			result = Query.Select(sum).From(table).Execute(DB.TestDB);

			Assert.AreEqual(1, result.Count);
			Assert.AreEqual(totalSum / count, sum[0, result]);
		}

		[TestMethod]
		public void Test_03() {

			DateTime minDate = new DateTime(2010, 01, 01);
			DateTime maxDate = new DateTime(2010, 12, 31);

			using(Transaction transaction = new Transaction(DB.TestDB)) {

				Tables.DateTimeTable.Row row = new Tables.DateTimeTable.Row();

				row.Id = Guid.NewGuid();
				row.Dt = minDate;
				row.Update(transaction);

				row = new Tables.DateTimeTable.Row();

				row.Id = Guid.NewGuid();
				row.Dt = maxDate;
				row.Update(transaction);

				transaction.Commit();
			}

			Tables.DateTimeTable.Table table = Tables.DateTimeTable.Table.INSTANCE;

			Function.MaxDateTime max = new Function.MaxDateTime(table.Dt);

			IResult result = Query.Select(max).From(table).Execute(DB.TestDB);
			Assert.AreEqual(1, result.Count);
			Assert.AreEqual(maxDate, max[0, result]);

			result = Query.Select(max).From(table).Where(table.Dt > maxDate).Execute(DB.TestDB);
			Assert.AreEqual(1, result.Count);
			Assert.AreEqual(null, max[0, result]);

			Function.MinDateTime min = new Function.MinDateTime(table.Dt);

			result = Query.Select(min).From(table).Execute(DB.TestDB);
			Assert.AreEqual(1, result.Count);
			Assert.AreEqual(minDate, min[0, result]);

			result = Query.Select(min).From(table).Where(table.Dt > maxDate).Execute(DB.TestDB);
			Assert.AreEqual(1, result.Count);
			Assert.AreEqual(null, min[0, result]);
		}

		[TestMethod]
		public void Test_04() {

			const int count = 3;

			using(Transaction transaction = new Transaction(DB.TestDB)) {

				for(int index = 0; index < count; index++) {

					Tables.IntTable.Row row = new Tables.IntTable.Row();

					row.Id = Guid.NewGuid();
					row.IntValue = index;
					row.Update(transaction);
				}
				transaction.Commit();
			}

			Function.CurrentDateTime current = new Function.CurrentDateTime(DB.TestDB.DatabaseType);

			IResult result = Query.Select(current).Top(1).From(Tables.IntTable.Table.INSTANCE).Execute(DB.TestDB);

			Assert.AreEqual(1, result.Count);
			Assert.IsNotNull(current[0, result]);
			Assert.AreEqual(DateTime.Now.Year, current[0, result].Year);
		}

		[TestMethod]
		public void Test_05() {

			DateTime minDate = new DateTime(2010, 01, 01, 05, 23, 59);
			DateTime maxDate = new DateTime(2011, 12, 31, 01, 12, 37);

			using(Transaction transaction = new Transaction(DB.TestDB)) {

				Tables.DateTimeTable.Row row = new Tables.DateTimeTable.Row();

				row.Id = Guid.NewGuid();
				row.Dt = minDate;
				row.Update(transaction);

				row = new Tables.DateTimeTable.Row();

				row.Id = Guid.NewGuid();
				row.Dt = maxDate;
				row.Update(transaction);

				transaction.Commit();
			}

			Tables.DateTimeTable.Table table = Tables.DateTimeTable.Table.INSTANCE;

			Function.DateFunction year = new Sql.Function.DateFunction(table.Dt, DatePart.Year);
			Function.DateFunction month = new Sql.Function.DateFunction(table.Dt, DatePart.Month);
			Function.DateFunction day = new Sql.Function.DateFunction(table.Dt, DatePart.DayOfMonth);
			Function.DateFunction hour = new Sql.Function.DateFunction(table.Dt, DatePart.Hour);
			Function.DateFunction minute = new Sql.Function.DateFunction(table.Dt, DatePart.Minute);
			Function.DateFunction second = new Sql.Function.DateFunction(table.Dt, DatePart.Second);

			IResult result = Query.Select(year, month, day, hour, minute, second)
				.From(table)
				.Where(year > 10)
				.GroupBy(year, month, day, hour, minute, second)
				.Having(year > 20 & year < 3000)
				.OrderBy(year, month, day, hour, minute, second)
				.Execute(DB.TestDB);

			Assert.AreEqual(2, result.Count);
			Assert.AreEqual(minDate.Year, year[0, result]);
			Assert.AreEqual(maxDate.Year, year[1, result]);

			Assert.AreEqual(minDate.Month, month[0, result]);
			Assert.AreEqual(maxDate.Month, month[1, result]);

			Assert.AreEqual(minDate.Day, day[0, result]);
			Assert.AreEqual(maxDate.Day, day[1, result]);

			Assert.AreEqual(minDate.Hour, hour[0, result]);
			Assert.AreEqual(maxDate.Hour, hour[1, result]);

			Assert.AreEqual(minDate.Minute, minute[0, result]);
			Assert.AreEqual(maxDate.Minute, minute[1, result]);

			Assert.AreEqual(minDate.Second, second[0, result]);
			Assert.AreEqual(maxDate.Second, second[1, result]);
		}
	}
}