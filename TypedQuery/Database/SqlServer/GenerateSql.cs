
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
using Sql.Interfaces;

namespace Sql.Database.SqlServer {

    internal static class GenerateSql {

        internal static string GetSelectQuery(ADatabase pDatabase, Core.QueryBuilder pQueryBuilder, Core.Parameters? pParameters, IAliasManager pAliasManager) {

            if(pAliasManager == null) {
                throw new NullReferenceException("pAliasManager cannot be null");
            }

            StringBuilder sql = new StringBuilder();

            IList<ATable> allTables = pQueryBuilder.GetAllTables();

            if(pQueryBuilder.UnionQuery != null) {

                sql.Append(GetSelectQuery(pDatabase, pQueryBuilder.UnionQuery, pParameters, pAliasManager));

                if(pQueryBuilder.UnionType == Sql.Core.UnionType.UNION) {
                    sql.Append(" UNION ");
                }
                else if(pQueryBuilder.UnionType == Sql.Core.UnionType.UNION_ALL) {
                    sql.Append(" UNION ALL ");
                }
                else if(pQueryBuilder.UnionType == Sql.Core.UnionType.INTERSECT) {
                    sql.Append(" INTERSECT ");
                }
                else if(pQueryBuilder.UnionType == Sql.Core.UnionType.EXCEPT) {
                    sql.Append(" EXCEPT ");
                }
                else {
                    throw new Exception("Unknown union type: " + pQueryBuilder.UnionType.ToString());
                }
            }

            sql.Append("SELECT ");

            if(pQueryBuilder.IsDistinct) {
                sql.Append("DISTINCT ");
            }

            if(pQueryBuilder.TopRows != null) {
                sql.Append("TOP ").Append(pQueryBuilder.TopRows.Value.ToString());
            }

            bool areAlisesRequired = pQueryBuilder.JoinList.Count > 0 && !pQueryBuilder.FromTable!.IsTemporaryTable;

            for(int index = 0; index < pQueryBuilder.SelectColumns.Length; index++) {

                ISelectable selectField = pQueryBuilder.SelectColumns[index];

                if(index > 0) {
                    sql.Append(',');
                }

                if(selectField is AColumn) {
                    sql.Append(GetColumnSql((AColumn)selectField, areAlisesRequired, pAliasManager));
                }
                else if(selectField is IFunction) {
                    sql.Append(((IFunction)selectField).GetFunctionSql(pDatabase, areAlisesRequired, pAliasManager));
                }
                else {
                    throw new Exception("Field type not supported yet");
                }
            }

            if(pQueryBuilder.IntoTable != null) {

                sql.Append(" INTO ");

                if(pQueryBuilder.IntoTable.IsTemporaryTable) {
                    sql.Append("#");
                }
                sql.Append(pQueryBuilder.IntoTable.TableName);
            }

            sql.Append(" FROM ");

            if(!string.IsNullOrEmpty(pQueryBuilder.FromTable!.Schema)) {
                sql.Append(pQueryBuilder.FromTable.Schema).Append(".");
            }

            if(pQueryBuilder.FromTable.IsTemporaryTable) {
                sql.Append("#");
            }

            sql.Append(pQueryBuilder.FromTable.TableName);

            sql.Append(" AS ").Append(pAliasManager.GetAlias(pQueryBuilder.FromTable));

            if(pQueryBuilder.FromHints != null && pQueryBuilder.FromHints.Length > 0) {

                sql.Append(" WITH(");

                for(int hintIndex = 0; hintIndex < pQueryBuilder.FromHints.Length; hintIndex++) {

                    string hint = pQueryBuilder.FromHints[hintIndex];

                    if(hintIndex > 0) {
                        sql.Append(",");
                    }
                    sql.Append(hint);
                }
                sql.Append(")");
            }

            if(pQueryBuilder.JoinList.Count > 0) {

                for(int index = 0; index < pQueryBuilder.JoinList.Count; index++) {

                    Core.Join join = pQueryBuilder.JoinList[index];

                    if(join.JoinType == Core.JoinType.JOIN) {
                        sql.Append(" JOIN ");
                    }
                    else if(join.JoinType == Core.JoinType.LEFT) {
                        sql.Append(" LEFT JOIN ");
                    }
                    else if(join.JoinType == Core.JoinType.RIGHT) {
                        sql.Append(" RIGHT JOIN ");
                    }
                    else {
                        throw new Exception("Unknown join type: " + join.JoinType.ToString());
                    }

                    if(!string.IsNullOrEmpty(join.Table.Schema)) {
                        sql.Append(join.Table.Schema).Append(".");
                    }

                    if(join.Table.IsTemporaryTable) {
                        sql.Append("#");
                    }

                    sql.Append(join.Table.TableName).Append(" AS ").Append(pAliasManager.GetAlias(join.Table)).Append(" ON ").Append(GetConditionSql(pDatabase, join.Condition, pParameters, true, pAliasManager));

                    if(join.Hints != null && join.Hints.Length > 0) {

                        sql.Append(" WITH(");

                        for(int hintIndex = 0; hintIndex < join.Hints.Length; hintIndex++) {

                            string hint = join.Hints[hintIndex];

                            if(hintIndex > 0) {
                                sql.Append(",");
                            }
                            sql.Append(hint);
                        }
                        sql.Append(")");
                    }
                }
            }

            if(pQueryBuilder.WhereCondition != null) {
                sql.Append(" WHERE ").Append(GetConditionSql(pDatabase, pQueryBuilder.WhereCondition, pParameters, areAlisesRequired, pAliasManager));
            }

            if(pQueryBuilder.GroupByColumns != null && pQueryBuilder.GroupByColumns.Length > 0) {

                sql.Append(" GROUP BY ");

                for(int index = 0; index < pQueryBuilder.GroupByColumns.Length; index++) {

                    ISelectable column = pQueryBuilder.GroupByColumns[index];

                    if(index > 0) {
                        sql.Append(',');
                    }

                    if(column is AColumn) {

                        AColumn aColumn = (AColumn)column;

                        if(areAlisesRequired) {
                            sql.Append(pAliasManager.GetAlias(aColumn.Table)).Append('.');
                        }
                        sql.Append(aColumn.ColumnName);
                    }
                    else if(column is IFunction) {
                        sql.Append(((IFunction)column).GetFunctionSql(pDatabase, areAlisesRequired, pAliasManager));
                    }
                    else {
                        throw new Exception("column type not supported yet");
                    }
                }
            }

            if(pQueryBuilder.HavingCondition != null) {
                sql.Append(" HAVING ").Append(GetConditionSql(pDatabase, pQueryBuilder.HavingCondition, pParameters, areAlisesRequired, pAliasManager));
            }

            if(pQueryBuilder.OrderByColumns != null && pQueryBuilder.OrderByColumns.Length > 0) {

                sql.Append(" ORDER BY ");

                for(int index = 0; index < pQueryBuilder.OrderByColumns.Length; index++) {

                    IOrderByColumn orderByColumn = pQueryBuilder.OrderByColumns[index];

                    if(index > 0) {
                        sql.Append(',');
                    }

                    ISelectable orderByField = orderByColumn.GetOrderByColumn.Column;

                    if(orderByField is AColumn) {
                        sql.Append(GetColumnSql((AColumn)orderByField, areAlisesRequired, pAliasManager));
                    }
                    else if(orderByField is IFunction) {
                        sql.Append(((IFunction)orderByField).GetFunctionSql(pDatabase, areAlisesRequired, pAliasManager));
                    }
                    else {
                        throw new Exception("Field type not supported yet");
                    }

                    switch(orderByColumn.GetOrderByColumn.OrderBy) {
                        case OrderBy.ASC:
                            sql.Append(" ASC");
                            break;
                        case OrderBy.DESC:
                            sql.Append(" DESC");
                            break;
                        case OrderBy.Default:
                            break;
                        default:
                            throw new Exception("Unknown OrderBy type: " + orderByColumn.GetOrderByColumn.OrderBy.ToString());
                    }
                }
            }

            if(!string.IsNullOrEmpty(pQueryBuilder.CustomSql)) {
                sql.Append(" ").Append(pQueryBuilder.CustomSql);
            }
            return sql.ToString();
        }

