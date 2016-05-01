
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
	public class DateTimeOffsetTest {
		
		[TestInitialize()]
		public void Init() {
			
			mDateTime1 = DateTimeOffset.Now;
			mDateTime2 = DateTimeOffset.Now.AddDays(20).AddMilliseconds(1234);
		
			Transaction transaction = new Transaction(DB.TestDB);
			
			try {
				
				Tables.DateTimeOffsetTable.Table table = Tables.DateTimeOffsetTable.Table.INSTANCE;
				
				Query.Delete(table).NoWhereCondition.Execute(transaction);
				
				transaction.Commit();
			}
			catch(Exception e){
				transaction.Rollback();
				throw e;
			}
		}
		
		private DateTimeOffset mDateTime1;
		private DateTimeOffset mDateTime2;
		
		[TestMethod]
		public void Test_01(){
			
			Tables.DateTimeOffsetTable.Table table = Tables.DateTimeOffsetTable.Table.INSTANCE;
			
			IResult result;
			
			using(Transaction transaction = new Transaction(DB.TestDB)){
				
				result = Query.Insert(table)
					.Set(table.Id, Guid.NewGuid())
					.Set(table.Dt, mDateTime1)
					.Execute(transaction);
				
				Assert.AreEqual(1, result.RowsEffected);
				transaction.Commit();
			}
			
			result = Query.Select(table.Dt, table.NullDt).From(table).Where(table.Dt == mDateTime1 & table.NullDt.IsNull).Execute(DB.TestDB);
			Assert.AreEqual(1, result.Count);
			
			CompareDates(table, mDateTime1, table[0, result].Dt);
			Assert.IsNull(table[0, result].NullDt);
			
			result = Query.Select(table.Dt).From(table).Where(table.Dt != mDateTime1).Execute(DB.TestDB);
			Assert.AreEqual(0, result.Count);
			
			result = Query.Select(table.Dt).From(table).Where(table.NullDt.IsNotNull).Execute(DB.TestDB);
			Assert.AreEqual(0, result.Count);
			
			result = Query.Select(table.Dt).From(table).Where(table.NullDt == mDateTime1).Execute(DB.TestDB);
			Assert.AreEqual(0, result.Count);
			
			DateTimeOffset before = mDateTime1.AddMilliseconds(-100);
			DateTimeOffset after = mDateTime1.AddMilliseconds(100);
			
			result = Query.Select(table.Dt, table.NullDt).From(table).Where(table.Dt > before & table.Dt < after).Execute(DB.TestDB);
			Assert.AreEqual(1, result.Count);
			CompareDates(table, mDateTime1, table[0, result].Dt);
			Assert.IsNull(table[0, result].NullDt);
			
			result = Query.Select(table.Dt, table.NullDt).From(table).Where(table.Dt >= mDateTime1 & table.Dt <= mDateTime1).Execute(DB.TestDB);
			Assert.AreEqual(1, result.Count);
			CompareDates(table, mDateTime1, table[0, result].Dt);
			Assert.IsNull(table[0, result].NullDt);
		}
		
		private void CompareDates(Tables.DateTimeOffsetTable.Table pTable, DateTimeOffset pD1, DateTimeOffset pD2){
			
			if(DB.TestDB.DatabaseType == DatabaseType.PostgreSql){
									
				//PostgreSql stores timestamp with time zone as UTC rather than as an offset like sql server
				//So this means we need to adjust the dates to the same time zone to compare. Dates are returned
				//in local time from postgresql rather than the offset time
				pD1 = pD1.ToUniversalTime();
				pD2 = pD2.ToUniversalTime();
					
				bool areEqual = pD1.Year == pD2.Year &&
					pD1.Month == pD2.Month &&
					pD1.Day == pD2.Day &&
					pD1.Hour == pD2.Hour &&
					pD1.Minute == pD2.Minute &&
					pD1.Second == pD2.Second &&
					pD1.Millisecond == pD2.Millisecond &&
					pD1.Offset == pD2.Offset;
				
				Assert.IsTrue(areEqual);
			}
			else {
				Assert.AreEqual(pD1, pD2);
			}
		}
		[TestMethod]
		public void Test_02(){
			
			Tables.DateTimeOffsetTable.Table table = Tables.DateTimeOffsetTable.Table.INSTANCE;
			
			using(Transaction transaction = new Transaction(DB.TestDB)){
				
				IResult insertResult = Query.Insert(table)
					.Set(table.Id, Guid.NewGuid())
					.Set(table.Dt, mDateTime1)
					.Set(table.NullDt, mDateTime1)
					.Execute(transaction);
				
				Assert.AreEqual(1, insertResult.RowsEffected);
				
				insertResult = Query.Insert(table)
					.Set(table.Id, Guid.NewGuid())
					.Set(table.Dt, mDateTime2)
					.Set(table.NullDt, mDateTime2)
					.Execute(transaction);
				
				Assert.AreEqual(1, insertResult.RowsEffected);
				transaction.Commit();
			}
			
			IResult result = Query.Select(table.Dt).From(table).Where(table.Dt == mDateTime1 | table.Dt == mDateTime2).Execute(DB.TestDB);			
			Assert.AreEqual(2, result.Count);
			
			Tables.DateTimeOffsetTable.Table table2 = new Tables.DateTimeOffsetTable.Table();
			
			result = Query.Select(table.Id, table2.Dt)
				.From(table)
				.Join(table2, table.Dt == table2.Dt)
				.Execute(DB.TestDB);
			
			Assert.AreEqual(2, result.Count);
		}
		
		[TestMethod]
		public void Test_03(){
			
			Tables.DateTimeOffsetTable.Table table = Tables.DateTimeOffsetTable.Table.INSTANCE;
			
			using(Transaction transaction = new Transaction(DB.TestDB)){
				
				Tables.DateTimeOffsetTable.Row row1 = new Sql.Tables.DateTimeOffsetTable.Row();
				row1.Id = Guid.NewGuid();
				row1.Dt = mDateTime1;
				
				Tables.DateTimeOffsetTable.Row row2 = new Sql.Tables.DateTimeOffsetTable.Row();
				row2.Id = Guid.NewGuid();
				row2.Dt = mDateTime2;
				row2.NullDt = mDateTime1;
				
				row1.Update(transaction);
				row2.Update(transaction);
				
				transaction.Commit();
			}
			
			IResult result = Query.Select(table.Dt).From(table).Where((table.Dt == mDateTime1 & table.NullDt.IsNull) | (table.Dt == mDateTime2 & table.NullDt == mDateTime1)).Execute(DB.TestDB);
			Assert.AreEqual(2, result.Count);
			
			result = Query.Select(table.Id)
				.From(table)
				.Where(table.Dt.In(mDateTime1, mDateTime2))
				.Execute(DB.TestDB);
			
			Assert.AreEqual(2, result.Count);
			
			List<DateTimeOffset> list = new List<DateTimeOffset>();
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
			
			Tables.DateTimeOffsetTable.Table table = Tables.DateTimeOffsetTable.Table.INSTANCE;
			
			using(Transaction transaction = new Transaction(DB.TestDB)){
				
				Sql.Query.Insert(table)
					.Set(table.Id, Guid.NewGuid())
					.Set(table.Dt, new Function.CurrentDateTimeOffset(DB.TestDB.DatabaseType))
					.Set(table.NullDt, new Function.CurrentDateTimeOffset(DB.TestDB.DatabaseType))
					.Execute(transaction);
				
				transaction.Commit();
			}
			
			IResult result = Query.Select(table.Dt).From(table).Execute(DB.TestDB);			
			Assert.AreEqual(1, result.Count);
			
			using(Transaction transaction = new Transaction(DB.TestDB)){
				
				Sql.Query.Update(table)
					.Set(table.Dt, new Function.CurrentDateTimeOffset(DB.TestDB.DatabaseType))
					.Set(table.NullDt, new Function.CurrentDateTimeOffset(DB.TestDB.DatabaseType))
					.NoWhereCondition()
					.Execute(transaction);
				
				transaction.Commit();
			}
		}
		
		[TestMethod]
		public void Test_05(){
			
			Tables.DateTimeOffsetTable.Table table = Tables.DateTimeOffsetTable.Table.INSTANCE;
			
			if(DB.TestDB.DatabaseType != DatabaseType.Mssql)
				return;
			
			string customFunction = string.Empty;
			
			if(DB.TestDB.DatabaseType != DatabaseType.Mssql)
				customFunction = "SYSDATETIMEOFFSET()";
			else if(DB.TestDB.DatabaseType != DatabaseType.PostgreSql)
				customFunction = "current_timestamp";
			
			using(Transaction transaction = new Transaction(DB.TestDB)){
				
				Sql.Query.Insert(table)
					.Set(table.Id, Guid.NewGuid())
					.Set(table.Dt, new Sql.Function.CustomSql(customFunction))
					.Set(table.NullDt, new Sql.Function.CustomSql(customFunction))
					.Execute(transaction);
				
				transaction.Commit();
			}
			
			IResult result = Query.Select(table.Dt).From(table).Execute(DB.TestDB);			
			Assert.AreEqual(1, result.Count);
			
			using(Transaction transaction = new Transaction(DB.TestDB)){
				
				Sql.Query.Update(table)
					.Set(table.Dt, new Function.CurrentDateTimeOffset(DB.TestDB.DatabaseType))
					.Set(table.NullDt, new Function.CurrentDateTimeOffset(DB.TestDB.DatabaseType))
					.NoWhereCondition()
					.Execute(transaction);
				
				transaction.Commit();
			}
		}
		
		[TestMethod]
		public void Test_06(){
			
			Tables.DateTimeOffsetTable.Table table = Tables.DateTimeOffsetTable.Table.INSTANCE;
			
			if(DB.TestDB.DatabaseType != DatabaseType.Mssql)
				return;
			
			using(Transaction transaction = new Transaction(DB.TestDB)) {
			
				string sql = "DECLARE @dt DATETIME;SET @dt=SYSDATETIMEOFFSET();";
				
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
					.Set(table.Dt, new Function.CurrentDateTimeOffset(DB.TestDB.DatabaseType))
					.NoWhereCondition()
					.Execute(transaction);
				
				transaction.Commit();
			}
		}
		
		[TestMethod]
		public void Test_07(){
			
			Tables.DateTimeOffsetTable.Table table = Tables.DateTimeOffsetTable.Table.INSTANCE;			
			
			mDateTime1 = new DateTimeOffset(2012, 12, 31, 23, 59, 59, new TimeSpan(5, 0, 0));
			
			IResult result;
			
			using(Transaction transaction = new Transaction(DB.TestDB)){
				
				result = Query.Insert(table)
					.Set(table.Id, Guid.NewGuid())
					.Set(table.Dt, mDateTime1)
					.Execute(transaction);
				
				Assert.AreEqual(1, result.RowsEffected);
				transaction.Commit();
			}
			
			result = Query.Select(table.Dt, table.NullDt).From(table).Where(table.Dt == mDateTime1 & table.NullDt.IsNull).Execute(DB.TestDB);
			Assert.AreEqual(1, result.Count);
			
			CompareDates(table, mDateTime1, table[0, result].Dt);
			Assert.IsNull(table[0, result].NullDt);
			
			result = Query.Select(table.Dt).From(table).Where(table.Dt != mDateTime1).Execute(DB.TestDB);
			Assert.AreEqual(0, result.Count);
			
			result = Query.Select(table.Dt).From(table).Where(table.NullDt.IsNotNull).Execute(DB.TestDB);
			Assert.AreEqual(0, result.Count);
			
			result = Query.Select(table.Dt).From(table).Where(table.NullDt == mDateTime1).Execute(DB.TestDB);
			Assert.AreEqual(0, result.Count);
			
			DateTimeOffset before = mDateTime1.AddMilliseconds(-100);
			DateTimeOffset after = mDateTime1.AddMilliseconds(100);
			
			result = Query.Select(table.Dt, table.NullDt).From(table).Where(table.Dt > before & table.Dt < after).Execute(DB.TestDB);
			Assert.AreEqual(1, result.Count);
			CompareDates(table, mDateTime1, table[0, result].Dt);
			Assert.IsNull(table[0, result].NullDt);
			
			result = Query.Select(table.Dt, table.NullDt).From(table).Where(table.Dt >= mDateTime1 & table.Dt <= mDateTime1).Execute(DB.TestDB);
			Assert.AreEqual(1, result.Count);
			CompareDates(table, mDateTime1, table[0, result].Dt);
			Assert.IsNull(table[0, result].NullDt);
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
				Init();
				Test_07();
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
				Init();
				Test_07();
			}
			finally {
				Settings.UseParameters = true;
			}
		}
	}
}