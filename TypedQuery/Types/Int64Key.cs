using System;

namespace Sql.Types {

	public struct Int64Key<TABLE> {

		public Int64 Value { get; private set; }

		public Int64Key(Int64 pValue) {
			Value = pValue;
		}

		public static bool operator ==(Int64Key<TABLE> pA, Int64Key<TABLE> pB) {
			return pA.Equals(pB);
		}
		public static bool operator !=(Int64Key<TABLE> pA, Int64Key<TABLE> pB) {
			return !pA.Equals(pB);
		}

		public override bool Equals(object obj) {

			if(obj is Int64Key<TABLE>) {
				return Value.CompareTo(((Int64Key<TABLE>)obj).Value) == 0;
			}
			return false;
		}
		public override int GetHashCode() {
			return Value.GetHashCode();
		}
	}
}