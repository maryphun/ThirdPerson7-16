using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames.BehaviorTrees.Actions.UnityVector2
{
	[Category("UnityEngine/Vector2")]
	[Tooltip("Returns a vector that is made from the largest components of two vectors.")]
	[HelpURL("https://docs.unity3d.com/ScriptReference/Vector2.Max.html")]
	public class Max: Action
	{
		[Tooltip("")]
		public Vector2Variable lhs;
		[Tooltip("")]
		public Vector2Variable rhs;
		[Shared]
		public Vector2Variable m_Max;

		public override TaskStatus OnUpdate ()
		{
			m_Max.Value = Vector2.Max(lhs, rhs);
			return TaskStatus.Success;
		}
	}
}
