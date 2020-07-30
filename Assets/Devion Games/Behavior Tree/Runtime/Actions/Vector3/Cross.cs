using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames.BehaviorTrees.Actions.UnityVector3
{
	[Category ("UnityEngine/Vector3")]
	[Tooltip ("Cross Product of two vectors.")]
	[HelpURL ("https://docs.unity3d.com/ScriptReference/Vector3.Cross.html")]
	public class Cross: Action
	{
		public Vector3Variable lhs;
		public Vector3Variable rhs;
		[Shared]
		public Vector3Variable m_Cross;

		public override TaskStatus OnUpdate ()
		{
			m_Cross.Value = Vector3.Cross (lhs, rhs);
			return TaskStatus.Success;
		}
	}
}
