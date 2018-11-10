
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
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Sql.Tables;

namespace Sql.Tests {

	[TestClass]
	public class GuidTests {

		[TestInitialize()]
		public void Init() {

			using(Transaction transaction = new Transaction(DB.TestDB)) {

				Tables.GuidTable.Table table = Tables.GuidTable.Table.INSTANCE;

				Query.Delete(table).NoWhereCondition.Execute(transaction);

				transaction.Commit();
			}
		}

		[TestMethod]
		public void Test_01() {

			using(Transaction transaction = new Transaction(DB.TestDB)) {

				Tables.GuidTable.Row row = new Tables.GuidTable.Row();

				row.Id = Guid.NewGuid();
				row.Update(transaction);

				Tables.GuidTable.Table table = Tables.GuidTable.Table.INSTANCE;

				IResult result = Query.Select(table.Id).From(table).Execute(transaction);

				Assert.AreEqual(1, result.Count);
				Assert.AreEqual(row.Id, table[0, result].Id);

				transaction.Commit();
			}
		}

		[TestMethod]
		public void Test_02() {

			Guid id = Guid.NewGuid();

			using(Transaction transaction = new Transaction(DB.TestDB)) {

				Tables.GuidTable.Row insertRow = new Tables.GuidTable.Row();

				insertRow.Id = id;
				insertRow.Update(transaction);

				transaction.Commit();
			}

			Tables.GuidTable.Table table = Tables.GuidTable.Table.INSTANCE;

			IResult result = Query.Select(table.Id).From(table).Execute(DB.TestDB);

			Assert.AreEqual(1, result.Count);

			Tables.GuidTable.Row row = table[0, result];

			Assert.AreEqual(id, row.Id);

			row.Delete();

			using(Transaction transaction = new Transaction(DB.TestDB)) {
				row.Update(transaction);
				transaction.Commit();
			}

			result = Query.Select(table.Id).From(table).Execute(DB.TestDB);
			Assert.AreEqual(0, result.Count);
		}

		[TestMethod]
		public void Test_03() {

			Guid id = Guid.NewGuid();

			using(Transaction transaction = new Transaction(DB.TestDB)) {

				Tables.GuidTable.Row row = new Tables.GuidTable.Row();

				row.Id = id;
				row.Update(transaction);

				Tables.GuidTable.Table table = Tables.GuidTable.Table.INSTANCE;

				IResult result = Query.Select(table.Id).From(table).Where(table.Id == id).Execute(transaction);

				Assert.AreEqual(1, result.Count);
				Assert.AreEqual(id, table[0, result].Id);

				result = Query.Select(table.Id).From(table).Where(table.Id != id).Execute(transaction);
				Assert.AreEqual(0, result.Count);

				result = Query.Select(table.Id).From(table).Where(table.Id.In(id)).Execute(transaction);

				Assert.AreEqual(1, result.Count);
				Assert.AreEqual(id, table[0, result].Id);

				result = Query.Select(table.Id).From(table).Where(table.Id.NotIn(id)).Execute(transaction);
				Assert.AreEqual(0, result.Count);

				transaction.Commit();
			}
		}

		[TestMethod]
		public void Test_04() {

			Guid id = Guid.NewGuid();

			using(Transaction transaction = new Transaction(DB.TestDB)) {

				Tables.GuidTable.Row row = new Tables.GuidTable.Row();

				row.Id = id;
				row.Update(transaction);

				Tables.GuidTable.Table table = Tables.GuidTable.Table.INSTANCE;

				IResult result = Query.Select(table.Id).From(table).Where(table.Id == id).Execute(transaction);

				Assert.AreEqual(1, result.Count);
				Assert.AreEqual(id, table[0, result].Id);

				Guid id2 = Guid.NewGuid();

				IResult updateResult = Query.Update(table).Set(table.Id, id2).Where(table.Id != id).Execute(transaction);
				Assert.AreEqual(0, updateResult.RowsEffected);

				updateResult = Query.Update(table).Set(table.Id, id2).Where(table.Id == id).Execute(transaction);
				Assert.AreEqual(1, updateResult.RowsEffected);

				result = Query.Select(table.Id).From(table).Execute(transaction);

				Assert.AreEqual(1, result.Count);
				Assert.AreEqual(id2, table[0, result].Id);

				transaction.Commit();
			}
		}

