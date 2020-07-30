using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames.BehaviorTrees.Actions.UnityVector3
{
	[Category ("UnityEngine/Vector3")]
	[Tooltip ("Dot Product of two vectors.")]
	[HelpURL ("https://docs.unity3d.com/ScriptReference/Vector3.Dot.html")]
	public class Dot: Action
	{
		public Vector3Variable lhs;
		public Vector3Variable rhs;
		[Shared]
		public FloatVariable m_Dot;

		public override TaskStatus OnUpdate ()
		{
			m_Dot.Value = Vector3.Dot (lhs, rhs);
			return TaskStatus.Success;
		}
	}
}
