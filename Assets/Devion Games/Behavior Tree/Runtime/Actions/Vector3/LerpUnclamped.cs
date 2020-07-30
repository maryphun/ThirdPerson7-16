using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames.BehaviorTrees.Actions.UnityVector3
{
	[Category ("UnityEngine/Vector3")]
	[Tooltip ("Linearly interpolates between two vectors.")]
	[HelpURL ("https://docs.unity3d.com/ScriptReference/Vector3.LerpUnclamped.html")]
	public class LerpUnclamped: Action
	{
		public Vector3Variable a;
		public Vector3Variable b;
		public FloatVariable t;
		[Shared]
		public Vector3Variable m_LerpUnclamped;

		public override TaskStatus OnUpdate ()
		{
			m_LerpUnclamped.Value = Vector3.LerpUnclamped (a, b, t);
			return TaskStatus.Success;
		}
	}
}
