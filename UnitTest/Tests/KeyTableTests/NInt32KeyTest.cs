﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Sql.Tables.KeyTables.NInt32KeyTestTable;
using System.Collections.Generic;
using Sql.Types;

namespace Sql.Tests {

    [TestClass]
    public class NInt32KeyTest {

        [TestInitialize()]
        public void Init() {

            using(Transaction transaction = new Transaction(DB.TestDB)) {

                Table table = new Table();

                Sql.Query.Delete(table).NoWhereCondition.Execute(transaction);

                transaction.Commit();
            }
        }

        [TestMethod]
        public void Test_01() {

            Table table = new Table();

            List<Int32Key<Table>> list = new List<Int32Key<Table>>();

            int iterations = 10;

            using(Transaction transaction = new Transaction(DB.TestDB)) {

                for(int index = 0; index < iterations; index++) {

                    Int32Key<Table> id = new Int32Key<Table>(index);

                    list.Add(id);
                    Row row = new Row();
                    row.Id = id;
                    row.Update(transaction);
                }
                transaction.Commit();
            }

            Sql.IResult result = Sql.Query
                .Select(table.Id)
                .From(table)
                .Where(table.Id.In(list))
                .Execute(DB.TestDB);

            Assert.AreEqual(iterations, result.Count);

            for(int index = 0; index < result.Count; index++) {
                Row row = table.GetRow(index, result);
                Assert.IsTrue(list.Contains(row.Id!));
            }

            result = Sql.Query
               .Select(table.Id)
               .From(table)
               .Where(table.Id.In(list.ToArray()))
               .Execute(DB.TestDB);

            Assert.AreEqual(iterations, result.Count);

            for(int index = 0; index < result.Count; index++) {
                Row row = table.GetRow(index, result);
                Assert.IsTrue(list.Contains(row.Id!));
            }

            list.Clear();

            for(int index = 0; index < iterations; index++) {
                list.Add(new Int32Key<Table>(index + 1000));
            }

            result = Sql.Query
               .Select(table.Id)
               .From(table)
               .Where(table.Id.NotIn(list))
               .Execute(DB.TestDB);

            Assert.AreEqual(iterations, result.Count);

            for(int index = 0; index < result.Count; index++) {
                Row row = table.GetRow(index, result);
                Assert.IsTrue(row.Id!.Value != int.MaxValue);
            }

            result = Sql.Query
               .Select(table.Id)
               .From(table)
               .Where(table.Id.NotIn(list.ToArray()))
               .Execute(DB.TestDB);

            Assert.AreEqual(iterations, result.Count);

            for(int index = 0; index < result.Count; index++) {
                Row row = table.GetRow(index, result);
                Assert.IsTrue(row.Id!.Value != int.MaxValue);
            }
        }

        [TestMethod]
        public void Test_02() {

            Table table = new Table();

            List<Int32Key<Table>> list = new List<Int32Key<Table>>();

            int iterations = 10;

            using(Transaction transaction = new Transaction(DB.TestDB)) {

                for(int index = 0; index < iterations; index++) {

                    Int32Key<Table> id = new Int32Key<Table>(index);

                    list.Add(id);
                    Row row = new Row();
                    row.Id = id;
                    row.Update(transaction);
                }
                transaction.Commit();
            }

            foreach(Int32Key<Table> key in list) {

                Sql.IResult result = Sql.Query
                    .Select(table)
                    .From(table)
                    .Where(table.Id == key)
                    .Execute(DB.TestDB);

                Assert.AreEqual(result.Count, 1);
                Assert.AreEqual(table.GetRow(0, result).Id, key);
            }

            foreach(Int32Key<Table> key in list) {

                Sql.IResult result = Sql.Query
                    .Select(table)
                    .From(table)
                    .Where(table.Id != key) //Not equal
                    .Execute(DB.TestDB);

                Assert.AreEqual(result.Count, iterations - 1);
            }
        }

        [TestMethod]
        public void Test_03() {

            Table table = new Table();

            List<Int32Key<Table>> list = new List<Int32Key<Table>>();

            int iterations = 10;

            using(Transaction transaction = new Transaction(DB.TestDB)) {

                for(int index = 0; index < iterations; index++) {

                    Int32Key<Table> id = new Int32Key<Table>(index);

                    list.Add(id);
                    Row row = new Row();
                    row.Id = id;
                    row.Update(transaction);
                }
                transaction.Commit();
            }

            Table table2 = new Table();
            foreach(Int32Key<Table> key in list) {

                Sql.IResult result = Sql.Query
                    .Select(table, table2)
                    .From(table)
                    .Join(table2, table.Id == table2.Id)    //Test column join
                    .Where(table.Id == key)
                    .Execute(DB.TestDB);

                Assert.AreEqual(result.Count, 1);
                Assert.AreEqual(table.GetRow(0, result).Id, key);
            }

            foreach(Int32Key<Table> key in list) {

                Sql.IResult result = Sql.Query
                    .Select(table, table2)
                    .From(table)
                    .Join(table2, table.Id != table2.Id)    //Test column join not equal
                    .Where(table.Id == key)
                    .Execute(DB.TestDB);

                Assert.AreEqual(result.Count, iterations - 1);
                Assert.AreEqual(table.GetRow(0, result).Id, key);
            }
        }

