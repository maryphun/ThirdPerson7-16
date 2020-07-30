using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames.BehaviorTrees.Actions.UnityMath
{
	[Category ("Math")]
	[Tooltip ("Sets the absolute value of the int variable.")]
	public class AbsInt : Action
	{
		[Shared]
		[Tooltip ("Int variable.")]
		public IntVariable m_Variable;

		public override TaskStatus OnUpdate ()
		{
			m_Variable.Value = Mathf.Abs (m_Variable.Value);
			return TaskStatus.Success;
		}
	}
}
