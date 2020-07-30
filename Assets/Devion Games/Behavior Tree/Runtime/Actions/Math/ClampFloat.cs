using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames.BehaviorTrees.Actions.UnityMath
{
	[Category ("Math")]
	[Tooltip ("Clamps a float value between min and max value.")]
	public class ClampFloat : Action
	{
		[Shared]
		[Tooltip ("Float variable to clamp.")]
		public FloatVariable m_Variable;
		[Tooltip ("Minimum")]
		public FloatVariable m_Min;
		[Tooltip ("Maximum")]
		public FloatVariable m_Max;

		public override TaskStatus OnUpdate ()
		{
			m_Variable.Value = Mathf.Clamp (m_Variable.Value, m_Min.Value, m_Max.Value);
			return TaskStatus.Success;
		}
	}
}
