
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
using System.Collections.Generic;

using Sql.Tables;

namespace Sql.Tests {
	
	[TestClass]
	public class DateTime2Test {
		
		[TestInitialize()]
		public void Init() {
			
			mDateTime1 = DateTime.Now.AddTicks(321);
			mDateTime2 = DateTime.Now.AddDays(50).AddTicks(92751);
			
			Transaction transaction = new Transaction(DB.TestDB);
			
			try {
				
				Tables.DateTime2Table.Table table = Tables.DateTime2Table.Table.INSTANCE;
				
				Query.Delete(table).NoWhereCondition.Execute(transaction);
				
				transaction.Commit();
			}
			catch(Exception e){
				transaction.Rollback();
				throw e;
			}
		}
		
		private DateTime mDateTime1;
		private DateTime mDateTime2;
		
		[TestMethod]
		public void Test_01(){
			
			Tables.DateTime2Table.Table table = Tables.DateTime2Table.Table.INSTANCE;
			
			IResult result;
			
			using(Transaction transaction = new Transaction(DB.TestDB)){
				
				result = Query.Insert(table)
					.Set(table.Id, Guid.NewGuid())
					.Set(table.Dt, mDateTime1)
					.Execute(transaction);
				
				Assert.AreEqual(1, result.RowsEffected);
				transaction.Commit();
			}
			
			result = Query.Select(table.Dt).From(table).Where(table.Dt == mDateTime1).Execute(DB.TestDB);			
			Assert.AreEqual(1, result.Count);
			
			CompareDates(table, mDateTime1, table[0, result].Dt);
			
			result = Query.Select(table.Dt).From(table).Where(table.Dt != mDateTime1).Execute(DB.TestDB);
			Assert.AreEqual(0, result.Count);
			
			DateTime before = mDateTime1.AddMilliseconds(-100);
			DateTime after = mDateTime1.AddMilliseconds(100);
			
			result = Query.Select(table.Dt).From(table).Where(table.Dt > before & table.Dt < after).Execute(DB.TestDB);			
			Assert.AreEqual(1, result.Count);
			CompareDates(table, mDateTime1, table[0, result].Dt);
			
			result = Query.Select(table.Dt).From(table).Where(table.Dt >= mDateTime1 & table.Dt <= mDateTime1).Execute(DB.TestDB);			
			Assert.AreEqual(1, result.Count);
			CompareDates(table, mDateTime1, table[0, result].Dt);
		}
		
		private void CompareDates(Tables.DateTime2Table.Table pTable, DateTime pD1, DateTime pD2){
			
			if(DB.TestDB.DatabaseType == DatabaseType.PostgreSql){
					
				bool areEqual = pD1.Year == pD2.Year &&
					pD1.Month == pD2.Month &&
					pD1.Day == pD2.Day &&
					pD1.Hour == pD2.Hour &&
					pD1.Minute == pD2.Minute &&
					pD1.Second == pD2.Second &&
					pD1.Millisecond == pD2.Millisecond;
				
				Assert.IsTrue(areEqual);
			}
			else {
				Assert.AreEqual(pD1, pD2);
			}
		}
		
		[TestMethod]
		public void Test_02(){
			
			Tables.DateTime2Table.Table table = Tables.DateTime2Table.Table.INSTANCE;
			
			using(Transaction transaction = new Transaction(DB.TestDB)){
				
				IResult insertResult = Query.Insert(table)
					.Set(table.Id, Guid.NewGuid())
					.Set(table.Dt, mDateTime1)
					.Execute(transaction);
				
				Assert.AreEqual(1, insertResult.RowsEffected);
				
				insertResult = Query.Insert(table)
					.Set(table.Id, Guid.NewGuid())
					.Set(table.Dt, mDateTime2)
					.Execute(transaction);
				
				Assert.AreEqual(1, insertResult.RowsEffected);
				transaction.Commit();
			}
			
			IResult result = Query.Select(table.Dt).From(table).Where(table.Dt == mDateTime1 | table.Dt == mDateTime2).Execute(DB.TestDB);			
			Assert.AreEqual(2, result.Count);
			
			Tables.DateTime2Table.Table table2 = new Tables.DateTime2Table.Table();
			
			result = Query.Select(table.Id, table2.Dt)
				.From(table)
				.Join(table2, table.Dt == table2.Dt)
				.Execute(DB.TestDB);
			
			Assert.AreEqual(2, result.Count);
		}
		
