
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

using Sql.Tables;

namespace Sql.Tests {
	
	[TestClass]
	public class MetaDataTest {
		
		[TestInitialize()]
		public void Init() {
			
		}
		
		[TestMethod]
		public void Test_01() {
			
			Tables.GuidTable.Table table = Tables.GuidTable.Table.INSTANCE;
			
			Sql.Database.GenerateMetaDataSql metaData = new Sql.Database.GenerateMetaDataSql();
			
			string commentSql = metaData.GenerateSql(table, DB.TestDB);
			
			Tables.EnumTable.Table enumTable = Tables.EnumTable.Table.INSTANCE;
			
			commentSql = metaData.GenerateSql(enumTable, DB.TestDB);
			return;
		}
	}
}