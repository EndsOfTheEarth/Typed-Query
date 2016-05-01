
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
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Sql.Tests {
	
	[TestClass]
	public class AutoBigIntTest {
		
		private bool mRunOnce = false;

		[TestInitialize()]
		public void Init() {
			
			Transaction transaction = new Transaction(DB.TestDB);
			
			try {
				
				Tables.BigIntTable.Table table = Tables.BigIntTable.Table.INSTANCE;
				
				Query.Delete(table).NoWhereCondition.Execute(transaction);
				
				if(!mRunOnce) {
					
					if(DB.TestDB.DatabaseType == DatabaseType.Mssql) {
						Query.ExecuteNonQuery("DBCC CHECKIDENT (" + table.TableName + ", RESEED, 1)",DB.TestDB, transaction);
					}
					else if(DB.TestDB.DatabaseType == DatabaseType.PostgreSql){
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
		
		[TestMethod]
		public void Test_01(){
			
			Int64 id = -1;
			
			using(Transaction transaction = new Transaction(DB.TestDB)){
				
				Tables.BigIntTable.Row row = new Sql.Tables.BigIntTable.Row();
				
				row.Update(transaction);
				
				transaction.Commit();
				
				id = row.Id;
			}
			
			Tables.BigIntTable.Table autoTable = Tables.BigIntTable.Table.INSTANCE;
			
			IResult result = Query.Select(autoTable.Id, autoTable.IntValue)
										.From(autoTable)
										.Where(autoTable.Id == id)
										.Execute(DB.TestDB);
			
			Assert.AreEqual(1, result.Count);
			Assert.AreEqual(id, autoTable[0, result].Id);
			Assert.AreEqual(null, autoTable[0, result].IntValue);
		}

		[TestMethod]
		public void Test_02(){
			
			using(Transaction transaction = new Transaction(DB.TestDB)){
				
				Tables.BigIntTable.Row row = new Sql.Tables.BigIntTable.Row();
				
				row.Update(transaction);
				
				Int64 id = row.Id;
				
				transaction.Commit();
			}
		}

		[TestMethod]
		public void Test_03(){
			Tables.BigIntTable.Row row = new Sql.Tables.BigIntTable.Row();
			try {
				Int64 id = row.Id;
			}
			catch(Exception e){
				if(e.Message != "Auto id on column 'Id' has not been set. Row probably hasn't been persisted to database")
					throw e;
				return;
			}
			Assert.IsTrue(false);
		}

		[TestMethod]
		public void Test_04(){
			
			Tables.BigIntTable.Row row = new Sql.Tables.BigIntTable.Row();
			
			for(int index = 0; index < 5; index++){
				
				Int64 id = -1;
				
				using(Transaction transaction = new Transaction(DB.TestDB)) {
					
					
					row.Update(transaction);
					
					id = row.Id;
				}	//Does a rollback
				
				try {
					id = row.Id;
				}
				catch(Exception e){
					if(e.Message != "Auto id on column 'Id' has not been set. Row probably hasn't been persisted to database")
						throw e;
					continue;
				}
				Assert.IsTrue(false);
			}
		}

		[TestMethod]
		public void ParametersTurnedOff() {
			
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
				Init();
				Test_03();
				Init();
				Test_04();
			}
			finally {
				Settings.UseParameters = true;
			}
		}

		[TestMethod]
		public void Permissions() {
			
			Sql.Database.SqlServer.GrantPermissions grants = new Sql.Database.SqlServer.GrantPermissions();
			
			grants.AddPermission(Tables.BigIntTable.Table.INSTANCE, Sql.Database.SqlServer.Permission.Select, Sql.Database.SqlServer.Permission.Insert);
			grants.AddPermission(Tables.DecimalTable.Table.INSTANCE, Sql.Database.SqlServer.Permission.Select, Sql.Database.SqlServer.Permission.Delete);
			
			string readonlySqlScript = grants.CreateGrantScript("Secure", true);
			string sqlScript = grants.CreateGrantScript("Secure", false);
			
			return;
		}
	}
}