        [TestMethod]
        public void Test_04() {

            Table table = new Table();

            List<Int32Key<Table>> list = new List<Int32Key<Table>>();

            int iterations = 10;

            using(Transaction transaction = new Transaction(DB.TestDB)) {

                for(int index = 0; index < iterations; index++) {

                    Int32Key<Table> id = new Int32Key<Table>(index);

                    list.Add(id);
                    Row row = new Row();
                    row.Id = id;
                    row.Update(transaction);
                }
                transaction.Commit();
            }

            Table table2 = new Table();

            foreach(Int32Key<Table> key in list) {

                Sql.IResult result = Sql.Query
                    .Select(table)
                    .From(table)
                    .Where(
                        table.Id.In(
                            Sql.Query
                            .Select(table2.Id)
                            .From(table2)
                            .Where(table2.Id == key)
                        )
                    )
                    .Execute(DB.TestDB);

                Assert.AreEqual(result.Count, 1);
                Assert.AreEqual(table.GetRow(0, result).Id, key);
            }

            foreach(Int32Key<Table> key in list) {

                Sql.IResult result = Sql.Query
                    .Select(table)
                    .From(table)
                    .Where(
                        table.Id.In(
                            Sql.Query
                            .Select(table2.Id)
                            .From(table2)
                            .Where(table2.Id != key)
                        )
                    )
                    .Execute(DB.TestDB);

                Assert.AreEqual(result.Count, iterations - 1);
            }
        }

        [TestMethod]
        public void Test_05() {

            Table table = new Table();

            List<Int32Key<Table>> list = new List<Int32Key<Table>>();

            int iterations = 10;

            using(Transaction transaction = new Transaction(DB.TestDB)) {

                for(int index = 0; index < iterations; index++) {

                    Int32Key<Table> id = new Int32Key<Table>(index);

                    list.Add(id);

                    Sql.IResult insertResult = Sql.Query
                        .Insert(table)
                        .Set(table.Id, id)
                        .Execute(transaction);

                    Assert.AreEqual(insertResult.RowsEffected, 1);
                }
                transaction.Commit();
            }

            Sql.IResult result = Sql.Query
                .Select(table.Id)
                .From(table)
                .Where(table.Id.In(list))
                .Execute(DB.TestDB);

            Assert.AreEqual(iterations, result.Count);

            for(int index = 0; index < result.Count; index++) {
                Row row = table.GetRow(index, result);
                Assert.IsTrue(list.Contains(row.Id!));
            }

            using(Transaction transaction = new Transaction(DB.TestDB)) {

                foreach(Int32Key<Table> id in list) {

                    Int32Key<Table> newId = new Int32Key<Table>(id.Value + 1000);

                    Sql.IResult updateResult = Sql.Query
                        .Update(table)
                        .Set(table.Id, newId)
                        .Where(table.Id == id)
                        .Execute(transaction);

                    Assert.AreEqual(updateResult.RowsEffected, 1);

                    result = Sql.Query
                        .Select(table)
                        .From(table)
                        .Where(table.Id == newId)
                        .Execute(transaction);

                    Assert.AreEqual(result.Count, 1);
                    Assert.AreEqual(table.GetRow(0, result).Id, newId);
                }
                transaction.Commit();
            }
        }

        [TestMethod]
        public void Test_06() {

            Table table = new Table();

            using(Sql.Transaction transaction = new Transaction(DB.TestDB)) {

                Int32Key<Table> id = new Int32Key<Table>(1);

                Sql.IResult result = Sql.Query
                    .Insert(table)
                    .Set(table.Id, id)
                    .Execute(transaction);

                Assert.AreEqual(result.RowsEffected, 1);

                result = Sql.Query
                    .Select(table)
                    .From(table)
                    .Where(table.Id.IsNull)
                    .Execute(transaction);

                Assert.AreEqual(result.Count, 0);

                result = Sql.Query
                    .Select(table)
                    .From(table)
                    .Where(table.Id.IsNotNull)
                    .Execute(transaction);

                Assert.AreEqual(result.Count, 1);
                Assert.AreEqual(table.GetRow(0, result).Id, id);

                transaction.Commit();
            }
        }
    }
}