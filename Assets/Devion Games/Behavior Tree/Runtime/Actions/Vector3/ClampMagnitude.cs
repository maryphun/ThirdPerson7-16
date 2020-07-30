using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames.BehaviorTrees.Actions.UnityVector3
{
	[Category ("UnityEngine/Vector3")]
	[Tooltip ("Gets a new vector with its magnitude clamped to maxLength.")]
	[HelpURL ("https://docs.unity3d.com/ScriptReference/Vector3.ClampMagnitude.html")]
	public class ClampMagnitude: Action
	{
		public Vector3Variable vector;
		public FloatVariable maxLength;
		[Shared]
		public Vector3Variable m_ClampMagnitude;

		public override TaskStatus OnUpdate ()
		{
			m_ClampMagnitude.Value = Vector3.ClampMagnitude (vector, maxLength);
			return TaskStatus.Success;
		}
	}
}