        internal static string GetInsertQuery(ADatabase pDatabase, Core.InsertBuilder pInsertBuilder, Core.Parameters? pParameters) {

            StringBuilder sql = new StringBuilder("INSERT INTO ");

            IAliasManager aliasManager = new AliasManager();

            if(!string.IsNullOrEmpty(pInsertBuilder.Table.Schema)) {
                sql.Append(pInsertBuilder.Table.Schema).Append(".");
            }

            sql.Append(pInsertBuilder.Table.TableName);

            sql.Append("(");

            for(int index = 0; index < pInsertBuilder.SetValueList.Count; index++) {

                Core.SetValue setValue = pInsertBuilder.SetValueList[index];

                if(index > 0) {
                    sql.Append(',');
                }
                sql.Append(setValue.Column.ColumnName);
            }

            sql.Append(")");

            if(pInsertBuilder.ReturnColumns != null && pInsertBuilder.ReturnColumns.Length > 0) {

                sql.Append(" OUTPUT ");

                for(int index = 0; index < pInsertBuilder.ReturnColumns.Length; index++) {

                    if(index > 0) {
                        sql.Append(",");
                    }
                    sql.Append("INSERTED.").Append(pInsertBuilder.ReturnColumns[index].ColumnName);
                }
            }

            sql.Append(" VALUES(");

            for(int index = 0; index < pInsertBuilder.SetValueList.Count; index++) {

                Core.SetValue setValue = pInsertBuilder.SetValueList[index];

                if(index > 0) {
                    sql.Append(',');
                }

                if(setValue.Value == null) {

                    if(pParameters != null) {
                        sql.Append(pParameters.AddParameter(setValue.Column.DbType, null));
                    }
                    else {
                        sql.Append("NULL");
                    }
                }
                else {
                    sql.Append(GetValue(pDatabase, setValue.Value, pParameters, setValue.Column.DbType, false, aliasManager));
                }
            }
            sql.Append(")");

            return sql.ToString();
        }

