
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
	public class WindowFunctionTest {
		
		[TestInitialize()]
		public void Init() {
			
			Transaction transaction = new Transaction(DB.TestDB);
			
			try {
				
				Tables.IntTable.Table table = Tables.IntTable.Table.INSTANCE;
				
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
			
			Tables.IntTable.Table table = Tables.IntTable.Table.INSTANCE;
			
			const int rows = 100;
			
			using(Transaction transaction = new Transaction(DB.TestDB)) {
				
				for (int index = 0; index < rows; index++) {
					
					Sql.Query.Insert(table)
						.Set(table.Id, Guid.NewGuid())
						.Set(table.IntValue, index)
						.Execute(transaction);
				}
				transaction.Commit();
			}
			
			Sql.Function.AvgInteger avg = new Sql.Function.AvgInteger(table.IntValue);
			
			
			IResult result = Query.Select(
				avg.Over()
			).From(table).Execute(DB.TestDB);
			
			Assert.AreEqual(100, result.Count);
			
			for (int index = 0; index < 100; index++) {
				
				decimal avgValue = avg[index, result].Value;
				
				if(DB.TestDB.DatabaseType == DatabaseType.PostgreSql)
					Assert.IsTrue(avgValue == 49.50m);
				else if(DB.TestDB.DatabaseType == DatabaseType.Mssql)
					Assert.IsTrue(avgValue == 49.00m);
				else
					Assert.IsTrue(false);
			}
		}
		
		[TestMethod]
		public void Test_02() {
			
			Tables.IntTable.Table table = Tables.IntTable.Table.INSTANCE;
			
			using(Transaction transaction = new Transaction(DB.TestDB)) {
				
					Sql.Query.Insert(table)
						.Set(table.Id, Guid.NewGuid())
						.Set(table.IntValue, 1)
						.Execute(transaction);
					
					Sql.Query.Insert(table)
						.Set(table.Id, Guid.NewGuid())
						.Set(table.IntValue, 1)
						.Execute(transaction);
					
					Sql.Query.Insert(table)
						.Set(table.Id, Guid.NewGuid())
						.Set(table.IntValue, 1)
						.Execute(transaction);
					
					Sql.Query.Insert(table)
						.Set(table.Id, Guid.NewGuid())
						.Set(table.IntValue, 2)
						.Execute(transaction);
					
					Sql.Query.Insert(table)
						.Set(table.Id, Guid.NewGuid())
						.Set(table.IntValue, 2)
						.Execute(transaction);
					
					Sql.Query.Insert(table)
						.Set(table.Id, Guid.NewGuid())
						.Set(table.IntValue, 3)
						.Execute(transaction);
					
					Sql.Query.Insert(table)
						.Set(table.Id, Guid.NewGuid())
						.Set(table.IntValue, 3)
						.Execute(transaction);
				
				transaction.Commit();
			}
			
			Sql.Function.AvgInteger avg = new Sql.Function.AvgInteger(table.IntValue);
			Sql.Function.MinInteger min = new Sql.Function.MinInteger(table.IntValue);
			Sql.Function.MaxInt max = new Sql.Function.MaxInt(table.IntValue);
			Sql.Function.SumInteger sum = new Sql.Function.SumInteger(table.IntValue);
			Sql.Function.RowNumber rowNumber = new Sql.Function.RowNumber();
			Sql.Function.Rank rank = new Sql.Function.Rank();
			Sql.Function.DenseRank denseRank = new Sql.Function.DenseRank();
			
			
			IResult result;
			
			if(DB.TestDB.DatabaseType == DatabaseType.PostgreSql){
				
				result = Query.Select(
					table.IntValue, avg.OverPartitionBy(table.IntValue).OrderBy(table.IntValue),
					min.OverPartitionBy(table.IntValue),
					max.OverPartitionBy(table.IntValue),
					sum.OverPartitionBy(table.IntValue),
					rowNumber.Over(),
					rank.Over().OrderBy(table.IntValue),
					denseRank.Over().OrderBy(table.IntValue)
				)
				.From(table)
				.OrderBy(table.IntValue.ASC)
				.Execute(DB.TestDB);
			}
			else if(DB.TestDB.DatabaseType == DatabaseType.Mssql) {
				
				result = Query.Select(
					table.IntValue, avg.OverPartitionBy(table.IntValue),	//Sql server doesn't allow partition-order by syntax for non 'Ranking' window functions where as postgreSql does
					min.OverPartitionBy(table.IntValue),
					max.OverPartitionBy(table.IntValue),
					sum.OverPartitionBy(table.IntValue),
					rowNumber.Over().OrderBy(table.IntValue),
					rank.Over().OrderBy(table.IntValue),	//Sql server requires an order by clause with ranking window functions where as postgreSql doesn't
					denseRank.Over().OrderBy(table.IntValue)
				)
				.From(table)
				.OrderBy(table.IntValue.ASC)
				.Execute(DB.TestDB);
			}
			else
				throw new Exception("Unknown database type");
			
			Assert.AreEqual(7, result.Count);			
			
			Assert.AreEqual(table[0, result].IntValue, 1);
			Assert.AreEqual(table[1, result].IntValue, 1);
			Assert.AreEqual(table[2, result].IntValue, 1);
			
			Assert.AreEqual(table[3, result].IntValue, 2);
			Assert.AreEqual(table[4, result].IntValue, 2);
			
			Assert.AreEqual(table[5, result].IntValue, 3);
			Assert.AreEqual(table[6, result].IntValue, 3);
			
			Assert.AreEqual(min[0, result].Value, 1);
			Assert.AreEqual(min[1, result].Value, 1);
			Assert.AreEqual(min[2, result].Value, 1);
			
			Assert.AreEqual(min[3, result].Value, 2);
			Assert.AreEqual(min[4, result].Value, 2);
			
			Assert.AreEqual(min[5, result].Value, 3);
			Assert.AreEqual(min[6, result].Value, 3);
			
			Assert.AreEqual(max[0, result].Value, 1);
			Assert.AreEqual(max[1, result].Value, 1);
			Assert.AreEqual(max[2, result].Value, 1);
			
			Assert.AreEqual(max[3, result].Value, 2);
			Assert.AreEqual(max[4, result].Value, 2);
			
			Assert.AreEqual(max[5, result].Value, 3);
			Assert.AreEqual(max[6, result].Value, 3);
			
			Assert.AreEqual(sum[0, result].Value, 3);
			Assert.AreEqual(sum[1, result].Value, 3);
			Assert.AreEqual(sum[2, result].Value, 3);
			
			Assert.AreEqual(sum[3, result].Value, 4);
			Assert.AreEqual(sum[4, result].Value, 4);
			
			Assert.AreEqual(sum[5, result].Value, 6);
			Assert.AreEqual(sum[6, result].Value, 6);	
			
			Assert.AreEqual(avg[0, result].Value, 1);
			Assert.AreEqual(avg[1, result].Value, 1);
			Assert.AreEqual(avg[2, result].Value, 1);
			
			Assert.AreEqual(avg[3, result].Value, 2);
			Assert.AreEqual(avg[4, result].Value, 2);
			
			Assert.AreEqual(avg[5, result].Value, 3);
			Assert.AreEqual(avg[6, result].Value, 3);
			
			Assert.AreEqual(rowNumber[0, result].Value, 1);
			Assert.AreEqual(rowNumber[1, result].Value, 2);
			Assert.AreEqual(rowNumber[2, result].Value, 3);
			
			Assert.AreEqual(rowNumber[3, result].Value, 4);
			Assert.AreEqual(rowNumber[4, result].Value, 5);
			
			Assert.AreEqual(rowNumber[5, result].Value, 6);
			Assert.AreEqual(rowNumber[6, result].Value, 7);
			
			
			Assert.AreEqual(rank[0, result].Value, 1);
			Assert.AreEqual(rank[1, result].Value, 1);
			Assert.AreEqual(rank[2, result].Value, 1);
			
			Assert.AreEqual(rank[3, result].Value, 4);
			Assert.AreEqual(rank[4, result].Value, 4);
			
			Assert.AreEqual(rank[5, result].Value, 6);
			Assert.AreEqual(rank[6, result].Value, 6);
			
			
			Assert.AreEqual(denseRank[0, result].Value, 1);
			Assert.AreEqual(denseRank[1, result].Value, 1);
			Assert.AreEqual(denseRank[2, result].Value, 1);
			
			Assert.AreEqual(denseRank[3, result].Value, 2);
			Assert.AreEqual(denseRank[4, result].Value, 2);
			
			Assert.AreEqual(denseRank[5, result].Value, 3);
			Assert.AreEqual(denseRank[6, result].Value, 3);
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