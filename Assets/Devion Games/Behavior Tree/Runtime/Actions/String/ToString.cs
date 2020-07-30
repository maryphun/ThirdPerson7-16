using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames.BehaviorTrees.Actions.UnityString
{
	[Category ("String")]
	public class ToString : Action
	{
		[Tooltip ("The target string.")]
		public GenericVariable m_TargetValue;
		[Shared]
		[Tooltip ("String result.")]
		public StringVariable m_Store;

		public override TaskStatus OnUpdate ()
		{
			this.m_Store.Value = this.m_TargetValue.Value.ToString ();
			return TaskStatus.Success;
		}
	}
}