		[TestMethod]
		public void Test_05() {

			using(Transaction transaction = new Transaction(DB.TestDB)) {

				Guid id = Guid.NewGuid();
				Guid id2 = Guid.NewGuid();

				Tables.GuidTable.Row row = new Tables.GuidTable.Row();

				row.Id = id;
				row.Update(transaction);

				Tables.GuidTable.Table table = Tables.GuidTable.Table.INSTANCE;

				IResult result = Query.Select(table.Id).From(table).Where(table.Id == id).Execute(transaction);

				Assert.AreEqual(1, result.Count);
				Assert.AreEqual(id, table[0, result].Id);

				Tables.GuidTable.Row row2 = new Tables.GuidTable.Row();

				row2.Id = id2;
				row2.Update(transaction);

				result = Query.Select(table.Id).From(table).Where(table.Id == id | table.Id == id2).Execute(transaction);
				Assert.AreEqual(2, result.Count);

				result = Query.Select(table.Id).From(table).Execute(transaction);
				Assert.AreEqual(2, result.Count);

				result = Query.Select(table.Id).From(table).Where(table.Id.In(id, id2)).Execute(transaction);
				Assert.AreEqual(2, result.Count);

				transaction.Commit();
			}
		}

		[TestMethod]
		public void Test_06() {

			using(Transaction transaction = new Transaction(DB.TestDB)) {

				Guid id = Guid.NewGuid();

				Tables.GuidTable.Row row = new Tables.GuidTable.Row();

				row.Id = id;
				row.Update(transaction);

				Tables.GuidTable.Table table = Tables.GuidTable.Table.INSTANCE;

				IResult result = Query.Select(table.Id).From(table).Where(table.Id == id).Execute(transaction);

				Assert.AreEqual(1, result.Count);
				Assert.AreEqual(id, table[0, result].Id);

				IResult deleteResult = Query.Delete(table).NoWhereCondition.Execute(transaction);
				Assert.AreEqual(1, deleteResult.RowsEffected);

				result = Query.Select(table.Id).From(table).Execute(transaction);
				Assert.AreEqual(0, result.Count);

				transaction.Commit();
			}
		}

