
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
	public class FloatTest {

		[TestInitialize()]
		public void Init() {

			using(Transaction transaction = new Transaction(DB.TestDB)) {

				Tables.FloatTable.Table table = Tables.FloatTable.Table.INSTANCE;

				Query.Delete(table).NoWhereCondition.Execute(transaction);

				transaction.Commit();
			}
		}

		private float mFloat1 = 25.250f;

		[TestMethod]
		public void Test_01() {

			using(Transaction transaction = new Transaction(DB.TestDB)) {

				Tables.FloatTable.Row row = new Sql.Tables.FloatTable.Row();

				row.Id = Guid.NewGuid();
				row.Value += mFloat1;   //+= tests the default values on a new row
				row.Update(transaction);

				transaction.Commit();
			}

			Tables.FloatTable.Table table = Tables.FloatTable.Table.INSTANCE;

			IResult result = Query.Select(table.Value).From(table).Execute(DB.TestDB);
			Assert.AreEqual(1, result.Count);
			Assert.AreEqual(mFloat1, table[0, result].Value);

			result = Query.Select(table.Value).From(table).Where(table.Value == mFloat1).Execute(DB.TestDB);
			Assert.AreEqual(1, result.Count);
			Assert.AreEqual(mFloat1, table[0, result].Value);

			result = Query.Select(table.Value).From(table).Where(table.Value != mFloat1).Execute(DB.TestDB);
			Assert.AreEqual(0, result.Count);

			result = Query.Select(table.Value).From(table).Where(table.Value > 0).Execute(DB.TestDB);
			Assert.AreEqual(1, result.Count);
			Assert.AreEqual(mFloat1, table[0, result].Value);

			result = Query.Select(table.Value).From(table).Where(table.Value < 0).Execute(DB.TestDB);
			Assert.AreEqual(0, result.Count);

			result = Query.Select(table.Value).From(table).Where(table.Value <= mFloat1).Execute(DB.TestDB);
			Assert.AreEqual(1, result.Count);
			Assert.AreEqual(mFloat1, table[0, result].Value);

			result = Query.Select(table.Value).From(table).Where(table.Value >= mFloat1).Execute(DB.TestDB);
			Assert.AreEqual(1, result.Count);
			Assert.AreEqual(mFloat1, table[0, result].Value);

			result = Query.Select(table.Value).From(table).Where(table.Value <= (mFloat1 - 1f)).Execute(DB.TestDB);
			Assert.AreEqual(0, result.Count);

			result = Query.Select(table.Value).From(table).Where(table.Value >= (mFloat1 + 1f)).Execute(DB.TestDB);
			Assert.AreEqual(0, result.Count);

			result = Query.Select(table.Value).From(table).Where(table.Value.In(mFloat1, 5f)).Execute(DB.TestDB);
			Assert.AreEqual(1, result.Count);
			Assert.AreEqual(mFloat1, table[0, result].Value);

			result = Query.Select(table.Value).From(table).Where(table.Value.NotIn(mFloat1, 5f)).Execute(DB.TestDB);
			Assert.AreEqual(0, result.Count);

			System.Collections.Generic.List<float> list = new System.Collections.Generic.List<float>();
			list.Add(mFloat1);

			result = Query.Select(table.Value).From(table).Where(table.Value.In(list)).Execute(DB.TestDB);
			Assert.AreEqual(1, result.Count);
			Assert.AreEqual(mFloat1, table[0, result].Value);

			result = Query.Select(table.Value).From(table).Where(table.Value.NotIn(list)).Execute(DB.TestDB);
			Assert.AreEqual(0, result.Count);

			Tables.FloatTable.Table table2 = new Sql.Tables.FloatTable.Table();

			result = Query.Select(table, table2)
				.From(table)
				.Join(table2, table.Value == table2.Value)
				.Execute(DB.TestDB);

			Assert.AreEqual(1, result.Count);
			Assert.AreEqual(mFloat1, table[0, result].Value);
			Assert.AreEqual(mFloat1, table2[0, result].Value);

			result = Query.Select(table, table2)
				.From(table)
				.Join(table2, table.Value != table2.Value)
				.Execute(DB.TestDB);

			Assert.AreEqual(0, result.Count);

			result = Query.Select(table, table2)
				.From(table)
				.Join(table2, table.Value >= table2.Value)
				.Execute(DB.TestDB);

			Assert.AreEqual(1, result.Count);
			Assert.AreEqual(mFloat1, table[0, result].Value);
			Assert.AreEqual(mFloat1, table2[0, result].Value);

			result = Query.Select(table, table2)
				.From(table)
				.Join(table2, table.Value <= table2.Value)
				.Execute(DB.TestDB);

			Assert.AreEqual(1, result.Count);
			Assert.AreEqual(mFloat1, table[0, result].Value);
			Assert.AreEqual(mFloat1, table2[0, result].Value);

			result = Query.Select(table, table2)
				.From(table)
				.Join(table2, table.Value < table2.Value)
				.Execute(DB.TestDB);

			Assert.AreEqual(0, result.Count);

			result = Query.Select(table, table2)
				.From(table)
				.Join(table2, table.Value > table2.Value)
				.Execute(DB.TestDB);

			Assert.AreEqual(0, result.Count);

			result = Query.Select(table)
				.From(table)
				.Where(table.Value.In(Query.Select(table2.Value).Distinct.From(table2)))
				.Execute(DB.TestDB);

			Assert.AreEqual(1, result.Count);
			Assert.AreEqual(mFloat1, table[0, result].Value);

			result = Query.Select(table)
				.From(table)
				.Where(table.Value.NotIn(Query.Select(table2.Value).Distinct.From(table2)))
				.Execute(DB.TestDB);

			Assert.AreEqual(0, result.Count);

			result = Query.Select(table)
				.From(table)
				.Where(table.Value + 5f > 2f)
				.Execute(DB.TestDB);

			return;
		}

		[TestMethod]
		public void Test_02() {

			using(Transaction transaction = new Transaction(DB.TestDB)) {

				Tables.FloatTable.Row row = new Sql.Tables.FloatTable.Row();

				row.Id = Guid.NewGuid();
				row.Value = 1f;
				row.NValue = mFloat1;
				row.Update(transaction);

				transaction.Commit();
			}

			Tables.FloatTable.Table table = Tables.FloatTable.Table.INSTANCE;

			IResult result = Query.Select(table.NValue).From(table).Execute(DB.TestDB);
			Assert.AreEqual(1, result.Count);
			Assert.AreEqual(mFloat1, table[0, result].NValue);

			result = Query.Select(table.NValue).From(table).Where(table.NValue == mFloat1).Execute(DB.TestDB);
			Assert.AreEqual(1, result.Count);
			Assert.AreEqual(mFloat1, table[0, result].NValue);

			result = Query.Select(table.NValue).From(table).Where(table.NValue != mFloat1).Execute(DB.TestDB);
			Assert.AreEqual(0, result.Count);

			result = Query.Select(table.NValue).From(table).Where(table.NValue > 0).Execute(DB.TestDB);
			Assert.AreEqual(1, result.Count);
			Assert.AreEqual(mFloat1, table[0, result].NValue);

			result = Query.Select(table.NValue).From(table).Where(table.NValue < 0).Execute(DB.TestDB);
			Assert.AreEqual(0, result.Count);

			result = Query.Select(table.NValue).From(table).Where(table.NValue <= mFloat1).Execute(DB.TestDB);
			Assert.AreEqual(1, result.Count);
			Assert.AreEqual(mFloat1, table[0, result].NValue);

			result = Query.Select(table.NValue).From(table).Where(table.NValue >= mFloat1).Execute(DB.TestDB);
			Assert.AreEqual(1, result.Count);
			Assert.AreEqual(mFloat1, table[0, result].NValue);

			result = Query.Select(table.NValue).From(table).Where(table.NValue <= (mFloat1 - 1f)).Execute(DB.TestDB);
			Assert.AreEqual(0, result.Count);

			result = Query.Select(table.NValue).From(table).Where(table.NValue >= (mFloat1 + 1f)).Execute(DB.TestDB);
			Assert.AreEqual(0, result.Count);

			result = Query.Select(table.NValue).From(table).Where(table.NValue.In(mFloat1, 5f)).Execute(DB.TestDB);
			Assert.AreEqual(1, result.Count);
			Assert.AreEqual(mFloat1, table[0, result].NValue);

			result = Query.Select(table.NValue).From(table).Where(table.NValue.NotIn(mFloat1, 5f)).Execute(DB.TestDB);
			Assert.AreEqual(0, result.Count);

			System.Collections.Generic.List<float> list = new System.Collections.Generic.List<float>();
			list.Add(mFloat1);

			result = Query.Select(table.NValue).From(table).Where(table.NValue.In(list)).Execute(DB.TestDB);
			Assert.AreEqual(1, result.Count);
			Assert.AreEqual(mFloat1, table[0, result].NValue);

			result = Query.Select(table.NValue).From(table).Where(table.NValue.NotIn(list)).Execute(DB.TestDB);
			Assert.AreEqual(0, result.Count);

			Tables.FloatTable.Table table2 = new Sql.Tables.FloatTable.Table();

			result = Query.Select(table, table2)
				.From(table)
				.Join(table2, table.NValue == table2.NValue)
				.Execute(DB.TestDB);

			Assert.AreEqual(1, result.Count);
			Assert.AreEqual(mFloat1, table[0, result].NValue);
			Assert.AreEqual(mFloat1, table2[0, result].NValue);

			result = Query.Select(table, table2)
				.From(table)
				.Join(table2, table.NValue != table2.NValue)
				.Execute(DB.TestDB);

			Assert.AreEqual(0, result.Count);

			result = Query.Select(table, table2)
				.From(table)
				.Join(table2, table.NValue >= table2.NValue)
				.Execute(DB.TestDB);

			Assert.AreEqual(1, result.Count);
			Assert.AreEqual(mFloat1, table[0, result].NValue);
			Assert.AreEqual(mFloat1, table2[0, result].NValue);

			result = Query.Select(table, table2)
				.From(table)
				.Join(table2, table.NValue <= table2.NValue)
				.Execute(DB.TestDB);

			Assert.AreEqual(1, result.Count);
			Assert.AreEqual(mFloat1, table[0, result].NValue);
			Assert.AreEqual(mFloat1, table2[0, result].NValue);

			result = Query.Select(table, table2)
				.From(table)
				.Join(table2, table.NValue < table2.NValue)
				.Execute(DB.TestDB);

			Assert.AreEqual(0, result.Count);

			result = Query.Select(table, table2)
				.From(table)
				.Join(table2, table.NValue > table2.NValue)
				.Execute(DB.TestDB);

			Assert.AreEqual(0, result.Count);

			result = Query.Select(table)
				.From(table)
				.Where(table.NValue.In(Query.Select(table2.NValue).Distinct.From(table2)))
				.Execute(DB.TestDB);

			Assert.AreEqual(1, result.Count);
			Assert.AreEqual(mFloat1, table[0, result].NValue);

			result = Query.Select(table)
				.From(table)
				.Where(table.NValue.NotIn(Query.Select(table2.NValue).Distinct.From(table2)))
				.Execute(DB.TestDB);

			Assert.AreEqual(0, result.Count);

			result = Query.Select(table)
				.From(table)
				.Where(table.NValue + 5f > 2f)
				.Execute(DB.TestDB);

			return;
		}

		[TestMethod]
		public void ParametersTurnedOff() {

			Settings.UseParameters = false;

			try {
				Init();
				Test_01();
				Init();
				Test_02();
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
			}
			finally {
				Settings.UseParameters = true;
			}
		}
	}
}