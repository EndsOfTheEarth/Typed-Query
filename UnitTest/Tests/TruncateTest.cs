
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
	public class TruncateTest {
		
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
			
			IResult result = Query.Select(table).From(table).Execute(DB.TestDB);
			
			Assert.AreEqual(rows, result.Count);
			
			using(Transaction transaction = new Transaction(DB.TestDB)) {
				Sql.Query.Truncate(table).Execute(transaction);
				transaction.Commit();
			}
			
			result = Query.Select(table).From(table).Execute(DB.TestDB);
			
			Assert.AreEqual(0, result.Count);
		}
	}
}
