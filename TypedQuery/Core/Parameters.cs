
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
using System.Data.Common;

namespace Sql.Core {
	
	internal class Parameters {

		private readonly System.Data.Common.DbCommand mCommand;
		private int mParamCounter = 1;

		public Parameters(System.Data.Common.DbCommand pCommand) {

			if(pCommand == null) {
				throw new NullReferenceException($"{ nameof(pCommand) } cannot be null");
			}

			mCommand = pCommand;
		}
		
		public string AddParameter(System.Data.DbType pDbType, object pValue) {
			
			if(mCommand.Parameters.Count < 15){	//If there aren't too many parameters then check to see if pValue is a duplicate value that already has a parameter
				foreach(DbParameter param in mCommand.Parameters){
					if(param.Value.Equals(pValue) && param.DbType == pDbType) {
						return param.ParameterName;
					}
				}
			}
			System.Data.Common.DbParameter parameter = mCommand.CreateParameter();
			parameter.ParameterName = "@" + mParamCounter;
			parameter.DbType = pDbType;
			parameter.Value = pValue != null ? pValue : DBNull.Value;
			mCommand.Parameters.Add(parameter);
			mParamCounter++;
			return parameter.ParameterName;
		}
	}
}