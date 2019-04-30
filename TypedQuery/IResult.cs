
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

namespace Sql {

    public interface IResult {

        /// <summary>
        /// Number of rows in query result
        /// </summary>
        int Count { get; }

        ///<summary>
        /// Number of rows effected by an insert, update or delete
        /// </summary>
        int RowsEffected { get; }

        /// <summary>
        /// Query used to populate query result
        /// </summary>
        string SqlQuery { get; }

        /// <summary>
        /// Returns row for pTable and pIndex
        /// </summary>
        /// <param name="pTable"></param>
        /// <param name="pIndex"></param>
        /// <returns></returns>
        ARow GetRow(ATable pTable, int pIndex);

        /// <summary>
        /// Gets function value
        /// </summary>
        /// <param name="pFunction"></param>
        /// <param name="pIndex"></param>
        /// <returns></returns>
        object GetValue(ISelectable pFunction, int pIndex);

        /// <summary>
        /// Gets result size in bytes. This is an aprox value.
        /// </summary>
        /// <returns></returns>
        int GetDataSetSizeInBytes();

        #region Hide Members
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        bool Equals(object pObject);
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        int GetHashCode();
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        Type GetType();
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        string ToString();
        #endregion
    }
}