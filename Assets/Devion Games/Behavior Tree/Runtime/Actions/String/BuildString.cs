using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames.BehaviorTrees.Actions.UnityString
{
	[Category ("String")]
	[Tooltip ("Builds a string from multiple other strings.")]
	public class BuildString : Action
	{
		[Tooltip ("The array of strings.")]
		public StringVariable[] values;
		[Shared]
		[Tooltip ("String result.")]
		public StringVariable m_Store;

		public override TaskStatus OnUpdate ()
		{
			string value = string.Empty;
			for (int i = 0; i < values.Length; i++) {
				value += values [i].Value;
			}
			m_Store.Value = value;
			return TaskStatus.Success;
		}
	}
}