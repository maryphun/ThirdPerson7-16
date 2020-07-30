using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames.BehaviorTrees.Conditionals.UnityString
{
	[Category ("String")]
	[Tooltip ("Returns success if the target string is equal to the specified value string.")]
	public class Equals : Conditional
	{
		[Tooltip ("The target string.")]
		public StringVariable m_TargetValue;
		[Tooltip ("The string.")]
		public StringVariable m_value;

		public override TaskStatus OnUpdate ()
		{
			return  m_TargetValue.Value.Equals (m_value.Value) ? TaskStatus.Success : TaskStatus.Failure;
		}
	}
}