using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames.BehaviorTrees
{
	
	[System.Serializable]
	public class FloatVariable : Variable
	{
		[SerializeField]
		private float m_Value;

		public float Value {
			get{ return this.m_Value; }
			set{ this.m_Value = value; }
		}

		public override object RawValue {
			get {
				return this.m_Value;
			}
			set {
				this.m_Value = System.Convert.ToSingle (value);
			}
		}

		public override System.Type type {
			get {
				return typeof(float);
			}
		}

		public FloatVariable ()
		{
		}

		public FloatVariable (string name) : base (name)
		{
		}

		public FloatVariable (FloatVariable source) : base (source)
		{
			if (source != null) {
				this.Value = source.Value;
			}
		}

		public override Variable Clone ()
		{
			return new FloatVariable (this);
		}

		public static implicit operator FloatVariable (float value)
		{
			return new FloatVariable () {
				Value = value
			};
		}

		public static implicit operator float (FloatVariable value)
		{
			return value.Value;
		}
	}
}