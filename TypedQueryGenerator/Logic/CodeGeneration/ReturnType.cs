
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
using Sql.Types;
using System.Collections.Generic;
using System.Data;
using TypedQuery.Logic;

namespace TypedQueryGenerator.Logic.CodeGeneration {
    
    public static class ReturnType {

        public static string GetReturnType(DbType pDbType, bool pIsNullable, IColumn? pColumn, ITableDetails? pTable, bool pGenerateKeyTypes) {

            string value;

            List<KeyColumn> matchingKeyColumns = new List<KeyColumn>();

            if(pColumn != null && pTable != null) {

                foreach(IForeignKey foreignKey in pTable.ForeignKeys) {

                    foreach(KeyColumn keyColumn in foreignKey.KeyColumns) {

                        if(keyColumn.ForeignKeyColumn == pColumn) {
                            matchingKeyColumns.Add(keyColumn);
                        }
                    }
                }
            }

            if(pDbType == DbType.Boolean) {
                value = !pIsNullable ? "bool" : "bool?";
            }
            else if(pDbType == DbType.DateTime) {
                value = !pIsNullable ? "DateTime" : "DateTime?";
            }
            else if(pDbType == DbType.DateTime2) {
                value = !pIsNullable ? "DateTime" : "DateTime?";
            }
            else if(pDbType == DbType.DateTimeOffset) {
                value = !pIsNullable ? "DateTimeOffset" : "DateTimeOffset?";
            }
            else if(pDbType == DbType.Decimal) {
                value = !pIsNullable ? "decimal" : "decimal?";
            }

            else if(pDbType == DbType.Guid || pDbType == DbType.Int16 || pDbType == DbType.Int32 || pDbType == DbType.Int64 || pDbType == DbType.String) {

                string keyType = pDbType switch
                {
                    DbType.Guid => typeof(GuidKey<>).Name,
                    DbType.Int16 => typeof(Int16Key<>).Name,
                    DbType.Int32 => typeof(Int32Key<>).Name,
                    DbType.Int64 => typeof(Int64Key<>).Name,
                    DbType.String => typeof(StringKey<>).Name,
                    _ => "???"
                };

                keyType = keyType.Replace("`1", string.Empty);    //Remove generic tag

                string valueType = pDbType switch
                {
                    DbType.Guid => "Guid",
                    DbType.Int16 => "short",
                    DbType.Int32 => "int",
                    DbType.Int64 => "long",
                    DbType.String => "string",
                    _ => "???"
                };

                if(pGenerateKeyTypes && matchingKeyColumns.Count > 0) {

                    if(matchingKeyColumns.Count == 1) {
                        value = $"{ keyType  }<{ ColumnType.GetIdentifierName(matchingKeyColumns[0].PrimaryKeyTableName) }>" + (pIsNullable ? "?" : string.Empty);
                    }
                    else {
                        value = $"{ keyType  }<{ColumnType.GetIdentifierName(matchingKeyColumns[0].PrimaryKeyTableName) }<??? Column Belongs to multipule foreign keys ???>>" + (pIsNullable ? "?" : string.Empty);
                    }
                }
                else if(pGenerateKeyTypes && pColumn != null && pColumn.IsPrimaryKey) {
                    value = $"{ keyType  }<{ (pTable != null ? ColumnType.GetIdentifierName(pTable.TableName) : "???")}>" + (pIsNullable ? "?" : string.Empty);
                }
                else {
                    value = !pIsNullable ? valueType : $"{ valueType  }?";
                }
            }
            else if(pDbType == DbType.Binary) {
                value = !pIsNullable ? "byte[]" : "byte[]?";
            }
            else if(pDbType == DbType.Byte) {
                value = !pIsNullable ? "byte" : "byte?";
            }
            else if(pDbType == DbType.Single) {
                value = !pIsNullable ? "float" : "float?";
            }
            else if(pDbType == DbType.Double) {
                value = !pIsNullable ? "double" : "double?";
            }
            else {
                value = "UNKNOWN_COLUMN_TYPE";
            }
            return value;
        }
    }
}