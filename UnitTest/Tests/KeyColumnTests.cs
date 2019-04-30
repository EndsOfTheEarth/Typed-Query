
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
using Sql.Types;

namespace Sql.Tests {

	public class KeyColumnTests {

		[TestInitialize()]
		public void Init() {

			using(Sql.Transaction transaction = new Transaction(DB.TestDB)) {

				{

					Sql.Tables.Person.Table personTable = Sql.Tables.Person.Table.INSTANCE;
					Sql.Tables.OrderLog.Table orderLogTable = Sql.Tables.OrderLog.Table.INSTANCE;

					Sql.Query.Delete(orderLogTable).NoWhereCondition.Execute(transaction);
					Sql.Query.Delete(personTable).NoWhereCondition.Execute(transaction);
				}

				{

					Sql.Tables.PersonId.Table personIdTable = Sql.Tables.PersonId.Table.INSTANCE;
					Sql.Tables.OrderLogId.Table orderLogIdTable = Sql.Tables.OrderLogId.Table.INSTANCE;

					Sql.Query.Delete(orderLogIdTable).NoWhereCondition.Execute(transaction);
					Sql.Query.Delete(personIdTable).NoWhereCondition.Execute(transaction);
				}

				transaction.Commit();
			}
		}

		[TestMethod]
		public void Test_01() {

			GuidKey<Tables.Person.Table> personKey = new GuidKey<Tables.Person.Table>(Guid.NewGuid());

			using(Sql.Transaction transaction = new Transaction(DB.TestDB)) {

				Sql.Tables.Person.Row personRow = new Sql.Tables.Person.Row();

				personRow.Key = personKey;
				personRow.FirstName = "first name";
				personRow.Surname = "surname";

				personRow.Update(transaction);

				Sql.Tables.OrderLog.Row orderRow = new Sql.Tables.OrderLog.Row();
				orderRow.Key = Guid.NewGuid();
				orderRow.PersonKey = personRow.Key;
				orderRow.Item = "Item Details";

				orderRow.Update(transaction);
				transaction.Commit();
			}

			Sql.Tables.Person.Table personTable = Sql.Tables.Person.Table.INSTANCE;
			Sql.Tables.OrderLog.Table orderLogTable = Sql.Tables.OrderLog.Table.INSTANCE;

			Sql.IResult result = Sql.Query.Select(personTable, orderLogTable)
				.From(orderLogTable)
				.Join(personTable, orderLogTable.PersonKey == personTable.Key)
				.Execute(DB.TestDB);

			Assert.AreEqual(result.Count, 1);

			Assert.AreEqual(personTable[0, result].Key, personKey);

			result = Sql.Query.Select(personTable)
				.From(personTable)
				.Where(personTable.Key == personKey)
				.Execute(DB.TestDB);

			Assert.AreEqual(result.Count, 1);
		}

		[TestMethod]
		public void Test_02() {

			GuidKey<Tables.Person.Table> personKey = new GuidKey<Tables.Person.Table>(Guid.NewGuid());

			Sql.Tables.Person.Table personTable = Sql.Tables.Person.Table.INSTANCE;
			Sql.Tables.OrderLog.Table orderLogTable = Sql.Tables.OrderLog.Table.INSTANCE;

			using(Sql.Transaction transaction = new Transaction(DB.TestDB)) {

				Sql.Query.Insert(personTable)
					.Set(personTable.Key, personKey)
					.Set(personTable.FirstName, "first name")
					.Set(personTable.Surname, "surname")
					.Execute(transaction);

				Sql.Query.Insert(orderLogTable)
					.Set(orderLogTable.Key, Guid.NewGuid())
					.Set(orderLogTable.PersonKey, personKey)
					.Set(orderLogTable.Item_, "Item Details")
					.Execute(transaction);

				transaction.Commit();
			}

			Sql.IResult result = Sql.Query.Select(personTable, orderLogTable)
				.From(orderLogTable)
				.Join(personTable, orderLogTable.PersonKey == personTable.Key)
				.Execute(DB.TestDB);

			Assert.AreEqual(result.Count, 1);

			Assert.AreEqual(personTable[0, result].Key, personKey);

			result = Sql.Query.Select(personTable)
				.From(personTable)
				.Where(personTable.Key == personKey)
				.Execute(DB.TestDB);

			Assert.AreEqual(result.Count, 1);

			using(Sql.Transaction transaction = new Transaction(DB.TestDB)) {

				Sql.IResult updateResult = Sql.Query
					.Update(personTable)
					.Set(personTable.FirstName, "first name a")
					.Set(personTable.Surname, "surname b")
					.NoWhereCondition()
					.Execute(transaction);

				Assert.AreEqual(updateResult.RowsEffected, 1);

				updateResult = Sql.Query
					.Update(orderLogTable)
					.Set(orderLogTable.Item_, "Item Details a")
					.NoWhereCondition()
					.Execute(transaction);

				Assert.AreEqual(updateResult.RowsEffected, 1);

				transaction.Commit();
			}

			using(Sql.Transaction transaction = new Transaction(DB.TestDB)) {

				Sql.IResult updateResult = Sql.Query
					.Update(personTable)
					.Set(personTable.FirstName, orderLogTable.Item_)
					.Set(personTable.Surname, orderLogTable.Item_)
					.Join(orderLogTable, personTable.Key == orderLogTable.PersonKey)
					.NoWhereCondition()
					.Returning(personTable.FirstName, personTable.Surname)
					.Execute(transaction);

				Assert.AreEqual(updateResult.RowsEffected, 1);

				Assert.AreEqual(personTable[0, updateResult].FirstName, "Item Details a");
				Assert.AreEqual(personTable[0, updateResult].Surname, "Item Details a");
			}
		}

