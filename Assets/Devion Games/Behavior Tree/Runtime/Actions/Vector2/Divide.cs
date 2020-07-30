using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames.BehaviorTrees.Actions.UnityVector2
{
	[Category ("UnityEngine/Vector2")]
	[Tooltip ("Divides a vector by a number.")]
	public class Divide : Action
	{
		[Tooltip ("The Vector2.")]
		public Vector2Variable m_A;
		[Tooltip ("The number.")]
		public FloatVariable m_Number;
		[Shared]
		[Tooltip ("Store the result")]
		public Vector2Variable m_StoreResult;

		public override TaskStatus OnUpdate ()
		{
			m_StoreResult.Value = m_A.Value / m_Number.Value;
			return TaskStatus.Success;
		}
	}
}
