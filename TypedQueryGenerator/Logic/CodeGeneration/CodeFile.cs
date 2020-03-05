
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
    
    public class CodeFile {

        private readonly StringBuilder mText = new StringBuilder();

        private readonly string mIndent;

        public CodeFile(string indent) {

            mIndent = indent;

            if(string.IsNullOrEmpty(mIndent)) {
                mIndent = "    ";
            }
        }
        public CodeFile Indent(int indent) {

            if(indent < 0 || indent > 100) {
                throw new Exception($"{ nameof(indent) } must be >= 0 and <= 100");
            }

            for(int index = 0; index < indent; index++) {
                mText.Append(mIndent);
            }
            return this;
        }
        public CodeFile Append(string pValue) {
            mText.Append(pValue);
            return this;
        }
        public CodeFile EndLine() {
            mText.Append(Environment.NewLine);
            return this;
        }
        public override string ToString() {
            return mText.ToString();
        }
    }
}