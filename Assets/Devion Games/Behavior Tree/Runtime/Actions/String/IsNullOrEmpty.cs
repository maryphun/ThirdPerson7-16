using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames.BehaviorTrees.Conditionals.UnityString
{
	[Category ("String")]
	[Tooltip ("Returns success if the target string is null or empty.")]
	public class IsNullOrEmpty : Conditional
	{
		[Tooltip ("The target string.")]
		public StringVariable m_TargetValue;

		public override TaskStatus OnUpdate ()
		{
			return  string.IsNullOrEmpty (m_TargetValue.Value) ? TaskStatus.Success : TaskStatus.Failure;
		}
	}
}