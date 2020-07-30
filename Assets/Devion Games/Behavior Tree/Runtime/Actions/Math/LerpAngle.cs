using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames.BehaviorTrees.Actions.UnityMath
{
	[Category ("Math")]
	[Tooltip ("Linearly interpolates between fromValue and toValue by amount, but makes sure the values interpolate correctly when they wrap around 360 degrees.")]
	[HelpURL ("")]
	public class LerpAngle : Action
	{
		[Tooltip ("The start value.")]
		public FloatVariable m_FromValue;
		[Tooltip ("The end value.")]
		public FloatVariable m_ToValue;
		[Tooltip ("The interpolation value between two floats.")]
		public FloatVariable m_Amount;
		[Shared]
		[Tooltip ("Result")]
		public FloatVariable m_StoreResult;

		public override TaskStatus OnUpdate ()
		{
			m_StoreResult.Value = Mathf.LerpAngle (m_FromValue.Value, m_ToValue.Value, m_Amount.Value);
			return TaskStatus.Success;
		}
	}
}
