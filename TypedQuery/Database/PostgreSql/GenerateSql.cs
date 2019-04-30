
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

namespace Sql.Database.PostgreSql {

    internal static class GenerateSql {

        internal static string GetSelectQuery(ADatabase pDatabase, Core.QueryBuilder pQueryBuilder, Core.Parameters pParameters, IAliasManager pAliasManager) {

            StringBuilder sql = new StringBuilder();

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

            bool areAlisesRequired = pQueryBuilder.JoinList.Count > 0 && !pQueryBuilder.FromTable.IsTemporaryTable;

            for(int index = 0; index < pQueryBuilder.SelectColumns.Length; index++) {

                ISelectable selectField = pQueryBuilder.SelectColumns[index];

                if(index > 0) {
                    sql.Append(',');
                }
                if(selectField is AColumn) {
                    sql.Append(GetColumnSql((AColumn)selectField, pAliasManager));
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
                    sql.Append("TEMP ");
                }
                sql.Append(pQueryBuilder.IntoTable.TableName);
            }

            sql.Append(" FROM ");

            if(!string.IsNullOrEmpty(pQueryBuilder.FromTable.Schema)) {
                sql.Append(pQueryBuilder.FromTable.Schema).Append(".");
            }

            sql.Append(pQueryBuilder.FromTable.TableName).Append(" AS ").Append(pAliasManager.GetAlias(pQueryBuilder.FromTable));

            if(pQueryBuilder.FromHints != null && pQueryBuilder.FromHints.Length > 0) {
                throw new Exception("From table hints not supported in postgresql sql generator");
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

                    sql.Append(join.Table.TableName).Append(" AS ").Append(pAliasManager.GetAlias(join.Table)).Append(" ON ").Append(GetConditionSql(pDatabase, join.Condition, pParameters, pAliasManager));

                    if(join.Hints != null && join.Hints.Length > 0) {
                        throw new Exception("Join hints not supported in postgresql sql generator");
                    }
                }
            }

            if(pQueryBuilder.WhereCondition != null) {
                sql.Append(" WHERE ").Append(GetConditionSql(pDatabase, pQueryBuilder.WhereCondition, pParameters, pAliasManager));
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
                        sql.Append(pAliasManager.GetAlias(aColumn.Table)).Append('.').Append(aColumn.ColumnName);
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
                sql.Append(" HAVING ").Append(GetConditionSql(pDatabase, pQueryBuilder.HavingCondition, pParameters, pAliasManager));
            }

