using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames.BehaviorTrees.Actions.UnityString
{
	[Category ("String")]
	[Tooltip ("Removes all leading and trailing white-space characters from the string.")]
	[HelpURL ("http://msdn.microsoft.com/en-us/library/t97s7bs3(v=vs.110).aspx")]
	public class Trim : Action
	{
		[Tooltip ("The target string.")]
		public StringVariable m_TargetValue;
		[Shared]
		[Tooltip ("String result.")]
		public StringVariable m_Store;

		public override TaskStatus OnUpdate ()
		{

			this.m_Store.Value = this.m_TargetValue.Value.Trim ();
			return TaskStatus.Success;
		}
	}
}