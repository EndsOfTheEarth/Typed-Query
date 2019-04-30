
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
using System.Reflection;

namespace Sql {

    [AttributeUsage(AttributeTargets.Class)]
    public sealed class TableAttribute : Attribute {

        public string Description { get; private set; }

        public TableAttribute(string pDescription) {
            Description = pDescription ?? string.Empty;
        }
    }

    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public sealed class ColumnAttribute : Attribute {

        public string Description { get; private set; }

        public ColumnAttribute(string pDescription) {
            Description = pDescription ?? string.Empty;
        }
    }
}