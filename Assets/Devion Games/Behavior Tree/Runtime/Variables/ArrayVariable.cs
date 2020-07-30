using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace DevionGames.BehaviorTrees
{
	[System.Serializable]
	public class ArrayVariable : Variable
	{
		private object[] m_Value = new object[0];

		public object[] Value {
			get{ return this.m_Value; }
			set{ this.m_Value = value; }
		}

		public override object RawValue {
			get {
				return this.Value;
			}
			set {
				this.Value = (value as IList).Cast<object> ().ToArray (); 
			}
		}

		public override bool isShared {
			get {
				return true;
			}
			set {
				base.isShared = true;
				if (!value) {
					Debug.LogWarning ("ArrayVariable is always shared!");
				}
			}
		}

		public override System.Type type {
			get {
				return typeof(object[]);
			}
		}

		public ArrayVariable ()
		{
		}

		public ArrayVariable (string name) : base (name)
		{
		}

		public ArrayVariable (ArrayVariable source) : base (source)
		{
			if (source != null) {
				this.Value = source.Value;
			}
		}

		public override Variable Clone ()
		{
			return new ArrayVariable (this);
		}

		public void Add (object element)
		{
			System.Array.Resize<object> (ref m_Value, (int)m_Value.Length + 1);
			m_Value [(int)m_Value.Length - 1] = element;
		}

		public void AddRange (object[] elements)
		{
			List<object> ts = new List<object> (m_Value);
			ts.AddRange (elements);
			m_Value = ts.ToArray ();
		}

		public void Remove (object element)
		{
			List<object> ts = new List<object> (m_Value);
			ts.Remove (element);
			m_Value = ts.ToArray ();
		}

		public void RemoveAt (int index)
		{
			List<object> ts = new List<object> (m_Value);
			ts.RemoveAt (index);
			m_Value = ts.ToArray ();
		}

		public static implicit operator ArrayVariable (object[] value)
		{
			return new ArrayVariable () {
				Value = value
			};
		}

		public static implicit operator object[] (ArrayVariable value)
		{
			return value.Value;
		}
	}
}