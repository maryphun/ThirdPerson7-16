using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames.BehaviorTrees
{
	[System.Serializable]
	public class ColorVariable : Variable
	{
		[SerializeField]
		private Color m_Value = Color.white;

		public Color Value {
			get{ return this.m_Value; }
			set{ this.m_Value = value; }
		}

		public override object RawValue {
			get {
				return this.m_Value;
			}
			set {
				this.m_Value = (Color)value;
			}
		}

		public override System.Type type {
			get {
				return typeof(Color);
			}
		}

		public ColorVariable ()
		{
		}

		public ColorVariable (string name) : base (name)
		{
		}

		public ColorVariable (ColorVariable source) : base (source)
		{
			if (source != null) {
				this.Value = source.Value;
			}
		}

		public override Variable Clone ()
		{
			return new ColorVariable (this);
		}

		public static implicit operator ColorVariable (Color value)
		{
			return new ColorVariable () {
				Value = value
			};
		}

		public static implicit operator Color (ColorVariable value)
		{
			return value.Value;
		}
	}
}