using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames.BehaviorTrees.Actions.UnityVector2
{
	[Category("UnityEngine/Vector2")]
	[Tooltip("Returns a copy of vector with its magnitude clamped to maxLength.")]
	[HelpURL("https://docs.unity3d.com/ScriptReference/Vector2.ClampMagnitude.html")]
	public class ClampMagnitude: Action
	{
		[Tooltip("")]
		public Vector2Variable vector;
		[Tooltip("")]
		public FloatVariable maxLength;
		[Shared]
		public Vector2Variable m_ClampMagnitude;

		public override TaskStatus OnUpdate ()
		{
			m_ClampMagnitude.Value = Vector2.ClampMagnitude(vector, maxLength);
			return TaskStatus.Success;
		}
	}
}
