using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames.BehaviorTrees
{
	[System.Serializable]
	public class MaterialVariable : Variable
	{
		[SerializeField]
		private Material m_Value;

		public Material Value {
			get{ return this.m_Value; }
			set{ this.m_Value = value; }
		}

		public override object RawValue {
			get {
				return this.m_Value;
			}
			set {
				this.m_Value = (Material)value;
			}
		}

		public override System.Type type {
			get {
				return typeof(Material);
			}
		}

		public MaterialVariable ()
		{
		}

		public MaterialVariable (string name) : base (name)
		{
		}

		public MaterialVariable (MaterialVariable source) : base (source)
		{
			if (source != null) {
				this.Value = source.Value;
			}
		}

		public override Variable Clone ()
		{
			return new MaterialVariable (this);
		}

		public static implicit operator MaterialVariable (Material value)
		{
			return new MaterialVariable () {
				Value = value
			};
		}

		public static implicit operator Material (MaterialVariable value)
		{
			return value.Value;
		}
	}
}