        internal static string GetBulkInsertQuery(ADatabase pDatabase, List<Core.InsertBuilder> pInsertBuilders) {

            if(pInsertBuilders.Count == 0) {
                return string.Empty;
            }

            IAliasManager aliasManager = new AliasManager();

            int counter = 0;

            StringBuilder headerSql = new StringBuilder();

            headerSql.Append("INSERT INTO ");

            Core.InsertBuilder firstBuilder = pInsertBuilders[0];

            if(!string.IsNullOrEmpty(firstBuilder.Table.Schema)) {
                headerSql.Append(firstBuilder.Table.Schema).Append(".");
            }

            headerSql.Append(firstBuilder.Table.TableName);

            headerSql.Append("(");

            for(int index = 0; index < firstBuilder.SetValueList.Count; index++) {

                Core.SetValue setValue = firstBuilder.SetValueList[index];

                if(index > 0) {
                    headerSql.Append(',');
                }
                headerSql.Append(setValue.Column.ColumnName);
            }

            headerSql.Append(") VALUES");

            StringBuilder sql = new StringBuilder();

            bool isFirstValue = true;

            for(int insertIndex = 0; insertIndex < pInsertBuilders.Count; insertIndex++) {

                if(insertIndex == 0 || (insertIndex + 1) % 1000 == 0) {

                    if(sql.Length > 0) {
                        sql.Append(";");
                    }
                    sql.Append(headerSql.ToString());
                    isFirstValue = true;
                }

                counter++;

                if(!isFirstValue) {
                    sql.Append(",");
                }

                isFirstValue = false;

                sql.Append("(");

                Core.InsertBuilder insertBuilder = pInsertBuilders[insertIndex];

                for(int index = 0; index < insertBuilder.SetValueList.Count; index++) {

                    Core.SetValue setValue = insertBuilder.SetValueList[index];

                    if(index > 0) {
                        sql.Append(',');
                    }

                    if(setValue.Value == null) {
                        sql.Append("NULL");
                    }
                    else {
                        sql.Append(GetValue(pDatabase, setValue.Value, null, setValue.Column.DbType, false, aliasManager));
                    }
                }
                sql.Append(")");
            }
            return sql.ToString();
        }

        internal static string GetInsertSelectQuery(ADatabase pDatabase, Core.InsertSelectBuilder pInsertBuilder, Core.Parameters? pParameters) {

            if(pInsertBuilder.InsertColumns == null) {
                throw new Exception("There are no insert columns in sql query");
            }

            StringBuilder sql = new StringBuilder("INSERT INTO ");

            IAliasManager aliasManager = new AliasManager();

            if(!string.IsNullOrEmpty(pInsertBuilder.Table.Schema)) {
                sql.Append(pInsertBuilder.Table.Schema).Append(".");
            }

            sql.Append(pInsertBuilder.Table.TableName);
            sql.Append("(");

            for(int index = 0; index < pInsertBuilder.InsertColumns.Length; index++) {

                AColumn column = pInsertBuilder.InsertColumns[index];

                if(index > 0) {
                    sql.Append(',');
                }
                sql.Append(column.ColumnName);
            }
            sql.Append(")");
            sql.Append(GetSelectQuery(pDatabase, (Core.QueryBuilder)pInsertBuilder.SelectQuery!, pParameters, aliasManager));
            return sql.ToString();
        }

