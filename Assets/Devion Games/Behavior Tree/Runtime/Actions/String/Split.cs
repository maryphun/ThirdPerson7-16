using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

namespace DevionGames.BehaviorTrees.Actions.UnityString
{
	[Category ("String")]
	[Tooltip ("Splits a string into substrings based on the strings in an array. You can specify whether the substrings include empty array elements.")]
	[HelpURL ("https://msdn.microsoft.com/de-de/library/b873y76a(v=vs.110).aspx")]
	public class Split : Action
	{
		[Tooltip ("The target string.")]
		public StringVariable m_TargetValue;
		[Tooltip ("A string array that delimits the substrings in the target string, an empty array that contains no delimiters.")]
		public StringVariable[] m_Seperators;

		[Tooltip ("StringSplitOptions.RemoveEmptyEntries to omit empty array elements from the array returned; or StringSplitOptions.None to include empty array elements in the array returned.")]
		public StringSplitOptions options;
		[Tooltip ("String array result.")]
		public ArrayVariable m_Store;

		public override TaskStatus OnUpdate ()
		{

			this.m_Store.Value = this.m_TargetValue.Value.Split (m_Seperators.Select (x => x.Value).ToArray (), options);
			return TaskStatus.Success;
		}
	}
}