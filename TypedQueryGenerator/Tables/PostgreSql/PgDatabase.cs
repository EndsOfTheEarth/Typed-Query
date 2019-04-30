
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
using Npgsql;

namespace Postgresql {

    public class PgDatabase : Sql.ADatabase {

        public readonly static PgDatabase Instance = new PgDatabase("abc");

        private string mConnectionString;

        public PgDatabase(string pConnectionString) : base("<<db>>", Sql.DatabaseType.PostgreSql) {

            if(string.IsNullOrEmpty(pConnectionString))
                throw new Exception("pConnectionString cannot be null or empty");

            mConnectionString = pConnectionString;
        }

        public void SetConnectionString(string pConnectionString) {
            mConnectionString = pConnectionString;
        }

        public override System.Data.Common.DbConnection GetConnection(bool pCanBeReadonly) {
            lock(this) {
                Npgsql.NpgsqlConnection connection = new Npgsql.NpgsqlConnection(mConnectionString);
                connection.Open();
                return connection;
            }
        }
    }
}