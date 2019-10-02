
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

    public sealed class Int64Key<TABLE> {

        public Int64 Value { get; private set; }

        public Int64Key(Int64 pValue) {
            Value = pValue;
        }

        public static bool operator ==(Int64Key<TABLE>? pA, Int64Key<TABLE>? pB) {
            return pA!.Equals(pB);
        }
        public static bool operator !=(Int64Key<TABLE>? pA, Int64Key<TABLE>? pB) {
            return !pA!.Equals(pB);
        }

        public override bool Equals(object? obj) {

            if(obj is Int64Key<TABLE>) {
                return Value.CompareTo(((Int64Key<TABLE>)obj).Value) == 0;
            }
            return false;
        }
        public override int GetHashCode() {
            return Value.GetHashCode();
        }
    }
}