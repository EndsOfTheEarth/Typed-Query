
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

namespace Sql.Database.SqlServer {
	
	public sealed class GrantPermissions {
		
		private readonly List<PermSet> mPermList = new List<PermSet>();
		
		public GrantPermissions() {
			
		}
		
		public void AddPermission(ATable pTable, params Permission[] pPermissions){
			
			if(pTable == null)
				throw new NullReferenceException("pTable cannot be null");
			
			if(pPermissions == null)
				throw new NullReferenceException("pPermissions cannot be null");
			
			mPermList.Add(new PermSet(pTable, pPermissions));
		}
		
		public string CreateGrantScript(string pUserName, bool pReadonly) {
			
			if(string.IsNullOrWhiteSpace(pUserName))
				throw new Exception("pUserName cannot be null or empty");
			
			if(mPermList.Count == 0)
				return string.Empty;
			
			StringBuilder sql = new StringBuilder();
			
			Dictionary<string, object> seenLookup = new Dictionary<string, object>();
			
			foreach(PermSet permSet in mPermList){
				
				string seenKey = permSet.Table.TableName.ToLower();
				
				if(seenLookup.ContainsKey(seenKey))
					throw new Exception("Table is in permissions set more than once");
				
				seenLookup.Add(seenKey, null);
				
				if(!pReadonly || HasPermission(Permission.Select, permSet))
					sql.Append(CreateGrantLine(permSet, pUserName, pReadonly));
				
				sql.Append(CreateRevokeLine(permSet, pUserName, pReadonly));
			}			
			return sql.ToString();
		}
		
		private string CreateGrantLine(PermSet pPermSet, string pUserName, bool pReadonly){
			
			StringBuilder grantSql = new StringBuilder();
			
			if(pPermSet.Permissions.Length == 0)
				return string.Empty;
			
			grantSql.Append("GRANT ");
			
			bool addCommar = false;
			
			if(HasPermission(Permission.Select, pPermSet)){
				
				if(addCommar)
					grantSql.Append(",");
				
				addCommar = true;
				grantSql.Append("SELECT");
			}
			
			if(!pReadonly) {
				
				if(HasPermission(Permission.Insert, pPermSet)){
					
					if(addCommar)
						grantSql.Append(",");
					
					addCommar = true;
					grantSql.Append("INSERT");
				}
				
				if(HasPermission(Permission.Update, pPermSet)){
					
					if(addCommar)
						grantSql.Append(",");
					
					addCommar = true;
					grantSql.Append("UPDATE");
				}
				
				if(HasPermission(Permission.Delete, pPermSet)){
					
					if(addCommar)
						grantSql.Append(",");
					
					addCommar = true;
					grantSql.Append("DELETE");
				}
			}
			
			if(!addCommar)
				return string.Empty;
			
			grantSql.Append(" ON ").Append(pPermSet.Table.TableName).Append(" TO ").Append(pUserName).Append(";").Append(System.Environment.NewLine);
			
			return grantSql.ToString();
		}
		
		private string CreateRevokeLine(PermSet pPermSet, string pUserName, bool pReadonly){
			
			
			StringBuilder revokeSql = new StringBuilder();
			
			if(pPermSet.Permissions.Length == 0)
				return string.Empty;
			
			revokeSql.Append("REVOKE ");
			
			bool addCommar = false;
			
			if(!HasPermission(Permission.Select, pPermSet)) {
				
				if(addCommar)
					revokeSql.Append(",");
				
				addCommar = true;
				revokeSql.Append("SELECT");
			}
			
			if(pReadonly || !HasPermission(Permission.Insert, pPermSet)) {
				
				if(addCommar)
					revokeSql.Append(",");
				
				addCommar = true;
				revokeSql.Append("INSERT");
			}
			
			if(pReadonly || !HasPermission(Permission.Update, pPermSet)) {
				
				if(addCommar)
					revokeSql.Append(",");
				
				addCommar = true;
				revokeSql.Append("UPDATE");
			}
			
			if(pReadonly || !HasPermission(Permission.Delete, pPermSet)) {
				
				if(addCommar)
					revokeSql.Append(",");
				
				addCommar = true;
				revokeSql.Append("DELETE");
			}
			
			if(!addCommar)
				return string.Empty;
			
			revokeSql.Append(" ON ").Append(pPermSet.Table.TableName).Append(" TO ").Append(pUserName).Append(";").Append(System.Environment.NewLine);
			
			return revokeSql.ToString();
		}
		
		private bool HasPermission(Permission pPermission, PermSet pPermSet) {
		
			foreach(Permission perm in pPermSet.Permissions) {
				
				if(perm == pPermission)
					return true;
			}
			return false;
		}
		
		
		
		private class PermSet {
		
			public ATable Table { get; private set; }
			public Permission[] Permissions { get; private set; }
			
			public PermSet(ATable pTable, Permission[] pPermissions){
				Table = pTable;
				Permissions = pPermissions;
			}
		}
	}
	
	public enum Permission {
		Select,
		Insert,
		Update,
		Delete
	}
}