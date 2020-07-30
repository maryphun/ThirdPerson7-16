using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames.BehaviorTrees.Actions.UnityVector3
{
	[Category ("UnityEngine/Vector3")]
	[Tooltip ("Gets a vector that is made from the smallest components of two vectors.")]
	[HelpURL ("https://docs.unity3d.com/ScriptReference/Vector3.Min.html")]
	public class Min: Action
	{
		public Vector3Variable lhs;
		public Vector3Variable rhs;
		[Shared]
		public Vector3Variable m_Min;

		public override TaskStatus OnUpdate ()
		{
			m_Min.Value = Vector3.Min (lhs, rhs);
			return TaskStatus.Success;
		}
	}
}