		[TestMethod]
		public void Test_07() {

			using(Transaction transaction = new Transaction(DB.TestDB)) {

				Guid id = Guid.NewGuid();
				Guid id2 = Guid.NewGuid();

				Tables.GuidTable.Table table = Tables.GuidTable.Table.INSTANCE;

				IResult insertResult = Query.Insert(table).Set(table.Id, id).Execute(transaction);
				Assert.AreEqual(1, insertResult.RowsEffected);

				insertResult = Query.Insert(table).Set(table.Id, id2).Execute(transaction);
				Assert.AreEqual(1, insertResult.RowsEffected);

				IResult result = Query.Select(table.Id).From(table).Execute(transaction);
				Assert.AreEqual(2, result.Count);

				for(int index = 0; index < result.Count; index++) {
					Tables.GuidTable.Row row = table[index, result];
					Assert.IsTrue(row.Id == id || row.Id == id2);
				}

				Tables.GuidTable.Table table2 = new Tables.GuidTable.Table();

				result = Query.Select(table.Id, table2.Id)
					.From(table)
					.Join(table2, table.Id == table2.Id)
					.Execute(transaction);

				Assert.AreEqual(2, result.Count);

				for(int index = 0; index < result.Count; index++) {
					Tables.GuidTable.Row row1 = table[index, result];
					Tables.GuidTable.Row row2 = table2[index, result];
					Assert.IsTrue(row1 != row2);
					Assert.IsTrue(row1.Id == id || row1.Id == id2);
					Assert.IsTrue(row2.Id == id || row2.Id == id2);
					Assert.IsTrue(row1.Id == row2.Id);
				}

				result = Query.Select(table.Id, table2.Id)
					.From(table)
					.LeftJoin(table2, table.Id == table2.Id)
					.Execute(transaction);

				Assert.AreEqual(2, result.Count);

				for(int index = 0; index < result.Count; index++) {
					Tables.GuidTable.Row row1 = table[index, result];
					Tables.GuidTable.Row row2 = table2[index, result];
					Assert.IsTrue(row1 != row2);
					Assert.IsTrue(row1.Id == id || row1.Id == id2);
					Assert.IsTrue(row2.Id == id || row2.Id == id2);
					Assert.IsTrue(row1.Id == row2.Id);
				}

				result = Query.Select(table.Id, table2.Id)
					.From(table)
					.LeftJoin(table2, table.Id == Guid.NewGuid())   //left join that returns null
					.Execute(transaction);

				Assert.AreEqual(2, result.Count);

				for(int index = 0; index < result.Count; index++) {
					Tables.GuidTable.Row row1 = table[index, result];
					Tables.GuidTable.Row row2 = table2[index, result];
					Assert.IsTrue(row1 != row2);

					Assert.AreEqual(true, row2.IsRowNull());
				}

				result = Query.Select(table.Id, table2.Id)
					.From(table)
					.RightJoin(table2, table.Id == table2.Id)
					.Execute(transaction);

				Assert.AreEqual(2, result.Count);

				for(int index = 0; index < result.Count; index++) {
					Tables.GuidTable.Row row1 = table[index, result];
					Tables.GuidTable.Row row2 = table2[index, result];
					Assert.IsTrue(row1 != row2);
					Assert.IsTrue(row1.Id == id || row1.Id == id2);
					Assert.IsTrue(row2.Id == id || row2.Id == id2);
					Assert.IsTrue(row1.Id == row2.Id);
				}

				result = Query.Select(table.Id, table2.Id)
					.From(table)
					.LeftJoin(table2, table.Id != table2.Id)
					.Execute(transaction);

				Assert.AreEqual(2, result.Count);

				for(int index = 0; index < result.Count; index++) {
					Tables.GuidTable.Row row1 = table[index, result];
					Tables.GuidTable.Row row2 = table2[index, result];
					Assert.IsTrue(row1 != row2);
					Assert.IsTrue(row1.Id == id || row1.Id == id2);
					Assert.IsTrue(row2.Id == id || row2.Id == id2);
					Assert.IsTrue(row1.Id != row2.Id);
				}

				result = Query.Select(table.Id)
					.From(table)
					.Where(table.Id.In(
						Query.Select(table2.Id).From(table2))
						  )
					.Execute(transaction);

				Assert.AreEqual(2, result.Count);

				for(int index = 0; index < result.Count; index++) {
					Tables.GuidTable.Row row1 = table[index, result];
					Assert.IsTrue(row1.Id == id || row1.Id == id2);
				}

				transaction.Commit();
			}
		}

		[TestMethod]
		public void Test_08() {

			using(Transaction transaction = new Transaction(DB.TestDB)) {

				Guid id = Guid.NewGuid();

				Tables.GuidTable.Row row = new Tables.GuidTable.Row();

				row.Id = id;
				row.Update(transaction);

				Tables.GuidTable.Table table = Tables.GuidTable.Table.INSTANCE;

				IResult result = Query.Select(table.Id).From(table).Where(table.Id.IsNotNull).Execute(transaction);

				Assert.AreEqual(1, result.Count);
				Assert.AreEqual(id, table[0, result].Id);

				result = Query.Select(table.Id).From(table).Where(table.Id.IsNull).Execute(transaction);
				Assert.AreEqual(0, result.Count);

				transaction.Commit();
			}
		}

		[TestMethod]
		public void Test_09() {

			using(Transaction transaction = new Transaction(DB.TestDB)) {

				Guid id = Guid.NewGuid();
				Guid id2 = Guid.NewGuid();

				Tables.GuidTable.Table table = Tables.GuidTable.Table.INSTANCE;

				IResult insertResult = Query.Insert(table).Set(table.Id, id).Execute(transaction);
				Assert.AreEqual(1, insertResult.RowsEffected);

				insertResult = Query.Insert(table).Set(table.Id, id2).Execute(transaction);
				Assert.AreEqual(1, insertResult.RowsEffected);

				IResult result = Query.Select(table.Id).From(table).Execute(transaction);
				Assert.AreEqual(2, result.Count);

				result = Query.Select(table.Id).From(table).OrderBy(table.Id.ASC).Execute(transaction);
				Assert.AreEqual(2, result.Count);

				Guid first = table[0, result].Id;
				Guid second = table[1, result].Id;

				Assert.IsTrue(first != second);

				result = Query.Select(table.Id).From(table).OrderBy(table.Id.DESC).Execute(transaction);
				Assert.AreEqual(2, result.Count);

				Assert.AreEqual(first, table[1, result].Id);
				Assert.AreEqual(second, table[0, result].Id);

				transaction.Commit();
			}
		}

