using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames.BehaviorTrees.Actions.UnityVector2
{
	[Category("UnityEngine/Vector2")]
	[Tooltip("Returns the distance between a and b.")]
	[HelpURL("https://docs.unity3d.com/ScriptReference/Vector2.Distance.html")]
	public class Distance: Action
	{
		[Tooltip("")]
		public Vector2Variable a;
		[Tooltip("")]
		public Vector2Variable b;
		[Shared]
		public FloatVariable m_Distance;

		public override TaskStatus OnUpdate ()
		{
			m_Distance.Value = Vector2.Distance(a, b);
			return TaskStatus.Success;
		}
	}
}
