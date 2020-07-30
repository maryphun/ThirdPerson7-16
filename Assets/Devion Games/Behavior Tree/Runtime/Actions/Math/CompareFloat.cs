using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames.BehaviorTrees.Conditionals.UnityMath
{
	[Category ("Math")]
	[Tooltip ("Compares two floats.")]
	public class CompareFloat : Conditional
	{
		[Tooltip ("Comparison.")]
		public Comparison m_Comparison;
		[Tooltip ("The first float.")]
		public FloatVariable m_A;
		[Tooltip ("The second float.")]
		public FloatVariable m_B;

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