		[TestMethod]
		public void Test_10() {

			using(Transaction transaction = new Transaction(DB.TestDB)) {

				Guid id = Guid.NewGuid();

				Tables.GuidTable.Row row = new Tables.GuidTable.Row();

				row.Id = id;
				row.Update(transaction);

				Tables.GuidTable.Table table = Tables.GuidTable.Table.INSTANCE;

				IResult result = Query.Select(table.Id).From(table).Where(table.Id == id).Execute(transaction);

				row = table[0, result];

				Assert.AreEqual(1, result.Count);
				Assert.AreEqual(id, row.Id);

				row.Delete();
				row.Update(transaction);

				result = Query.Select(table.Id).From(table).Where(table.Id == id).Execute(transaction);
				Assert.AreEqual(0, result.Count);

				transaction.Commit();
			}
		}


		[TestMethod]
		public void Test_11() {

			using(Transaction transaction = new Transaction(DB.TestDB)) {

				Guid id = Guid.NewGuid();

				Tables.GuidTable.Row row = new Tables.GuidTable.Row();

				row.Id = id;
				row.Update(transaction);

				Tables.GuidTable.Table table = Tables.GuidTable.Table.INSTANCE;

				IResult result = Query.Select(table.Id).From(table).Where(table.Id == id).Execute(transaction);

				row = table[0, result];

				Assert.AreEqual(1, result.Count);
				Assert.AreEqual(id, row.Id);

				row.Delete();
				row.Update(transaction);

				bool exceptionThrown = false;

				try {
					Guid getId = row.Id;
				}
				catch(Exception ex) {
					if(!ex.Message.StartsWith("Cannot access a columns data when is deleted"))
						throw ex;
					exceptionThrown = true;
				}
				Assert.IsTrue(exceptionThrown);

				transaction.Commit();
			}
		}

		[TestMethod]
		public void Test_12() {

			const int rows = 10;

			using(Transaction transaction = new Transaction(DB.TestDB)) {

				for(int index = 0; index < rows; index++) {
					Tables.GuidTable.Row insertRow = new Tables.GuidTable.Row();

					insertRow.Id = Guid.NewGuid();
					insertRow.Update(transaction);
				}
				transaction.Commit();
			}

			Tables.GuidTable.Table table = Tables.GuidTable.Table.INSTANCE;

			IResult result = Query.Select(table).From(table).Execute(DB.TestDB);
			Assert.AreEqual(rows, result.Count);

			IList<Guid> idList = SqlHelper.ToList(result, table.Id);
			Assert.AreEqual(rows, idList.Count);

			Guid lastGuid = Guid.Empty;

			for(int index = 0; index < rows; index++) {

				Tables.GuidTable.Row row = table[index, result];

				Assert.IsTrue(row.Id != Guid.Empty);

				if(lastGuid != Guid.Empty)
					Assert.AreNotEqual(lastGuid, row.Id);

				lastGuid = row.Id;
			}
		}

		[TestMethod]
		public void Test_13() {

			Settings.UseParameters = true;

			try {
				const int rows = 10;

				using(Transaction transaction = new Transaction(DB.TestDB)) {

					for(int index = 0; index < rows; index++) {
						Tables.GuidTable.Row insertRow = new Tables.GuidTable.Row();

						insertRow.Id = Guid.NewGuid();
						insertRow.Update(transaction);
					}
					transaction.Commit();
				}

				Tables.GuidTable.Table table = Tables.GuidTable.Table.INSTANCE;

				Guid guid = Guid.NewGuid();

				//Check that only one parameter is used for 5 of the same values
				IResult result = Query.Select(table).From(table).Where(table.Id.In(guid, guid, guid, guid, guid)).Execute(DB.TestDB);
				Assert.AreEqual(0, result.Count);
				Assert.IsTrue(!result.SqlQuery.Contains("@2"));
				Assert.IsTrue(!result.SqlQuery.Contains("@3"));
				Assert.IsTrue(!result.SqlQuery.Contains("@4"));
				Assert.IsTrue(!result.SqlQuery.Contains("@5"));
			}
			finally {
				Settings.UseParameters = false;
			}
		}

