
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
using System.Text;
using System.Collections.Generic;

namespace Sql.Core {
	
	public class AlaisCounter {
		
		private readonly static char[] CHARS = new char[] { '0','1','2','3','4','5','6','7','8','9','a','b','c','d','e','f','g','h','i','j','k','l','m','n','o','p','q','r','s','t','u','v','w','x','y','z' };
		private int[] mCounter = null;
		
		public AlaisCounter() {
			
		}
		
		/// <summary>
		/// Returns the next alias value.
		/// </summary>
		/// <returns></returns>
		public string GetNextAlias() {
			
			lock(this) {
				
				if(mCounter == null)
					mCounter = new int[]{ 0 };
				else {					
					//Increment alias counter
					
					bool increaseCounter = true;
					
					for (int index = mCounter.Length - 1; index >= 0; index--) {
						
						int value = mCounter[index];
						
						if(value == (CHARS.Length - 1))
							mCounter[index] = 0;
						else {
							mCounter[index]++;
							increaseCounter = false;
							break;
						}
					}					
					if(increaseCounter)
						mCounter = new int[mCounter.Length + 1];	//All zeros					
				}
				return GetCurrentAlias();
			}
		}
		
		private string GetCurrentAlias(){
			
			StringBuilder alias = new StringBuilder();
			
			for (int index = 0; index < mCounter.Length; index++)
					alias.Append(CHARS[mCounter[index]]);
			
			return alias.ToString();
		}
	}
}