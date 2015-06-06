
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
		
			if(pTable == null)
				throw new NullReferenceException("pTable cannot be null");
			
			if(!mTables.ContainsKey(pTable)){
				mTables.Add(pTable, "_" + mAliasCounter.ToString());
				mAliasCounter++;
			}
			
			return mTables[pTable];
		}
	}
}