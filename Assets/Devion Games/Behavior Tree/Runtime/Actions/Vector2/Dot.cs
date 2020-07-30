using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames.BehaviorTrees.Actions.UnityVector2
{
	[Category("UnityEngine/Vector2")]
	[Tooltip("Dot Product of two vectors.")]
	[HelpURL("https://docs.unity3d.com/ScriptReference/Vector2.Dot.html")]
	public class Dot: Action
	{
		[Tooltip("")]
		public Vector2Variable lhs;
		[Tooltip("")]
		public Vector2Variable rhs;
		[Shared]
		public FloatVariable m_Dot;

		public override TaskStatus OnUpdate ()
		{
			m_Dot.Value = Vector2.Dot(lhs, rhs);
			return TaskStatus.Success;
		}
	}
}
