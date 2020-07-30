using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames.BehaviorTrees.Actions.UnityMath
{
	[Category ("Math")]
	[Tooltip ("Sets the value of the int variable.")]
	public class SetInt : Action
	{
		[Shared]
		[Tooltip ("Int variable.")]
		public IntVariable m_Variable;
		public IntVariable m_value;

		public override TaskStatus OnUpdate ()
		{
			m_Variable.Value = m_value.Value;
			return TaskStatus.Success;
		}
	}
}
