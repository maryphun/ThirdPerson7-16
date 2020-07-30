using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames.BehaviorTrees.Actions.UnityMath
{
	[Category ("Math")]
	[Tooltip ("Sets the value of the bool variable.")]
	public class SetBool: Action
	{
		[Shared]
		public BoolVariable m_Variable;
		public BoolVariable m_value;

		public override TaskStatus OnUpdate ()
		{
			m_Variable.Value = m_value.Value;
			return TaskStatus.Success;
		}
	}
}
