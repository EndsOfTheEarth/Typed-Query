
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
	public class NDecimalTest {
		
		[TestInitialize()]
		public void Init() {
			
			Transaction transaction = new Transaction(DB.TestDB);
			
			try {
				
				Query.Delete(Tables.NDecimalTable.Table.INSTANCE).NoWhereCondition.Execute(transaction);
				Query.Delete(Tables.DecimalTable.Table.INSTANCE).NoWhereCondition.Execute(transaction);
				
				transaction.Commit();
			}
			catch(Exception e) {
				transaction.Rollback();
				throw e;
			}
		}
		
		private decimal mDecimal1 = 25.250m;
		
		[TestMethod]
		public void Test_01() {
			
			using(Transaction transaction = new Transaction(DB.TestDB)){
				
				Tables.NDecimalTable.Row row = new Sql.Tables.NDecimalTable.Row();
				
				row.DecimalValue = mDecimal1;
				row.Update(transaction);
				
				transaction.Commit();
			}
			
			Tables.NDecimalTable.Table table = Tables.NDecimalTable.Table.INSTANCE;
			
			IResult result = Query.Select(table.DecimalValue).From(table).Execute(DB.TestDB);
			Assert.AreEqual(1, result.Count);
			Assert.AreEqual(mDecimal1, table[0, result].DecimalValue);
			
			result = Query.Select(table.DecimalValue).From(table).Where(table.DecimalValue == mDecimal1).Execute(DB.TestDB);
			Assert.AreEqual(1, result.Count);
			Assert.AreEqual(mDecimal1, table[0, result].DecimalValue);
			
			result = Query.Select(table.DecimalValue).From(table).Where(table.DecimalValue != mDecimal1).Execute(DB.TestDB);
			Assert.AreEqual(0, result.Count);
			
			result = Query.Select(table.DecimalValue).From(table).Where(table.DecimalValue > 0).Execute(DB.TestDB);
			Assert.AreEqual(1, result.Count);
			Assert.AreEqual(mDecimal1, table[0, result].DecimalValue);
			
			result = Query.Select(table.DecimalValue).From(table).Where(table.DecimalValue < 0).Execute(DB.TestDB);
			Assert.AreEqual(0, result.Count);
			
			result = Query.Select(table.DecimalValue).From(table).Where(table.DecimalValue <= mDecimal1).Execute(DB.TestDB);
			Assert.AreEqual(1, result.Count);
			Assert.AreEqual(mDecimal1, table[0, result].DecimalValue);
			
			result = Query.Select(table.DecimalValue).From(table).Where(table.DecimalValue >= mDecimal1).Execute(DB.TestDB);
			Assert.AreEqual(1, result.Count);
			Assert.AreEqual(mDecimal1, table[0, result].DecimalValue);
			
			result = Query.Select(table.DecimalValue).From(table).Where(table.DecimalValue <= (mDecimal1 - 1m)).Execute(DB.TestDB);
			Assert.AreEqual(0, result.Count);
			
			result = Query.Select(table.DecimalValue).From(table).Where(table.DecimalValue >= (mDecimal1 + 1m)).Execute(DB.TestDB);
			Assert.AreEqual(0, result.Count);
			
			result = Query.Select(table.DecimalValue).From(table).Where(table.DecimalValue.In(mDecimal1, 5m)).Execute(DB.TestDB);
			Assert.AreEqual(1, result.Count);
			Assert.AreEqual(mDecimal1, table[0, result].DecimalValue);
			
			result = Query.Select(table.DecimalValue).From(table).Where(table.DecimalValue.NotIn(mDecimal1, 5m)).Execute(DB.TestDB);
			Assert.AreEqual(0, result.Count);			
			
			System.Collections.Generic.List<decimal> list = new System.Collections.Generic.List<decimal>();
			list.Add(mDecimal1);
			
			result = Query.Select(table.DecimalValue).From(table).Where(table.DecimalValue.In(list)).Execute(DB.TestDB);
			Assert.AreEqual(1, result.Count);
			Assert.AreEqual(mDecimal1, table[0, result].DecimalValue);
			
			result = Query.Select(table.DecimalValue).From(table).Where(table.DecimalValue.NotIn(list)).Execute(DB.TestDB);
			Assert.AreEqual(0, result.Count);			
			
			Tables.NDecimalTable.Table table2 = new Sql.Tables.NDecimalTable.Table();
			
			result = Query.Select(table, table2)
				.From(table)
				.Join(table2, table.DecimalValue == table2.DecimalValue)
				.Execute(DB.TestDB);
			
			Assert.AreEqual(1, result.Count);
			Assert.AreEqual(mDecimal1, table[0, result].DecimalValue);
			Assert.AreEqual(mDecimal1, table2[0, result].DecimalValue);
			
			result = Query.Select(table, table2)
				.From(table)
				.Join(table2, table.DecimalValue != table2.DecimalValue)
				.Execute(DB.TestDB);
			
			Assert.AreEqual(0, result.Count);
			
			result = Query.Select(table, table2)
				.From(table)
				.Join(table2, table.DecimalValue >= table2.DecimalValue)
				.Execute(DB.TestDB);
			
			Assert.AreEqual(1, result.Count);
			Assert.AreEqual(mDecimal1, table[0, result].DecimalValue);
			Assert.AreEqual(mDecimal1, table2[0, result].DecimalValue);
			
			result = Query.Select(table, table2)
				.From(table)
				.Join(table2, table.DecimalValue <= table2.DecimalValue)
				.Execute(DB.TestDB);
			
			Assert.AreEqual(1, result.Count);
			Assert.AreEqual(mDecimal1, table[0, result].DecimalValue);
			Assert.AreEqual(mDecimal1, table2[0, result].DecimalValue);
			
			result = Query.Select(table, table2)
				.From(table)
				.Join(table2, table.DecimalValue < table2.DecimalValue)
				.Execute(DB.TestDB);
			
			Assert.AreEqual(0, result.Count);
			
			result = Query.Select(table, table2)
				.From(table)
				.Join(table2, table.DecimalValue > table2.DecimalValue)
				.Execute(DB.TestDB);
			
			Assert.AreEqual(0, result.Count);
			
			result = Query.Select(table)
				.From(table)
				.Where(table.DecimalValue.In(Query.Select(table2.DecimalValue).Distinct.From(table2)))
				.Execute(DB.TestDB);
			
			Assert.AreEqual(1, result.Count);
			Assert.AreEqual(mDecimal1, table[0, result].DecimalValue);
			
			result = Query.Select(table)
				.From(table)
				.Where(table.DecimalValue.NotIn(Query.Select(table2.DecimalValue).Distinct.From(table2)))
				.Execute(DB.TestDB);
			
			Assert.AreEqual(0, result.Count);
		}
		
		[TestMethod]
		public void Test_02() {
			
			using(Transaction transaction = new Transaction(DB.TestDB)) {
				
				Tables.NDecimalTable.Row row = new Sql.Tables.NDecimalTable.Row();
				
				row.DecimalValue = null;
				row.Update(transaction);
				
				Tables.DecimalTable.Row decRow = new Sql.Tables.DecimalTable.Row();
				
				decRow.DecimalValue = mDecimal1;
				decRow.Update(transaction);
				
				transaction.Commit();
			}
			
			Tables.NDecimalTable.Table table = Tables.NDecimalTable.Table.INSTANCE;
			
			IResult result = Query.Select(table.DecimalValue).From(table).Execute(DB.TestDB);
			Assert.AreEqual(1, result.Count);
			Assert.AreEqual(null, table[0, result].DecimalValue);
			
			result = Query.Select(table.DecimalValue).From(table).Where(table.DecimalValue.IsNull).Execute(DB.TestDB);
			Assert.AreEqual(1, result.Count);
			Assert.AreEqual(null, table[0, result].DecimalValue);
			
			result = Query.Select(table.DecimalValue).From(table).Where(table.DecimalValue.IsNotNull).Execute(DB.TestDB);
			Assert.AreEqual(0, result.Count);
			
			Tables.DecimalTable.Table decTable = Tables.DecimalTable.Table.INSTANCE;
			
			result = Query.Select(table, decTable)
				.From(table)
				.Join(decTable, table.DecimalValue == decTable.DecimalValue)
				.Execute(DB.TestDB);
			
			Assert.AreEqual(0, result.Count);
			
			result = Query.Select(table, decTable)
				.From(table)
				.Join(decTable, table.DecimalValue != decTable.DecimalValue)
				.Execute(DB.TestDB);
			
			Assert.AreEqual(0, result.Count);
			
			using(Transaction transaction = new Transaction(DB.TestDB)) {
				
				Query.Update(table).Set(table.DecimalValue, mDecimal1).NoWhereCondition().Execute(transaction);
				
				transaction.Commit();
			}
			
			result = Query.Select(table, decTable)
				.From(table)
				.Join(decTable, table.DecimalValue == decTable.DecimalValue)
				.Execute(DB.TestDB);
			
			Assert.AreEqual(1, result.Count);
			Assert.AreEqual(mDecimal1, table[0, result].DecimalValue);
			Assert.AreEqual(mDecimal1, decTable[0, result].DecimalValue);
			
			result = Query.Select(table, decTable)
				.From(table)
				.Join(decTable, decTable.DecimalValue == table.DecimalValue)
				.Execute(DB.TestDB);
			
			Assert.AreEqual(1, result.Count);
			Assert.AreEqual(mDecimal1, table[0, result].DecimalValue);
			Assert.AreEqual(mDecimal1, decTable[0, result].DecimalValue);			
			
			result = Query.Select(table, decTable)
				.From(table)
				.Join(decTable, table.DecimalValue != decTable.DecimalValue)
				.Execute(DB.TestDB);
			
			Assert.AreEqual(0, result.Count);
			
			result = Query.Select(table, decTable)
				.From(table)
				.Join(decTable, decTable.DecimalValue != table.DecimalValue)
				.Execute(DB.TestDB);
			
			Assert.AreEqual(0, result.Count);
			
			result = Query.Select(table, decTable)
				.From(table)
				.Join(decTable, table.DecimalValue < decTable.DecimalValue)
				.Execute(DB.TestDB);
			
			Assert.AreEqual(0, result.Count);
			
			result = Query.Select(table, decTable)
				.From(table)
				.Join(decTable, table.DecimalValue > decTable.DecimalValue)
				.Execute(DB.TestDB);
			
			Assert.AreEqual(0, result.Count);
			
			result = Query.Select(table, decTable)
				.From(table)
				.Join(decTable, table.DecimalValue >= decTable.DecimalValue)
				.Execute(DB.TestDB);
			
			Assert.AreEqual(1, result.Count);
			Assert.AreEqual(mDecimal1, table[0, result].DecimalValue);
			Assert.AreEqual(mDecimal1, decTable[0, result].DecimalValue);
			
			result = Query.Select(table, decTable)
				.From(table)
				.Join(decTable, table.DecimalValue <= decTable.DecimalValue)
				.Execute(DB.TestDB);
			
			Assert.AreEqual(1, result.Count);
			Assert.AreEqual(mDecimal1, table[0, result].DecimalValue);
			Assert.AreEqual(mDecimal1, decTable[0, result].DecimalValue);
			
			result = Query.Select(table, decTable)
				.From(table)
				.Join(decTable, decTable.DecimalValue < table.DecimalValue)
				.Execute(DB.TestDB);
			
			Assert.AreEqual(0, result.Count);
			
			result = Query.Select(table, decTable)
				.From(table)
				.Join(decTable, decTable.DecimalValue > table.DecimalValue)
				.Execute(DB.TestDB);
			
			Assert.AreEqual(0, result.Count);
			
			result = Query.Select(table, decTable)
				.From(table)
				.Join(decTable, decTable.DecimalValue >= table.DecimalValue)
				.Execute(DB.TestDB);
			
			Assert.AreEqual(1, result.Count);
			Assert.AreEqual(mDecimal1, table[0, result].DecimalValue);
			Assert.AreEqual(mDecimal1, decTable[0, result].DecimalValue);
			
			result = Query.Select(table, decTable)
				.From(table)
				.Join(decTable, decTable.DecimalValue <= table.DecimalValue)
				.Execute(DB.TestDB);
			
			Assert.AreEqual(1, result.Count);
			Assert.AreEqual(mDecimal1, table[0, result].DecimalValue);
			Assert.AreEqual(mDecimal1, decTable[0, result].DecimalValue);
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