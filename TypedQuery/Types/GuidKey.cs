using System;

namespace Sql.Types {

	public struct GuidKey<TABLE> {

		public Guid Value { get; private set; }

		public GuidKey(Guid pValue) {
			Value = pValue;
		}

		public static bool operator ==(GuidKey<TABLE> pA, GuidKey<TABLE> pB) {
			return pA.Equals(pB);
		}
		public static bool operator !=(GuidKey<TABLE> pA, GuidKey<TABLE> pB) {
			return !pA.Equals(pB);
		}

		public override bool Equals(object obj) {

			if(obj is GuidKey<TABLE>) {
				return Value.CompareTo(((GuidKey<TABLE>)obj).Value) == 0;
			}
			return false;
		}
		public override int GetHashCode() {
			return Value.GetHashCode();
		}
	}
}