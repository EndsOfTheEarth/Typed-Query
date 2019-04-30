
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
using System.Threading.Tasks;

namespace Sql.Tests {
	
	[TestClass]
	public class MultithreadTest {
		
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
			
			const int rows = 1000;
			
			List<Guid> idList = new List<Guid>(rows);
			
			using(Transaction transaction = new Transaction(DB.TestDB)) {
				
				for (int index = 0; index < rows; index++) {
					Tables.GuidTable.Row row = new Tables.GuidTable.Row();
					row.Id = Guid.NewGuid();
					row.Update(transaction);
					idList.Add(row.Id);
				}
				transaction.Commit();
			}
			
			Tables.GuidTable.Table table = Tables.GuidTable.Table.INSTANCE;
			
//			for (int index = 0; index < 25; index++) {
				
				int count = 0;
				
				Parallel.ForEach(idList, id => {
				                 	IResult result = Sql.Query.Select(table.Id).From(table).Where(table.Id == id).Execute(DB.TestDB);
				                 	Assert.AreEqual(1, result.Count);
				                 	Assert.AreEqual(id, table[0, result].Id);
				                 	result = Sql.Query.Select(table.Id).From(table).Where(table.Id != id).Execute(DB.TestDB);
				                 	Assert.AreEqual(rows - 1, result.Count);
				                 	lock(table) {
				                 		count += result.Count;
				                 	}
				                 });
				Assert.AreEqual(rows * (rows - 1), count);
//			}
		}
		
		[TestMethod]
		public void Test_02() {
			
			using(Sql.Transaction transaction = new Transaction(DB.TestDB)) {
				
				Tables.GuidTable.Row row = new Tables.GuidTable.Row();
				
				row.Id = Guid.NewGuid();
				row.Update(transaction);
				
				try {
					//Timeout test
					Settings.DefaultTimeout = 1;
					
					Tables.GuidTable.Table table = Tables.GuidTable.Table.INSTANCE;
					
					Sql.Query.Select(table).From(table).Execute(DB.TestDB);	//Query against a locked row
				}
				catch(System.Data.SqlClient.SqlException e) {					
					if(!e.Message.Contains("Timeout expired"))
						throw e;
				}
				finally {
					Settings.DefaultTimeout = 30;
				}
			}
		}
		
		[TestMethod]
		public void Test_03() {
			
			using(Sql.Transaction transaction = new Transaction(DB.TestDB)) {
				
				Tables.GuidTable.Row row = new Tables.GuidTable.Row();
				
				row.Id = Guid.NewGuid();
				row.Update(transaction);
				
				try {
					
					Tables.GuidTable.Table table = Tables.GuidTable.Table.INSTANCE;
					//This probably works on postgreSql because of its locking model
					Sql.Query.Select(table).From(table).Timeout(1).Execute(DB.TestDB);	//Query against a locked row
				}
				catch(System.Data.SqlClient.SqlException e) {					
					if(!e.Message.Contains("Timeout expired"))
						throw e;
				}
				finally {
					
				}
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