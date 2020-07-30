using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames.BehaviorTrees.Conditionals.UnityString
{
	[Category ("String")]
	[Tooltip ("Returns success if the target string contains the specified value string.")]
	public class Contains : Conditional
	{
		[Tooltip ("The target string.")]
		public StringVariable m_TargetValue;
		[Tooltip ("The string sequence.")]
		public StringVariable m_value;

		public override TaskStatus OnUpdate ()
		{
			return  m_TargetValue.Value.Contains (m_value.Value) ? TaskStatus.Success : TaskStatus.Failure;
		}
	}
}