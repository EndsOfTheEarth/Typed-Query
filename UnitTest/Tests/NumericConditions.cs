
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

namespace Sql.Tests {

	[TestClass]
	public class NumericConditions {

		[TestInitialize()]
		public void Init() {

			using(Transaction transaction = new Transaction(DB.TestDB)) {

				Tables.IntTable.Table table = Tables.IntTable.Table.INSTANCE;

				Query.Delete(table).NoWhereCondition.Execute(transaction);

				transaction.Commit();
			}
		}

		[TestMethod]
		public void Test_01() {

			using(Sql.Transaction transaction = new Transaction(DB.TestDB)) {

				Tables.IntTable.Row row = new Sql.Tables.IntTable.Row();
				row.Id = Guid.NewGuid();
				row.IntValue = 10;

				row.Update(transaction);
				transaction.Commit();
			}

			Tables.IntTable.Table table = Tables.IntTable.Table.INSTANCE;

			string sql = Sql.Query.Select(table.Id)
				.From(table)
				.Where(((table.IntValue + table.IntValue) / table.IntValue) > 0)
				.GetSql(DB.TestDB);

		}
	}
}