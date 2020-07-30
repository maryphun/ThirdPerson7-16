using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames.BehaviorTrees.Conditionals.UnityString
{
	[Category ("String")]
	[Tooltip ("Returns success if the target string ends with the specified value string.")]
	public class EndsWith : Conditional
	{
		[Tooltip ("The target string.")]
		public StringVariable m_TargetValue;
		[Tooltip ("The ending string sequence.")]
		public StringVariable m_value;

		public override TaskStatus OnUpdate ()
		{
			return  m_TargetValue.Value.EndsWith (m_value.Value) ? TaskStatus.Success : TaskStatus.Failure;
		}
	}
}