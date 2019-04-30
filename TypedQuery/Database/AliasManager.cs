
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

namespace Sql.Database {

    public interface IAliasManager {
        string GetAlias(ATable pTable);
    }

    public class AliasManager : IAliasManager {

        private readonly Dictionary<ATable, string> mTables = new Dictionary<ATable, string>();
        private int mAliasCounter = 0;

        public string GetAlias(ATable pTable) {

            if(pTable == null) {
                throw new NullReferenceException("pTable cannot be null");
            }

            if(!mTables.ContainsKey(pTable)) {
                mTables.Add(pTable, "_" + mAliasCounter.ToString());
                mAliasCounter++;
            }
            return mTables[pTable];
        }
    }
}