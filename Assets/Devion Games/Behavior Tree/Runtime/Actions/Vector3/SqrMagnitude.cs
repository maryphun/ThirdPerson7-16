using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames.BehaviorTrees.Actions.UnityVector3
{
	[Category ("UnityEngine/Vector3")]
	[Tooltip ("Gets squared length of this vector")]
	[HelpURL ("https://docs.unity3d.com/ScriptReference/Vector3.SqrMagnitude.html")]
	public class SqrMagnitude: Action
	{
		public Vector3Variable a;
		[Shared]
		public FloatVariable m_SqrMagnitude;

		public override TaskStatus OnUpdate ()
		{
			m_SqrMagnitude.Value = Vector3.SqrMagnitude (a);
			return TaskStatus.Success;
		}
	}
}
