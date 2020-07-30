using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames.BehaviorTrees.Actions.UnityVector2
{
	[Category("UnityEngine/Vector2")]
	[Tooltip("Returns a vector that is made from the smallest components of two vectors.")]
	[HelpURL("https://docs.unity3d.com/ScriptReference/Vector2.Min.html")]
	public class Min: Action
	{
		[Tooltip("")]
		public Vector2Variable lhs;
		[Tooltip("")]
		public Vector2Variable rhs;
		[Shared]
		public Vector2Variable m_Min;

		public override TaskStatus OnUpdate ()
		{
			m_Min.Value = Vector2.Min(lhs, rhs);
			return TaskStatus.Success;
		}
	}
}
