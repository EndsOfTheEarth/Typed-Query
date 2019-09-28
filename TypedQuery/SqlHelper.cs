
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
using System.Text;

namespace Sql {

    public static class SqlHelper {

        public static void TestSetValue(AColumn pColumn, ARow pRow, object pValue) {
            pColumn.TestSetValue(pRow, pValue);
        }

        public static object? TestGetValue(AColumn pColumn, ARow pRow) {
            return pColumn.TestGetValue(pRow);
        }

        /// <summary>
        /// Returns list of distinct Guids
        /// </summary>
        /// <param name="pResult"></param>
        /// <param name="pColumn"></param>
        /// <returns></returns>
        public static IList<Guid> ToList(IResult pResult, Column.GuidColumn pColumn) {

            List<Guid> list = new List<Guid>();

            for(int index = 0; index < pResult.Count; index++) {

                Guid value = (Guid)pResult.GetRow(pColumn.Table, index).GetValue(pColumn)!;

                if(!list.Contains(value)) {
                    list.Add(value);
                }
            }
            return list;
        }

        /// <summary>
        /// Returns list of distinct Guid?'s
        /// </summary>
        /// <param name="pResult"></param>
        /// <param name="pColumn"></param>
        /// <returns></returns>
        public static IList<Guid?> ToList(IResult pResult, Column.NGuidColumn pColumn) {

            List<Guid?> list = new List<Guid?>();

            for(int index = 0; index < pResult.Count; index++) {

                Guid? value = (Guid?)pResult.GetRow(pColumn.Table, index).GetValue(pColumn);

                if(!list.Contains(value)) {
                    list.Add(value);
                }
            }
            return list;
        }

        /// <summary>
        /// Returns dictionary. Throws an exception if the key column does not contains unique values.
        /// </summary>
        /// <param name="pResult"></param>
        /// <param name="pKeyColumn"></param>
        /// <param name="pValueColumn"></param>
        /// <returns></returns>
        public static IDictionary<Guid, string> ToDictionary(IResult pResult, Column.GuidColumn pKeyColumn, Column.StringColumn pValueColumn) {

            Dictionary<Guid, string> dict = new Dictionary<Guid, string>();

            for(int index = 0; index < pResult.Count; index++) {
                Guid key = (Guid)pResult.GetRow(pKeyColumn.Table, index).GetValue(pKeyColumn)!;
                string value = (string)pResult.GetRow(pValueColumn.Table, index).GetValue(pValueColumn)!;
                dict.Add(key, value);
            }
            return dict;
        }

        /// <summary>
        /// Returns the byte size of object as it would most likely be stored in database.
        /// e.g. string = 2 bytes per character (assumes uni code)
        /// </summary>
        /// <param name="pObject"></param>
        /// <returns></returns>
        public static int GetAproxByteSizeOf(object? pObject) {

            int value = 0;

            if(pObject == null) {
                value = 0;
            }
            else if(pObject is string) {
                value = ((string)pObject).Length * 2;
            }
            else if(pObject is int || pObject is int?) {
                value = 4;
            }
            else if(pObject is Int16 || pObject is Int16?) {
                value = 2;
            }
            else if(pObject is Int64 || pObject is Int64? || pObject is long) {
                value = 8;
            }
            else if(pObject is decimal || pObject is decimal?) {
                value = 9;  //Can be 5, 9, 13 or 17 in sql server
            }
            else if(pObject is DateTime || pObject is DateTime?) {
                value = 8;
            }
            else if(pObject is DateTimeOffset || pObject is DateTimeOffset?) {
                value = 9;  //Could be 8, 9 or 0 bytes in sql server
            }
            else if(pObject is bool || pObject is bool?) {
                value = 1;  //One byte. Can be one bit on some dbs.
            }
            else if(pObject is Guid || pObject is Guid?) {
                value = 16;
            }
            else if(pObject is byte[]) {
                value = ((byte[])pObject).Length;
            }
            else if(pObject is byte || pObject is byte?) {
                value = 1;
            }
            else if(pObject is double || pObject is double?) {
                value = 8;
            }
            else if(pObject is float || pObject is float?) {
                value = 32;
            }
            else if(pObject is DateTime || pObject is DateTime?) {
                value = 8;
            }
            else {
                throw new Exception("Unknown data type: '" + pObject.GetType().ToString() + "'");
            }
            return value;
        }

        /// <summary>
        /// Formats sql query into a more readable format
        /// </summary>
        /// <param name="pSql"></param>
        /// <returns></returns>
        public static string FormatSql(string pSql) {

            if(string.IsNullOrEmpty(pSql)) {
                return string.Empty;
            }

            List<string> keyWords = new List<string> { "select", "from", "left join", "right join", "cross join", "join", "where", "group by", "order by", "having", "limit", "union", "union all", "except", "insert", "update", "delete" };

            List<string> tokens = SplitIntoTokens(pSql);

            StringBuilder str = new StringBuilder();

            for(int index = 0; index < tokens.Count; index++) {

                char previousChar = str.Length > 0 ? str[str.Length - 1] : ' ';

                string currentToken = tokens[index];

                if(!char.IsWhiteSpace(previousChar) && previousChar != '(' && previousChar != ')' && currentToken != "(" && currentToken != ")") {
                    str.Append(' ');
                }

                string previousTokenLowerCase = (index > 0 ? tokens[index - 1] : string.Empty).ToLower();
                string currentTokenLowerCase = currentToken.ToLower();
                string nextTokenLowerCase = ((index + 1) < tokens.Count ? tokens[index + 1] : string.Empty).ToLower();

                if((keyWords.Contains(currentTokenLowerCase) && (string.IsNullOrEmpty(previousTokenLowerCase) || !keyWords.Contains(previousTokenLowerCase + ' ' + currentTokenLowerCase))) || keyWords.Contains(currentTokenLowerCase + ' ' + nextTokenLowerCase)) {
                    str.Append(Environment.NewLine);
                }
                str.Append(currentToken);
            }
            return str.ToString().Trim();
        }

        private static List<string> SplitIntoTokens(string pSql) {

            List<string> tokens = new List<string>();
            StringBuilder currentWord = new StringBuilder();

            for(int index = 0; index < pSql.Length; index++) {

                char c = pSql[index];

                if(char.IsWhiteSpace(c) || c == '(' || c == ')') {

                    tokens.Add(currentWord.ToString());

                    if(!char.IsWhiteSpace(c)) {
                        tokens.Add("" + c);
                    }
                    currentWord.Clear();
                }
                else {
                    currentWord.Append(c);
                }
            }

            if(currentWord.Length > 0) {
                tokens.Add(currentWord.ToString());
            }
            return tokens;
        }
    }
}