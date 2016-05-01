
/*
 * 
 * Copyright (C) 2009-2016 JFo.nz
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
using System.Web.Script.Serialization;
using System.IO;

namespace TypedQuery.Connection {

	public class Connection {

		public Sql.DatabaseType DatabaseType { get; set; }
		public string ConnectionString { get; set; }

		public Connection() { }

		public Connection(Connection pConnection) {
			DatabaseType = pConnection.DatabaseType;
			ConnectionString = pConnection.ConnectionString;
		}

		public override string ToString() {
			return DatabaseType.ToString() + " -> " + ConnectionString;
		}
	}

	public class ConnectionsFile {

		private readonly static string sFileName = "settings.json";

		public void Save(List<Connection> pConnections) {
			File.WriteAllText(sFileName, new JavaScriptSerializer().Serialize(pConnections));
		}

		public List<Connection> Load() {
			
			if(!File.Exists(sFileName))
				return new List<Connection>();

			return new JavaScriptSerializer().Deserialize<List<Connection>>(File.ReadAllText(sFileName));
		}
	}
}