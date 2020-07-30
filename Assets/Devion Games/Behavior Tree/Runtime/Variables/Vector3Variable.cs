﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames.BehaviorTrees
{
	[System.Serializable]
	public class Vector3Variable : Variable
	{
		[SerializeField]
		private Vector3 m_Value;

		public Vector3 Value {
			get{ return this.m_Value; }
			set{ this.m_Value = value; }
		}

		public override object RawValue {
			get {
				return this.m_Value;
			}
			set {
				this.m_Value = (Vector3)value;
			}
		}

		public override System.Type type {
			get {
				return typeof(Vector3);
			}
		}

		public Vector3Variable ()
		{
		}

		public Vector3Variable (string name) : base (name)
		{
		}

		public Vector3Variable (Vector3Variable source) : base (source)
		{
			if (source != null) {
				this.Value = source.Value;
			}
		}

		public override Variable Clone ()
		{
			return new Vector3Variable (this);
		}

		public static implicit operator Vector3Variable (Vector3 value)
		{
			return new Vector3Variable () {
				Value = value
			};
		}

		public static implicit operator Vector3 (Vector3Variable value)
		{
			return value.Value;
		}
	}
}