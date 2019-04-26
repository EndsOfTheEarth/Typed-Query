
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
using System.Data.SqlClient;

namespace Sql {

    public sealed class SqlServerDatabase : Sql.ADatabase {

        private readonly string mConnectionString;

        public SqlServerDatabase(string pDbTitle, string pServerName, string pDbName, string pUserName, string pPassword, bool pEncrypt) : base(pDbTitle, DatabaseType.Mssql) {

            if(string.IsNullOrEmpty(pServerName)) {
                throw new Exception($"{ nameof(pServerName) } cannot be null or empty");
            }

            if(string.IsNullOrEmpty(pDbName)) {
                throw new Exception($"{ nameof(pDbName) } cannot be null or empty");
            }

            if(string.IsNullOrEmpty(pUserName)) {
                throw new Exception($"{ nameof(pUserName) } cannot be null or empty");
            }

            if(string.IsNullOrEmpty(pPassword)) {
                throw new Exception($"{ nameof(pPassword) } cannot be null or empty");
            }

            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();

            builder.DataSource = pServerName;
            builder.InitialCatalog = pDbName;
            builder.UserID = pUserName;
            builder.Password = pPassword;
            builder.Encrypt = pEncrypt;

            mConnectionString = builder.ConnectionString;
        }

        public SqlServerDatabase(string pDbTitle, SqlConnectionStringBuilder pSqlConnectionStringBuilder) : base(pDbTitle, DatabaseType.Mssql) {
            mConnectionString = pSqlConnectionStringBuilder.ConnectionString;
        }

        public SqlServerDatabase(string pDbTitle, string pConnectionString) : base(pDbTitle, DatabaseType.Mssql) {

            if(string.IsNullOrEmpty(pConnectionString)) {
                throw new Exception($"{ nameof(pConnectionString) } cannot be null or empty");
            }
            mConnectionString = pConnectionString;
        }

        public override System.Data.Common.DbConnection GetConnection(bool pCanBeReadonly) {
            SqlConnection connection = new SqlConnection(mConnectionString);
            connection.Open();
            return connection;
        }
    }
}