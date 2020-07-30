using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames.BehaviorTrees.Actions.UnityMath
{
	[Category ("Math")]
	[Tooltip ("Sets the absolute value of the float variable.")]
	public class AbsFloat : Action
	{
		[Shared]
		[Tooltip ("Float variable.")]
		public FloatVariable m_Variable;

		public override TaskStatus OnUpdate ()
		{
			m_Variable.Value = Mathf.Abs (m_Variable.Value);
			return TaskStatus.Success;
		}
	}
}
