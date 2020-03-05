
/*
 * 
 * Copyright (C) 2009-2020 JFo.nz
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
using Sql.Column;
using System.Collections.Generic;
using System.Data;
using TypedQuery.Logic;

namespace TypedQueryGenerator.Logic.CodeGeneration {
    
    public static class ColumnType {

        public static string GetColumnType(IColumn pColumn, ITableDetails pTable, bool pGenerateKeyTypes) {

            string value;

            List<KeyColumn> matchingKeyColumns = new List<KeyColumn>();

            foreach(IForeignKey foreignKey in pTable.ForeignKeys) {

                foreach(KeyColumn keyColumn in foreignKey.KeyColumns) {

                    if(keyColumn.ForeignKeyColumn == pColumn) {
                        matchingKeyColumns.Add(keyColumn);
                    }
                }
            }

            if(pColumn.DbType == DbType.Boolean) {
                value = !pColumn.IsNullable ? typeof(Sql.Column.BoolColumn).Name : typeof(Sql.Column.NBoolColumn).Name;
            }
            else if(pColumn.DbType == DbType.DateTime) {
                value = !pColumn.IsNullable ? typeof(Sql.Column.DateTimeColumn).Name : typeof(Sql.Column.NDateTimeColumn).Name;
            }
            else if(pColumn.DbType == DbType.DateTime2) {
                value = !pColumn.IsNullable ? typeof(Sql.Column.DateTime2Column).Name : typeof(Sql.Column.NDateTime2Column).Name;
            }
            else if(pColumn.DbType == DbType.DateTimeOffset) {
                value = !pColumn.IsNullable ? typeof(Sql.Column.DateTimeOffsetColumn).Name : typeof(Sql.Column.NDateTimeOffsetColumn).Name;
            }
            else if(pColumn.DbType == DbType.Decimal) {
                value = !pColumn.IsNullable ? typeof(Sql.Column.DecimalColumn).Name : typeof(Sql.Column.NDecimalColumn).Name;
            }

            else if(pColumn.DbType == DbType.Guid || pColumn.DbType == DbType.Int16 || pColumn.DbType == DbType.Int32 || pColumn.DbType == DbType.Int64 || pColumn.DbType == DbType.String) {

                string keyType = (pColumn.DbType, pColumn.IsNullable) switch
                {
                    (DbType.Guid, false) => typeof(GuidKeyColumn<>).Name,
                    (DbType.Guid, true) => typeof(NGuidKeyColumn<>).Name,
                    (DbType.Int16, false) => typeof(SmallIntegerKeyColumn<>).Name,
                    (DbType.Int16, true) => typeof(NSmallIntegerKeyColumn<>).Name,
                    (DbType.Int32, false) => typeof(IntegerKeyColumn<>).Name,
                    (DbType.Int32, true) => typeof(NIntegerKeyColumn<>).Name,
                    (DbType.Int64, false) => typeof(BigIntegerKeyColumn<>).Name,
                    (DbType.Int64, true) => typeof(NBigIntegerKeyColumn<>).Name,
                    (DbType.String, false) => typeof(StringKeyColumn<>).Name,
                    (DbType.String, true) => typeof(NStringKeyColumn<>).Name,
                    _ => "???"
                };

                keyType = keyType.Replace("`1", string.Empty);    //Remove generic tag

                string nonKeyType = (pColumn.DbType, pColumn.IsNullable) switch
                {
                    (DbType.Guid, false) => typeof(GuidColumn).Name,
                    (DbType.Guid, true) => typeof(NGuidColumn).Name,
                    (DbType.Int16, false) => typeof(SmallIntegerColumn).Name,
                    (DbType.Int16, true) => typeof(NSmallIntegerColumn).Name,
                    (DbType.Int32, false) => typeof(IntegerColumn).Name,
                    (DbType.Int32, true) => typeof(NIntegerColumn).Name,
                    (DbType.Int64, false) => typeof(BigIntegerColumn).Name,
                    (DbType.Int64, true) => typeof(NBigIntegerColumn).Name,
                    (DbType.String, false) => typeof(StringColumn).Name,
                    (DbType.String, true) => typeof(NStringColumn).Name,
                    _ => "???"
                };

                if(pGenerateKeyTypes && matchingKeyColumns.Count > 0) {

                    if(matchingKeyColumns.Count == 1) {
                        value = keyType + "<" + GetIdentifierName(matchingKeyColumns[0].PrimaryKeyTableName) + ">";
                    }
                    else {
                        value = keyType + "<" + GetIdentifierName(matchingKeyColumns[0].PrimaryKeyTableName) + "<??? Column Belongs to multipule foreign keys ???>>";
                    }
                }
                else if(pGenerateKeyTypes && pColumn.IsPrimaryKey) {
                    value = keyType + "<" + GetIdentifierName(pTable.TableName) + ">";
                }
                else {
                    value = nonKeyType;
                }
            }
            else if(pColumn.DbType == DbType.Binary) {
                value = !pColumn.IsNullable ? typeof(Sql.Column.BinaryColumn).Name : typeof(Sql.Column.NBinaryColumn).Name;
            }
            else if(pColumn.DbType == DbType.Byte) {
                value = !pColumn.IsNullable ? typeof(Sql.Column.ByteColumn).Name : typeof(Sql.Column.NByteColumn).Name;
            }
            else if(pColumn.DbType == DbType.Single) {
                value = !pColumn.IsNullable ? typeof(Sql.Column.FloatColumn).Name : typeof(Sql.Column.NFloatColumn).Name;
            }
            else if(pColumn.DbType == DbType.Double) {
                value = !pColumn.IsNullable ? typeof(Sql.Column.DoubleColumn).Name : typeof(Sql.Column.NDoubleColumn).Name;
            }
            else {
                value = "UNKNOWN_COLUMN_TYPE";
            }
            return value;
        }

        public static string GetIdentifierName(string pTableName) {
            return pTableName + "Id";
        }
    }
}