            if(pQueryBuilder.OrderByColumns != null && pQueryBuilder.OrderByColumns.Length > 0) {

                sql.Append(" ORDER BY ");

                for(int index = 0; index < pQueryBuilder.OrderByColumns.Length; index++) {

                    IOrderByColumn orderByColumn = pQueryBuilder.OrderByColumns[index];

                    if(index > 0) {
                        sql.Append(',');
                    }

                    bool orderingByFieldPosition = false;

                    if(pQueryBuilder.UnionQuery != null) {

                        for(int fieldIndex = 0; fieldIndex < pQueryBuilder.SelectColumns.Length; fieldIndex++) {

                            ISelectable selectField = pQueryBuilder.SelectColumns[fieldIndex];

                            if(selectField.Equals(orderByColumn)) {
                                sql.Append((fieldIndex + 1).ToString());
                                orderingByFieldPosition = true;
                            }
                        }
                    }

                    if(!orderingByFieldPosition) {

                        ISelectable orderByField = orderByColumn.GetOrderByColumn.Column;

                        if(orderByField is AColumn) {
                            sql.Append(GetColumnSql((AColumn)orderByField, pAliasManager));
                        }
                        else if(orderByField is IFunction) {
                            sql.Append(((IFunction)orderByField).GetFunctionSql(pDatabase, areAlisesRequired, pAliasManager));
                        }
                        else {
                            throw new Exception("Field type not supported yet");
                        }
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

            if(pQueryBuilder.TopRows != null) {
                sql.Append(" LIMIT ").Append(pQueryBuilder.TopRows.Value.ToString());
            }

            if(!string.IsNullOrEmpty(pQueryBuilder.CustomSql)) {
                sql.Append(" ").Append(pQueryBuilder.CustomSql);
            }
            return sql.ToString();
        }

        internal static string GetInsertQuery(ADatabase pDatabase, Core.InsertBuilder pInsertBuilder, Core.Parameters pParameters) {

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

            sql.Append(")VALUES(");

            for(int index = 0; index < pInsertBuilder.SetValueList.Count; index++) {

                Core.SetValue setValue = pInsertBuilder.SetValueList[index];

                if(index > 0) {
                    sql.Append(',');
                }

                if(setValue.Value == null) {

                    if(pParameters != null) {

                        if(setValue.Column.DbType == System.Data.DbType.DateTime2) {  //npgsql doesn't work with datetime2 types yet
                            sql.Append(pParameters.AddParameter(System.Data.DbType.DateTime, null));
                        }
                        else {
                            sql.Append(pParameters.AddParameter(setValue.Column.DbType, null));
                        }
                    }
                    else {
                        sql.Append("NULL");
                    }
                }
                else {
                    sql.Append(GetValue(pDatabase, setValue.Value, pParameters, setValue.Column.DbType, aliasManager));
                }
            }
            sql.Append(")");

            if(pInsertBuilder.ReturnColumns != null && pInsertBuilder.ReturnColumns.Length > 0) {

                sql.Append(" RETURNING ");

                for(int index = 0; index < pInsertBuilder.ReturnColumns.Length; index++) {

                    if(index > 0) {
                        sql.Append(",");
                    }
                    sql.Append(pInsertBuilder.ReturnColumns[index].ColumnName);
                }
            }
            return sql.ToString();
        }

        internal static string GetBulkInsertQuery(ADatabase pDatabase, List<Core.InsertBuilder> pInsertBuilders) {

            if(pInsertBuilders.Count == 0) {
                return string.Empty;
            }

            IAliasManager aliasManager = new AliasManager();

            StringBuilder sql = new StringBuilder("INSERT INTO ");

            Core.InsertBuilder firstBuilder = pInsertBuilders[0];

            if(!string.IsNullOrEmpty(firstBuilder.Table.Schema)) {
                sql.Append(firstBuilder.Table.Schema).Append(".");
            }

            sql.Append(firstBuilder.Table.TableName);

            sql.Append("(");

            for(int index = 0; index < firstBuilder.SetValueList.Count; index++) {

                Core.SetValue setValue = firstBuilder.SetValueList[index];

                if(index > 0) {
                    sql.Append(',');
                }
                sql.Append(setValue.Column.ColumnName);
            }

            sql.Append(") VALUES");

            for(int insertIndex = 0; insertIndex < pInsertBuilders.Count; insertIndex++) {

                if(insertIndex > 0) {
                    sql.Append(",");
                }

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
                        sql.Append(GetValue(pDatabase, setValue.Value, null, setValue.Column.DbType, aliasManager));
                    }
                }
                sql.Append(")");
            }
            return sql.ToString();
        }

        internal static string GetInsertSelectQuery(ADatabase pDatabase, Core.InsertSelectBuilder pInsertBuilder, Core.Parameters pParameters) {

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
            sql.Append(GetSelectQuery(pDatabase, (Core.QueryBuilder)pInsertBuilder.SelectQuery, pParameters, aliasManager));
            return sql.ToString();
        }

        internal static string GetUpdateQuery(ADatabase pDatabase, Core.UpdateBuilder pUpdateBuilder, Core.Parameters pParameters) {

            StringBuilder sql = new StringBuilder("UPDATE ");

            IAliasManager aliasManager = new AliasManager();

            if(!string.IsNullOrEmpty(pUpdateBuilder.Table.Schema)) {
                sql.Append(pUpdateBuilder.Table.Schema).Append(".");
            }

            sql.Append(pUpdateBuilder.Table.TableName).Append(" AS ").Append(aliasManager.GetAlias(pUpdateBuilder.Table));

            sql.Append(" SET ");

            for(int index = 0; index < pUpdateBuilder.SetValueList.Count; index++) {

                Core.SetValue setValue = pUpdateBuilder.SetValueList[index];

                if(index > 0) {
                    sql.Append(',');
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
                    sql.Append("=").Append(GetValue(pDatabase, setValue.Value, pParameters, setValue.Column.DbType, aliasManager));
                }
            }

            Condition joinCondition = null;

            if(pUpdateBuilder.JoinList.Count > 0) {

                sql.Append(" FROM ");

                for(int joinIndex = 0; joinIndex < pUpdateBuilder.JoinList.Count; joinIndex++) {

                    Sql.Core.Join join = pUpdateBuilder.JoinList[joinIndex];

                    if(joinIndex > 0) {
                        sql.Append(",");
                    }
                    sql.Append(join.Table.TableName).Append(" AS ").Append(aliasManager.GetAlias(join.Table));

                    if(joinCondition == null) {
                        joinCondition = join.Condition;
                    }
                    else {
                        joinCondition = joinCondition & join.Condition;
                    }
                }
            }

            Condition whereCondition = null;

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
                sql.Append(" WHERE ").Append(GetConditionSql(pDatabase, whereCondition, pParameters, aliasManager));
            }

