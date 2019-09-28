
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
using Sql.Tables;

namespace Sql.Tests {

	[TestClass]
	public class RollbackTest {

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

			Tables.StringTable.Table stringTable = Tables.StringTable.Table.INSTANCE;

			if(true) {
				Sql.Function.Count count = new Sql.Function.Count();

				Sql.IResult result = Sql.Query.Select(count).From(stringTable).Execute(DB.TestDB);

				Assert.AreEqual(result.Count, 1);
				Assert.AreEqual(count[0, result]!.Value, 0);
			}

			using(Sql.Transaction transaction = new Transaction(DB.TestDB)) {

				for(int index = 0; index < 100; index++) {

					Tables.StringTable.Row row = new Sql.Tables.StringTable.Row();
					row.Str = index.ToString();
					row.Update(transaction);
				}

				Sql.Function.Count count = new Sql.Function.Count();

				Sql.IResult result = Sql.Query.Select(count).From(stringTable).Execute(transaction);

				Assert.AreEqual(result.Count, 1);
				Assert.AreEqual(count[0, result]!.Value, 100);

				result = Sql.Query.Select(stringTable).From(stringTable).Execute(transaction);

				for(int index = 0; index < result.Count; index++) {

					string str = stringTable[index, result].Str;

					Sql.Query.Update(stringTable)
						.Set(stringTable.Str, str + "abc")
						.Where(stringTable.Str == str)
						.Execute(transaction);
				}
			}

			if(true) {
				Sql.Function.Count count = new Sql.Function.Count();

				Sql.IResult result = Sql.Query.Select(count).From(stringTable).Execute(DB.TestDB);

				Assert.AreEqual(result.Count, 1);
				Assert.AreEqual(count[0, result]!.Value, 0);
			}
		}

		[TestMethod]
		public void Test_02() {

			Tables.StringTable.Table stringTable = Tables.StringTable.Table.INSTANCE;

			if(true) {
				Sql.Function.Count count = new Sql.Function.Count();

				Sql.IResult result = Sql.Query.Select(count).From(stringTable).Execute(DB.TestDB);

				Assert.AreEqual(result.Count, 1);
				Assert.AreEqual(count[0, result]!.Value, 0);
			}

			using(Sql.Transaction transaction = new Transaction(DB.TestDB)) {

				for(int index = 0; index < 100; index++) {

					Sql.Query.Insert(stringTable)
						.Set(stringTable.Str, index.ToString())
						.Execute(transaction);
				}

				Sql.Function.Count count = new Sql.Function.Count();

				Sql.IResult result = Sql.Query.Select(count).From(stringTable).Execute(transaction);

				Assert.AreEqual(result.Count, 1);
				Assert.AreEqual(count[0, result]!.Value, 100);

				result = Sql.Query.Select(stringTable).From(stringTable).Execute(transaction);

				for(int index = 0; index < result.Count; index++) {

					string str = stringTable[index, result].Str;

					Sql.Query.Update(stringTable)
						.Set(stringTable.Str, str + "abc")
						.Where(stringTable.Str == str)
						.Execute(transaction);
				}
			}

			if(true) {
				Sql.Function.Count count = new Sql.Function.Count();

				Sql.IResult result = Sql.Query.Select(count).From(stringTable).Execute(DB.TestDB);

				Assert.AreEqual(result.Count, 1);
				Assert.AreEqual(count[0, result]!.Value, 0);
			}
		}

