
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
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace TypedQuery.Logic {

    public class ValidationError {

        public string Schema { get; private set; }
        public string Table { get; private set; }
        public string Column { get; private set; }
        public string Message { get; private set; }

        public ValidationError(string pSchema, string pTable, string pColumn, string pMessage) {
            Schema = pSchema;
            Table = pTable;
            Column = pColumn;
            Message = pMessage;
        }
    }

    public static class DefinitionChecker {

        public static List<ValidationError> CheckTable(Sql.ATable pTable, ITableDetails? pTableDetails) {

            List<ValidationError> issues = new List<ValidationError>();

            if(pTableDetails == null) {
                issues.Add(new ValidationError(pTable.Schema, pTable.TableName, string.Empty, $"Unable to check table as { nameof(pTableDetails) } does not exist"));
                return issues;
            }

            if(pTableDetails.IsView != pTable.IsView) {
                issues.Add(new ValidationError(pTable.Schema, pTable.TableName, string.Empty, "IsView settings is different between code table and database view."));
            }

            for(int index = 0; index < pTable.Columns.Count; index++) { //Using indexes as there might be problems comparing columns with the equals operator as column define their own equals operator for using in queries

                Sql.AColumn column = pTable.Columns[index];

                for(int index2 = 0; index2 < pTable.Columns.Count; index2++) {

                    if(index != index2) {

                        Sql.AColumn column2 = pTable.Columns[index2];

                        if(column.Equals(column2)) {
                            issues.Add(new ValidationError(pTable.Schema, pTable.TableName, column.ColumnName, "Column has been added more than once to the AddColumns(...) method"));
                        }
                        else if(string.Compare(column.ColumnName, column2.ColumnName, true) == 0) {
                            issues.Add(new ValidationError(pTable.Schema, pTable.TableName, column.ColumnName, "Column name matches another column on the Table class."));
                        }
                    }
                }
            }

            foreach(Sql.AColumn column in pTable.Columns) {

                IColumn? dbColumn = GetMatchingDbColumn(column, pTableDetails);

                Type columnType = column.GetType();
                Type tableType = pTable.GetType();

                if(dbColumn == null) {
                    issues.Add(new ValidationError(pTable.Schema, pTable.TableName, column.ColumnName, "Table column does not exist in database."));
                    continue;
                }

                if(dbColumn.DbType != column.DbType) {
                    issues.Add(new ValidationError(pTable.Schema, pTable.TableName, column.ColumnName, "Column data types do not match. Code type = " + column.DbType.ToString() + ", DB type = " + dbColumn.DbType.ToString()));
                }

                if(dbColumn.IsPrimaryKey && !column.IsPrimaryKey) {
                    issues.Add(new ValidationError(pTable.Schema, pTable.TableName, column.ColumnName, "Database column is a primary key column but code column is not."));
                }
                else if(!dbColumn.IsPrimaryKey && column.IsPrimaryKey) {
                    issues.Add(new ValidationError(pTable.Schema, pTable.TableName, column.ColumnName, "Database column is not a primary key column but code column is."));
                }
                else if(column.IsPrimaryKey && dbColumn.IsPrimaryKey) {

                    //
                    //	Check that the primary key table columns are using the correct key column types
                    //
                    if(dbColumn.DbType == System.Data.DbType.Guid) {

                        if(!columnType.IsGenericType || columnType.GetGenericTypeDefinition() != typeof(Sql.Column.GuidKeyColumn<>)) {
                            issues.Add(new ValidationError(pTable.Schema, pTable.TableName, column.ColumnName, "A guid primary key table column must use the column type: Sql.Column.GuidKeyColumn<>."));
                        }
                    }
                    else if(dbColumn.DbType == System.Data.DbType.Int16) {

                        if(!columnType.IsGenericType || columnType.GetGenericTypeDefinition() != typeof(Sql.Column.SmallIntegerKeyColumn<>)) {
                            issues.Add(new ValidationError(pTable.Schema, pTable.TableName, column.ColumnName, "An Int16 primary key table column must use the column type: Sql.Column.SmallIntegerKeyColumn<>."));
                        }
                    }
                    else if(dbColumn.DbType == System.Data.DbType.Int32) {

                        if(!columnType.IsGenericType || columnType.GetGenericTypeDefinition() != typeof(Sql.Column.IntegerKeyColumn<>)) {
                            issues.Add(new ValidationError(pTable.Schema, pTable.TableName, column.ColumnName, "An Int32 primary key table column must use the column type: Sql.Column.IntegerKeyColumn<>."));
                        }
                    }
                    else if(dbColumn.DbType == System.Data.DbType.Int64) {

                        if(!columnType.IsGenericType || columnType.GetGenericTypeDefinition() != typeof(Sql.Column.BigIntegerKeyColumn<>)) {
                            issues.Add(new ValidationError(pTable.Schema, pTable.TableName, column.ColumnName, "An Int64 primary key table column must use the column type: Sql.Column.BigIntegerKeyColumn<>."));
                        }
                    }
                    else if(dbColumn.DbType == System.Data.DbType.String) {

                        if(!columnType.IsGenericType || columnType.GetGenericTypeDefinition() != typeof(Sql.Column.StringKeyColumn<>)) {
                            issues.Add(new ValidationError(pTable.Schema, pTable.TableName, column.ColumnName, "A string primary key table column must use the column type: Sql.Column.StringKeyColumn<>."));
                        }
                    }
                    else if(columnType.IsGenericType) {
                        issues.Add(new ValidationError(pTable.Schema, pTable.TableName, column.ColumnName, "A primary key table column must use a key column field. The definition checker is unsure if this is a valid key column e.g. Sql.Column.GuidKeyColumn<>. Either the column type is not right or the definition checker needs updating to support this particular key column."));
                    }
                    else {
                        issues.Add(new ValidationError(pTable.Schema, pTable.TableName, column.ColumnName, "A primary key table column must use a key column field. An example of a key column is Sql.Column.GuidKeyColumn<>."));
                    }
                }

                if(IsColumnAForeignKey(dbColumn, pTableDetails)) {

                    //
                    //	Check that the foreign key table columns are using the correct key column types
                    //
                    if(dbColumn.DbType == System.Data.DbType.Guid) {

                        if(dbColumn.IsNullable) {

                            if(!columnType.IsGenericType || columnType.GetGenericTypeDefinition() != typeof(Sql.Column.NGuidKeyColumn<>)) {
                                issues.Add(new ValidationError(pTable.Schema, pTable.TableName, column.ColumnName, "A guid foreign key table column must use the column type: Sql.Column.NGuidKeyColumn<>."));
                            }
                        }
                        else {

                            if(!columnType.IsGenericType || columnType.GetGenericTypeDefinition() != typeof(Sql.Column.GuidKeyColumn<>)) {
                                issues.Add(new ValidationError(pTable.Schema, pTable.TableName, column.ColumnName, "A guid foreign key table column must use the column type: Sql.Column.GuidKeyColumn<>."));
                            }
                        }
                    }
                    else if(dbColumn.DbType == System.Data.DbType.Int16) {

                        if(dbColumn.IsNullable) {

                            if(!columnType.IsGenericType || columnType.GetGenericTypeDefinition() != typeof(Sql.Column.NSmallIntegerKeyColumn<>)) {
                                issues.Add(new ValidationError(pTable.Schema, pTable.TableName, column.ColumnName, "An Int16 foreign key table column must use the column type: Sql.Column.NSmallIntegerKeyColumn<>."));
                            }
                        }
                        else {

                            if(!columnType.IsGenericType || columnType.GetGenericTypeDefinition() != typeof(Sql.Column.SmallIntegerKeyColumn<>)) {
                                issues.Add(new ValidationError(pTable.Schema, pTable.TableName, column.ColumnName, "An Int16 foreign key table column must use the column type: Sql.Column.SmallIntegerKeyColumn<>."));
                            }
                        }
                    }
                    else if(dbColumn.DbType == System.Data.DbType.Int32) {

                        if(dbColumn.IsNullable) {

                            if(!columnType.IsGenericType || columnType.GetGenericTypeDefinition() != typeof(Sql.Column.NIntegerKeyColumn<>)) {
                                issues.Add(new ValidationError(pTable.Schema, pTable.TableName, column.ColumnName, "An Int32 foreign key table column must use the column type: Sql.Column.NIntegerKeyColumn<>."));
                            }
                        }
                        else {

                            if(!columnType.IsGenericType || columnType.GetGenericTypeDefinition() != typeof(Sql.Column.IntegerKeyColumn<>)) {
                                issues.Add(new ValidationError(pTable.Schema, pTable.TableName, column.ColumnName, "An Int32 foreign key table column must use the column type: Sql.Column.IntegerKeyColumn<>."));
                            }
                        }
                    }
                    else if(dbColumn.DbType == System.Data.DbType.Int64) {

                        if(dbColumn.IsNullable) {

                            if(!columnType.IsGenericType || columnType.GetGenericTypeDefinition() != typeof(Sql.Column.NBigIntegerKeyColumn<>)) {
                                issues.Add(new ValidationError(pTable.Schema, pTable.TableName, column.ColumnName, "An Int64 foreign key table column must use the column type: Sql.Column.NBigIntegerKeyColumn<>."));
                            }
                        }
                        else {

                            if(!columnType.IsGenericType || columnType.GetGenericTypeDefinition() != typeof(Sql.Column.BigIntegerKeyColumn<>)) {
                                issues.Add(new ValidationError(pTable.Schema, pTable.TableName, column.ColumnName, "An Int64 foreign key table column must use the column type: Sql.Column.BigIntegerKeyColumn<>."));
                            }
                        }
                    }
                    else if(dbColumn.DbType == System.Data.DbType.String) {

                        if(dbColumn.IsNullable) {

                            if(!columnType.IsGenericType || columnType.GetGenericTypeDefinition() != typeof(Sql.Column.NStringKeyColumn<>)) {
                                issues.Add(new ValidationError(pTable.Schema, pTable.TableName, column.ColumnName, "A string foreign key table column must use the column type: Sql.Column.NStringKeyColumn<>."));
                            }
                        }
                        else {
                            if(!columnType.IsGenericType || columnType.GetGenericTypeDefinition() != typeof(Sql.Column.StringKeyColumn<>)) {
                                issues.Add(new ValidationError(pTable.Schema, pTable.TableName, column.ColumnName, "A string foreign key table column must use the column type: Sql.Column.StringKeyColumn<>."));
                            }
                        }
                    }
                    else if(columnType.IsGenericType) {
                        issues.Add(new ValidationError(pTable.Schema, pTable.TableName, column.ColumnName, " foreign key table column must use a key column field. The definition checker is unsure if this is a valid key column e.g. Sql.Column.GuidKeyColumn<>. Either the column type is not right or the definition checker needs updating to support this particular key column."));
                    }
                    else {
                        issues.Add(new ValidationError(pTable.Schema, pTable.TableName, column.ColumnName, "A foreign key table column must use a key column field. An example of a key column is Sql.Column.GuidKeyColumn<>."));
                    }
                }

                if(dbColumn.IsNullable && !column.AllowsNulls) {
                    issues.Add(new ValidationError(pTable.Schema, pTable.TableName, column.ColumnName, "Database column allows nulls but code column does not."));
                }

                if(!dbColumn.IsNullable && column.AllowsNulls) {
                    issues.Add(new ValidationError(pTable.Schema, pTable.TableName, column.ColumnName, "Database column does not allows nulls but code column does."));
                }

                if(typeof(Sql.IColumnLength).IsAssignableFrom(columnType)) {

                    Sql.IColumnLength stringColumn = (Sql.IColumnLength)column;

                    if(stringColumn.MaxLength != dbColumn.MaxLength) {

                        if(dbColumn.MaxLength != -1 || (dbColumn.MaxLength == -1 && stringColumn.MaxLength != Int32.MaxValue)) {
                            issues.Add(new ValidationError(pTable.Schema, pTable.TableName, column.ColumnName, "Table column max length value does not match max length in database. Code Length = " + stringColumn.MaxLength.ToString() + ". Database length = " + dbColumn.MaxLength.ToString()));
                        }
                    }
                }

                if(dbColumn.IsAutoGenerated && !column.IsAutoId) {
                    issues.Add(new ValidationError(pTable.Schema, pTable.TableName, column.ColumnName, "Database column is auto generated but code column is not. There is a parameter on the Column constructor to define a column as being auto generated or not."));
                }

                if(!dbColumn.IsAutoGenerated && column.IsAutoId) {
                    issues.Add(new ValidationError(pTable.Schema, pTable.TableName, column.ColumnName, "Database column is not auto generated but code column is. There is a parameter on the Column constructor to define a column as being auto generated or not."));
                }
            }

            foreach(IColumn dbColumn in pTableDetails.Columns) {

                if(!ColumnExistsInCode(dbColumn, pTable)) {
                    issues.Add(new ValidationError(pTable.Schema, pTable.TableName, dbColumn.ColumnName, "Column missing in code."));
                }
            }
            return issues;
        }

        public static List<ValidationError> CheckRow(Sql.ATable pTable, Sql.ARow pRow) {

            if(pRow.RowState != Sql.ARow.RowStateEnum.AddPending) {
                throw new Exception("pRow must newly created row that does not exists in database table");
            }

            List<ValidationError> issues = new List<ValidationError>();

            Type tableType = pTable.GetType();
            Type rowType = pRow.GetType();

            System.Reflection.PropertyInfo[] tableProperties = tableType.GetProperties();

            foreach(System.Reflection.PropertyInfo tableProperty in tableProperties) {

                try {

                    if(typeof(Sql.AColumn).IsAssignableFrom(tableProperty.PropertyType)) {

                        Sql.AColumn column = (Sql.AColumn)tableProperty.GetValue(pTable)!;

                        System.Reflection.PropertyInfo? rowProperty;

                        try {
                            rowProperty = rowType.GetProperty(tableProperty.Name);
                        }
                        catch(System.Reflection.AmbiguousMatchException) {
                            issues.Add(new ValidationError(pTable.Schema, pTable.TableName, column.ColumnName, "Property name is ambiguous in Row class. Unable to test column getter and setter on Row class."));
                            continue;
                        }

                        if(rowProperty == null) {
                            issues.Add(new ValidationError(pTable.Schema, pTable.TableName, column.ColumnName, "Cannot match Table columns with Row property. Table columns and row properties must have identical names."));
                            continue;
                        }

                        if(!rowProperty.CanRead) {
                            issues.Add(new ValidationError(pTable.Schema, pTable.TableName, column.ColumnName, "Row property is missing a getter."));
                        }
                        if(column.IsAutoId && rowProperty.CanWrite) {
                            issues.Add(new ValidationError(pTable.Schema, pTable.TableName, column.ColumnName, "Row property can be set but table column is an AutoId. Auto id columns must not have a setter on their row column."));
                        }
                        else if(pTable.IsView && rowProperty.CanWrite) {
                            issues.Add(new ValidationError(pTable.Schema, pTable.TableName, column.ColumnName, "Row property can be set but table is a view."));
                        }
                        else if(!column.IsAutoId && !pTable.IsView && !rowProperty.CanWrite) {
                            issues.Add(new ValidationError(pTable.Schema, pTable.TableName, column.ColumnName, "Row property can not be set and table is not a view and table column is not an AutoId. Something is wrong with the row column setter."));
                        }

                        if(!GetValueForType(column, rowProperty.PropertyType, out object? value, out object? secondValue)) {
                            issues.Add(new ValidationError(pTable.Schema, pTable.TableName, column.ColumnName, "Cannot test setting Row property as Type is not supported by tester. Type = " + rowProperty.PropertyType.ToString()));
                            continue;
                        }

                        if(rowProperty.CanRead) {

                            //
                            //	Test reading and writing from the table column property and the row column property. This checks they are defined correctly.
                            //

                            //Set value from table column
                            if(!pTable.Columns.Contains(column)) {
                                issues.Add(new ValidationError(pTable.Schema, pTable.TableName, column.ColumnName, "Column property on Table has not been added to the Table object using the AddColumns(...) method."));
                                continue;
                            }

                            object? defaultType = column.GetDefaultType();

                            if(defaultType != null && defaultType.GetType() != value.GetType()) {
                                issues.Add(new ValidationError(pTable.Schema, pTable.TableName, column.ColumnName, "Row set value test failed because row and column types do not match. " + defaultType.GetType().ToString() + " != " + value.GetType().ToString()));
                                continue;
                            }

                            Sql.SqlHelper.TestSetValue(column, pRow, value);

                            object? returnValue;

                            try {
                                returnValue = rowProperty.GetValue(pRow);   //Get value from row column
                            }
                            catch(Exception e) {
                                issues.Add(new ValidationError(pTable.Schema, pTable.TableName, column.ColumnName, "Exception Occurred. This probably relates to the table column not being added to the Table correctly using the AddColumns(...) method. Maybe it has not been added or added more than once to the table." + System.Environment.NewLine + e.Message + Environment.NewLine + e.StackTrace));
                                continue;
                            }

                            if(!value.Equals(returnValue)) {
                                issues.Add(new ValidationError(pTable.Schema, pTable.TableName, column.ColumnName, "Get and set value test from table column property failed. Please check that the get and set fields on row are setup correctly and are using the same table property."));
                                continue;
                            }

                            returnValue = Sql.SqlHelper.TestGetValue(column, pRow);

                            if(!value.Equals(returnValue)) {
                                issues.Add(new ValidationError(pTable.Schema, pTable.TableName, column.ColumnName, "Get and set value test from table column property failed. Please check that the get and set fields on row are setup correctly and are using the same table property."));
                                continue;
                            }

                            Sql.SqlHelper.TestSetValue(column, pRow, secondValue);

                            returnValue = rowProperty.GetValue(pRow);

                            if(!secondValue.Equals(returnValue)) {
                                issues.Add(new ValidationError(pTable.Schema, pTable.TableName, column.ColumnName, "Unable to set value on table column. Please check that the get and set fields on row are setup correctly and are using the same table property."));
                                continue;
                            }

                            returnValue = Sql.SqlHelper.TestGetValue(column, pRow);

                            if(!secondValue.Equals(returnValue)) {
                                issues.Add(new ValidationError(pTable.Schema, pTable.TableName, column.ColumnName, "Get and set value test from table column property failed. Please check that the get and set fields on row are setup correctly and are using the same table property."));
                                continue;
                            }

                            if(rowProperty.CanWrite) {

                                rowProperty.SetValue(pRow, value);
                                returnValue = rowProperty.GetValue(pRow);

                                if(!value.Equals(returnValue)) {
                                    issues.Add(new ValidationError(pTable.Schema, pTable.TableName, column.ColumnName, "Get and set value test on Row property failed. Please check that the get and set fields on row are setup correctly and are using the same table property."));
                                    continue;
                                }

                                rowProperty.SetValue(pRow, secondValue);
                                returnValue = rowProperty.GetValue(pRow);

                                if(!secondValue.Equals(returnValue)) {
                                    issues.Add(new ValidationError(pTable.Schema, pTable.TableName, column.ColumnName, "Get and set value test on Row property failed. Please check that the get and set fields on row are setup correctly and are using the same table property."));
                                    continue;
                                }
                            }
                        }
                    }
                }
                catch(Exception e) {
                    issues.Add(new ValidationError(pTable.Schema, pTable.TableName, string.Empty, e.Message + System.Environment.NewLine + e.StackTrace));
                    continue;
                }
            }

            List<ValidationError> rowIssues = CheckForMissingRowProperties(pTable, pRow);

            if(rowIssues.Count > 0) {
                issues.AddRange(rowIssues);
            }
            return issues;
        }

        private static bool IsColumnAForeignKey(IColumn pColumn, ITableDetails pTableDetails) {

            foreach(IForeignKey foreignKey in pTableDetails.ForeignKeys) {

                foreach(IForeignKeyColumns foreignKeyColumn in foreignKey.KeyColumns) {

                    if(foreignKeyColumn.ForeignKeyColumn.Equals(pColumn)) {
                        return true;
                    }
                }
            }
            return false;
        }

        private static List<ValidationError> CheckForMissingRowProperties(Sql.ATable pTable, Sql.ARow pRow) {

            Type tableType = pTable.GetType();
            Type rowType = pRow.GetType();

            System.Reflection.PropertyInfo[] rowProperties = rowType.GetProperties();
            System.Reflection.PropertyInfo[] tableProperties = tableType.GetProperties();

            List<ValidationError> issues = new List<ValidationError>();

            foreach(System.Reflection.PropertyInfo rowProp in rowProperties) {

                if(rowProp.DeclaringType == typeof(Sql.ARow)) {   //Skip properties inherited from Sql.ARow
                    continue;
                }

                bool foundMatch = false;

                foreach(System.Reflection.PropertyInfo tableProp in tableProperties) {

                    if(string.Compare(rowProp.Name, tableProp.Name, false) == 0) {
                        foundMatch = true;
                        break;
                    }
                }
                if(!foundMatch) {
                    issues.Add(new ValidationError(pTable.Schema, pTable.TableName, rowProp.Name, "Row property does not have a matching table propety. Table columns and row properties must have identical names. Column property: " + rowProp.Name));
                }
            }
            return issues;
        }

        private static bool GetValueForType(Sql.AColumn pColumn, Type pType, [MaybeNullWhen(false)] out object pValue, [MaybeNullWhen(false)] out object pSecondValue) {

            if(pType == typeof(string)) {

                int? maxLength = null;

                if(pColumn is Sql.IColumnLength) {
                    maxLength = ((Sql.IColumnLength)pColumn).MaxLength;
                }

                string value = Guid.NewGuid().ToString();
                pValue = maxLength != null && maxLength < value.Length ? value.Substring(0, maxLength.Value) : value;

                string secondValue = Guid.NewGuid().ToString();
                pSecondValue = maxLength != null && maxLength < secondValue.Length ? secondValue.Substring(0, maxLength.Value) : secondValue;
                return true;
            }

            if(pType == typeof(Guid)) {
                pValue = Guid.NewGuid();
                pSecondValue = Guid.NewGuid();
                return true;
            }

            if(pType == typeof(Guid?)) {
                pValue = Guid.NewGuid();
                pSecondValue = Guid.NewGuid();
                return true;
            }

            if(pType == typeof(bool)) {
                pValue = false;
                pSecondValue = true;
                return true;
            }

            if(pType == typeof(bool?)) {
                pValue = (bool?)false;
                pSecondValue = (bool?)true;
                return true;
            }

            if(pType == typeof(Int16)) {
                pValue = Int16.MaxValue;
                pSecondValue = Int16.MinValue;
                return true;
            }

            if(pType == typeof(Int16?)) {
                pValue = (Int16?)Int16.MaxValue;
                pSecondValue = (Int16?)1;
                return true;
            }

            if(pType == typeof(Int32)) {
                pValue = Int32.MaxValue;
                pSecondValue = (Int32?)2;
                return true;
            }

            if(pType == typeof(Int32?)) {
                pValue = (Int32?)Int32.MaxValue;
                pSecondValue = (Int32?)3;
                return true;
            }

            if(pType == typeof(Int64)) {
                pValue = Int64.MaxValue;
                pSecondValue = (Int64)4;
                return true;
            }

            if(pType == typeof(Int64?)) {
                pValue = (Int64?)Int64.MaxValue;
                pSecondValue = (Int64?)5;
                return true;
            }

            if(pType == typeof(DateTime)) {
                pValue = DateTime.MaxValue;
                pSecondValue = DateTime.MinValue;
                return true;
            }

            if(pType == typeof(DateTimeOffset)) {
                pValue = (DateTimeOffset)DateTimeOffset.MaxValue;
                pSecondValue = (DateTimeOffset)DateTimeOffset.MinValue;
                return true;
            }

            if(pType == typeof(DateTime?)) {
                pValue = (DateTime?)DateTime.MaxValue;
                pSecondValue = (DateTime?)DateTime.MinValue;
                return true;
            }

            if(pType == typeof(DateTimeOffset?)) {
                pValue = (DateTimeOffset?)DateTimeOffset.MaxValue;
                pSecondValue = (DateTimeOffset?)DateTimeOffset.MinValue;
                return true;
            }

            if(pType == typeof(decimal)) {
                pValue = decimal.MaxValue;
                pSecondValue = 6.0m;
                return true;
            }

            if(pType == typeof(decimal?)) {
                pValue = (decimal?)decimal.MaxValue;
                pSecondValue = (decimal?)7.1m;
                return true;
            }

            if(pType == typeof(float)) {
                pValue = float.MaxValue;
                pSecondValue = 8.0f;
                return true;
            }

            if(pType == typeof(float?)) {
                pValue = (float?)float.MaxValue;
                pSecondValue = (float?)9.1f;
                return true;
            }

            if(pType == typeof(byte)) {

                Random random = new Random();

                byte[] bytes = new byte[1];
                random.NextBytes(bytes);
                pValue = bytes[0];

                random.NextBytes(bytes);
                pSecondValue = bytes[0];
                return true;
            }

            if(pType == typeof(byte?)) {

                Random random = new Random();

                byte[] bytes = new byte[1];
                random.NextBytes(bytes);
                pValue = bytes[0];

                random.NextBytes(bytes);
                pSecondValue = bytes[0];
                return true;
            }

            if(pType == typeof(byte[])) {   //TODO: Check byte array length when that property is added to the column

                Random random = new Random();

                byte[] bytes = new byte[10];
                random.NextBytes(bytes);
                pValue = bytes;

                random.NextBytes(bytes);
                pSecondValue = bytes;
                return true;
            }

            if(pType == typeof(byte?[])) {  //TODO: Check byte array length when that property is added to the column

                Random random = new Random();

                byte[] bytes = new byte[10];
                random.NextBytes(bytes);
                pValue = bytes;

                random.NextBytes(bytes);
                pSecondValue = bytes;
                return true;
            }

            if(pType.Name == "GuidKey`1") {
                Random random = new Random();
                pValue = Activator.CreateInstance(pType, Guid.NewGuid())!;
                pSecondValue = Activator.CreateInstance(pType, Guid.NewGuid())!;
                return true;
            }
            if(pType.Name == "Int16Key`1") {
                Random random = new Random();
                pValue = Activator.CreateInstance(pType, random.Next(short.MinValue, short.MaxValue))!;
                pSecondValue = Activator.CreateInstance(pType, random.Next(short.MinValue, short.MaxValue))!;
                return true;
            }
            if(pType.Name == "Int32Key`1") {
                Random random = new Random();
                pValue = Activator.CreateInstance(pType, random.Next())!;
                pSecondValue = Activator.CreateInstance(pType, random.Next())!;
                return true;
            }
            if(pType.Name == "Int64Key`1") {
                Random random = new Random();
                pValue = Activator.CreateInstance(pType, (long)random.Next())!;
                pSecondValue = Activator.CreateInstance(pType, (long)random.Next())!;
                return true;
            }
            if(pType.Name == "StringKey`1") {


                int? maxLength = null;

                if(pColumn is Sql.IColumnLength) {
                    maxLength = ((Sql.IColumnLength)pColumn).MaxLength;
                }

                string value = Guid.NewGuid().ToString();
                value = maxLength != null && maxLength < value.Length ? value.Substring(0, maxLength.Value) : value;

                string secondValue = Guid.NewGuid().ToString();
                secondValue = maxLength != null && maxLength < secondValue.Length ? secondValue.Substring(0, maxLength.Value) : secondValue;

                pValue = Activator.CreateInstance(pType, value)!;
                pSecondValue = Activator.CreateInstance(pType, secondValue)!;
                return true;
            }

            //TODO: Implement the remaining types

            pValue = new object();
            pSecondValue = new object();
            return false;
        }
        private static IColumn? GetMatchingDbColumn(Sql.AColumn pColumn, ITableDetails pTableDetails) {

            foreach(IColumn dbColumn in pTableDetails.Columns) {

                if(string.Compare(pColumn.ColumnName, dbColumn.ColumnName, true) == 0) {
                    return dbColumn;
                }
            }
            return null;
        }

        private static bool ColumnExistsInCode(IColumn pDbColumn, Sql.ATable pTable) {

            foreach(Sql.AColumn column in pTable.Columns) {

                if(string.Compare(column.ColumnName, pDbColumn.ColumnName, true) == 0) {
                    return true;
                }
            }
            return false;
        }

        public static List<ValidationError> CheckTable(Sql.ARow pRow, Sql.ADatabase pDatabase) {

            Sql.ATable table = pRow.ParentTable;

            string schema = table.Schema;

            if(string.IsNullOrEmpty(schema)) {

                if(pDatabase.DatabaseType == Sql.DatabaseType.Mssql) {
                    schema = "dbo";
                }
                else if(pDatabase.DatabaseType == Sql.DatabaseType.PostgreSql) {
                    schema = "public";
                }
                else {
                    throw new Exception("Unknown database type: '" + pDatabase.DatabaseType.ToString() + "'");
                }
            }

            ITableDetails? tableDetails;

            string errorText;

            if(pDatabase.DatabaseType == Sql.DatabaseType.Mssql) {

                if(!new Logic.SqlServerSchema().GetTableDetails(pDatabase, table.TableName, schema, out tableDetails, out errorText)) {
                    return new List<ValidationError>() { new ValidationError(schema, table.TableName, string.Empty, errorText) };
                }
            }
            else if(pDatabase.DatabaseType == Sql.DatabaseType.PostgreSql) {

                if(!new Logic.PostgreSqlSchema().GetTableDetails(pDatabase, table.TableName, schema, out tableDetails, out errorText)) {
                    return new List<ValidationError>() { new ValidationError(schema, table.TableName, string.Empty, errorText) };
                }
            }
            else {
                throw new Exception("Unknown database type: '" + pDatabase.DatabaseType.ToString() + "'");
            }

            List<ValidationError> issues = CheckTable(table, tableDetails);

            issues.AddRange(CheckRow(table, pRow));
            return issues;
        }
    }
}