		[TestMethod]
		public void Test_03(){
			
			Tables.DateTime2Table.Table table = Tables.DateTime2Table.Table.INSTANCE;
			
			using(Transaction transaction = new Transaction(DB.TestDB)){
				
				Tables.DateTime2Table.Row row1 = new Sql.Tables.DateTime2Table.Row();
				row1.Id = Guid.NewGuid();
				row1.Dt = mDateTime1;
				
				Tables.DateTime2Table.Row row2 = new Sql.Tables.DateTime2Table.Row();
				row2.Id = Guid.NewGuid();
				row2.Dt = mDateTime2;
				
				row1.Update(transaction);
				row2.Update(transaction);
				
				transaction.Commit();
			}
			
			IResult result = Query.Select(table.Dt).From(table).Where(table.Dt == mDateTime1 | table.Dt == mDateTime2).Execute(DB.TestDB);			
			Assert.AreEqual(2, result.Count);
			
			result = Query.Select(table.Id)
				.From(table)
				.Where(table.Dt.In(mDateTime1, mDateTime2))
				.Execute(DB.TestDB);
			
			Assert.AreEqual(2, result.Count);
			
			List<DateTime> list = new List<DateTime>();
			list.Add(mDateTime1);
			list.Add(mDateTime2);
			
			result = Query.Select(table.Id)
				.From(table)
				.Where(table.Dt.In(list))
				.Execute(DB.TestDB);
			
			Assert.AreEqual(2, result.Count);
		}
		
		[TestMethod]
		public void Test_04(){
			
			Tables.DateTime2Table.Table table = Tables.DateTime2Table.Table.INSTANCE;
			
			using(Transaction transaction = new Transaction(DB.TestDB)){
				
				Sql.Query.Insert(table)
					.Set(table.Id, Guid.NewGuid())
					.Set(table.Dt, new Function.CurrentDateTime(DB.TestDB.DatabaseType))
					.Execute(transaction);
				
				transaction.Commit();
			}
			
			IResult result = Query.Select(table.Dt).From(table).Execute(DB.TestDB);			
			Assert.AreEqual(1, result.Count);
			
			using(Transaction transaction = new Transaction(DB.TestDB)){
				
				Sql.Query.Update(table)
					.Set(table.Dt, new Function.CurrentDateTime(DB.TestDB.DatabaseType))
					.NoWhereCondition()
					.Execute(transaction);
				
				transaction.Commit();
			}
		}
		
		[TestMethod]
		public void Test_05(){
			
			Tables.DateTime2Table.Table table = Tables.DateTime2Table.Table.INSTANCE;
			
			if(DB.TestDB.DatabaseType != DatabaseType.Mssql)
				return;
			
			using(Transaction transaction = new Transaction(DB.TestDB)){
				
				Sql.Query.Insert(table)
					.Set(table.Id, Guid.NewGuid())
					.Set(table.Dt, new Sql.Function.CustomSql("GETDATE()"))
					.Execute(transaction);
				
				transaction.Commit();
			}
			
			IResult result = Query.Select(table.Dt).From(table).Execute(DB.TestDB);			
			Assert.AreEqual(1, result.Count);
			
			using(Transaction transaction = new Transaction(DB.TestDB)){
				
				Sql.Query.Update(table)
					.Set(table.Dt, new Function.CurrentDateTime(DB.TestDB.DatabaseType))
					.NoWhereCondition()
					.Execute(transaction);
				
				transaction.Commit();
			}
		}
		
		[TestMethod]
		public void Test_06(){
			
			Tables.DateTime2Table.Table table = Tables.DateTime2Table.Table.INSTANCE;
			
			if(DB.TestDB.DatabaseType != DatabaseType.Mssql)
				return;
			
			using(Transaction transaction = new Transaction(DB.TestDB)) {
			
				string sql = "DECLARE @dt DATETIME;SET @dt=GETDATE();";
				
				sql += Sql.Query.Insert(table)
					.Set(table.Id, Guid.NewGuid())
					.Set(table.Dt, new Sql.Function.CustomSql("@dt"))
					.GetSql(DB.TestDB);
				
				Sql.Query.ExecuteNonQuery(sql, DB.TestDB, transaction);				
				transaction.Commit();
			}
			
			IResult result = Query.Select(table.Dt).From(table).Execute(DB.TestDB);			
			Assert.AreEqual(1, result.Count);
			
			using(Transaction transaction = new Transaction(DB.TestDB)){
				
				Sql.Query.Update(table)
					.Set(table.Dt, new Function.CurrentDateTime(DB.TestDB.DatabaseType))
					.NoWhereCondition()
					.Execute(transaction);
				
				transaction.Commit();
			}
		}
		
		[TestMethod]
		public void TestsWithoutParameters(){
			
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
		public void TestsWithParameters(){
			
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