
/*
 * 
 * Copyright (C) 2009-2020 JFo.nz
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
using System.Data;
using TypedQuery.Logic;

namespace TypedQueryGenerator.Logic.CodeGeneration {
    
    public class StoredProcedureCodeGenerator {

        public string Generate(IStoredProcedureDetail pProc, string pColumnPrefix, bool pIncludeSchema) {

            CodeFile codeFile = new CodeFile("    ");

            codeFile.Append("using System;").EndLine();
            codeFile.Append("using System.Data;").EndLine();
            codeFile.Append("using System.Data.SqlClient;").EndLine().EndLine();

            codeFile.Append("namespace Tables.").Append(pProc.Name).Append(" {").EndLine();
            codeFile.EndLine();
            codeFile.Indent(1).Append("public sealed class Proc : ").Append(typeof(Sql.AStoredProc).ToString()).Append(" {").EndLine();
            codeFile.EndLine();
            codeFile.Indent(2).Append("public static readonly Proc Instance = new Proc();").EndLine();

            codeFile.EndLine();
            codeFile.Indent(2).Append("public Proc() : base(DATABASE, \"").Append(pIncludeSchema && !string.IsNullOrEmpty(pProc.Schema) ? pProc.Schema + "." : string.Empty).Append(pProc.Name).Append("\", typeof(Row)) {").EndLine();
            codeFile.EndLine();

            codeFile.Indent(3).Append("//AddColumns(");
            codeFile.Append(");").EndLine();
            codeFile.Indent(2).Append("}").EndLine();
            codeFile.EndLine();

            codeFile.Indent(2).Append("public Sql.IResult Execute(");

            for(int index = 0; index < pProc.Parameters.Count; index++) {

                ISpParameter param = pProc.Parameters[index];

                if(index > 0) {
                    codeFile.Append(", ");
                }

                if(param.Direction == ParameterDirection.Output || param.Direction == ParameterDirection.ReturnValue) {
                    codeFile.Append("out ");
                }
                codeFile.Append(TypedQueryGenerator.Logic.CodeGeneration.ReturnType.GetReturnType(param.ParamType, false, null, null, false)).Append(" ").Append(param.Name);
            }

            if(pProc.Parameters.Count > 0) {
                codeFile.Append(", ");
            }

            codeFile.Append("Sql.Transaction pTransaction) {").EndLine().EndLine();

            for(int index = 0; index < pProc.Parameters.Count; index++) {

                ISpParameter param = pProc.Parameters[index];

                codeFile.Indent(3).Append("SqlParameter p").Append(index.ToString()).Append(" = new SqlParameter(\"").Append(param.Name).Append("\", SqlDbType.").Append(ConvertToSqlDbType(param.ParamType).ToString()).Append(");").EndLine();

                codeFile.Indent(3).Append("p").Append(index.ToString()).Append(".Direction = ParameterDirection.").Append(param.Direction.ToString()).Append(";").EndLine();

                if(param.Direction == ParameterDirection.Input || param.Direction == ParameterDirection.InputOutput) {
                    codeFile.Indent(3).Append("p").Append(index.ToString()).Append(".Value = ").Append(param.Name).Append(";").EndLine();
                }
                codeFile.EndLine();
            }

            codeFile.Indent(3).Append("Sql.IResult result = ExecuteProcedure(pTransaction");

            for(int index = 0; index < pProc.Parameters.Count; index++) {
                codeFile.Append(", p").Append(index.ToString());
            }

            codeFile.Append(");").EndLine().EndLine();

            for(int index = 0; index < pProc.Parameters.Count; index++) {

                ISpParameter param = pProc.Parameters[index];

                if(param.Direction == ParameterDirection.InputOutput || param.Direction == ParameterDirection.Output || param.Direction == ParameterDirection.ReturnValue) {
                    codeFile.Indent(3).Append(param.Name).Append(" = (").Append(TypedQueryGenerator.Logic.CodeGeneration.ReturnType.GetReturnType(param.ParamType, false, null, null, false)).Append(")").Append("p").Append(index.ToString()).Append(".Value;").EndLine();
                }
            }

            codeFile.Indent(3).Append("return result;").EndLine();
            codeFile.Indent(2).Append("}").EndLine().EndLine();

            codeFile.Indent(2).Append("public Row this[int pIndex, Sql.IResult pResult] {").EndLine();
            codeFile.Indent(3).Append("get { return (Row)pResult.GetRow(this, pIndex); }").EndLine();
            codeFile.Indent(2).Append("}").EndLine();
            codeFile.Indent(1).Append("}").EndLine();

            codeFile.EndLine();

            //
            //	Generate row code
            //
            codeFile.Indent(1).Append("public sealed class Row : ").Append(typeof(Sql.ARow).ToString()).Append(" {").EndLine();
            codeFile.EndLine();
            codeFile.Indent(2).Append("private new Proc Tbl {").EndLine();
            codeFile.Indent(3).Append("get { return (Proc)base.Tbl; }").EndLine();
            codeFile.Indent(2).Append("}").EndLine();
            codeFile.EndLine();
            codeFile.Indent(2).Append("public Row() : base(Proc.Instance) {").EndLine();
            codeFile.Indent(2).Append("}").EndLine();

            codeFile.Indent(1).Append("}").EndLine();
            codeFile.Append("}");

            return codeFile.ToString();
        }

        private static SqlDbType ConvertToSqlDbType(DbType pDbType) {

            if(pDbType == DbType.Int16) {
                return SqlDbType.SmallInt;
            }
            if(pDbType == DbType.Int32) {
                return SqlDbType.Int;
            }
            if(pDbType == DbType.Int64) {
                return SqlDbType.BigInt;
            }
            if(pDbType == DbType.String) {
                return SqlDbType.VarChar;
            }
            if(pDbType == DbType.Decimal) {
                return SqlDbType.Decimal;
            }
            if(pDbType == DbType.DateTime) {
                return SqlDbType.DateTime;
            }
            if(pDbType == DbType.DateTime2) {
                return SqlDbType.DateTime2;
            }
            if(pDbType == DbType.DateTimeOffset) {
                return SqlDbType.DateTimeOffset;
            }
            if(pDbType == DbType.Byte) {
                return SqlDbType.TinyInt;
            }
            if(pDbType == DbType.Double) {
                return SqlDbType.Float;
            }
            if(pDbType == DbType.Single) {
                return SqlDbType.Real;
            }
            return 0;
        }
    }
}