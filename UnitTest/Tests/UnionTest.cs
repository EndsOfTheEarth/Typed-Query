
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

namespace Sql.Tests {
	
	[TestClass]
	public class UnionTest {
	
		[TestInitialize()]
		public void Init() {
			
			Transaction transaction = new Transaction(DB.TestDB);
			
			try {
				
				Tables.GuidTable.Table table = Tables.GuidTable.Table.INSTANCE;
				Query.Delete(table).NoWhereCondition.Execute(transaction);
				
				Tables.BigIntTable.Table bigIntTable = Tables.BigIntTable.Table.INSTANCE;
				Query.Delete(bigIntTable).NoWhereCondition.Execute(transaction);
				
				transaction.Commit();
			}
			catch(Exception e){
				transaction.Rollback();
				throw e;
			}
		}
		
		[TestMethod]
		public void Test_01(){
			
			Transaction transaction = new Transaction(DB.TestDB);
			
			try {
				
				Tables.GuidTable.Row row = new Tables.GuidTable.Row();
				
				row.Id = Guid.NewGuid();
				row.Update(transaction);
				
				Tables.GuidTable.Table table = Tables.GuidTable.Table.INSTANCE;
				
				IResult result = Query.Select(table.Id)
					.From(table)
					.UnionAll(table.Id)
					.From(table)
					.Execute(transaction);
				
				Assert.AreEqual(2, result.Count);
				Assert.AreEqual(row.Id, table[0, result].Id);
				Assert.AreEqual(row.Id, table[1, result].Id);				
				
				result = Query.Select(table.Id)
					.From(table)
					.Where(table.Id == row.Id)
					.UnionAll(table.Id)
					.From(table)
					.Where(table.Id == row.Id)
					.Execute(transaction);
				
				Assert.AreEqual(2, result.Count);
				Assert.AreEqual(row.Id, table[0, result].Id);
				Assert.AreEqual(row.Id, table[1, result].Id);
				
				result = Query.Select(table.Id)
					.From(table)
					.Where(table.Id == row.Id)
					.UnionAll(table.Id)
					.From(table)
					.Where(table.Id != row.Id)
					.Execute(transaction);
				
				Assert.AreEqual(1, result.Count);
				Assert.AreEqual(row.Id, table[0, result].Id);
				
				result = Query.Select(table.Id)
					.From(table)
					.Union(table.Id)
					.From(table)
					.Execute(transaction);
				
				Assert.AreEqual(1, result.Count);
				Assert.AreEqual(row.Id, table[0, result].Id);
				
				transaction.Commit();
			}
			catch(Exception e){
				transaction.Rollback();
				throw e;
			}
		}
		
		[TestMethod]
		public void Test_02(){
			
			Tables.BigIntTable.Table table = Tables.BigIntTable.Table.INSTANCE;
			
			using(Sql.Transaction transaction = new Transaction(DB.TestDB)) {
				
				Sql.Query.Insert(table)
					.Set(table.IntValue, 1)
					.Execute(transaction);
				
				Sql.Query.Insert(table)
					.Set(table.IntValue, 1)
					.Execute(transaction);
				
				transaction.Commit();
			}
			
			IResult result = Sql.Query.Select(table.IntValue).From(table)
				.Intersect(table.IntValue).From(table)
				.Execute(DB.TestDB);
			
			Assert.AreEqual(1, result.Count);
			Assert.AreEqual(1, table[0, result].IntValue);
			
			result = Sql.Query.Select(table.IntValue).From(table)
				.Except(table.IntValue).From(table)
				.Execute(DB.TestDB);
			
			Assert.AreEqual(0, result.Count);
			
			using(Sql.Transaction transaction = new Transaction(DB.TestDB)) {
				
				Sql.Query.Insert(table)
					.Set(table.IntValue, 2)
					.Execute(transaction);
				
				transaction.Commit();
			}
			
			result = Sql.Query.Select(table.IntValue).From(table)
				.Except(table.IntValue).From(table).Where(table.IntValue != 2)
				.Execute(DB.TestDB);
			
			Assert.AreEqual(1, result.Count);
			Assert.AreEqual(2, table[0, result].IntValue);
			
			result = Sql.Query.Select(table.IntValue).From(table)
				.Intersect(table.IntValue).From(table).Where(table.IntValue != 2)
				.Execute(DB.TestDB);
			
			Assert.AreEqual(1, result.Count);
			Assert.AreEqual(1, table[0, result].IntValue);
		}
		
		[TestMethod]
		public void ParametersTurnedOff() {
		
			Settings.UseParameters = false;
			
			try {
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
