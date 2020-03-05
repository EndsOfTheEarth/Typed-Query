
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

namespace TypedQueryGenerator.Logic.CodeGeneration {

    public static class NameFormatter {

        public static string Format(string pString) {

            StringBuilder str = new StringBuilder();

            for(int index = 0; index < pString.Length; index++) {

                char c = pString[index];

                if(index == 0 || (index > 0 && pString[index - 1] == '_')) {
                    str.Append((string.Empty + c).ToUpper());
                }
                else {
                    str.Append(c);
                }
            }
            return str.ToString();
        }
    }
}