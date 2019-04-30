
/*
 * 
 * Copyright (C) 2009-2019 JFo.nz
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
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Sql.Tables;

namespace Sql.Tests {
	
	[TestClass]
	public class BulkInsertTest {
		
		[TestInitialize()]
		public void Init() {
			
			using(Transaction transaction = new Transaction(DB.TestDB)){
				
				Tables.GuidTable.Table table = Tables.GuidTable.Table.INSTANCE;
				
				Query.Delete(table).NoWhereCondition.Execute(transaction);
				
				transaction.Commit();
			}
		}
		
		[TestMethod]
		public void Test_01() {
			
			using(Transaction transaction = new Transaction(DB.TestDB)) {
				
				Tables.GuidTable.Table table = Tables.GuidTable.Table.INSTANCE;
				
				BulkInsert bulkInsert = new BulkInsert();
				
				int count = 100000;
				
				for (int index = 0; index < count; index++) {
					
					bulkInsert.AddValues(
						Sql.Query.Insert(table).Set(table.Id, Guid.NewGuid())
					);
				}
				
				System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
				
				sw.Start();
				
				int rows = bulkInsert.Execute(transaction);
				
				sw.Stop();
				
				long time = sw.ElapsedMilliseconds;

				Assert.AreEqual(count, rows);
				
				IResult result = Query.Select(table.Id).From(table).Execute(transaction);
				
				Assert.AreEqual(count, result.Count);
				
				transaction.Commit();
			}
		}
		
		[TestMethod]
		public void Test_02() {
			
			using(Transaction transaction = new Transaction(DB.TestDB)) {
				
				Tables.GuidTable.Table table = Tables.GuidTable.Table.INSTANCE;
				
				int count = 100000;
				
				System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
				sw.Start();
				
				for (int index = 0; index < count; index++) {
					Sql.Query.Insert(table).Set(table.Id, Guid.NewGuid()).Execute(transaction);
				}
				
				sw.Stop();
				
				long time = sw.ElapsedMilliseconds;
				
				IResult result = Query.Select(table.Id).From(table).Execute(transaction);
				
				Assert.AreEqual(count, result.Count);
				
				transaction.Commit();
			}
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