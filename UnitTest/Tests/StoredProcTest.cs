
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
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

using Sql.Tables;

namespace Sql.Tests {
	
	[TestClass]
	public class StoredProcTest	{
		
		public StoredProcTest() {
		}
		
		[TestInitialize()]
		public void Init() {
			
		}
		
		[TestMethod]
		public void Test_01() {
			
			if(DB.TestDB.DatabaseType != DatabaseType.Mssql)
				return;

			using(Transaction transaction = new Transaction(DB.TestDB)) {

				StoredProc.SP_Test_In_Out.SP sp_test = Sql.StoredProc.SP_Test_In_Out.SP.INSTANCE;

				int inParam = 12345;
				int outParam = 0;

				IResult result = sp_test.Execute(inParam, out outParam, transaction);

				Assert.AreEqual(inParam, outParam);

				Assert.AreEqual(4, result.Count);

				Assert.AreEqual(12345, sp_test[0, result].IntValue);
				Assert.AreEqual(123456, sp_test[1, result].IntValue);
				Assert.AreEqual(1234567, sp_test[2, result].IntValue);
				Assert.AreEqual(12345678, sp_test[3, result].IntValue);

				transaction.Commit();
			}
		}
	}
}