        internal static string GetUpdateQuery(ADatabase pDatabase, Core.UpdateBuilder pUpdateBuilder, Core.Parameters? pParameters) {

            StringBuilder sql = new StringBuilder("UPDATE ");

            bool useAlias = pUpdateBuilder.JoinList.Count > 0;

            IAliasManager aliasManager = new AliasManager();

            if(!useAlias) {

                if(!string.IsNullOrEmpty(pUpdateBuilder.Table.Schema)) {
                    sql.Append(pUpdateBuilder.Table.Schema).Append(".");
                }
                sql.Append(pUpdateBuilder.Table.TableName);
            }
            else {
                sql.Append(aliasManager.GetAlias(pUpdateBuilder.Table));
            }

            sql.Append(" SET ");

            for(int index = 0; index < pUpdateBuilder.SetValueList.Count; index++) {

                Core.SetValue setValue = pUpdateBuilder.SetValueList[index];

                if(index > 0) {
                    sql.Append(',');
                }

                if(useAlias) {
                    sql.Append(aliasManager.GetAlias(setValue.Column.Table)).Append(".");
                }

                sql.Append(setValue.Column.ColumnName);

                if(setValue.Value == null) {

                    if(pParameters != null) {
                        sql.Append("=").Append(pParameters.AddParameter(setValue.Column.DbType, null));
                    }
                    else {
                        sql.Append("=NULL");
                    }
                }
                else {
                    sql.Append("=").Append(GetValue(pDatabase, setValue.Value, pParameters, setValue.Column.DbType, useAlias, aliasManager));
                }
            }

            if(pUpdateBuilder.ReturnColumns != null && pUpdateBuilder.ReturnColumns.Length > 0) {

                sql.Append(" OUTPUT ");

                for(int index = 0; index < pUpdateBuilder.ReturnColumns.Length; index++) {

                    if(index > 0) {
                        sql.Append(",");
                    }
                    //INSERTED is the correct value even though this is an update query
                    sql.Append("INSERTED.").Append(pUpdateBuilder.ReturnColumns[index].ColumnName);
                }
            }

            Condition? joinCondition = null;

            if(pUpdateBuilder.JoinList.Count > 0) {

                sql.Append(" FROM ");

                if(!string.IsNullOrEmpty(pUpdateBuilder.Table.Schema)) {
                    sql.Append(pUpdateBuilder.Table.Schema).Append(".");
                }

                sql.Append(pUpdateBuilder.Table.TableName);
                sql.Append(" AS ").Append(aliasManager.GetAlias(pUpdateBuilder.Table)).Append(" ");

                for(int joinIndex = 0; joinIndex < pUpdateBuilder.JoinList.Count; joinIndex++) {

                    Sql.Core.Join join = pUpdateBuilder.JoinList[joinIndex];

                    sql.Append(",");
                    sql.Append(join.Table.TableName).Append(" AS ").Append(aliasManager.GetAlias(join.Table));

                    if(joinCondition == null) {
                        joinCondition = join.Condition;
                    }
                    else {
                        joinCondition = joinCondition & join.Condition;
                    }
                }
            }

            Condition? whereCondition = null;

            if(joinCondition != null && pUpdateBuilder.WhereCondition != null) {
                whereCondition = joinCondition & pUpdateBuilder.WhereCondition;
            }
            else if(joinCondition != null) {
                whereCondition = joinCondition;
            }
            else if(pUpdateBuilder.WhereCondition != null) {
                whereCondition = pUpdateBuilder.WhereCondition;
            }

            if(whereCondition != null) {
                sql.Append(" WHERE ").Append(GetConditionSql(pDatabase, whereCondition, pParameters, pUpdateBuilder.JoinList.Count > 0, aliasManager));
            }
            return sql.ToString();
        }

        internal static string GetDeleteQuery(ADatabase pDatabase, Core.DeleteBuilder pDeleteBuilder, Core.Parameters? pParameters) {

            StringBuilder sql = new StringBuilder("DELETE FROM ");

            if(!string.IsNullOrEmpty(pDeleteBuilder.Table.Schema)) {
                sql.Append(pDeleteBuilder.Table.Schema).Append(".");
            }

            sql.Append(pDeleteBuilder.Table.TableName);

            if(pDeleteBuilder.ReturnColumns != null && pDeleteBuilder.ReturnColumns.Length > 0) {

                sql.Append(" OUTPUT ");

                for(int index = 0; index < pDeleteBuilder.ReturnColumns.Length; index++) {

                    if(index > 0) {
                        sql.Append(",");
                    }
                    sql.Append("DELETED.").Append(pDeleteBuilder.ReturnColumns[index].ColumnName);
                }
            }

            IAliasManager aliasManager = new AliasManager();

            if(pDeleteBuilder.WhereCondition != null) {
                sql.Append(" WHERE ").Append(GetConditionSql(pDatabase, pDeleteBuilder.WhereCondition, pParameters, false, aliasManager));
            }
            return sql.ToString();
        }

        internal static string GetTruncateQuery(ATable pTable) {
            string schema = !string.IsNullOrEmpty(pTable.Schema) ? pTable.Schema + "." : string.Empty;
            return "TRUNCATE TABLE " + schema + pTable.TableName;
        }

        internal static string GetStoreProcedureQuery(ADatabase pDatabase, ATable pTable, Core.Parameters? pParameters, object[] pParams, IAliasManager pAliasManager) {

            StringBuilder sql = new StringBuilder();

            sql.Append("EXEC ").Append(pTable.TableName);

            for(int index = 0; index < pParams.Length; index++) {

                if(index > 0) {
                    sql.Append(",");
                }
                sql.Append(GetValue(pDatabase, pParams[index], pParameters, null, false, pAliasManager));
            }
            return sql.ToString();
        }

