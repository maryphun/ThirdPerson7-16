using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames.BehaviorTrees.Actions.UnityString
{
	[Category ("String")]
	[Tooltip ("Gets a string converted to uppercase.")]
	[HelpURL ("https://msdn.microsoft.com/en-us/library/ewdd6aed(v=vs.110).aspx")]
	public class ToUpper : Action
	{
		[Tooltip ("The target string.")]
		public StringVariable m_TargetValue;
		[Shared]
		[Tooltip ("String result.")]
		public StringVariable m_Store;

		public override TaskStatus OnUpdate ()
		{

			this.m_Store.Value = this.m_TargetValue.Value.ToUpper ();
			return TaskStatus.Success;
		}
	}
}