using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames.BehaviorTrees.Actions.UnityString
{
	[Category ("String")]
	[Tooltip ("Gets a new string in which all occurrences of a specified string are replaced with another specified string.")]
	[HelpURL ("http://msdn.microsoft.com/en-us/library/czx8s9ts(v=vs.110).aspx")]
	public class Replace : Action
	{
		[Tooltip ("The target string.")]
		public StringVariable m_TargetValue;
		[Tooltip ("Old value to replace.")]
		public StringVariable m_OldValue;
		[Tooltip ("New value to replace with.")]
		public StringVariable m_NewValue;
		[Shared]
		[Tooltip ("String result.")]
		public StringVariable m_Store;

		public override TaskStatus OnUpdate ()
		{

			this.m_Store.Value = this.m_TargetValue.Value.Replace (m_OldValue.Value, m_NewValue.Value);
			return TaskStatus.Success;
		}
	}
}