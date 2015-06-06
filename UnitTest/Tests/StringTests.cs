
/*
 * 
 * Copyright (C) 2009-2015 JFo.nz
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
	public class StringTests {
		
		[TestInitialize()]
		public void Init() {
			
			Transaction transaction = new Transaction(DB.TestDB);
			
			try {
				
				Tables.StringTable.Table table = Tables.StringTable.Table.INSTANCE;
				
				Query.Delete(table).NoWhereCondition.Execute(transaction);
				
				transaction.Commit();
			}
			catch(Exception e){
				transaction.Rollback();
				throw e;
			}
		}
		
		[TestMethod]
		public void Test_01() {
			
			Transaction transaction = new Transaction(DB.TestDB);
			
			try {
				
				Tables.StringTable.Row row = new Tables.StringTable.Row();
				
				row.Str += "string',";
				row.Update(transaction);
				
				Tables.StringTable.Table table = Tables.StringTable.Table.INSTANCE;
				
				IResult result = Query.Select(table.Str).From(table).Execute(transaction);
				
				Assert.AreEqual(1, result.Count);
				Assert.AreEqual(row.Str, table[0, result].Str);
				
				transaction.Commit();
			}
			catch(Exception e){
				transaction.Rollback();
				throw e;
			}
		}
		
		[TestMethod]
		public void Test_02() {
			
			Transaction transaction = new Transaction(DB.TestDB);
			
			string str = "string',";
			
			try {
				
				Tables.StringTable.Row insertRow = new Tables.StringTable.Row();
				
				insertRow.Str = str;
				insertRow.Update(transaction);
				
				transaction.Commit();
			}
			catch(Exception e){
				transaction.Rollback();
				throw e;
			}
			
			Tables.StringTable.Table table = Tables.StringTable.Table.INSTANCE;
			
			IResult result = Query.Select(table.Str).From(table).Execute();
			
			Assert.AreEqual(1, result.Count);
			
			Tables.StringTable.Row row = table[0, result];
			
			Assert.AreEqual(str, row.Str);
			
			row.Delete();
			
			transaction = new Transaction(DB.TestDB);
			
			try {
				
				row.Update(transaction);				
				transaction.Commit();
			}
			catch(Exception e){
				transaction.Rollback();
				throw e;
			}
			
			result = Query.Select(table.Str).From(table).Execute();
			Assert.AreEqual(0, result.Count);
		}
		
		[TestMethod]
		public void Test_03() {
			
			Transaction transaction = new Transaction(DB.TestDB);
			
			string str = "string',";
			
			try {
				
				Tables.StringTable.Row row = new Tables.StringTable.Row();
				
				row.Str = str;
				row.Update(transaction);
				
				Tables.StringTable.Table table = Tables.StringTable.Table.INSTANCE;
				
				IResult result = Query.Select(table.Str).From(table).Where(table.Str == str).Execute(transaction);
				
				Assert.AreEqual(1, result.Count);
				Assert.AreEqual(str, table[0, result].Str);
				
				result = Query.Select(table.Str).From(table).Where(table.Str != str).Execute(transaction);
				Assert.AreEqual(0, result.Count);
				
				result = Query.Select(table.Str).From(table).Where(table.Str.In(str)).Execute(transaction);
				
				Assert.AreEqual(1, result.Count);
				Assert.AreEqual(str, table[0, result].Str);
				
				result = Query.Select(table.Str).From(table).Where(table.Str.NotIn(str)).Execute(transaction);
				Assert.AreEqual(0, result.Count);				
				
				List<string> list = new List<string>();
				list.Add(str);
				
				result = Query.Select(table.Str).From(table).Where(table.Str.In(list)).Execute(transaction);
				
				Assert.AreEqual(1, result.Count);
				Assert.AreEqual(str, table[0, result].Str);
				
				result = Query.Select(table.Str).From(table).Where(table.Str.NotIn(str)).Execute(transaction);
				Assert.AreEqual(0, result.Count);
				
				transaction.Commit();
			}
			catch(Exception e){
				transaction.Rollback();
				throw e;
			}
		}
		
		[TestMethod]
		public void Test_04() {
			
			Transaction transaction = new Transaction(DB.TestDB);
			
			string str = "string''";
			
			try {
				
				Tables.StringTable.Row row = new Tables.StringTable.Row();
				
				row.Str = str;
				row.Update(transaction);
				
				Tables.StringTable.Table table = Tables.StringTable.Table.INSTANCE;
				
				IResult result = Query.Select(table.Str).From(table).Where(table.Str == str).Execute(transaction);
				
				Assert.AreEqual(1, result.Count);
				Assert.AreEqual(str, table[0, result].Str);
				
				string str2 = "string2";
				
				IResult updateResult = Query.Update(table).Set(table.Str, str2).Where(table.Str != str).Execute(transaction);				
				Assert.AreEqual(0, updateResult.RowsEffected);
				
				updateResult = Query.Update(table).Set(table.Str, str2).Where(table.Str == str).Execute(transaction);				
				Assert.AreEqual(1, updateResult.RowsEffected);
				
				result = Query.Select(table.Str).From(table).Execute(transaction);
				
				Assert.AreEqual(1, result.Count);
				Assert.AreEqual(str2, table[0, result].Str);
				
				transaction.Commit();
			}
			catch(Exception e){
				transaction.Rollback();
				throw e;
			}
		}
		
		[TestMethod]
		public void Test_05() {
			
			Transaction transaction = new Transaction(DB.TestDB);
			
			string str = "string";
			string str2 = "string2";
			
			try {
				
				Tables.StringTable.Row row = new Tables.StringTable.Row();
				
				row.Str = str;
				row.Update(transaction);
				
				Tables.StringTable.Table table = Tables.StringTable.Table.INSTANCE;
				
				IResult result = Query.Select(table.Str).From(table).Where(table.Str == str).Execute(transaction);
				
				Assert.AreEqual(1, result.Count);
				Assert.AreEqual(str, table[0, result].Str);
				
				Tables.StringTable.Row row2 = new Tables.StringTable.Row();
				
				row2.Str = str2;				
				row2.Update(transaction);
				
				result = Query.Select(table.Str).From(table).Where(table.Str == str | table.Str == str2).Execute(transaction);				
				Assert.AreEqual(2, result.Count);
				
				result = Query.Select(table.Str).From(table).Execute(transaction);				
				Assert.AreEqual(2, result.Count);
				
				result = Query.Select(table.Str).From(table).Where(table.Str.In(str, str2)).Execute(transaction);
				Assert.AreEqual(2, result.Count);
				
				transaction.Commit();
			}
			catch(Exception e){
				transaction.Rollback();
				throw e;
			}
		}
		
		[TestMethod]
		public void Test_06() {
			
			Transaction transaction = new Transaction(DB.TestDB);
			
			string str = "string";
			
			try {
				
				Tables.StringTable.Row row = new Tables.StringTable.Row();
				
				row.Str = str;
				row.Update(transaction);
				
				Tables.StringTable.Table table = Tables.StringTable.Table.INSTANCE;
				
				IResult result = Query.Select(table.Str).From(table).Where(table.Str == str).Execute(transaction);
				
				Assert.AreEqual(1, result.Count);
				Assert.AreEqual(str, table[0, result].Str);
				
				IResult deleteResult = Query.Delete(table).NoWhereCondition.Execute(transaction);				
				Assert.AreEqual(1, deleteResult.RowsEffected);
				
				result = Query.Select(table.Str).From(table).Execute(transaction);
				Assert.AreEqual(0, result.Count);
				
				transaction.Commit();
			}
			catch(Exception e){
				transaction.Rollback();
				throw e;
			}
		}
		
		[TestMethod]
		public void Test_07() {
			
			Transaction transaction = new Transaction(DB.TestDB);
			
			string str = "string";
			string str2 = "string2";
			
			try {				
				
				Tables.StringTable.Table table = Tables.StringTable.Table.INSTANCE;
				
				IResult insertResult = Query.Insert(table).Set(table.Str, str).Execute(transaction);
				Assert.AreEqual(1, insertResult.RowsEffected);
				
				insertResult = Query.Insert(table).Set(table.Str, str2).Execute(transaction);
				Assert.AreEqual(1, insertResult.RowsEffected);
				
				IResult result = Query.Select(table.Str).From(table).Execute(transaction);
				Assert.AreEqual(2, result.Count);
				
				for(int index = 0; index < result.Count; index++){
					Tables.StringTable.Row row = table[index, result];
					Assert.IsTrue(row.Str == str || row.Str == str2);
				}
				
				Tables.StringTable.Table table2 = new Tables.StringTable.Table();
				
				result = Query.Select(table.Str, table2.Str)
								.From(table)
								.Join(table2, table.Str == table2.Str)
								.Execute(transaction);
				
				Assert.AreEqual(2, result.Count);
				
				for(int index = 0; index < result.Count; index++){
					Tables.StringTable.Row row1 = table[index, result];
					Tables.StringTable.Row row2 = table2[index, result];					
					Assert.IsTrue(row1 != row2);					
					Assert.IsTrue(row1.Str == str || row1.Str == str2);
					Assert.IsTrue(row2.Str == str || row2.Str == str2);
					Assert.IsTrue(row1.Str == row2.Str);
				}
				
				result = Query.Select(table.Str, table2.Str)
								.From(table)
								.LeftJoin(table2, table.Str == table2.Str)
								.Execute(transaction);
				
				Assert.AreEqual(2, result.Count);
				
				for(int index = 0; index < result.Count; index++){
					Tables.StringTable.Row row1 = table[index, result];
					Tables.StringTable.Row row2 = table2[index, result];					
					Assert.IsTrue(row1 != row2);					
					Assert.IsTrue(row1.Str == str || row1.Str == str2);
					Assert.IsTrue(row2.Str == str || row2.Str == str2);
					Assert.IsTrue(row1.Str == row2.Str);
				}
				
				result = Query.Select(table.Str, table2.Str)
								.From(table)
								.RightJoin(table2, table.Str == table2.Str)
								.Execute(transaction);
				
				Assert.AreEqual(2, result.Count);
				
				for(int index = 0; index < result.Count; index++){
					Tables.StringTable.Row row1 = table[index, result];
					Tables.StringTable.Row row2 = table2[index, result];					
					Assert.IsTrue(row1 != row2);					
					Assert.IsTrue(row1.Str == str || row1.Str == str2);
					Assert.IsTrue(row2.Str == str || row2.Str == str2);
					Assert.IsTrue(row1.Str == row2.Str);
				}
				
				result = Query.Select(table.Str, table2.Str)
								.From(table)
								.LeftJoin(table2, table.Str != table2.Str)
								.Execute(transaction);
				
				Assert.AreEqual(2, result.Count);
				
				for(int index = 0; index < result.Count; index++){
					Tables.StringTable.Row row1 = table[index, result];
					Tables.StringTable.Row row2 = table2[index, result];					
					Assert.IsTrue(row1 != row2);					
					Assert.IsTrue(row1.Str == str || row1.Str == str2);
					Assert.IsTrue(row2.Str == str || row2.Str == str2);
					Assert.IsTrue(row1.Str != row2.Str);
				}
				
				result = Query.Select(table.Str)
								.From(table)
								.Where(table.Str.In(
										Query.Select(table2.Str).From(table2))
								)
								.Execute(transaction);
				
				Assert.AreEqual(2, result.Count);
				
				for(int index = 0; index < result.Count; index++){
					Tables.StringTable.Row row1 = table[index, result];
					Assert.IsTrue(row1.Str == str || row1.Str == str2);
				}
				
				transaction.Commit();
			}
			catch(Exception e){
				transaction.Rollback();
				throw e;
			}
		}
		
		[TestMethod]
		public void Test_08() {
			
			Transaction transaction = new Transaction(DB.TestDB);
			
			string str = "string";
			
			try {
				
				Tables.StringTable.Row row = new Tables.StringTable.Row();
				
				row.Str = str;
				row.Update(transaction);
				
				Tables.StringTable.Table table = Tables.StringTable.Table.INSTANCE;
				
				IResult result = Query.Select(table.Str).From(table).Where(table.Str.IsNotNull).Execute(transaction);
				
				Assert.AreEqual(1, result.Count);
				Assert.AreEqual(str, table[0, result].Str);
				
				result = Query.Select(table.Str).From(table).Where(table.Str.IsNull).Execute(transaction);
				Assert.AreEqual(0, result.Count);
				
				transaction.Commit();
			}
			catch(Exception e){
				transaction.Rollback();
				throw e;
			}
		}
		
		[TestMethod]
		public void Test_09() {
			
			Transaction transaction = new Transaction(DB.TestDB);
			
			string str = "string";
			string str2 = "string2";
			
			try {				
				
				Tables.StringTable.Table table = Tables.StringTable.Table.INSTANCE;
				
				IResult insertResult = Query.Insert(table).Set(table.Str, str).Execute(transaction);
				Assert.AreEqual(1, insertResult.RowsEffected);
				
				insertResult = Query.Insert(table).Set(table.Str, str2).Execute(transaction);
				Assert.AreEqual(1, insertResult.RowsEffected);
				
				IResult result = Query.Select(table.Str).From(table).Execute(transaction);
				Assert.AreEqual(2, result.Count);
				
				result = Query.Select(table.Str).From(table).OrderBy(table.Str.ASC).Execute(transaction);
				Assert.AreEqual(2, result.Count);
				
				string first = table[0, result].Str;
				string second = table[1, result].Str;
				
				Assert.IsTrue(first != second);
				
				result = Query.Select(table.Str).From(table).OrderBy(table.Str.DESC).Execute(transaction);
				Assert.AreEqual(2, result.Count);
				
				Assert.AreEqual(first, table[1, result].Str);
				Assert.AreEqual(second, table[0, result].Str);
				
				transaction.Commit();
			}
			catch(Exception e){
				transaction.Rollback();
				throw e;
			}
		}
		
		[TestMethod]
		public void Test_10() {
			
			Transaction transaction = new Transaction(DB.TestDB);
			
			string str = "string";
			
			try {
				
				Tables.StringTable.Row row = new Tables.StringTable.Row();
				
				row.Str = str;
				row.Update(transaction);
				
				Tables.StringTable.Table table = Tables.StringTable.Table.INSTANCE;
				
				IResult result = Query.Select(table.Str).From(table).Where(table.Str == str).Execute(transaction);
				
				row = table[0, result];
				
				Assert.AreEqual(1, result.Count);
				Assert.AreEqual(str, row.Str);
				
				row.Delete();				
				row.Update(transaction);
				
				result = Query.Select(table.Str).From(table).Where(table.Str == str).Execute(transaction);
				Assert.AreEqual(0, result.Count);
				
				transaction.Commit();
			}
			catch(Exception e){
				transaction.Rollback();
				throw e;
			}
		}
		
		
		[TestMethod]
		public void Test_11() {
			
			Transaction transaction = new Transaction(DB.TestDB);
			
			string str = "string";
			
			try {
				
				Tables.StringTable.Row row = new Tables.StringTable.Row();
				
				row.Str = str;
				row.Update(transaction);
				
				Tables.StringTable.Table table = Tables.StringTable.Table.INSTANCE;
				
				IResult result = Query.Select(table.Str).From(table).Where(table.Str == str).Execute(transaction);
				
				row = table[0, result];
				
				Assert.AreEqual(1, result.Count);
				Assert.AreEqual(str, row.Str);
				
				row.Delete();				
				row.Update(transaction);
				
				bool exceptionThrown = false;
				
				try{
					string getStr = row.Str;
				}
				catch(Exception ex){
					if(!ex.Message.StartsWith("Cannot access a columns data when is deleted"))
						throw ex;
					exceptionThrown = true;
				}				
				Assert.IsTrue(exceptionThrown);
				
				transaction.Commit();
			}
			catch(Exception e){
				transaction.Rollback();
				throw e;
			}
		}
		
		[TestMethod]
		public void Test_12() {
			
			Settings.UseParameters = false;
			
			Test_07();
		}
		
		
		[TestMethod]
		public void Test_13() {
			
			Transaction transaction = new Transaction(DB.TestDB);
			
			string str = "string";
			
			try {
				
				Tables.StringTable.Row insertRow = new Tables.StringTable.Row();
				
				insertRow.Str = str;
				insertRow.Update(transaction);
				
				transaction.Commit();
			}
			catch(Exception e){
				transaction.Rollback();
				throw e;
			}
			
			Tables.StringTable.Table table = Tables.StringTable.Table.INSTANCE;
			
			IResult result = Query.Select(table.Str).From(table).Where(table.Str.Like("%" + str.ToUpper() + "%")).Execute();
			
			Assert.AreEqual(1, result.Count);
			Assert.AreEqual(str, table[0, result].Str);
			
			result = Query.Select(table.Str).From(table).Where(table.Str.NotLike("%" + str.ToUpper() + "%")).Execute();
			Assert.AreEqual(0, result.Count);		
		}
		
		[TestMethod]
		public void Test_14() {
		
			Tables.StringTable.Table table = Tables.StringTable.Table.INSTANCE;
			
			table.Str.GetHashCode();
			
			Assert.IsTrue(table.Str.ToString() != null);
			Assert.IsTrue(table.Str.DbType == System.Data.DbType.String);
		}
		
		
		[TestMethod]
		public void Test_15() {
			
			using(Transaction transaction = new Transaction(DB.TestDB)) {
				
				Tables.StringTable.Row row = new Sql.Tables.StringTable.Row();
				
				row.Str = "abc";
				row.Update(transaction);
				
				row = new Sql.Tables.StringTable.Row();
				row.Str = "abcd";
				row.Update(transaction);
				
				row = new Sql.Tables.StringTable.Row();
				row.Str = "123";
				row.Update(transaction);
				
				transaction.Commit();
			}
			
			Tables.StringTable.Table table = Tables.StringTable.Table.INSTANCE;
			
			Sql.Function.Count count = new Sql.Function.Count();
			
			IResult result = Sql.Query
				.Select(table.Str, count)
				.From(table)
				.GroupBy(table.Str)
				.OrderBy(count, table.Str)
				.Execute();
			
			Assert.AreEqual(3, result.Count);
			Assert.AreEqual(1, count[0, result].Value);
			Assert.AreEqual(1, count[1, result].Value);
			Assert.AreEqual(1, count[2, result].Value);
			
			Assert.AreEqual("123", table[0, result].Str);
			Assert.AreEqual("abc", table[1, result].Str);
			Assert.AreEqual("abcd", table[2, result].Str);
		}
		
		[TestMethod]
		public void Test_16() {
			
			Transaction transaction = new Transaction(DB.TestDB);
			
			try {
				
				Tables.StringTable.Row row = new Tables.StringTable.Row();
				
				row.Str += "string";
				row.Update(transaction);
				
				Tables.StringTable.Table table = Tables.StringTable.Table.INSTANCE;
				
				
				IResult result = Query.Update(table)
					.Set(table.Str, table.Str)
					.NoWhereCondition()
					.Returning(table.Str)
					.Execute(transaction);
				
				Assert.AreEqual(1, result.RowsEffected);
				Assert.AreEqual(table[0, result].Str, "string");
				
				Tables.StringTable.Table tableB = new Sql.Tables.StringTable.Table();
				
				result = Query.Update(table)
					.Set(table.Str, table.Str)
					.Join(tableB, table.Str == tableB.Str)
					.NoWhereCondition()
					.Returning(table.Str)
					.Execute(transaction);
				
				Assert.AreEqual(1, result.RowsEffected);
				Assert.AreEqual(table[0, result].Str, "string");
				
				result = Query.Update(table)
					.Set(table.Str, tableB.Str)
					.Join(tableB, table.Str == tableB.Str)
					.NoWhereCondition()
					.Returning(table.Str)
					.Execute(transaction);
				
				Assert.AreEqual(1, result.RowsEffected);
				Assert.AreEqual(table[0, result].Str, "string");
				
				transaction.Commit();
			}
			catch(Exception e){
				e = e ?? e;
				transaction.Rollback();
				throw;
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
			}
			finally {
				Settings.UseParameters = true;
			}
		}
	}
}