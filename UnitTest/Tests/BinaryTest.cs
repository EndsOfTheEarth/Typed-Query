
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
	public class BinaryTest {
		
		[TestInitialize()]
		public void Init() {
			
			Transaction transaction = new Transaction(DB.TestDB);
			
			try {
				
				Tables.BinaryTable.Table table = Tables.BinaryTable.Table.INSTANCE;
				
				Query.Delete(table).NoWhereCondition.Execute(transaction);
				
				transaction.Commit();
			}
			catch(Exception e){
				transaction.Rollback();
				throw e;
			}
		}

		private bool AreByteArraysEqual(byte[] pBytesA, byte[] pBytesB) {

			if (pBytesA.Length != pBytesB.Length) {
				return false;
			}

			for (int index = 0; index < pBytesA.Length; index++) {
				if (pBytesA[index] != pBytesB[index]) {
					return false;
				}
			}
			return true;
		}
		[TestMethod]
		public void Test_01() {
			
			Tables.BinaryTable.Table table = Tables.BinaryTable.Table.INSTANCE;
			
			byte[] bytes = new byte[]{ 1, 2, 3, 4, 5};
			
			using(Sql.Transaction transaction = new Transaction(DB.TestDB)) {
				
				Sql.Query.Insert(table)
					.Set(table.BinaryValue, bytes)
					.Set(table.NBinaryValue, (byte[]) null)
					.Execute(transaction);
				
				transaction.Commit();
			}
			
			Sql.IResult result = Sql.Query
				.Select(table.BinaryValue, table.NBinaryValue)
				.From(table)
				.Execute(DB.TestDB);
			
			Assert.AreEqual(1, result.Count);
			Assert.IsTrue(AreByteArraysEqual(bytes, table[0, result].BinaryValue));
			Assert.IsTrue(table[0, result].NBinaryValue == null);
			
			result = Sql.Query
				.Select(table.BinaryValue, table.NBinaryValue)
				.From(table)
				.Where(table.NBinaryValue.IsNull)
				.Execute(DB.TestDB);
			
			Assert.AreEqual(1, result.Count);
			Assert.IsTrue(AreByteArraysEqual(bytes, table[0, result].BinaryValue));
			Assert.IsTrue(table[0, result].NBinaryValue == null);
			
			result = Sql.Query
				.Select(table.BinaryValue, table.NBinaryValue)
				.From(table)
				.Where(table.BinaryValue == bytes)
				.Execute(DB.TestDB);
			
			Assert.AreEqual(1, result.Count);
			Assert.IsTrue(AreByteArraysEqual(bytes, table[0, result].BinaryValue));
			Assert.IsTrue(table[0, result].NBinaryValue == null);
			
			result = Sql.Query
				.Select(table.BinaryValue, table.NBinaryValue)
				.From(table)
				.Where(table.BinaryValue != bytes)
				.Execute(DB.TestDB);
			
			Assert.AreEqual(0, result.Count);
		}
		
		[TestMethod]
		public void Test_02() {
			
			Tables.BinaryTable.Table table = Tables.BinaryTable.Table.INSTANCE;
			
			byte[] bytes = new byte[]{ 1, 2, 3, 4, 5};
			
			using(Sql.Transaction transaction = new Transaction(DB.TestDB)) {
				
				Sql.Query.Insert(table)
					.Set(table.BinaryValue, bytes)
					.Set(table.NBinaryValue, bytes)
					.Execute(transaction);
				
				transaction.Commit();
			}
			
			Sql.IResult result = Sql.Query
				.Select(table.BinaryValue, table.NBinaryValue)
				.From(table)
				.Where(table.NBinaryValue.IsNotNull)
				.Execute(DB.TestDB);
			
			Assert.AreEqual(1, result.Count);
			Assert.IsTrue(AreByteArraysEqual(bytes, table[0, result].BinaryValue));
			Assert.IsTrue(AreByteArraysEqual(bytes, table[0, result].NBinaryValue));
			
			result = Sql.Query
				.Select(table.BinaryValue, table.NBinaryValue)
				.From(table)
				.Where(table.NBinaryValue == bytes)
				.Execute(DB.TestDB);
			
			Assert.AreEqual(1, result.Count);
			Assert.IsTrue(AreByteArraysEqual(bytes, table[0, result].BinaryValue));
			Assert.IsTrue(AreByteArraysEqual(bytes, table[0, result].NBinaryValue));
			
			result = Sql.Query
				.Select(table.BinaryValue, table.NBinaryValue)
				.From(table)
				.Where(table.NBinaryValue != bytes)
				.Execute(DB.TestDB);
			
			Assert.AreEqual(0, result.Count);
		}
		
		[TestMethod]
		public void Test_03() {
			
			Tables.BinaryTable.Table table = Tables.BinaryTable.Table.INSTANCE;
			
			byte[] bytes = new byte[]{ 1, 2, 3, 4, 5};
			
			using(Sql.Transaction transaction = new Transaction(DB.TestDB)) {
				
				Tables.BinaryTable.Row row = new Sql.Tables.BinaryTable.Row();
				
				row.BinaryValue = bytes;
				row.NBinaryValue = bytes;
				
				row.Update(transaction);
				transaction.Commit();
			}
			
			Sql.IResult result = Sql.Query
				.Select(table.BinaryValue, table.NBinaryValue)
				.From(table)
				.Execute(DB.TestDB);
			
			Assert.AreEqual(1, result.Count);
			Assert.IsTrue(AreByteArraysEqual(bytes, table[0, result].BinaryValue));
			Assert.IsTrue(AreByteArraysEqual(bytes, table[0, result].NBinaryValue));
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
			}
			finally {
				Settings.UseParameters = true;
			}
		}
	}
}