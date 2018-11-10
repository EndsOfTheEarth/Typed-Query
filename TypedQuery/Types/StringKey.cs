using System;

namespace Sql.Types {

	public struct StringKey<TABLE> {

		private string mValue;

		public string Value {
			get {
				if(mValue == null) {
					mValue = string.Empty;
				}
				return mValue;
			}
		}

		public StringKey(string pValue) {
			mValue = pValue ?? string.Empty;
		}

		public static bool operator ==(StringKey<TABLE> pA, StringKey<TABLE> pB) {
			return pA.Equals(pB);
		}
		public static bool operator !=(StringKey<TABLE> pA, StringKey<TABLE> pB) {
			return !pA.Equals(pB);
		}

		public override bool Equals(object obj) {

			if(obj is StringKey<TABLE>) {
				return Value.CompareTo(((StringKey<TABLE>)obj).Value) == 0;
			}
			return false;
		}
		public override int GetHashCode() {
			return Value.GetHashCode();
		}
	}
}