        private static string GetConditionSql(ADatabase pDatabase, Condition pCondition, Core.Parameters? pParameters, bool pUseAlias, IAliasManager pAliasManager) {

            if(pCondition.Operator == Operator.IS_NULL) {
                return "(" + GetSideSql(pDatabase, pCondition.Left, pParameters, pUseAlias, null, pAliasManager) + " IS NULL)";
            }
            else if(pCondition.Operator == Operator.IS_NOT_NULL) {
                return "(" + GetSideSql(pDatabase, pCondition.Left, pParameters, pUseAlias, null, pAliasManager) + " IS NOT NULL)";
            }
            string condSql = GetSideSql(pDatabase, pCondition.Left, pParameters, pUseAlias, null, pAliasManager) + GetOperator(pCondition.Operator) + GetSideSql(pDatabase, pCondition.Right!, pParameters, pUseAlias, pCondition.RightDbType, pAliasManager);
            return pCondition.Left is Condition || pCondition.Right is Condition ? "(" + condSql + ")" : condSql;
        }

        private static string GetSideSql(ADatabase pDatabase, object pCond, Core.Parameters? pParameters, bool pUseAlias, System.Data.DbType? pDbType, IAliasManager pAliasManager) {

            if(pCond is Condition) {
                return GetConditionSql(pDatabase, (Condition)pCond, pParameters, pUseAlias, pAliasManager);
            }
            else if(pCond is AColumn) {
                return GetColumnSql((AColumn)pCond, pUseAlias, pAliasManager);
            }
            else if(pCond is Core.QueryBuilder) {
                return "(" + GetSelectQuery(pDatabase, (Core.QueryBuilder)pCond, pParameters, pAliasManager) + ")";
            }
            else if(pCond is INumericCondition) {

                INumericCondition numCond = (INumericCondition)pCond;

                string opp;

                switch(numCond.Operator) {
                    case NumericOperator.ADD:
                        opp = "+";
                        break;
                    case NumericOperator.SUBTRACT:
                        opp = "-";
                        break;
                    case NumericOperator.DIVIDE:
                        opp = "/";
                        break;
                    case NumericOperator.MULTIPLY:
                        opp = "*";
                        break;
                    case NumericOperator.MODULO:
                        opp = "%";
                        break;
                    default:
                        throw new Exception("Unknown numeric operator : '" + numCond.Operator.ToString());
                }
                return "(" + GetSideSql(pDatabase, numCond.Left, pParameters, pUseAlias, null, pAliasManager) + opp + GetSideSql(pDatabase, numCond.Right!, pParameters, pUseAlias, null, pAliasManager) + ")";
            }
            else {
                return GetValue(pDatabase, pCond, pParameters, pDbType, false, pAliasManager);
            }
        }

