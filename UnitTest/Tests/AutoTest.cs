
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

namespace Sql.Tests {

	[TestClass]
	public class AutoTest {

		private bool mRunOnce = false;

		[TestInitialize()]
		public void Init() {

			using(Transaction transaction = new Transaction(DB.TestDB)) {

				Tables.AutoTable.Table table = Tables.AutoTable.Table.INSTANCE;

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

		[TestMethod]
		public void Test_01() {

			int id = -1;
			string text = "Text";

			using(Transaction transaction = new Transaction(DB.TestDB)) {

				Tables.AutoTable.Row row = new Sql.Tables.AutoTable.Row();
				row.Text = text;
				row.Update(transaction);

				transaction.Commit();

				id = row.Id;
			}

			Tables.AutoTable.Table autoTable = Tables.AutoTable.Table.INSTANCE;

			IResult result = Query.Select(autoTable.Id, autoTable.Text)
										.From(autoTable)
										.Where(autoTable.Id == id)
										.Execute(DB.TestDB);

			Assert.AreEqual(1, result.Count);
			Assert.AreEqual(id, autoTable[0, result].Id);
			Assert.AreEqual(text, autoTable[0, result].Text);
		}

		[TestMethod]
		public void Test_02() {

			using(Transaction transaction = new Transaction(DB.TestDB)) {

				Tables.AutoTable.Row row = new Sql.Tables.AutoTable.Row();
				row.Text = "Text";
				row.Update(transaction);

				int id = row.Id;

				transaction.Commit();
			}
		}

		[TestMethod]
		public void Test_03() {
			Tables.AutoTable.Row row = new Sql.Tables.AutoTable.Row();
			try {
				int id = row.Id;
			}
			catch(Exception e) {
				if(e.Message != "Auto id on column 'Id' has not been set. Row probably hasn't been persisted to database")
					throw e;
				return;
			}
			Assert.IsTrue(false);
		}

		[TestMethod]
		public void Test_04() {

			Tables.AutoTable.Row row = new Sql.Tables.AutoTable.Row();

			row.Text = "Text";

			for(int index = 0; index < 5; index++) {

				int id = -1;

				using(Transaction transaction = new Transaction(DB.TestDB)) {

					row.Text = "Text";
					row.Update(transaction);

					id = row.Id;
				}   //Does a rollback

				try {
					id = row.Id;
				}
				catch(Exception e) {
					if(e.Message != "Auto id on column 'Id' has not been set. Row probably hasn't been persisted to database")
						throw e;
					continue;
				}
				Assert.IsTrue(false);
			}
		}

		[TestMethod]
		public void Test_05() {

			Tables.AutoTable.Table table = Tables.AutoTable.Table.INSTANCE;

			using(Transaction transaction = new Transaction(DB.TestDB)) {

				IResult result = Sql.Query.Insert(table)
					.Set(table.Text, "Text")
					.Returning(table.Id)
					.Execute(transaction);

				int id = table[0, result].Id;
				transaction.Commit();
			}
		}

		[TestMethod]
		public void Test_06() {

			Tables.AutoTable.Table table = Tables.AutoTable.Table.INSTANCE;

			using(Transaction transaction = new Transaction(DB.TestDB)) {

				IResult result = Sql.Query.Insert(table)
					.Set(table.Text, "Text")
					.Returning(table.Id, table.Text)
					.Execute(transaction);

				Assert.IsTrue(result.RowsEffected == 1);

				int id = table[0, result].Id;
				string text = table[0, result].Text;

				Assert.IsTrue(id > 0);
				Assert.AreEqual("Text", text);

				result = Sql.Query.Update(table)
					.Set(table.Text, "Text_123")
					.Where(table.Id == id)
					.Returning(table.Text)
					.Execute(transaction);

				Assert.IsTrue(result.Count == 1);
				text = table[0, result].Text;

				Assert.AreEqual("Text_123", text);

				result = Sql.Query.Delete(table).Where(table.Id == id).Returning(table.Id, table.Text).Execute(transaction);

				Assert.AreEqual(1, result.RowsEffected);

				id = table[0, result].Id;
				text = table[0, result].Text;

				Assert.AreEqual("Text_123", text);

				transaction.Commit();
			}
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
				Init();
				Test_05();
				Init();
				Test_06();
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
				Init();
				Test_05();
				Init();
				Test_06();
			}
			finally {
				Settings.UseParameters = true;
			}
		}
	}
}