            if(pUpdateBuilder.ReturnColumns != null && pUpdateBuilder.ReturnColumns.Length > 0) {

                sql.Append(" RETURNING ");

                for(int index = 0; index < pUpdateBuilder.ReturnColumns.Length; index++) {

                    if(index > 0) {
                        sql.Append(",");
                    }

                    AColumn returnColumn = pUpdateBuilder.ReturnColumns[index];

                    sql.Append(aliasManager.GetAlias(returnColumn.Table)).Append(".");
                    sql.Append(returnColumn.ColumnName);
                }
            }
            return sql.ToString();
        }

        internal static string GetDeleteQuery(ADatabase pDatabase, Core.DeleteBuilder pDeleteBuilder, Core.Parameters pParameters) {

            StringBuilder sql = new StringBuilder("DELETE FROM ");

            IAliasManager aliasManager = new AliasManager();

            if(!string.IsNullOrEmpty(pDeleteBuilder.Table.Schema)) {
                sql.Append(pDeleteBuilder.Table.Schema).Append(".");
            }

            sql.Append(pDeleteBuilder.Table.TableName).Append(" AS ").Append(aliasManager.GetAlias(pDeleteBuilder.Table));

            if(pDeleteBuilder.WhereCondition != null) {
                sql.Append(" WHERE ").Append(GetConditionSql(pDatabase, pDeleteBuilder.WhereCondition, pParameters, aliasManager));
            }

            if(pDeleteBuilder.ReturnColumns != null && pDeleteBuilder.ReturnColumns.Length > 0) {

                sql.Append(" RETURNING ");

                for(int index = 0; index < pDeleteBuilder.ReturnColumns.Length; index++) {

                    if(index > 0) {
                        sql.Append(",");
                    }
                    sql.Append(pDeleteBuilder.ReturnColumns[index].ColumnName);
                }
            }
            return sql.ToString();
        }

        internal static string GetTruncateQuery(ATable pTable) {
            string schema = !string.IsNullOrEmpty(pTable.Schema) ? pTable.Schema + "." : string.Empty;
            return "TRUNCATE TABLE " + schema + pTable.TableName;
        }

        private static string GetConditionSql(ADatabase pDatabase, Condition pCondition, Core.Parameters pParameters, IAliasManager pAliasManager) {

            if(pCondition.Operator == Operator.IS_NULL) {
                return "(" + GetSideSql(pDatabase, pCondition.Left, pParameters, pAliasManager) + " IS NULL)";
            }
            else if(pCondition.Operator == Operator.IS_NOT_NULL) {
                return "(" + GetSideSql(pDatabase, pCondition.Left, pParameters, pAliasManager) + " IS NOT NULL)";
            }

            System.Data.DbType? dbType = null;

            if(pCondition.Left is Sql.AColumn) {
                dbType = ((Sql.AColumn)pCondition.Left).DbType;
            }
            return "(" + GetSideSql(pDatabase, pCondition.Left, pParameters, pAliasManager) + GetOperator(pCondition.Operator) + GetSideSql(pDatabase, pCondition.Right, pParameters, dbType, pAliasManager) + ")";
        }

        private static string GetSideSql(ADatabase pDatabase, object pCond, Core.Parameters pParameters, IAliasManager pAliasManager) {
            return GetSideSql(pDatabase, pCond, pParameters, null, pAliasManager);
        }
        private static string GetSideSql(ADatabase pDatabase, object pCond, Core.Parameters pParameters, System.Data.DbType? pDbType, IAliasManager pAliasManager) {

            if(pCond is Condition) {
                return GetConditionSql(pDatabase, (Condition)pCond, pParameters, pAliasManager);
            }
            else if(pCond is AColumn) {
                return GetColumnSql((AColumn)pCond, pAliasManager);
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
                return "(" + GetSideSql(pDatabase, numCond.Left, pParameters, pAliasManager) + opp + GetSideSql(pDatabase, numCond.Right, pParameters, pAliasManager) + ")";
            }
            else {
                return GetValue(pDatabase, pCond, pParameters, pDbType, pAliasManager);
            }
        }

