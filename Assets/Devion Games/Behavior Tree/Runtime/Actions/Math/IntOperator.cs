using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames.BehaviorTrees.Actions.UnityMath
{
	[Category ("Math")]
	[Tooltip ("Performs a math operation on two integers.")]
	public class IntOperator : Action
	{
		[Tooltip ("Operation.")]
		public Operation m_Operation;
		[Tooltip ("The first int")]
		public IntVariable m_A;
		[Tooltip ("The second int")]
		public IntVariable m_B;
		[Shared]
		[Tooltip ("Store the result")]
		public IntVariable m_StoreResult;

		public override TaskStatus OnUpdate ()
		{
			switch (this.m_Operation) {
			case Operation.Add:
				m_StoreResult.Value = m_A.Value + m_B.Value;
				break;
			case Operation.Subtract:
				m_StoreResult.Value = m_A.Value - m_B.Value;
				break;
			case Operation.Divide:
				m_StoreResult.Value = m_A.Value / m_B.Value;
				break;
			case Operation.Multiply:
				m_StoreResult.Value = m_A.Value * m_B.Value;
				break;
			case Operation.Min:
				m_StoreResult.Value = Mathf.Min (m_A.Value, m_B.Value);
				break;
			case Operation.Max:
				m_StoreResult.Value = Mathf.Max (m_A.Value, m_B.Value);
				break;
			}
			return TaskStatus.Success;
		}
	}
}
