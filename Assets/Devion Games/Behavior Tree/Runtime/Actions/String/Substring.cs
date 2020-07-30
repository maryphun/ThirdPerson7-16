using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames.BehaviorTrees.Actions.UnityString
{
	[Category ("String")]
	[Tooltip ("Retrieves a substring. The substring starts at a specified character position and continues to the end of the string.")]
	[HelpURL ("http://msdn.microsoft.com/en-us/library/hxthx5h6(v=vs.110).aspx")]
	public class Substring : Action
	{
		[Tooltip ("The target string.")]
		public StringVariable m_TargetValue;
		[Tooltip ("The zero-based starting character position of the substring.")]
		public IntVariable m_StartIndex;
		[Shared]
		[Tooltip ("String result.")]
		public StringVariable m_Store;

		public override TaskStatus OnUpdate ()
		{

			this.m_Store.Value = this.m_TargetValue.Value.Substring (m_StartIndex.Value);
			return TaskStatus.Success;
		}
	}
}