        internal static string GetValue(ADatabase pDatabase, object pValue, System.Data.DbType? pDbType, IAliasManager pAliasManager) {
            return GetValue(pDatabase, pValue, null, pDbType, pAliasManager);
        }
        internal static string GetValue(ADatabase pDatabase, object pValue, Core.Parameters pParameters, System.Data.DbType? pDbType, IAliasManager pAliasManager) {

            if(pValue is int) {

                if(pParameters != null) {
                    return pParameters.AddParameter(System.Data.DbType.Int32, pValue);
                }
                return pValue.ToString();
            }
            else if(pValue is int?) {

                if(pParameters != null) {
                    return pParameters.AddParameter(System.Data.DbType.Int32, pValue);
                }
                return pValue == null ? "NULL" : pValue.ToString();
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
                return pValue.ToString();
            }
            else if(pValue is Int16?) {

                if(pParameters != null) {
                    return pParameters.AddParameter(System.Data.DbType.Int16, pValue);
                }
                return pValue == null ? "NULL" : pValue.ToString();
            }
            else if(pValue is Int64) {

                if(pParameters != null) {
                    return pParameters.AddParameter(System.Data.DbType.Int64, pValue);
                }
                return pValue.ToString();
            }
            else if(pValue is Int64?) {

                if(pParameters != null) {
                    return pParameters.AddParameter(System.Data.DbType.Int64, pValue);
                }
                return pValue == null ? "NULL" : pValue.ToString();
            }
            else if(pValue is decimal) {

                if(pParameters != null) {
                    return pParameters.AddParameter(System.Data.DbType.Decimal, pValue);
                }
                return pValue.ToString();
            }
            else if(pValue is decimal?) {

                if(pParameters != null) {
                    return pParameters.AddParameter(System.Data.DbType.Decimal, pValue);
                }
                return pValue == null ? "NULL" : pValue.ToString();
            }
            else if(pValue is DateTime) {

                if(pParameters != null) {
                    return pParameters.AddParameter(System.Data.DbType.DateTime, pValue);
                }

                if(pDbType != null && pDbType == System.Data.DbType.DateTime2) {
                    return "'" + ((DateTime)pValue).ToString("yyyy-MM-dd HH:mm:ss.fffffff") + "'";
                }
                return "'" + ((DateTime)pValue).ToString("yyyy-MM-dd HH:mm:ss.fff") + "'";
            }
            else if(pValue is DateTime?) {

                if(pParameters != null) {
                    return pParameters.AddParameter(System.Data.DbType.DateTime, pValue);
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
                return ((bool)pValue) ? "TRUE" : "FALSE";
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
                return "'" + ConvertBytes((byte[])pValue) + "'";
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

                foreach(object value in ((System.Collections.IEnumerable)pValue)) {

                    if(index > 0) {
                        sql.Append(',');
                    }

                    index++;

                    if(value == null) {
                        throw new Exception("Null values in an 'IN' or 'NOT IN' clause are not supported");
                    }
                    sql.Append(GetValue(pDatabase, value, pParameters, pDbType, pAliasManager));
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
                return GetColumnSql((AColumn)pValue, pAliasManager);
            }
            else {
                throw new Exception("Unknown type: " + pValue.GetType().ToString());
            }
        }

        private static string ConvertBytes(byte[] pBytes) {

            if(pBytes == null) {
                return "\\000";
            }

            StringBuilder str = new StringBuilder();

            for(int index = 0; index < pBytes.Length; index++) {
                str.Append("\\").Append(((int)pBytes[index]).ToString("000"));
            }
            return str.ToString();
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
                    return " ILIKE ";
                case Operator.NOT_LIKE:
                    return " NOT ILIKE ";
                default:
                    throw new Exception("Unknown operator: " + pOperator.ToString());
            }
        }
        private static string GetColumnSql(AColumn pColumn, IAliasManager pAliasManager) {
            return pAliasManager.GetAlias(pColumn.Table) + '.' + pColumn.ColumnName;
        }

        internal static string CreateTableComment(string pSchema, string pTableName, string pDescription) {
            string schema = !string.IsNullOrEmpty(pSchema) ? pSchema : string.Empty;
            return "COMMENT ON TABLE " + schema + pTableName + " IS " + (!string.IsNullOrWhiteSpace(pDescription) ? "'" + pDescription.Replace("'", "''") + "';" : "IS NULL;");
        }

        internal static string CreateColumnComment(string pSchema, string pTableName, string pColumnName, string pDescription) {
            string schema = !string.IsNullOrEmpty(pSchema) ? pSchema : string.Empty;
            return "COMMENT ON COLUMN " + schema + pTableName + "." + pColumnName + " IS " + (!string.IsNullOrWhiteSpace(pDescription) ? "'" + pDescription.Replace("'", "''") + "';" : "IS NULL;");
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