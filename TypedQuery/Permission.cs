
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
using System.Reflection;

namespace Sql {
	
	public abstract class ALogin {

		public string Name { get; private set; }

		public ALogin(string pName) {

			if(string.IsNullOrEmpty(pName)) {
				throw new ArgumentException($"{nameof(pName)} cannot be null or empty");
			}
			Name = pName;
		}
	}

	[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
	public sealed class GrantTable : Attribute {
		
		public ALogin User { get; private set; }
		public Privilege Privilege { get; private set; }
		
		public GrantTable(ALogin pUser, Privilege pPrivilege) {
			User = pUser;
			Privilege = pPrivilege;
		}
	}
	
	[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = true)]
	public sealed class GrantColumn : Attribute {
		
		public ALogin User { get; private set; }
		public ColumnPrivilege Privilege { get; private set; }
		
		public GrantColumn(ALogin pUser, ColumnPrivilege pPrivilege) {			
			User = pUser;
			Privilege = pPrivilege;
		}
	}
	
	[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = true)]
	public sealed class RevokeColumn : Attribute {
		
		public ALogin User { get; private set; }
		public ColumnPrivilege Privilege { get; private set; }
		
		public RevokeColumn(ALogin pUser, ColumnPrivilege pPrivilege) {			
			User = pUser;
			Privilege = pPrivilege;
		}
	}
	
	public enum Privilege {
		
		ALL = 1,
		SELECT = 2,
		INSERT = 4,
		UPDATE = 8,
		DELETE = 16,
		TRUNCATE = 32,
		REFERENCES = 64,
		TRIGGER = 128,
		EXECUTE = 256
	}
	
	public enum ColumnPrivilege {
		
		ALL = 1,
		SELECT = 2,
		UPDATE = 8,
		REFERENCES = 64,
	}
}