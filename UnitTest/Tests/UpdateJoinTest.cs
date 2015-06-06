
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
using Sql.Tables;

namespace Sql.Tests {
	
	[TestClass]
	public class UpdateJoinTest {
	
		[TestInitialize()]
		public void Init() {
			
			Transaction transaction = new Transaction(DB.TestDB);
			
			try {
				
				Tables.DecimalTable.Table table = Tables.DecimalTable.Table.INSTANCE;
				
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
				
				Tables.DecimalTable.Row rowA = new Tables.DecimalTable.Row();
				rowA.DecimalValue = 1;
				rowA.Update(transaction);
				
				Tables.DecimalTable.Row rowB = new Tables.DecimalTable.Row();
				rowB.DecimalValue = 2;
				rowB.Update(transaction);
				
				Tables.DecimalTable.Table tableA = Tables.DecimalTable.Table.INSTANCE;
				Tables.DecimalTable.Table tableB = new Sql.Tables.DecimalTable.Table();
				
				IResult result = Sql.Query.Update(tableA)
					.Set(tableA.DecimalValue, tableB.DecimalValue)
					.Join(tableB, tableA.Id != tableB.Id)
					.Where(tableA.DecimalValue == 1)
					.Execute(transaction);
				
				Assert.AreEqual(result.RowsEffected, 1);
				
				result = Query.Select(tableA).From(tableA).Execute(transaction);
				
				Assert.AreEqual(2, result.Count);
				
				for (int index = 0; index < result.Count; index++) {
					
					Tables.DecimalTable.Row row = tableA[index, result];
					
					Assert.AreEqual(row.DecimalValue, rowB.DecimalValue);
				}
				
				transaction.Commit();
			}
			catch(Exception e){
				transaction.Rollback();
				throw e;
			}
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