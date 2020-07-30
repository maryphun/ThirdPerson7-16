using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames.BehaviorTrees.Actions.UnityMath
{
	[Category ("Math")]
	[Tooltip ("Sets a random float value between min and max value.")]
	public class RandomFloat : Action
	{
		[Tooltip ("Minimum")]
		public FloatVariable m_Min;
		[Tooltip ("Maximum")]
		public FloatVariable m_Max;
		[Shared]
		[Tooltip ("Store the result")]
		public FloatVariable m_StoreResult;

		public override TaskStatus OnUpdate ()
		{
			m_StoreResult.Value = Random.Range (m_Min.Value, m_Max.Value);
			return TaskStatus.Success;
		}
	}
}
