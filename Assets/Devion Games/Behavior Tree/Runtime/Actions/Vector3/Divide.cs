using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames.BehaviorTrees.Actions.UnityVector3
{
	[Category ("UnityEngine/Vector3")]
	[Tooltip ("Divides a vector with a number.")]
	public class Divide : Action
	{
		[Tooltip ("The Vector3.")]
		public Vector3Variable m_A;
		[Tooltip ("The number.")]
		public FloatVariable m_Number;
		[Shared]
		[Tooltip ("Store the result")]
		public Vector3Variable m_StoreResult;

		public override TaskStatus OnUpdate ()
		{
			m_StoreResult.Value = m_A.Value / m_Number.Value;
			return TaskStatus.Success;
		}
	}
}
