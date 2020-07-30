using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames.BehaviorTrees
{
	[System.Serializable]
	public class TransformVariable : Variable
	{
		[SerializeField]
		private Transform m_Value;

		public Transform Value {
			get{ return this.m_Value; }
			set{ this.m_Value = value; }
		}

		public override object RawValue {
			get {
				return this.m_Value;
			}
			set {
				this.m_Value = (Transform)value;
			}
		}

		public override System.Type type {
			get {
				return typeof(Transform);
			}
		}

		public TransformVariable ()
		{
		}

		public TransformVariable (string name) : base (name)
		{
		}

		public TransformVariable (TransformVariable source) : base (source)
		{
			if (source != null) {
				this.Value = source.Value;
			}
		}

		public override Variable Clone ()
		{
			return new TransformVariable (this);
		}

		public static implicit operator TransformVariable (Transform value)
		{
			return new TransformVariable () {
				Value = value
			};
		}

		public static implicit operator Transform (TransformVariable value)
		{
			return value.Value;
		}
	}
}