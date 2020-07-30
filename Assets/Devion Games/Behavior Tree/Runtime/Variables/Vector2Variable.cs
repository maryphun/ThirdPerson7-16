using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames.BehaviorTrees
{
	[System.Serializable]
	public class Vector2Variable : Variable
	{
		[SerializeField]
		private Vector2 m_Value;

		public Vector2 Value {
			get{ return this.m_Value; }
			set{ this.m_Value = value; }
		}

		public override object RawValue {
			get {
				return this.m_Value;
			}
			set {
				this.m_Value = (Vector2)value;
			}
		}

		public override System.Type type {
			get {
				return typeof(Vector2);
			}
		}

		public Vector2Variable ()
		{
		}

		public Vector2Variable (string name) : base (name)
		{
		}

		public Vector2Variable (Vector2Variable source) : base (source)
		{
			if (source != null) {
				this.Value = source.Value;
			}
		}

		public override Variable Clone ()
		{
			return new Vector2Variable (this);
		}

		public static implicit operator Vector2Variable (Vector2 value)
		{
			return new Vector2Variable () {
				Value = value
			};
		}

		public static implicit operator Vector2 (Vector2Variable value)
		{
			return value.Value;
		}
	}
}