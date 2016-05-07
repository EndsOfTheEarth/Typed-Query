
using System;
using System.Data;
using System.Data.SqlClient;

namespace Sql.StoredProc.SP_Test_In_Out {
	
	//[Sql.GrantTable(typeof(string), Sql.Privilege.SELECT)]
	public sealed class SP : Sql.AStoredProc {
		
		public static SP INSTANCE = new SP();
		
		internal Sql.Column.GuidColumn Id;
		internal Sql.Column.IntegerColumn IntValue;
		
		private SP() : base(DB.TestDB, "SP_Test_In_Out", "dbo", typeof(Row)) {
			
			Id = new Sql.Column.GuidColumn(this, "Id");
			IntValue = new Sql.Column.IntegerColumn(this, "IntValue");
			
			AddColumns(Id, IntValue);
		}
		
		public Sql.IResult Execute(int pInParam, out int pOutParam, Sql.Transaction pTransaction){
			
			SqlParameter p1 = new SqlParameter("@In_param", SqlDbType.Int);
			p1.Value = pInParam;
			p1.Direction = ParameterDirection.Input;
			
			SqlParameter p2 = new SqlParameter("@Out_param", SqlDbType.Int);
			p2.Direction = ParameterDirection.Output;
			
			Sql.IResult result = ExecuteProcedure(pTransaction, p1, p2);
			
			pOutParam = (int)p2.Value;
			
			return result;
		}
		
		public Row this[int pIndex, Sql.IResult pQueryResult]{
			get { return (Row)pQueryResult.GetRow(this, pIndex); }
		}
	}
	public sealed class Row : Sql.ARow {
		
		public Row() : base(SP.INSTANCE) {
			
		}
		
		public Guid Id {
			get { return SP.INSTANCE.Id.ValueOf(this); }
		}
		public int IntValue {
			get { return SP.INSTANCE.IntValue.ValueOf(this); }
		}
	}
}