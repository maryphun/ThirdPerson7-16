using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames.BehaviorTrees.Actions.UnityVector3
{
	[Category ("UnityEngine/Vector3")]
	[Tooltip ("Moves a point current in a straight line towards a target point.")]
	[HelpURL ("https://docs.unity3d.com/ScriptReference/Vector3.MoveTowards.html")]
	public class MoveTowards: Action
	{
		public Vector3Variable current;
		public Vector3Variable target;
		public FloatVariable maxDistanceDelta;
		[Shared]
		public Vector3Variable m_MoveTowards;

		public override TaskStatus OnUpdate ()
		{
			m_MoveTowards.Value = Vector3.MoveTowards (current, target, maxDistanceDelta);
			return TaskStatus.Success;
		}
	}
}
