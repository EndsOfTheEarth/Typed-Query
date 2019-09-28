
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

namespace Sql.Interfaces {

    public interface IDistinct : ITop {
        /// <summary>
        /// Select rows with distinct values
        /// </summary>
        ITop Distinct { get; }
    }

    public interface ITop : IFromInto {
        /// <summary>
        /// Selects first top number of rows.
        /// </summary>
        IFromInto Top(int pRows);
    }

    public interface IFromInto : IFrom {

        /// <summary>
        /// Into table
        /// </summary>
        IFrom Into(ATable pTable);
    }

    public interface IFrom {
        /// <summary>
        /// From table
        /// </summary>
        IJoin From(ATable pTable, params string[] pHints);

        #region Hide Members
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        bool Equals(object pObject);
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        int GetHashCode();
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        Type GetType();
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        string? ToString();
        #endregion
    }

    public interface IJoin : IWhere {
        /// <summary>
        /// Joins pTable using condition pCondition
        /// </summary>
        IJoin Join(ATable pTable, Condition pCondition, params string[] pHints);

        /// <summary>
        /// Joins pTable using condition pCondition if pIncludeJoin is true
        /// </summary>
        IJoin JoinIf(bool pIncludeJoin, ATable pTable, Condition pCondition, params string[] pHints);

        /// <summary>
        /// Left joins pTable using condition pCondition
        /// </summary>
        IJoin LeftJoin(ATable pTable, Condition pCondition, params string[] pHints);

        /// <summary>
        /// Left joins pTable using condition pCondition if pIncludeJoin is true
        /// </summary>
        IJoin LeftJoinIf(bool pIncludeJoin, ATable pTable, Condition pCondition, params string[] pHints);

        /// <summary>
        /// Right joins pTable using condition pCondition
        /// </summary>
        IJoin RightJoin(ATable pTable, Condition pCondition, params string[] pHints);

        /// <summary>
        /// Right joins pTable using condition pCondition if pIncludeJoin is true
        /// </summary>
        IJoin RightJoinIf(bool pIncludeJoin, ATable pTable, Condition pCondition, params string[] pHints);
    }

    public interface IWhere : IGroupBy {
        /// <summary>
        /// Where condition of query
        /// </summary>
        IGroupBy Where(Condition pCondition);
    }

    public interface IGroupBy : IOrderBy {
        /// <summary>
        /// Group query by columns pColumns
        /// </summary>
        IHaving GroupBy(params ISelectable[] pColumns);
    }

    public interface IHaving : IOrderBy {
        /// <summary>
        /// Query having clause
        /// </summary>
        IOrderBy Having(Condition pCondition);
    }

    public interface IOrderBy : IAppend {
        /// <summary>
        /// Query order by clause
        /// </summary>
        IAppend OrderBy(params IOrderByColumn[] pOrderByColumns);

        /// <summary>
        /// Union query
        /// </summary>
        /// <returns></returns>
        IDistinct Union(ISelectableColumns pField, params ISelectableColumns[] pFields);

        /// <summary>
        /// Union All query
        /// </summary>
        /// <returns></returns>
        IDistinct UnionAll(ISelectableColumns pField, params ISelectableColumns[] pFields);

        /// <summary>
        /// Intersect query
        /// </summary>
        /// <returns></returns>
        IDistinct Intersect(ISelectableColumns pField, params ISelectableColumns[] pFields);

        /// <summary>
        /// Except query
        /// </summary>
        /// <returns></returns>
        IDistinct Except(ISelectableColumns pField, params ISelectableColumns[] pFields);
    }

    public interface IAppend : IUseParams {
        /// <summary>
        /// Allows options to be appended to the end of the query.
        /// Example: Sql Server - .OPTION("OPTION(MAXDOP 1)").Execute();
        /// Or
        /// Example: PostgreSql - .OPTION("FOR UPDATE").Execute();
        /// </summary>
        /// <returns></returns>
        IUseParams Append(string pCustomSql);
    }

    public interface IUseParams : ITimeout {

        /// <summary>
        /// Force the query to use parameters or not. If not set then the default is used from Sql.Settings.UseParameters
        /// </summary>
        /// <param name="pUseParameters"></param>
        /// <returns></returns>
        ITimeout UseParameters(bool pUseParameters);

        #region Hide Members
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        new bool Equals(object pObject);
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        new int GetHashCode();
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        new Type GetType();
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        new string? ToString();
        #endregion
    }

    public interface ITimeout : IExecute {

        /// <summary>
        /// Set query timeout. Overrides the default in Settings.DefaultTimeout
        /// </summary>
        /// <param name="pSeconds"></param>
        /// <returns></returns>
        IExecute Timeout(int pSeconds);

        #region Hide Members
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        new bool Equals(object pObject);
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        new int GetHashCode();
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        new Type GetType();
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        new string? ToString();
        #endregion
    }

    public interface IExecute {

        /// <summary>
        /// Rerurns query sql
        /// </summary>
        /// <returns></returns>
        string GetSql(ADatabase pDatabase);

        /// <summary>
        /// Executes query using default Isolation level
        /// </summary>
        /// <returns></returns>
        IResult Execute(ADatabase pDatabase);

        /// <summary>
        /// Executes query using read uncommited Isolation level
        /// </summary>
        /// <returns></returns>
        IResult ExecuteUncommitted(ADatabase pDatabase);

        /// <summary>
        /// Executes query using transaction provided
        /// </summary>
        /// <param name="pTransaction"></param>
        /// <returns></returns>
        IResult Execute(Transaction pTransaction);

        #region Hide Members
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        bool Equals(object pObject);
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        int GetHashCode();
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        Type GetType();
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        string? ToString();
        #endregion
    }
}