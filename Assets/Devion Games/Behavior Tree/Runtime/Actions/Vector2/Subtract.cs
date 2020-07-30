using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames.BehaviorTrees.Actions.UnityVector2
{
	[Category ("UnityEngine/Vector2")]
	[Tooltip ("Subtracts two Vectors.")]
	public class Subtract : Action
	{
		[Tooltip ("The first Vector2.")]
		public Vector2Variable m_A;
		[Tooltip ("The second Vector2.")]
		public Vector2Variable m_B;
		[Shared]
		[Tooltip ("Store the result")]
		public Vector2Variable m_StoreResult;

		public override TaskStatus OnUpdate ()
		{
			m_StoreResult.Value = m_A.Value - m_B.Value;
			return TaskStatus.Success;
		}
	}
}
