using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames.BehaviorTrees.Actions.UnityMath
{
	[Category ("Math")]
	[Tooltip ("Sets a random int value between min and max value.")]
	public class RandomInt : Action
	{
		[Tooltip ("Minimum")]
		public IntVariable m_Min;
		[Tooltip ("Maximum")]
		public IntVariable m_Max;
		[Shared]
		[Tooltip ("Store the result")]
		public IntVariable m_StoreResult;

		public override TaskStatus OnUpdate ()
		{
			m_StoreResult.Value = Random.Range (m_Min.Value, m_Max.Value);
			return TaskStatus.Success;
		}
	}
}