		[TestMethod]
		public void Test_14() {

			Settings.UseParameters = true;

			try {

				const int rows = 10;

				Tables.GuidTable.Table table = Tables.GuidTable.Table.INSTANCE;

				using(Transaction transaction = new Transaction(DB.TestDB)) {

					for(int index = 0; index < rows; index++) {

						Guid id = Guid.NewGuid();

						Sql.Query.Insert(table)
							.Set(table.Id, id)
							.UseParameters(false)
							.Execute(transaction);

						//Check that only one parameter is used for 5 of the same values
						IResult result = Query.Select(table).From(table).Where(table.Id.In(id, id, id, id, id)).UseParameters(false).Execute(transaction);

						Assert.AreEqual(1, result.Count);
						Assert.IsTrue(!result.SqlQuery.Contains("@1"));
						Assert.IsTrue(!result.SqlQuery.Contains("@2"));
						Assert.IsTrue(!result.SqlQuery.Contains("@3"));
						Assert.IsTrue(!result.SqlQuery.Contains("@4"));
						Assert.IsTrue(!result.SqlQuery.Contains("@5"));

						Sql.Query.Update(table)
							.Set(table.Id, Guid.NewGuid())
							.Where(table.Id == id)
							.UseParameters(false)
							.Execute(transaction);

						Sql.Query.Delete(table)
							.Where(table.Id == id)
							.UseParameters(false)
							.Execute(transaction);
					}
					transaction.Commit();
				}
			}
			finally {
				Settings.UseParameters = false;
			}
		}


		[TestMethod]
		public void Test_15() {

			const int rows = 10;

			using(Transaction transaction = new Transaction(DB.TestDB)) {

				for(int index = 0; index < rows; index++) {
					Tables.GuidTable.Row insertRow = new Tables.GuidTable.Row();

					insertRow.Id = Guid.NewGuid();
					insertRow.Update(transaction);
				}
				transaction.Commit();
			}

			Tables.GuidTable.Table table = Tables.GuidTable.Table.INSTANCE;

			Sql.Function.Count count = new Sql.Function.Count();

			Sql.IResult result =
				Sql.Query.Select(count)
				.From(table)
				.Execute(DB.TestDB);

			Assert.AreEqual(rows, count[0, result].Value);
		}

		[TestMethod]
		public void Test_16() {

		}

		[TestMethod]
		public void Test_17() {

		}

		private void CheckExists(Guid pId, Sql.Transaction pTransaction) {

			Tables.GuidTable.Table guidTable = Tables.GuidTable.Table.INSTANCE;

			Sql.IResult result = Sql.Query.Select(guidTable.Id)
				.From(guidTable)
				.Where(guidTable.Id == pId)
				.Execute(pTransaction);

			Assert.AreEqual(1, result.Count);
			Assert.AreEqual(pId, guidTable[0, result].Id);
		}

		[TestMethod]
		public void Test_18() {

			const int rows = 10;

			using(Transaction transaction = new Transaction(DB.TestDB)) {

				for(int index = 0; index < rows; index++) {

					Tables.GuidTable.Row insertRow = new Tables.GuidTable.Row();

					Assert.IsTrue(insertRow.RowState == Sql.ARow.RowStateEnum.AddPending);

					Guid id = Guid.NewGuid();

					insertRow.Id = id;
					insertRow.Update(transaction);

					Assert.IsTrue(insertRow.RowState == Sql.ARow.RowStateEnum.AddPerformedNotYetCommitted);
					CheckExists(id, transaction);
				}
				transaction.Commit();
			}

			Tables.GuidTable.Table table = Tables.GuidTable.Table.INSTANCE;

			Sql.Function.Count count = new Sql.Function.Count();

			Sql.IResult result =
				Sql.Query.Select(count)
				.From(table)
				.Execute(DB.TestDB);

			int value = count[0, result].Value;
			Assert.AreEqual(10, value);
		}