        private static string GetValue(ADatabase pDatabase, object pValue, System.Data.DbType? pDbType, IAliasManager pAliasManager) {
            return GetValue(pDatabase, pValue, null, pDbType, false, pAliasManager);
        }
        private static string GetValue(ADatabase pDatabase, object pValue, Core.Parameters? pParameters, System.Data.DbType? pDbType, bool pUseColumnAlias, IAliasManager pAliasManager) {

            if(pValue == null) {
                throw new NullReferenceException("pValue cannot be null");
            }

            if(pValue is int) {

                if(pParameters != null) {
                    return pParameters.AddParameter(System.Data.DbType.Int32, pValue);
                }
                return pValue.ToString()!;
            }
            else if(pValue is int?) {

                if(pParameters != null) {
                    return pParameters.AddParameter(System.Data.DbType.Int32, pValue);
                }
                return pValue == null ? "NULL" : pValue.ToString()!;
            }
            else if(pValue is string) {

                if(pParameters != null) {
                    return pParameters.AddParameter(System.Data.DbType.String, pValue);
                }
                return "'" + ((string)pValue).Replace("'", "''") + "'";
            }
            else if(pValue is Int16) {

                if(pParameters != null) {
                    return pParameters.AddParameter(System.Data.DbType.Int16, pValue);
                }
                return pValue.ToString()!;
            }
            else if(pValue is Int16?) {

                if(pParameters != null) {
                    return pParameters.AddParameter(System.Data.DbType.Int16, pValue);
                }
                return pValue == null ? "NULL" : pValue.ToString()!;
            }
            else if(pValue is Int64) {

                if(pParameters != null) {
                    return pParameters.AddParameter(System.Data.DbType.Int64, pValue);
                }
                return pValue.ToString()!;
            }
            else if(pValue is Int64?) {

                if(pParameters != null) {
                    return pParameters.AddParameter(System.Data.DbType.Int64, pValue);
                }
                return pValue == null ? "NULL" : pValue.ToString()!;
            }
            else if(pValue is decimal) {

                if(pParameters != null) {
                    return pParameters.AddParameter(System.Data.DbType.Decimal, pValue);
                }
                return pValue.ToString()!;
            }
            else if(pValue is decimal?) {

                if(pParameters != null) {
                    return pParameters.AddParameter(System.Data.DbType.Decimal, pValue);
                }
                return pValue == null ? "NULL" : pValue.ToString()!;
            }
            else if(pValue is DateTime) {

                if(pParameters != null) {

                    if(pDbType != null) {
                        return pParameters.AddParameter(pDbType.Value, pValue);
                    }
                    else {
                        return pParameters.AddParameter(System.Data.DbType.DateTime, pValue);
                    }
                }

                if(pDbType != null && pDbType == System.Data.DbType.DateTime2) {
                    return "'" + ((DateTime)pValue).ToString("yyyy-MM-dd HH:mm:ss.fffffff") + "'";
                }
                return "'" + ((DateTime)pValue).ToString("yyyy-MM-dd HH:mm:ss.fff") + "'";
            }
            else if(pValue is DateTime?) {

                if(pParameters != null) {

                    if(pDbType != null) {
                        return pParameters.AddParameter(pDbType.Value, pValue);
                    }
                    else {
                        return pParameters.AddParameter(System.Data.DbType.DateTime, pValue);
                    }
                }

                if(pDbType != null && pDbType == System.Data.DbType.DateTime2) {
                    return pValue == null ? "NULL" : ((DateTime?)pValue).Value.ToString("YYYY-MM-dd HH:mm:ss.fffffff");
                }
                return pValue == null ? "NULL" : ((DateTime?)pValue).Value.ToString("YYYY-MM-dd HH:mm:ss.fff");
            }
            else if(pValue is DateTimeOffset) {

                if(pParameters != null) {
                    return pParameters.AddParameter(System.Data.DbType.DateTimeOffset, pValue);
                }
                return "'" + ((DateTimeOffset)pValue).ToString("yyyy-MM-dd HH:mm:ss.fffffff zzz") + "'";
            }
            else if(pValue is DateTimeOffset?) {

                if(pParameters != null) {
                    return pParameters.AddParameter(System.Data.DbType.DateTimeOffset, pValue);
                }
                return pValue == null ? "NULL" : ((DateTimeOffset?)pValue).Value.ToString("YYYY-MM-dd HH:mm:ss.fffffff zzz");
            }
            else if(pValue is bool) {

                if(pParameters != null) {
                    return pParameters.AddParameter(System.Data.DbType.Boolean, pValue);
                }
                return ((bool)pValue) ? "1" : "0";
            }
            else if(pValue is bool?) {

                if(pParameters != null) {
                    return pParameters.AddParameter(System.Data.DbType.Boolean, pValue);
                }
                return pValue == null ? "NULL" : (((bool?)pValue).Value ? "TRUE" : "FALSE");
            }
            else if(pValue is Guid) {

                if(pParameters != null) {
                    return pParameters.AddParameter(System.Data.DbType.Guid, pValue);
                }
                return "'" + ((Guid)pValue).ToString() + "'";
            }
            else if(pValue is Guid?) {

                if(pParameters != null) {
                    return pParameters.AddParameter(System.Data.DbType.Guid, pValue);
                }
                return pValue == null ? "NULL" : "'" + ((Guid)pValue).ToString() + "'";
            }
            else if(pValue is byte) {

                if(pParameters != null) {
                    return pParameters.AddParameter(System.Data.DbType.Byte, pValue);
                }
                return ((byte)pValue).ToString();
            }
            else if(pValue is byte?) {

                if(pParameters != null) {
                    return pParameters.AddParameter(System.Data.DbType.Byte, pValue);
                }
                return pValue == null ? "NULL" : ((byte)pValue).ToString();
            }
            else if(pValue is byte[]) {

                if(pParameters != null) {
                    return pParameters.AddParameter(System.Data.DbType.Binary, pValue);
                }
                return "0x" + (BitConverter.ToString((byte[])pValue)).Replace("-", string.Empty);
            }
            else if(pValue is float) {

                if(pParameters != null) {
                    return pParameters.AddParameter(System.Data.DbType.Single, pValue);
                }
                return "'" + ((float)pValue).ToString() + "'";
            }
            else if(pValue is float?) {

                if(pParameters != null) {
                    return pParameters.AddParameter(System.Data.DbType.Single, pValue);
                }
                return pValue == null ? "NULL" : "'" + ((float)pValue).ToString() + "'";
            }
            else if(pValue is double) {

                if(pParameters != null) {
                    return pParameters.AddParameter(System.Data.DbType.Double, pValue);
                }
                return "'" + ((double)pValue).ToString() + "'";
            }
            else if(pValue is double?) {

                if(pParameters != null) {
                    return pParameters.AddParameter(System.Data.DbType.Double, pValue);
                }
                return pValue == null ? "NULL" : "'" + ((double)pValue).ToString() + "'";
            }
            else if(pValue is System.Collections.IEnumerable) {

                StringBuilder sql = new StringBuilder("(");

                int index = 0;

                foreach(object? value in ((System.Collections.IEnumerable)pValue)) {

                    if(index > 0) {
                        sql.Append(',');
                    }
                    index++;

                    if(value == null) {
                        throw new Exception("Null values in an 'IN' or 'NOT IN' clause are not supported");
                    }
                    sql.Append(GetValue(pDatabase, value, pParameters, pDbType, pUseColumnAlias, pAliasManager));
                }
                sql.Append(")");
                return sql.ToString();
            }
            else if(pValue is IFunction) {
                return ((IFunction)pValue).GetFunctionSql(pDatabase, true, pAliasManager);
            }
            else if(pValue is Enum) {
                return ((int)pValue).ToString();
            }
            else if(pValue is Sql.Function.CustomSql) {
                return ((Sql.Function.CustomSql)pValue).GetCustomSql();
            }
            else if(pValue is AColumn) {
                return GetColumnSql((AColumn)pValue, pUseColumnAlias, pAliasManager);
            }
            else {
                throw new Exception("Unknown type: " + pValue.GetType().ToString());
            }
        }

