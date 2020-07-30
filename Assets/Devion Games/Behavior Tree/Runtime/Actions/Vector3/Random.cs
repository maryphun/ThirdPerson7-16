using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames.BehaviorTrees.Actions.UnityVector3
{
	[Category ("UnityEngine/Vector3")]
	[Tooltip ("Generates a random vector3.")]
	public class Random : Action
	{
		[Tooltip ("Min value")]
		public FloatVariable m_Min;
		[Tooltip ("Max value")]
		public FloatVariable m_Max;
		[Shared]
		[Tooltip ("Store the result")]
		public Vector3Variable m_StoreResult;

		public override TaskStatus OnUpdate ()
		{
			m_StoreResult.Value = new Vector3 (
				UnityEngine.Random.Range (m_Min.Value, m_Max.Value),
				UnityEngine.Random.Range (m_Min.Value, m_Max.Value),
				UnityEngine.Random.Range (m_Min.Value, m_Max.Value)
			);
			return TaskStatus.Success;
		}
	}
}
