
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
	public class SmallIntegerTest {
		
		private bool mRunOnce = false;
		
		[TestInitialize()]
		public void Init() {
			
			Transaction transaction = new Transaction(DB.TestDB);
			
			try {
				
				Tables.SmallIntTable.Table table = Tables.SmallIntTable.Table.INSTANCE;
				
				Query.Delete(table).NoWhereCondition.Execute(transaction);
				
				if(!mRunOnce) {
					if(DB.TestDB.DatabaseType == DatabaseType.Mssql) {
						Query.ExecuteNonQuery("DBCC CHECKIDENT (" + table.TableName + ", RESEED, 1)", DB.TestDB, transaction);
					}
					else if(DB.TestDB.DatabaseType == DatabaseType.PostgreSql) {
						Sql.Query.ExecuteNonQuery("ALTER SEQUENCE auto_id_seq RESTART WITH 1;", DB.TestDB, transaction);
					}
					mRunOnce = true;
				}
				
				transaction.Commit();
			}
			catch(Exception e){
				transaction.Rollback();
				throw e;
			}
		}
		
		private Int16 mInt1 = 25;
		
		[TestMethod]
		public void Test_01() {
			
			using(Transaction transaction = new Transaction(DB.TestDB)){
				
				Tables.SmallIntTable.Row row = new Sql.Tables.SmallIntTable.Row();
				
				row.IntValue = mInt1;
				
				row.Update(transaction);
				
				transaction.Commit();
			}
			
			Tables.SmallIntTable.Table table = Tables.SmallIntTable.Table.INSTANCE;
			
			IResult result = Query.Select(table.IntValue).From(table).Execute(DB.TestDB);
			Assert.AreEqual(1, result.Count);
			Assert.AreEqual(mInt1, table[0, result].IntValue);
			
			result = Query.Select(table.IntValue).From(table).Where(table.IntValue == mInt1).Execute(DB.TestDB);
			Assert.AreEqual(1, result.Count);
			Assert.AreEqual(mInt1, table[0, result].IntValue);
			
			result = Query.Select(table.IntValue).From(table).Where(table.IntValue != mInt1).Execute(DB.TestDB);
			Assert.AreEqual(0, result.Count);
			
			result = Query.Select(table.IntValue).From(table).Where(table.IntValue > 0).Execute(DB.TestDB);
			Assert.AreEqual(1, result.Count);
			Assert.AreEqual(mInt1, table[0, result].IntValue);
			
			result = Query.Select(table.IntValue).From(table).Where(table.IntValue < 0).Execute(DB.TestDB);
			Assert.AreEqual(0, result.Count);
			
			result = Query.Select(table.IntValue).From(table).Where(table.IntValue <= mInt1).Execute(DB.TestDB);
			Assert.AreEqual(1, result.Count);
			Assert.AreEqual(mInt1, table[0, result].IntValue);
			
			result = Query.Select(table.IntValue).From(table).Where(table.IntValue >= mInt1).Execute(DB.TestDB);
			Assert.AreEqual(1, result.Count);
			Assert.AreEqual(mInt1, table[0, result].IntValue);
			
			result = Query.Select(table.IntValue).From(table).Where(table.IntValue <= (Int16)(mInt1 - 1)).Execute(DB.TestDB);
			Assert.AreEqual(0, result.Count);
			
			result = Query.Select(table.IntValue).From(table).Where(table.IntValue >= (Int16)(mInt1 + 1)).Execute(DB.TestDB);
			Assert.AreEqual(0, result.Count);
			
			result = Query.Select(table.IntValue).From(table).Where(table.IntValue.In(mInt1, 5)).Execute(DB.TestDB);
			Assert.AreEqual(1, result.Count);
			Assert.AreEqual(mInt1, table[0, result].IntValue);
			
			result = Query.Select(table.IntValue).From(table).Where(table.IntValue.NotIn(mInt1, 5)).Execute(DB.TestDB);
			Assert.AreEqual(0, result.Count);			
			
			System.Collections.Generic.List<Int16> list = new System.Collections.Generic.List<Int16>();
			list.Add(mInt1);
			
			result = Query.Select(table.IntValue).From(table).Where(table.IntValue.In(list)).Execute(DB.TestDB);
			Assert.AreEqual(1, result.Count);
			Assert.AreEqual(mInt1, table[0, result].IntValue);
			
			result = Query.Select(table.IntValue).From(table).Where(table.IntValue.NotIn(list)).Execute(DB.TestDB);
			Assert.AreEqual(0, result.Count);			
			
			Tables.SmallIntTable.Table table2 = new Sql.Tables.SmallIntTable.Table();
			
			result = Query.Select(table, table2)
				.From(table)
				.Join(table2, table.IntValue == table2.IntValue)
				.Execute(DB.TestDB);
			
			Assert.AreEqual(1, result.Count);
			Assert.AreEqual(mInt1, table[0, result].IntValue);
			Assert.AreEqual(mInt1, table2[0, result].IntValue);
			
			result = Query.Select(table, table2)
				.From(table)
				.Join(table2, table.IntValue != table2.IntValue)
				.Execute(DB.TestDB);
			
			Assert.AreEqual(0, result.Count);
			
			result = Query.Select(table, table2)
				.From(table)
				.Join(table2, table.IntValue >= table2.IntValue)
				.Execute(DB.TestDB);
			
			Assert.AreEqual(1, result.Count);
			Assert.AreEqual(mInt1, table[0, result].IntValue);
			Assert.AreEqual(mInt1, table2[0, result].IntValue);
			
			result = Query.Select(table, table2)
				.From(table)
				.Join(table2, table.IntValue <= table2.IntValue)
				.Execute(DB.TestDB);
			
			Assert.AreEqual(1, result.Count);
			Assert.AreEqual(mInt1, table[0, result].IntValue);
			Assert.AreEqual(mInt1, table2[0, result].IntValue);
			
			result = Query.Select(table, table2)
				.From(table)
				.Join(table2, table.IntValue < table2.IntValue)
				.Execute(DB.TestDB);
			
			Assert.AreEqual(0, result.Count);
			
			result = Query.Select(table, table2)
				.From(table)
				.Join(table2, table.IntValue > table2.IntValue)
				.Execute(DB.TestDB);
			
			Assert.AreEqual(0, result.Count);
			
			result = Query.Select(table)
				.From(table)
				.Where(table.IntValue.In(Query.Select(table2.IntValue).Distinct.From(table2)))
				.Execute(DB.TestDB);
			
			Assert.AreEqual(1, result.Count);
			Assert.AreEqual(mInt1, table[0, result].IntValue);
			
			result = Query.Select(table)
				.From(table)
				.Where(table.IntValue.NotIn(Query.Select(table2.IntValue).Distinct.From(table2)))
				.Execute(DB.TestDB);
			
			Assert.AreEqual(0, result.Count);
		}
		
		[TestMethod]
		public void ParametersTurnedOff() {
		
			Settings.UseParameters = false;
			
			try {
				Test_01();
			}
			finally {
				Settings.UseParameters = true;
			}
		}
	}
}