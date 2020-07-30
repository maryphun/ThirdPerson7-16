using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames.BehaviorTrees
{
	[System.Serializable]
	public class Vector4Variable : Variable
	{
		[SerializeField]
		private Vector4 m_Value;

		public Vector4 Value {
			get{ return this.m_Value; }
			set{ this.m_Value = value; }
		}

		public override object RawValue {
			get {
				return this.m_Value;
			}
			set {
				this.m_Value = (Vector4)value;
			}
		}

		public override System.Type type {
			get {
				return typeof(Vector4);
			}
		}

		public Vector4Variable ()
		{
		}

		public Vector4Variable (string name) : base (name)
		{
		}

		public Vector4Variable (Vector4Variable source) : base (source)
		{
			if (source != null) {
				this.Value = source.Value;
			}
		}

		public override Variable Clone ()
		{
			return new Vector4Variable (this);
		}

		public static implicit operator Vector4Variable (Vector4 value)
		{
			return new Vector4Variable () {
				Value = value
			};
		}

		public static implicit operator Vector4 (Vector4Variable value)
		{
			return value.Value;
		}
	}
}