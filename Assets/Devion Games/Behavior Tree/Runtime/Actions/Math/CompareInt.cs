using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames.BehaviorTrees.Conditionals.UnityMath
{
	[Category ("Math")]
	[Tooltip ("Compares two integers.")]
	public class CompareInt : Conditional
	{
		[Tooltip ("Comparison.")]
		public Comparison m_Comparison;
		[Tooltip ("The first int")]
		public IntVariable m_A;
		[Tooltip ("The second int")]
		public IntVariable m_B;

		public override TaskStatus OnUpdate ()
		{
			switch (this.m_Comparison) {
			case Comparison.Less:
				return m_A.Value < m_B.Value ? TaskStatus.Success : TaskStatus.Failure;
			case Comparison.LessOrEqual:
				return m_A.Value <= m_B.Value ? TaskStatus.Success : TaskStatus.Failure;
			case Comparison.Equal:
				return m_A.Value == m_B.Value ? TaskStatus.Success : TaskStatus.Failure;
			case Comparison.NotEqual:
				return m_A.Value != m_B.Value ? TaskStatus.Success : TaskStatus.Failure;
			case Comparison.GreaterOrEqual:
				return m_A.Value >= m_B.Value ? TaskStatus.Success : TaskStatus.Failure;
			case Comparison.Greater:
				return m_A.Value > m_B.Value ? TaskStatus.Success : TaskStatus.Failure;
			}
			return TaskStatus.Failure;
		}
	}
}
