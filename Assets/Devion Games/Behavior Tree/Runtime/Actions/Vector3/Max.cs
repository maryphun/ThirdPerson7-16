using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames.BehaviorTrees.Actions.UnityVector3
{
	[Category ("UnityEngine/Vector3")]
	[Tooltip ("Gets a vector that is made from the largest components of two vectors.")]
	[HelpURL ("https://docs.unity3d.com/ScriptReference/Vector3.Max.html")]
	public class Max: Action
	{
		public Vector3Variable lhs;
		public Vector3Variable rhs;
		[Shared]
		public Vector3Variable m_Max;

		public override TaskStatus OnUpdate ()
		{
			m_Max.Value = Vector3.Max (lhs, rhs);
			return TaskStatus.Success;
		}
	}
}
