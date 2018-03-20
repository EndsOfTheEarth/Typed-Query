
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
	public class ForceThreadTest {

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

			Tables.StringTable.Table table = Tables.StringTable.Table.INSTANCE;

			using(Transaction transaction = new Transaction(DB.TestDB, System.Data.IsolationLevel.ReadCommitted, true)) {

				Tables.StringTable.Row row = new Sql.Tables.StringTable.Row();

				row.Str = "a";
				row.Update(transaction);

				bool exceptionThrown = false;

				try {
					Sql.Query.Select(table)
						.From(table)
						.ExecuteUncommitted(DB.TestDB);
				}
				catch(Exception e) {
					Assert.AreEqual("Cannot create connection on this thread as it is being forced to use another transaction", e.Message);
					exceptionThrown = true;
				}
				Assert.IsTrue(exceptionThrown);
			}

			Sql.IResult result = Sql.Query
				.Select(table)
				.From(table)
				.ExecuteUncommitted(DB.TestDB);

			Assert.AreEqual(0, result.Count);

			using(Transaction transaction = new Transaction(DB.TestDB)) {

				Tables.StringTable.Row row = new Sql.Tables.StringTable.Row();

				row.Str = "a";
				row.Update(transaction);

				result = Sql.Query.Select(table)
					.From(table)
					.ExecuteUncommitted(DB.TestDB);

				//Sql server and postgreSql have different locking models so they return different results
				if(DB.TestDB.DatabaseType == DatabaseType.Mssql)
					Assert.AreEqual(1, result.Count);
				else if(DB.TestDB.DatabaseType == DatabaseType.PostgreSql)
					Assert.AreEqual(0, result.Count);
				else
					throw new Exception("Unknown database type");

				transaction.Commit();
			}
			result = Sql.Query
				.Select(table)
				.From(table)
				.ExecuteUncommitted(DB.TestDB);

			Assert.AreEqual(1, result.Count);
		}
	}
}