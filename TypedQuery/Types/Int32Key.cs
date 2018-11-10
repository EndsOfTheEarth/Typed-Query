using System;

namespace Sql.Types {

	public struct Int32Key<TABLE> {

		public int Value { get; private set; }

		public Int32Key(int pValue) {
			Value = pValue;
		}

		public static bool operator ==(Int32Key<TABLE> pA, Int32Key<TABLE> pB) {
			return pA.Equals(pB);
		}
		public static bool operator !=(Int32Key<TABLE> pA, Int32Key<TABLE> pB) {
			return !pA.Equals(pB);
		}

		public override bool Equals(object obj) {

			if(obj is Int32Key<TABLE>) {
				return Value.CompareTo(((Int32Key<TABLE>)obj).Value) == 0;
			}
			return false;
		}
		public override int GetHashCode() {
			return Value.GetHashCode();
		}
	}
}