        private static string GetOperator(Operator pOperator) {

            switch(pOperator) {
                case Operator.EQUALS:
                    return "=";
                case Operator.NOT_EQUALS:
                    return "!=";
                case Operator.GREATER_THAN:
                    return ">";
                case Operator.GREATER_THAN_OR_EQUAL:
                    return ">=";
                case Operator.LESS_THAN:
                    return "<";
                case Operator.LESS_THAN_OR_EQUAL:
                    return "<=";
                case Operator.AND:
                    return " AND ";
                case Operator.OR:
                    return " OR ";
                case Operator.IN:
                    return " IN ";
                case Operator.NOT_IN:
                    return " NOT IN ";
                case Operator.LIKE:
                    return " LIKE ";
                case Operator.NOT_LIKE:
                    return " NOT LIKE ";
                default:
                    throw new Exception("Unknown operator: " + pOperator.ToString());
            }
        }

        private static string GetColumnSql(AColumn pColumn, bool pUseAlias, IAliasManager pAliasManager) {
            return pUseAlias ? pAliasManager.GetAlias(pColumn.Table) + '.' + pColumn.ColumnName : pColumn.ColumnName;
        }

        internal static string CreateTableComment(string pSchema, string pTableName, string pDescription) {

            string schema = pSchema.Replace("'", "''");
            string table = pTableName.Replace("'", "''");
            string description = pDescription.Replace("'", "''");

            string sql =
                "IF NOT EXISTS(SELECT 1 FROM fn_listextendedproperty ('MS_DESCRIPTION','schema', '" + schema + "', 'table', '" + table + "', null, null))" + Environment.NewLine +
                "BEGIN" + Environment.NewLine +
                    "\tEXEC sp_addextendedproperty @name = N'MS_Description', @value = '" + description + "', @level0type = N'Schema', @level0name = '" + schema + "', @level1type = N'Table',  @level1name = '" + table + "';" + Environment.NewLine +
                "END" + Environment.NewLine +
                "ELSE BEGIN" + Environment.NewLine +
                    "\tEXEC sp_updateextendedproperty @name = N'MS_Description', @value = '" + description + "', @level0type = N'Schema', @level0name = '" + schema + "', @level1type = N'Table',  @level1name = '" + table + "';" + Environment.NewLine +
                "END" + Environment.NewLine;

            return sql;
        }

        internal static string CreateColumnComment(string pSchema, string pTableName, string pColumnName, string pDescription) {

            string schema = pSchema.Replace("'", "''");
            string table = pTableName.Replace("'", "''");
            string description = pDescription.Replace("'", "''");
            string column = pColumnName.Replace("'", "''");

            string sql =
                "IF NOT EXISTS(SELECT 1 FROM fn_listextendedproperty ('MS_DESCRIPTION','schema', '" + schema + "', 'table', '" + table + "', 'column', '" + column + "'))" + Environment.NewLine +
                "BEGIN" + Environment.NewLine +
                    "\tEXEC sp_addextendedproperty @name = N'MS_Description', @value = '" + description + "', @level0type = N'Schema', @level0name = '" + schema + "', @level1type = N'Table',  @level1name = '" + table + "', @level2type = N'Column', @level2name = '" + column + "';" + Environment.NewLine +
                "END" + Environment.NewLine +
                "ELSE BEGIN" + Environment.NewLine +
                    "\tEXEC sp_updateextendedproperty @name = N'MS_Description', @value = '" + description + "', @level0type = N'Schema', @level0name = '" + schema + "', @level1type = N'Table',  @level1name = '" + table + "', @level2type = N'Column', @level2name = '" + column + "';" + Environment.NewLine +
                "END" + Environment.NewLine;

            return sql;
        }

