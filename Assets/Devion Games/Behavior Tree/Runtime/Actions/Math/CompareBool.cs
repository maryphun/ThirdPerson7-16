using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames.BehaviorTrees.Conditionals.UnityMath
{
	[Category ("Math")]
	[Tooltip ("Compares two bools. Returns Success if both are equal.")]
	public class CompareBool : Conditional
	{
		[Tooltip ("The first bool")]
		public BoolVariable m_A;
		[Tooltip ("The second bool")]
		public BoolVariable m_B;

		public override TaskStatus OnUpdate ()
		{
			return m_A.Value == m_B.Value ? TaskStatus.Success : TaskStatus.Failure;
		}
	}
}
