using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames.BehaviorTrees
{
	[System.Serializable]
	public class StringVariable : Variable
	{
		[SerializeField]
		private string m_Value = string.Empty;

		public string Value {
			get{ return this.m_Value; }
			set{ this.m_Value = value; }
		}

		public override object RawValue {
			get {
				return this.m_Value;
			}
			set {
				this.m_Value = (string)value;
			}
		}

		public override System.Type type {
			get {
				return typeof(string);
			}
		}

		public StringVariable ()
		{
		}

		public StringVariable (string name) : base (name)
		{
		}

		public StringVariable (StringVariable source) : base (source)
		{
			if (source != null) {
				this.Value = source.Value;
			}
		}

		public override Variable Clone ()
		{
			return new StringVariable (this);
		}

		public static implicit operator StringVariable (string value)
		{
			return new StringVariable () {
				Value = value
			};
		}
	}
}