		[TestMethod]
		public void Test_03() {

			System.Data.SqlClient.SqlConnection.ClearAllPools();

			using(Transaction transaction = new Transaction(DB.TestDB, System.Data.IsolationLevel.Serializable)) {

				Tables.StringTable.Table stringTable = Tables.StringTable.Table.INSTANCE;

				Sql.Query.Select(stringTable).From(stringTable).Execute(transaction);

				transaction.Commit();
			}

			using(System.Data.Common.DbConnection connection = DB.TestDB.GetConnection(false)) {

				using(System.Data.Common.DbCommand command = connection.CreateCommand()) {

					if(DB.TestDB.DatabaseType == DatabaseType.Mssql) {

						command.CommandText = "DBCC useroptions";

						using(System.Data.Common.DbDataReader reader = command.ExecuteReader()) {

							int count = 0;
							while(reader.Read()) {

								string value = reader.GetString(0);
								string value2 = reader.GetString(1);
								count++;

								if(count == 12) {
									Assert.AreEqual(value2, "read committed");
									continue;
								}
							}
						}
					}
					else if(DB.TestDB.DatabaseType == DatabaseType.PostgreSql) {

						command.CommandText = "SHOW TRANSACTION ISOLATION LEVEL";

						using(System.Data.Common.DbDataReader reader = command.ExecuteReader()) {

							while(reader.Read()) {
								string value = reader.GetString(0);
								Assert.AreEqual(value, "read committed");
							}
						}
					}
					else
						throw new Exception("Unknown database type");
				}
			}

			using(Transaction transaction = new Transaction(DB.TestDB, System.Data.IsolationLevel.RepeatableRead)) {

				Tables.StringTable.Table stringTable = Tables.StringTable.Table.INSTANCE;

				Sql.Query.Select(stringTable).From(stringTable).Execute(transaction);

				transaction.Rollback();
			}

			using(System.Data.Common.DbConnection connection = DB.TestDB.GetConnection(false)) {

				using(System.Data.Common.DbCommand command = connection.CreateCommand()) {

					if(DB.TestDB.DatabaseType == DatabaseType.Mssql) {
						command.CommandText = "DBCC useroptions";

						using(System.Data.Common.DbDataReader reader = command.ExecuteReader()) {

							int count = 0;
							while(reader.Read()) {

								string value = reader.GetString(0);
								string value2 = reader.GetString(1);
								count++;

								if(count == 12) {
									Assert.AreEqual(value2, "read committed");
									continue;
								}
							}
						}
					}
					else if(DB.TestDB.DatabaseType == DatabaseType.PostgreSql) {

						command.CommandText = "SHOW TRANSACTION ISOLATION LEVEL";

						using(System.Data.Common.DbDataReader reader = command.ExecuteReader()) {

							while(reader.Read()) {
								string value = reader.GetString(0);
								Assert.AreEqual(value, "read committed");
							}
						}
					}
					else
						throw new Exception("Unknown database type");
				}
			}
		}

        [TestMethod]
        public void Test_04() {

            Transaction transaction = new Transaction(DB.TestDB);

            try {
                using(transaction) {
                    Query.ExecuteNonQuery("lakjdhflakjsdfha", transaction);
                    Assert.Fail("Exception was not thrown as expected");
                }
            }
            catch { }

            Assert.IsTrue(transaction.ConnectionState == System.Data.ConnectionState.Closed);
        }

        [TestMethod]
        public void Test_05() {

            if(DB.TestDB.DatabaseType != DatabaseType.Mssql) {
                return;
            }

            Tables.StringTable.Table stringTable = Tables.StringTable.Table.INSTANCE;

            using(Transaction transaction = new Transaction(DB.TestDB)) {

                for(int index = 0; index < 100; index++) {

                    Tables.StringTable.Row row = new Tables.StringTable.Row();

                    row.Str = index.ToString();
                    row.Update(transaction);
                }
                transaction.Commit();
            }

            using(var connection = DB.TestDB.GetConnection(false)) {

                using(var transaction = connection.BeginTransaction()) {

                    using(var command = connection.CreateCommand()) {

                        command.CommandText = Query
                            .Select(stringTable.Str)
                            .From(stringTable, "UPDLOCK, READPAST")
                            .GetSql(DB.TestDB);

                        command.Transaction = transaction;

                        using(var reader = command.ExecuteReader()) {

                            while(reader.Read()) {

                                string value = reader.GetString(reader.GetOrdinal(stringTable.Str.ColumnName));
                            }
                        }
                    }
                    transaction.Commit();
                }
            }
        }
    }
}