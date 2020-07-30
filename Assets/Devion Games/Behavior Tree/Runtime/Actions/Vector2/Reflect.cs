using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames.BehaviorTrees.Actions.UnityVector2
{
	[Category("UnityEngine/Vector2")]
	[Tooltip("Reflects a vector off the vector defined by a normal.")]
	[HelpURL("https://docs.unity3d.com/ScriptReference/Vector2.Reflect.html")]
	public class Reflect: Action
	{
		[Tooltip("")]
		public Vector2Variable inDirection;
		[Tooltip("")]
		public Vector2Variable inNormal;
		[Shared]
		public Vector2Variable m_Reflect;

		public override TaskStatus OnUpdate ()
		{
			m_Reflect.Value = Vector2.Reflect(inDirection, inNormal);
			return TaskStatus.Success;
		}
	}
}