		[TestMethod]
		public void Test_03() {

			Int32Key<Tables.PersonId.Table> personId = new Int32Key<Tables.PersonId.Table>(-1);

			using(Sql.Transaction transaction = new Transaction(DB.TestDB)) {

				Sql.Tables.PersonId.Row personRow = new Sql.Tables.PersonId.Row();

				personRow.FirstName = "first name";
				personRow.Surname = "surname";

				personRow.Update(transaction);

				personId = personRow.Id;

				Sql.Tables.OrderLogId.Row orderRow = new Sql.Tables.OrderLogId.Row();

				orderRow.PersonId = personId;
				orderRow.Item = "Item Details";

				orderRow.Update(transaction);
				transaction.Commit();
			}

			Sql.Tables.PersonId.Table personTable = Sql.Tables.PersonId.Table.INSTANCE;
			Sql.Tables.OrderLogId.Table orderLogTable = Sql.Tables.OrderLogId.Table.INSTANCE;

			Sql.IResult result = Sql.Query.Select(personTable, orderLogTable)
				.From(orderLogTable)
				.Join(personTable, orderLogTable.PersonId == personTable.Id)
				.Execute(DB.TestDB);

			Assert.AreEqual(result.Count, 1);

			Assert.AreEqual(personTable[0, result].Id, personId);

			Assert.IsTrue(personId.Value != -1);

			result = Sql.Query.Select(personTable)
				.From(personTable)
				.Where(personTable.Id == personId)
				.Execute(DB.TestDB);

			Assert.AreEqual(result.Count, 1);
		}

		[TestMethod]
		public void Test_04() {

			Int32Key<Tables.PersonId.Table> personId = new Int32Key<Tables.PersonId.Table>(-1);

			Sql.Tables.PersonId.Table personTable = Sql.Tables.PersonId.Table.INSTANCE;
			Sql.Tables.OrderLogId.Table orderLogTable = Sql.Tables.OrderLogId.Table.INSTANCE;

			using(Sql.Transaction transaction = new Transaction(DB.TestDB)) {

				Sql.IResult insertResult = Sql.Query.Insert(personTable)
					.Set(personTable.FirstName, "first name")
					.Set(personTable.Surname, "surname")
					.Returning(personTable.Id)
					.Execute(transaction);

				personId = personTable[0, insertResult].Id;

				Sql.Query.Insert(orderLogTable)
					.Set(orderLogTable.PersonId, personId)
					.Set(orderLogTable.Item_, "Item Details")
					.Execute(transaction);

				transaction.Commit();
			}

			Sql.IResult result = Sql.Query.Select(personTable, orderLogTable)
				.From(orderLogTable)
				.Join(personTable, orderLogTable.PersonId == personTable.Id)
				.Execute(DB.TestDB);

			Assert.AreEqual(result.Count, 1);

			Assert.AreEqual(personTable[0, result].Id, personId);

			Assert.IsTrue(personId.Value != -1);

			result = Sql.Query.Select(personTable)
				.From(personTable)
				.Where(personTable.Id == personId)
				.Execute(DB.TestDB);

			Assert.AreEqual(result.Count, 1);

			using(Sql.Transaction transaction = new Transaction(DB.TestDB)) {

				Sql.IResult updateResult = Sql.Query
					.Update(personTable)
					.Set(personTable.FirstName, "first name a")
					.Set(personTable.Surname, "surname b")
					.NoWhereCondition()
					.Execute(transaction);

				Assert.AreEqual(updateResult.RowsEffected, 1);

				updateResult = Sql.Query
					.Update(orderLogTable)
					.Set(orderLogTable.Item_, "Item Details a")
					.NoWhereCondition()
					.Execute(transaction);

				Assert.AreEqual(updateResult.RowsEffected, 1);

				transaction.Commit();
			}
		}
	}
}