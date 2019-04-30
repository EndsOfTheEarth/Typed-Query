
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

namespace Sql.Database.SqlServer {

    public static class TableHint {

        public readonly static string FASTFIRSTROW = "FASTFIRSTROW";
        public readonly static string FORCESCAN = "FORCESCAN";
        public readonly static string HOLDLOCK = "HOLDLOCK";
        public readonly static string NOLOCK = "NOLOCK";
        public readonly static string NOWAIT = "NOWAIT";
        public readonly static string PAGLOCK = "PAGLOCK";
        public readonly static string READCOMMITTED = "READCOMMITTED";
        public readonly static string READCOMMITTEDLOCK = "READCOMMITTEDLOCK";
        public readonly static string READPAST = "READPAST";
        public readonly static string READUNCOMMITTED = "READUNCOMMITTED";
        public readonly static string REPEATABLEREAD = "REPEATABLEREAD";
        public readonly static string ROWLOCK = "ROWLOCK";
        public readonly static string SERIALIZABLE = "SERIALIZABLE";
        public readonly static string TABLOCK = "TABLOCK";
        public readonly static string TABLOCKX = "TABLOCKX";
        public readonly static string UPDLOCK = "UPDLOCK";
        public readonly static string XLOCK = "XLOCK";
    }
}