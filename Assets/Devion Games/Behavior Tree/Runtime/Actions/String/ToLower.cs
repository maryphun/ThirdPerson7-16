using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames.BehaviorTrees.Actions.UnityString
{
	[Category ("String")]
	[Tooltip ("Gets a string converted to lowercase.")]
	[HelpURL ("http://msdn.microsoft.com/en-us/library/e78f86at(v=vs.110).aspx")]
	public class ToLower : Action
	{
		[Tooltip ("The target string.")]
		public StringVariable m_TargetValue;
		[Shared]
		[Tooltip ("String result.")]
		public StringVariable m_Store;

		public override TaskStatus OnUpdate ()
		{

			this.m_Store.Value = this.m_TargetValue.Value.ToLower ();
			return TaskStatus.Success;
		}
	}
}