using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames.BehaviorTrees.Actions.UnityVector3
{
	[Category ("UnityEngine/Vector3")]
	[Tooltip ("Spherically interpolates between two vectors.")]
	[HelpURL ("https://docs.unity3d.com/ScriptReference/Vector3.SlerpUnclamped.html")]
	public class SlerpUnclamped: Action
	{
		public Vector3Variable a;
		public Vector3Variable b;
		public FloatVariable t;
		[Shared]
		public Vector3Variable m_SlerpUnclamped;

		public override TaskStatus OnUpdate ()
		{
			m_SlerpUnclamped.Value = Vector3.SlerpUnclamped (a, b, t);
			return TaskStatus.Success;
		}
	}
}