		[TestMethod]
		public void Test_19() {

			for(int index = 0; index < 10; index++) {

				Tables.GuidTable.Row row = new Sql.Tables.GuidTable.Row();

				using(Transaction transaction = new Transaction(DB.TestDB)) {

					Guid id = Guid.NewGuid();

					row.Id = Guid.NewGuid();
					row.Id = Guid.NewGuid();
					row.Id = Guid.NewGuid();
					row.Id = Guid.NewGuid();
					row.Id = id;

					Assert.IsTrue(row.RowState == Sql.ARow.RowStateEnum.AddPending);

					row.Update(transaction);

					Assert.IsTrue(row.RowState == Sql.ARow.RowStateEnum.AddPerformedNotYetCommitted);

					transaction.Rollback();

					Assert.IsTrue(row.RowState == Sql.ARow.RowStateEnum.AddPending);

					Assert.AreEqual(row.Id, id);
				}
			}

			if(true) {

				Guid id = Guid.NewGuid();

				using(Transaction transaction = new Transaction(DB.TestDB)) {

					Tables.GuidTable.Row row = new Sql.Tables.GuidTable.Row();

					row.Id = Guid.NewGuid();
					row.Id = Guid.NewGuid();
					row.Id = Guid.NewGuid();
					row.Id = Guid.NewGuid();
					row.Id = id;

					Assert.IsTrue(row.RowState == Sql.ARow.RowStateEnum.AddPending);

					row.Update(transaction);
					Assert.IsTrue(row.RowState == Sql.ARow.RowStateEnum.AddPerformedNotYetCommitted);
					Assert.AreEqual(row.Id, id);

					transaction.Commit();

					Assert.IsTrue(row.RowState == Sql.ARow.RowStateEnum.Exists);
					Assert.AreEqual(row.Id, id);
				}

				Tables.GuidTable.Table guidTable = Tables.GuidTable.Table.INSTANCE;

				Sql.IResult result = Sql.Query.Select(guidTable).From(guidTable).Execute(DB.TestDB);

				Assert.AreEqual(1, result.Count);

				Tables.GuidTable.Row selectedRow = guidTable[0, result];

				Assert.AreEqual(id, selectedRow.Id);
				Assert.IsTrue(selectedRow.RowState == Sql.ARow.RowStateEnum.Exists);

				using(Transaction transaction = new Transaction(DB.TestDB)) {

					Assert.IsTrue(selectedRow.RowState == Sql.ARow.RowStateEnum.Exists);

					selectedRow.Delete();

					Assert.IsTrue(selectedRow.RowState == Sql.ARow.RowStateEnum.DeletePending);

					selectedRow.Update(transaction);

					Assert.IsTrue(selectedRow.RowState == Sql.ARow.RowStateEnum.DeletePerformedNotYetCommitted);

					transaction.Rollback();

					Assert.IsTrue(selectedRow.RowState == Sql.ARow.RowStateEnum.DeletePending);
				}

				using(Transaction transaction = new Transaction(DB.TestDB)) {

					Assert.IsTrue(selectedRow.RowState == Sql.ARow.RowStateEnum.DeletePending);

					selectedRow.Delete();

					Assert.IsTrue(selectedRow.RowState == Sql.ARow.RowStateEnum.DeletePending);

					selectedRow.Update(transaction);

					Assert.IsTrue(selectedRow.RowState == Sql.ARow.RowStateEnum.DeletePerformedNotYetCommitted);

					transaction.Commit();

					Assert.IsTrue(selectedRow.RowState == Sql.ARow.RowStateEnum.DeletedAndCommitted);
				}
			}

			if(true) {

				Tables.GuidTable.Row row = new Sql.Tables.GuidTable.Row();

				Guid id = Guid.NewGuid();

				using(Transaction transaction = new Transaction(DB.TestDB)) {

					row.Id = id;

					Assert.IsTrue(row.RowState == Sql.ARow.RowStateEnum.AddPending);

					row.Update(transaction);

					Assert.IsTrue(row.RowState == Sql.ARow.RowStateEnum.AddPerformedNotYetCommitted);

					transaction.Rollback();
					Assert.IsTrue(row.RowState == Sql.ARow.RowStateEnum.AddPending);
				}
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
				Init();
				Test_06();
				Init();
				Test_07();
				Init();
				Test_08();
				Init();
				Test_09();
				Init();
				Test_10();
				Init();
				Test_11();
				Init();
				Test_12();
				Init();
				Test_13();
				Init();
				Test_14();
				Init();
				Test_15();
				Init();
				Test_16();
				Init();
				Test_17();
				Init();
				Test_18();
				Init();
				Test_19();
			}
			finally {
				Settings.UseParameters = true;
			}
		}
	}
}