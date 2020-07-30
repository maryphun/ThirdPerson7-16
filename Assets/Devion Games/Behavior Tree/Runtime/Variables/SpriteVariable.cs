using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames.BehaviorTrees
{
	[System.Serializable]
	public class SpriteVariable : Variable
	{
		[SerializeField]
		private Sprite m_Value;

		public Sprite Value {
			get{ return this.m_Value; }
			set{ this.m_Value = value; }
		}

		public override object RawValue {
			get {
				return this.m_Value;
			}
			set {
				this.m_Value = (Sprite)value;
			}
		}

		public override System.Type type {
			get {
				return typeof(Sprite);
			}
		}

		public SpriteVariable ()
		{
		}

		public SpriteVariable (string name) : base (name)
		{
		}

		public SpriteVariable (SpriteVariable source) : base (source)
		{
			if (source != null) {
				this.Value = source.Value;
			}
		}

		public override Variable Clone ()
		{
			return new SpriteVariable (this);
		}

		public static implicit operator SpriteVariable (Sprite value)
		{
			return new SpriteVariable () {
				Value = value
			};
		}

		public static implicit operator Sprite (SpriteVariable value)
		{
			return value.Value;
		}
	}
}