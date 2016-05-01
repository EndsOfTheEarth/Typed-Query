
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
	public class BooleanTest {
	
		[TestInitialize()]
		public void Init() {
			
			Transaction transaction = new Transaction(DB.TestDB);
			
			try {
				
				Tables.BooleanTable.Table table = Tables.BooleanTable.Table.INSTANCE;
				
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
			
			using(Transaction transaction = new Transaction(DB.TestDB)) {
				
				Tables.BooleanTable.Row row = new Tables.BooleanTable.Row();
				
				row.Id = Guid.NewGuid();
				row.Bool = true;
				
				row.Update(transaction);
				transaction.Commit();
			}
			
			Tables.BooleanTable.Table table = Tables.BooleanTable.Table.INSTANCE;
			
			IResult result = Query.Select(table).From(table).Where(table.Bool == true).Execute(DB.TestDB);
			Assert.AreEqual(1, result.Count);
			Assert.AreEqual(true, table[0, result].Bool);
			
			result = Query.Select(table).From(table).Where(table.Bool != false).Execute(DB.TestDB);
			Assert.AreEqual(1, result.Count);
			Assert.AreEqual(true, table[0, result].Bool);
			
			result = Query.Select(table).From(table).Where(table.Bool == false).Execute(DB.TestDB);
			Assert.AreEqual(0, result.Count);
			
			result = Query.Select(table).From(table).Where(table.Bool != true).Execute(DB.TestDB);
			Assert.AreEqual(0, result.Count);
		}
		
		[TestMethod]
		public void Test_02() {
			
			using(Transaction transaction = new Transaction(DB.TestDB)) {
				
				Tables.BooleanTable.Row row = new Tables.BooleanTable.Row();
				
				row.Id = Guid.NewGuid();
				row.Bool = true;
				row.Update(transaction);
				
				row = new Tables.BooleanTable.Row();
				
				row.Id = Guid.NewGuid();
				row.Bool = false;
				row.Update(transaction);
				
				transaction.Commit();
			}
			
			Tables.BooleanTable.Table table = Tables.BooleanTable.Table.INSTANCE;
			
			IResult result = Query.Select(table).From(table).Where(table.Bool == true).Execute(DB.TestDB);
			Assert.AreEqual(1, result.Count);
			Assert.AreEqual(true, table[0, result].Bool);
			
			result = Query.Select(table).From(table).Where(table.Bool == false).Execute(DB.TestDB);
			Assert.AreEqual(1, result.Count);
			Assert.AreEqual(false, table[0, result].Bool);
			
			Tables.BooleanTable.Table table2 = new Tables.BooleanTable.Table();
			
			result = Query.Select(table)
				.From(table)
				.Join(table2, table.Bool == table2.Bool)
				.OrderBy(table.Bool.DESC)
				.Execute(DB.TestDB);
			
			Assert.AreEqual(2, result.Count);
			Assert.AreEqual(true, table[0, result].Bool);
			Assert.AreEqual(false, table[1, result].Bool);
			
			result = Query.Select(table)
				.From(table)
				.Join(table2, table.Bool != table2.Bool)
				.Where(table2.Bool == false)
				.OrderBy(table.Bool.DESC)
				.Execute(DB.TestDB);
			
			Assert.AreEqual(1, result.Count);
			Assert.AreEqual(true, table[0, result].Bool);
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