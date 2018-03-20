
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
	public class SelectDistinct {
	
		[TestInitialize()]
		public void Init() {

			using(Transaction transaction = new Transaction(DB.TestDB)) {

				Tables.GuidTable.Table table = Tables.GuidTable.Table.INSTANCE;

				Query.Delete(table).NoWhereCondition.Execute(transaction);

				transaction.Commit();
			}
		}
		
		[TestMethod]
		public void Test_01(){

			using(Transaction transaction = new Transaction(DB.TestDB)) {

				Guid id = Guid.NewGuid();
				Guid id2 = Guid.NewGuid();

				Tables.GuidTable.Table table = Tables.GuidTable.Table.INSTANCE;

				IResult insertResult = Query.Insert(table).Set(table.Id, id).Execute(transaction);
				Assert.AreEqual(1, insertResult.RowsEffected);

				insertResult = Query.Insert(table).Set(table.Id, id2).Execute(transaction);
				Assert.AreEqual(1, insertResult.RowsEffected);

				IResult result = Query.Select(table.Id).From(table).Execute(transaction);
				Assert.AreEqual(2, result.Count);

				result = Query.Select(table.Id).Distinct.From(table).Execute(transaction);
				Assert.AreEqual(2, result.Count);

				Assert.IsTrue(result.SqlQuery.ToLower().Contains("distinct"));

				transaction.Commit();
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