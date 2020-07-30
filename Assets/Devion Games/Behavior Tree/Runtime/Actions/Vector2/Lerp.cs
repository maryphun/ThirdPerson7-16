using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames.BehaviorTrees.Actions.UnityVector2
{
	[Category("UnityEngine/Vector2")]
	[Tooltip("Linearly interpolates between vectors a and b by t.")]
	[HelpURL("https://docs.unity3d.com/ScriptReference/Vector2.Lerp.html")]
	public class Lerp: Action
	{
		[Tooltip("")]
		public Vector2Variable a;
		[Tooltip("")]
		public Vector2Variable b;
		[Tooltip("")]
		public FloatVariable t;
		[Shared]
		public Vector2Variable m_Lerp;

		public override TaskStatus OnUpdate ()
		{
			m_Lerp.Value = Vector2.Lerp(a, b, t);
			return TaskStatus.Success;
		}
	}
}
