
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

namespace Sql.Types {

    public sealed class Int16Key<TABLE> {

        public Int16 Value { get; private set; }

        public Int16Key(Int16 pValue) {
            Value = pValue;
        }

        public static bool operator ==(Int16Key<TABLE>? pA, Int16Key<TABLE>? pB) {
            return pA!.Equals(pB);
        }
        public static bool operator !=(Int16Key<TABLE>? pA, Int16Key<TABLE>? pB) {
            return !pA!.Equals(pB);
        }

        public override bool Equals(object? obj) {

            if(obj is Int16Key<TABLE>) {
                return Value.CompareTo(((Int16Key<TABLE>)obj).Value) == 0;
            }
            return false;
        }
        public override int GetHashCode() {
            return Value.GetHashCode();
        }
    }
}