using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames.BehaviorTrees.Actions.UnityVector3
{
	[Category ("UnityEngine/Vector3")]
	[Tooltip ("Subtracts two Vectors.")]
	public class Subtract : Action
	{
		[Tooltip ("The first Vector3.")]
		public Vector3Variable m_A;
		[Tooltip ("The second Vector3.")]
		public Vector3Variable m_B;
		[Shared]
		[Tooltip ("Store the result")]
		public Vector3Variable m_StoreResult;

		public override TaskStatus OnUpdate ()
		{
			m_StoreResult.Value = m_A.Value - m_B.Value;
			return TaskStatus.Success;
		}
	}
}
