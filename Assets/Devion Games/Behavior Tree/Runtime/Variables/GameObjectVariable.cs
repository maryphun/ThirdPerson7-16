﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames.BehaviorTrees
{
	[System.Serializable]
	public class GameObjectVariable : Variable
	{
		[SerializeField]
		private GameObject m_Value = null;

		public GameObject Value {
			get{ return this.m_Value; }
			set{ this.m_Value = value; }
		}

		public override object RawValue {
			get {
				return this.m_Value;
			}
			set {
				this.m_Value = (GameObject)value;
			}
		}

		public override System.Type type {
			get {
				return typeof(GameObject);
			}
		}

		public GameObjectVariable ()
		{
		}

		public GameObjectVariable (string name) : base (name)
		{
		}

		public GameObjectVariable (GameObjectVariable source) : base (source)
		{
			if (source != null) {
				this.Value = source.Value;
			}
		}

		public override Variable Clone ()
		{
			return new GameObjectVariable (this);
		}

		public static implicit operator GameObjectVariable (GameObject value)
		{
			return new GameObjectVariable () {
				Value = value
			};
		}

		public static implicit operator GameObject (GameObjectVariable value)
		{
			return value.Value;
		}
	}
}