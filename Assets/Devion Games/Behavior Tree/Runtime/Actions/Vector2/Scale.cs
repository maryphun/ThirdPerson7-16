using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames.BehaviorTrees.Actions.UnityVector2
{
	[Category("UnityEngine/Vector2")]
	[Tooltip("Multiplies two vectors component-wise.")]
	[HelpURL("https://docs.unity3d.com/ScriptReference/Vector2.Scale.html")]
	public class Scale: Action
	{
		[Tooltip("")]
		public Vector2Variable a;
		[Tooltip("")]
		public Vector2Variable b;
		[Shared]
		public Vector2Variable m_Scale;

		public override TaskStatus OnUpdate ()
		{
			m_Scale.Value = Vector2.Scale(a, b);
			return TaskStatus.Success;
		}
	}
}
