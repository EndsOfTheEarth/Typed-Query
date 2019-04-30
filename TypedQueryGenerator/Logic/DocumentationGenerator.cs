
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

using Sql;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Web;

namespace TypedQuery.Logic {

    public class DocumentationGenerator {

        public void GenerateTestOnly(string pVersion, DateTime? pDateTime, List<Sql.ARow> pRows) {

            if(pRows == null) {
                throw new NullReferenceException($"{nameof(pRows)} cannot be null");
            }

            if(pRows.Count == 0) {
                throw new ArgumentException($"{nameof(pRows)} must contain at least one table");
            }

            string path = "C:\\Temp\\Schema\\";

            string stylePath = Path.Combine(path, "style.css");

            if(File.Exists(stylePath)) {
                File.Delete(stylePath);
            }
            File.WriteAllText(stylePath, GenerateStyleSheet());

            foreach(Sql.ARow row in pRows) {

                string filePath = Path.Combine(path, row.ParentTable.TableName + ".html");

                if(File.Exists(filePath)) {
                    File.Delete(filePath);
                }
                File.WriteAllText(filePath, GenerateForTable(pVersion, pDateTime, row.ParentTable));
            }
        }

        private string GenerateStyleSheet() {

            string text = @"
table {
    border-collapse: collapse;
	width: 100%;
}
table, th, td {
   border: 1px solid black;
}
";
            return text;
        }
        private string GenerateForTable(string pVersion, DateTime? pDateTime, Sql.ATable pTable) {

            if(pTable == null) {
                throw new NullReferenceException($"{nameof(pTable)} cannot be null");
            }

            string html = $@"
<html>
	<head>
		<title>{ HttpUtility.HtmlEncode(pTable.TableName) } { HttpUtility.HtmlEncode(pTable.IsView ? "(View)" : "(Table)") }</title>
		<link href=""style.css"" rel=""stylesheet"" type=""text/css"">
	  </head>
	<body>
		<h3> Version: { HttpUtility.HtmlEncode(pVersion) }</br>
		Date: { HttpUtility.HtmlEncode(pDateTime != null ? pDateTime.Value.ToString("yyyy-MM-dd HH:mm:ss") : string.Empty) }</h3>
		<h1>{ HttpUtility.HtmlEncode(!string.IsNullOrEmpty(pTable.Schema) ? pTable.Schema + "." : string.Empty) + HttpUtility.HtmlEncode(pTable.TableName) }</h1>
		<p>{ HttpUtility.HtmlEncode(GetTableComment(pTable)) }</p>
		<h2>Columns</h2>
		{ GetColumnHtml(pTable) }
	</body>
</html>
				";
            return html;
        }

        private string GetTableComment(Sql.ATable pTable) {

            object[] tableAttributes = pTable.GetType().GetCustomAttributes(true);

            string tableComment = string.Empty;

            foreach(object attribute in tableAttributes) {

                if(attribute is Sql.TableAttribute) {
                    tableComment = ((Sql.TableAttribute)attribute).Description;
                    break;
                }
            }
            return tableComment;
        }

        private Dictionary<string, string> GetColumnComments(Sql.ATable pTable) {

            Dictionary<string, string> columnLookup = new Dictionary<string, string>();

            foreach(PropertyInfo fieldInfo in pTable.GetType().GetProperties()) {

                if(typeof(AColumn).IsAssignableFrom(fieldInfo.PropertyType)) {

                    AColumn column = (AColumn)fieldInfo.GetValue(pTable);

                    object[] columnAttributes = fieldInfo.GetCustomAttributes(true);

                    string columnComment = null;

                    string valuesText = string.Empty;

                    if(column is Sql.Column.IEnumColumn) {

                        valuesText = GetEnumColumnValues((Sql.Column.IEnumColumn)column);
                    }

                    foreach(object attribute in columnAttributes) {

                        if(attribute is Sql.ColumnAttribute) {

                            string description = ((Sql.ColumnAttribute)attribute).Description;

                            if(!string.IsNullOrWhiteSpace(description)) {
                                description += " ";
                            }

                            if(!string.IsNullOrEmpty(valuesText)) {
                                description += "(" + valuesText + ")";
                            }
                            columnComment = description;
                            break;
                        }
                    }

                    if(columnComment == null) {

                        string description = string.Empty;

                        if(!string.IsNullOrEmpty(valuesText)) {
                            description += "(" + valuesText + ")";
                        }
                        columnComment = description;
                    }
                    columnLookup.Add(column.ColumnName, columnComment);
                }
            }
            return columnLookup;
        }

        private string GetEnumColumnValues(Sql.Column.IEnumColumn pEnumColumn) {

            Array enumValues = pEnumColumn.GetEnumType().GetEnumValues();

            StringBuilder text = new StringBuilder();

            foreach(object enumValue in enumValues) {

                if(text.Length > 0) {
                    text.Append(", ");
                }
                text.Append(enumValue.ToString()).Append(" = ").Append(((int)enumValue).ToString());
            }
            return text.ToString();
        }

        private string GetColumnHtml(Sql.ATable pTable) {

            StringBuilder html = new StringBuilder();

            Dictionary<string, string> columnDescLookup = GetColumnComments(pTable);

            html.Append("<table>");
            html.Append("<tr><td>Column</td><td>Data Type</td><td>Nullable</td><td>Description</d></tr>");

            foreach(var column in pTable.Columns) {

                string text = $@"
<tr>
	<td>{ HttpUtility.HtmlEncode(column.ColumnName) }</td>
	<td>{ HttpUtility.HtmlEncode(column.DbType.ToString() + GetLengthInformation(column)) }</td>
	<td>{ HttpUtility.HtmlEncode(column.AllowsNulls.ToString()) }</td>
	<td>{ HttpUtility.HtmlEncode((columnDescLookup.ContainsKey(column.ColumnName) ? columnDescLookup[column.ColumnName] : string.Empty))}</td>
</tr>";

                html.Append(text);
            }

            html.Append("</table>");
            return html.ToString();
        }

        private string GetLengthInformation(Sql.AColumn pColumn) {

            string lengthInfo = string.Empty;

            if(pColumn is Sql.IColumnLength) {
                lengthInfo = "(" + ((Sql.IColumnLength)pColumn).MaxLength.ToString() + ")";
            }
            return lengthInfo;
        }
    }
}