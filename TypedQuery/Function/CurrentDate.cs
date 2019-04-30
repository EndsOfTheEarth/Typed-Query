
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

namespace Sql.Function {

    public sealed class CurrentDateTime : ADateTimeFunction {

        private readonly DatabaseType mDatabaseType;

        public CurrentDateTime(DatabaseType pDatabaseType) {

            if(pDatabaseType != DatabaseType.Mssql && pDatabaseType != DatabaseType.PostgreSql) {
                throw new Exception("Unsupported database type: " + mDatabaseType.ToString());
            }
            mDatabaseType = pDatabaseType;
        }

        public DateTime this[int pIndex, IResult pResult] {
            get {
                return (DateTime)pResult.GetValue(this, pIndex);
            }
        }
        public override string GetFunctionSql(ADatabase pDatabase, bool pUseAlias, Sql.Database.IAliasManager pAliasManager) {

            if(mDatabaseType == DatabaseType.Mssql) {
                return "GETDATE()";
            }
            else if(mDatabaseType == DatabaseType.PostgreSql) {
                return "current_timestamp";
            }
            else {
                throw new Exception("Unknown database type: " + mDatabaseType.ToString());
            }
        }
        public override object GetValue(ADatabase pDatabase, System.Data.Common.DbDataReader pReader, int pColumnIndex) {
            return pReader.GetDateTime(pColumnIndex);
        }
    }
}