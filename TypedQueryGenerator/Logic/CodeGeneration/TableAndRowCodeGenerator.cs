﻿
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
using System.Data;
using TypedQuery.Logic;

namespace TypedQueryGenerator.Logic.CodeGeneration {
    
    public class TableAndRowCodeGenerator {

        public string Generate(ITableDetails pTableDetails, string pNamespace, ref string pColumnPrefix, bool pIncludeSchema, bool pGuessPrefix, bool pGenerateComments, bool pRemoveUnderscores, bool pGenerateKeyTypes) {

            if(pTableDetails == null) {
                return "Unable to load table details";
            }

            if(string.IsNullOrEmpty(pNamespace)) {
                pNamespace = "Tables";
            }

            if(pGuessPrefix && pTableDetails.Columns.Count > 1) {

                bool stop = false;

                int charIndex = 0;

                string prefix = string.Empty;

                while(!stop) {

                    char? c = null;

                    for(int index = 0; index < pTableDetails.Columns.Count; index++) {

                        IColumn column = pTableDetails.Columns[index];

                        if(charIndex >= column.ColumnName.Length - 1) {
                            stop = true;
                            break;
                        }

                        if(index == 0) {
                            c = column.ColumnName[charIndex];
                        }
                        else if(char.ToLower(c!.Value) != char.ToLower(column.ColumnName[charIndex])) {
                            stop = true;
                            break;
                        }
                    }

                    if(!stop && c != null) {
                        prefix += c.Value;
                        charIndex++;
                    }
                    else {
                        break; //Just incase
                    }
                }

                if(prefix.Length > 1 && prefix.Length < 6) {
                    pColumnPrefix = prefix;
                }
            }

            CodeFile codeFile = new CodeFile("    ");

            codeFile.Append("using System;").EndLine();

            if(pGenerateKeyTypes) {
                codeFile.Append("using Sql.Types;").EndLine();
            }
            codeFile.Append("using Sql.Column;").EndLine();

            codeFile.EndLine();
            codeFile.Append("namespace ").Append(pNamespace).Append(".").Append(pTableDetails.TableName.Replace(" ", string.Empty)).Append(" {").EndLine();
            codeFile.EndLine();

            if(pGenerateComments) {
                codeFile.Indent(1).Append("[Sql.TableAttribute(\"").Append(pTableDetails.Description).Append("\")]").EndLine();
            }

            codeFile.Indent(1).Append("public sealed class Table : ").Append(typeof(Sql.ATable).ToString()).Append(" {").EndLine();
            codeFile.EndLine();
            codeFile.Indent(2).Append("public static readonly Table Instance = new Table();").EndLine();
            codeFile.EndLine();

            for(int columnIndex = 0; columnIndex < pTableDetails.Columns.Count; columnIndex++) {

                IColumn column = pTableDetails.Columns[columnIndex];

                if(pGenerateComments) {

                    if(columnIndex > 0) {
                        codeFile.EndLine();
                    }
                    codeFile.Indent(2).Append("[Sql.ColumnAttribute(\"").Append(column.Description).Append("\")]").EndLine();
                }
                codeFile.Indent(2).Append("public ").Append(ColumnType.GetColumnType(column, pTableDetails, pGenerateKeyTypes)).Append(" ").Append(NameFormatter.Format(ColumnName.GetColumnName(column, pColumnPrefix, pRemoveUnderscores))).Append(" { get; private set; }").EndLine();
            }

            codeFile.EndLine();
            codeFile.Indent(2).Append("public Table() : base(\"").Append(pTableDetails.TableName).Append("\", \"").Append(pIncludeSchema ? pTableDetails.Schema : string.Empty).Append("\", ").Append(pTableDetails.IsView ? "true" : "false").Append(", typeof(Row)) {").EndLine();
            codeFile.EndLine();

            for(int columnIndex = 0; columnIndex < pTableDetails.Columns.Count; columnIndex++) {

                IColumn column = pTableDetails.Columns[columnIndex];
                string primaryKey = column.IsPrimaryKey ? "true" : "false";
                string isAutoGenerated = column.IsAutoGenerated ? "true" : "false";

                codeFile.Indent(3).Append(NameFormatter.Format(ColumnName.GetColumnName(column, pColumnPrefix, pRemoveUnderscores))).Append(" = new ").Append(ColumnType.GetColumnType(column, pTableDetails, pGenerateKeyTypes)).Append("(this, \"").Append(column.ColumnName).Append("\", ").Append(primaryKey);

                if(column.IsAutoGenerated) {
                    codeFile.Append(", ").Append(isAutoGenerated);
                }

                if(column.MaxLength != null && column.DbType != DbType.Binary && column.DbType != DbType.Byte) {

                    if(column.MaxLength == -1) {
                        codeFile.Append(", int.MaxValue");
                    }
                    else {
                        codeFile.Append(", " + column.MaxLength.Value.ToString());
                    }
                }
                codeFile.Append(");").EndLine();
            }

            codeFile.EndLine();

            codeFile.Indent(3).Append("AddColumns(");

            for(int columnIndex = 0; columnIndex < pTableDetails.Columns.Count; columnIndex++) {

                IColumn column = pTableDetails.Columns[columnIndex];

                if(columnIndex > 0) {
                    codeFile.Append(", ");
                }
                codeFile.Append(NameFormatter.Format(ColumnName.GetColumnName(column, pColumnPrefix, pRemoveUnderscores)));
            }

            codeFile.Append(");").EndLine();
            codeFile.Indent(2).Append("}").EndLine();
            codeFile.EndLine();

            codeFile.Indent(2).Append("public Row GetRow(int pIndex, Sql.IResult pResult) {").EndLine();
            codeFile.Indent(3).Append("return (Row)pResult.GetRow(this, pIndex);").EndLine();
            codeFile.Indent(2).Append("}").EndLine();
            codeFile.Indent(1).Append("}").EndLine();

            codeFile.EndLine();

            //
            //	Generate row code
            //
            codeFile.Indent(1).Append("public sealed class Row : ").Append(typeof(Sql.ARow).ToString()).Append(" {").EndLine();
            codeFile.EndLine();
            codeFile.Indent(2).Append("private new Table Tbl {").EndLine();
            codeFile.Indent(3).Append("get { return (Table)base.Tbl; }").EndLine();
            codeFile.Indent(2).Append("}").EndLine();
            codeFile.EndLine();
            codeFile.Indent(2).Append("public Row() : base(Table.Instance) {").EndLine();
            codeFile.Indent(2).Append("}").EndLine();
            codeFile.EndLine();

            for(int columnIndex = 0; columnIndex < pTableDetails.Columns.Count; columnIndex++) {

                IColumn column = pTableDetails.Columns[columnIndex];

                string columnName = NameFormatter.Format(ColumnName.GetColumnName(column, pColumnPrefix, pRemoveUnderscores));

                if(columnIndex > 0) {
                    codeFile.EndLine();
                }

                codeFile.Indent(2).Append("public ").Append(ReturnType.GetReturnType(column.DbType, column.IsNullable, column, pTableDetails, pGenerateKeyTypes)).Append(" ").Append(columnName).Append(" {").EndLine();
                codeFile.Indent(3).Append("get { return Tbl.").Append(columnName).Append(".ValueOf(this); }").EndLine();

                if(!column.IsAutoGenerated && !pTableDetails.IsView) {
                    codeFile.Indent(3).Append("set { Tbl.").Append(columnName).Append(".SetValue(this, value); }").EndLine();
                }
                codeFile.Indent(2).Append("}");
            }
            codeFile.Indent(1).Append("}").EndLine();
            codeFile.Append("}");

            return codeFile.ToString();
        }
    }
}