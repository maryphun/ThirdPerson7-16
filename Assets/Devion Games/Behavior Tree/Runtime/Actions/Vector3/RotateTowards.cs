using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames.BehaviorTrees.Actions.UnityVector3
{
	[Category ("UnityEngine/Vector3")]
	[Tooltip ("Rotates a vector current towards target.")]
	[HelpURL ("https://docs.unity3d.com/ScriptReference/Vector3.RotateTowards.html")]
	public class RotateTowards: Action
	{
		public Vector3Variable current;
		public Vector3Variable target;
		public FloatVariable maxRadiansDelta;
		public FloatVariable maxMagnitudeDelta;
		[Shared]
		public Vector3Variable m_RotateTowards;

		public override TaskStatus OnUpdate ()
		{
			m_RotateTowards.Value = Vector3.RotateTowards (current, target, maxRadiansDelta, maxMagnitudeDelta);
			return TaskStatus.Success;
		}
	}
}
