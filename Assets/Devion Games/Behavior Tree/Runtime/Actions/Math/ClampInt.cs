using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames.BehaviorTrees.Actions.UnityMath
{
	[Category ("Math")]
	[Tooltip ("Clamps an int value between min and max value.")]
	public class ClampInt : Action
	{
		[Shared]
		[Tooltip ("Int variable to clamp.")]
		public IntVariable m_Variable;
		[Tooltip ("Minimum")]
		public IntVariable m_Min;
		[Tooltip ("Maximum")]
		public IntVariable m_Max;

		public override TaskStatus OnUpdate ()
		{
			m_Variable.Value = Mathf.Clamp (m_Variable.Value, m_Min.Value, m_Max.Value);
			return TaskStatus.Success;
		}
	}
}
