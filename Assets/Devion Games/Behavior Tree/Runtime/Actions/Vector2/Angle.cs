using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames.BehaviorTrees.Actions.UnityVector2
{
	[Category("UnityEngine/Vector2")]
	[Tooltip("Returns the angle in degrees between from and to.")]
	[HelpURL("https://docs.unity3d.com/ScriptReference/Vector2.Angle.html")]
	public class Angle: Action
	{
		[Tooltip("")]
		public Vector2Variable from;
		[Tooltip("")]
		public Vector2Variable to;
		[Shared]
		public FloatVariable m_Angle;

		public override TaskStatus OnUpdate ()
		{
			m_Angle.Value = Vector2.Angle(from, to);
			return TaskStatus.Success;
		}
	}
}
