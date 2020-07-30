using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames.BehaviorTrees.Actions.UnityVector2
{
	[Category("UnityEngine/Vector2")]
	[Tooltip("")]
	[HelpURL("https://docs.unity3d.com/ScriptReference/Vector2.SqrMagnitude.html")]
	public class SqrMagnitude: Action
	{
		[Tooltip("")]
		public Vector2Variable a;
		[Shared]
		public FloatVariable m_SqrMagnitude;

		public override TaskStatus OnUpdate ()
		{
			m_SqrMagnitude.Value = Vector2.SqrMagnitude(a);
			return TaskStatus.Success;
		}
	}
}
