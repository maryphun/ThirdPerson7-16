using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames.BehaviorTrees.Actions.UnityMath
{
	[Category ("Math")]
	[Tooltip ("Sets the value of the float variable.")]
	public class SetFloat : Action
	{
		[Shared]
		[Tooltip ("Float variable.")]
		public FloatVariable m_Variable;
		public FloatVariable m_value;

		public override TaskStatus OnUpdate ()
		{
			m_Variable.Value = m_value.Value;
			return TaskStatus.Success;
		}
	}
}
