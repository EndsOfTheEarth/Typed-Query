
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
	public class AutoSmallIntTest {

		private bool mRunOnce = false;

		[TestInitialize()]
		public void Init() {

			using(Transaction transaction = new Transaction(DB.TestDB)) {

				Tables.SmallIntTable.Table table = Tables.SmallIntTable.Table.INSTANCE;

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

			Int16 id = -1;

			using(Transaction transaction = new Transaction(DB.TestDB)) {

				Tables.SmallIntTable.Row row = new Sql.Tables.SmallIntTable.Row();

				row.Update(transaction);

				transaction.Commit();

				id = row.Id;
			}

			Tables.SmallIntTable.Table autoTable = Tables.SmallIntTable.Table.INSTANCE;

			IResult result = Query.Select(autoTable.Id, autoTable.IntValue)
										.From(autoTable)
										.Where(autoTable.Id == id)
										.Execute(DB.TestDB);

			Assert.AreEqual(1, result.Count);
			Assert.AreEqual(id, autoTable[0, result].Id);
			Assert.AreEqual(null, autoTable[0, result].IntValue);
		}

		[TestMethod]
		public void Test_02() {

			using(Transaction transaction = new Transaction(DB.TestDB)) {

				Tables.SmallIntTable.Row row = new Sql.Tables.SmallIntTable.Row();

				row.Update(transaction);

				Int16 id = row.Id;

				transaction.Commit();
			}
		}

		[TestMethod]
		public void Test_03() {
			Tables.SmallIntTable.Row row = new Sql.Tables.SmallIntTable.Row();
			try {
				Int16 id = row.Id;
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

			Tables.SmallIntTable.Row row = new Sql.Tables.SmallIntTable.Row();

			for(int index = 0; index < 5; index++) {

				Int16 id = -1;

				using(Transaction transaction = new Transaction(DB.TestDB)) {

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