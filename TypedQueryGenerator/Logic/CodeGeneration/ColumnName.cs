
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
using System.Text;
using TypedQuery.Logic;

namespace TypedQueryGenerator.Logic.CodeGeneration {
    
    public static class ColumnName {

        public static string GetColumnName(IColumn pColumn, string pColumnPrefix, bool pRemoveUnderscores) {

            string value;

            if(!string.IsNullOrEmpty(pColumnPrefix) && pColumn.ColumnName.ToLower().StartsWith(pColumnPrefix.ToLower())) {
                value = pColumn.ColumnName.Substring(pColumnPrefix.Length);
            }
            else {
                value = pColumn.ColumnName;
            }

            if(pRemoveUnderscores) {

                StringBuilder name = new StringBuilder();

                bool upperCaseNextChar = true;

                for(int index = 0; index < value.Length; index++) {

                    char c = value[index];

                    if(c == '_') {
                        upperCaseNextChar = true;
                    }
                    else {

                        if(upperCaseNextChar) {
                            name.Append(Char.ToUpper(c));
                            upperCaseNextChar = false;
                        }
                        else {
                            name.Append(c);
                        }
                    }
                }
                return name.ToString();
            }
            return value;
        }
    }
}