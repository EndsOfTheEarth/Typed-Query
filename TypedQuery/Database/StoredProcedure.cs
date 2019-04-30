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
using System.Data;
using System.Text;

namespace Sql.Database {

    public class StoredProcedure {

        public string Schema { get; private set; }
        public string Name { get; private set; }

        public List<SpParameter> Parameters { get; private set; }

        public StoredProcedure(string pSchema, string pName) {
            Schema = pSchema;
            Name = pName;
            Parameters = new List<SpParameter>();
        }

        public void AddParameter(SpParameter pParameter) {
            Parameters.Add(pParameter);
        }
    }

    public class SpParameter {

        public int ParamId { get; private set; }
        public string Name { get; private set; }
        public DbType ParamType { get; private set; }
        public ParameterDirection Direction { get; private set; }

        public SpParameter(int pParamId, string pName, DbType pParamType, ParameterDirection pDirection) {
            ParamId = pParamId;
            Name = pName;
            ParamType = pParamType;
            Direction = pDirection;
        }
    }
}