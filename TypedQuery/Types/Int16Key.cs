using System;

namespace Sql.Types {

	public struct Int16Key<TABLE> {

		public Int16 Value { get; private set; }

		public Int16Key(Int16 pValue) {
			Value = pValue;
		}

		public static bool operator ==(Int16Key<TABLE> pA, Int16Key<TABLE> pB) {
			return pA.Equals(pB);
		}
		public static bool operator !=(Int16Key<TABLE> pA, Int16Key<TABLE> pB) {
			return !pA.Equals(pB);
		}

		public override bool Equals(object obj) {

			if(obj is Int16Key<TABLE>) {
				return Value.CompareTo(((Int16Key<TABLE>)obj).Value) == 0;
			}
			return false;
		}
		public override int GetHashCode() {
			return Value.GetHashCode();
		}
	}
}