        internal static string CreateGrantTable(string pSchema, string pTableName, string pUserName, Sql.Privilege pPrivilege, ADatabase pDatabase) {

            string schema = pSchema.Replace("'", "''");
            string table = pTableName.Replace("'", "''");
            string userName = pUserName.Replace("'", "''");

            StringBuilder sql = new StringBuilder("GRANT ");

            string priv = string.Empty;

            if((Privilege)(((int)pPrivilege) & ((int)Privilege.ALL)) == Privilege.ALL) {

                if(!string.IsNullOrEmpty(priv)) {
                    priv += ",";
                }
                priv += "ALL";
            }
            if((Privilege)(((int)pPrivilege) & ((int)Privilege.SELECT)) == Privilege.SELECT) {

                if(!string.IsNullOrEmpty(priv)) {
                    priv += ",";
                }
                priv += "SELECT";
            }
            if((Privilege)(((int)pPrivilege) & ((int)Privilege.INSERT)) == Privilege.INSERT) {

                if(!string.IsNullOrEmpty(priv)) {
                    priv += ",";
                }
                priv += "INSERT";
            }
            if((Privilege)(((int)pPrivilege) & ((int)Privilege.UPDATE)) == Privilege.UPDATE) {

                if(!string.IsNullOrEmpty(priv)) {
                    priv += ",";
                }
                priv += "UPDATE";
            }
            if((Privilege)(((int)pPrivilege) & ((int)Privilege.DELETE)) == Privilege.DELETE) {

                if(!string.IsNullOrEmpty(priv)) {
                    priv += ",";
                }
                priv += "DELETE";
            }
            if((Privilege)(((int)pPrivilege) & ((int)Privilege.TRUNCATE)) == Privilege.TRUNCATE) {

                if(!string.IsNullOrEmpty(priv)) {
                    priv += ",";
                }
                priv += "TRUNCATE";
            }
            if((Privilege)(((int)pPrivilege) & ((int)Privilege.REFERENCES)) == Privilege.REFERENCES) {

                if(!string.IsNullOrEmpty(priv)) {
                    priv += ",";
                }
                priv += "REFERENCES";
            }
            if((Privilege)(((int)pPrivilege) & ((int)Privilege.TRIGGER)) == Privilege.TRIGGER) {

                if(!string.IsNullOrEmpty(priv)) {
                    priv += ",";
                }
                priv += "TRIGGER";
            }
            if((Privilege)(((int)pPrivilege) & ((int)Privilege.EXECUTE)) == Privilege.EXECUTE) {

                if(!string.IsNullOrEmpty(priv)) {
                    priv += ",";
                }
                priv += "EXECUTE";
            }

            sql.Append(priv).Append(" ON ");

            if(!string.IsNullOrEmpty(schema)) {
                sql.Append(schema).Append(".");
            }

            sql.Append(table).Append(" TO ").Append(userName).Append(";");

            return sql.ToString();
        }

        internal static string CreateGrantOrRevokeColumn(PrivAction pPrivAction, string pSchema, string pTableName, string pColumnName, string pUserName, Sql.ColumnPrivilege pPrivilege, ADatabase pDatabase) {

            string schema = pSchema.Replace("'", "''");
            string table = pTableName.Replace("'", "''");
            string columnName = pColumnName.Replace("'", "''");
            string userName = pUserName.Replace("'", "''");

            StringBuilder sql = new StringBuilder();

            if(pPrivAction == PrivAction.GRANT) {
                sql.Append("GRANT ");
            }
            else if(pPrivAction == PrivAction.REVOKE) {
                sql.Append("REVOKE ");
            }
            else {
                throw new Exception("Unknown PrivAction = '" + pPrivAction.ToString() + "'");
            }

            string priv = string.Empty;

            if((ColumnPrivilege)(((int)pPrivilege) & ((int)ColumnPrivilege.ALL)) == ColumnPrivilege.ALL) {

                if(!string.IsNullOrEmpty(priv)) {
                    priv += ",";
                }
                priv += "ALL";
            }
            if((ColumnPrivilege)(((int)pPrivilege) & ((int)ColumnPrivilege.SELECT)) == ColumnPrivilege.SELECT) {

                if(!string.IsNullOrEmpty(priv)) {
                    priv += ",";
                }
                priv += "SELECT";
            }

            if((ColumnPrivilege)(((int)pPrivilege) & ((int)ColumnPrivilege.UPDATE)) == ColumnPrivilege.UPDATE) {

                if(!string.IsNullOrEmpty(priv)) {
                    priv += ",";
                }
                priv += "UPDATE";
            }
            if((ColumnPrivilege)(((int)pPrivilege) & ((int)ColumnPrivilege.REFERENCES)) == ColumnPrivilege.REFERENCES) {

                if(!string.IsNullOrEmpty(priv)) {
                    priv += ",";
                }
                priv += "REFERENCES";
            }

            sql.Append(priv).Append("(").Append(columnName).Append(")").Append(" ON ");

            if(!string.IsNullOrEmpty(schema)) {
                sql.Append(schema).Append(".");
            }
            sql.Append(table).Append(" TO ").Append(userName).Append(";");
            return sql.ToString();
        }
    }
}