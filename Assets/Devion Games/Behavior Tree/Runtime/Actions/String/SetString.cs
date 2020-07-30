using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames.BehaviorTrees.Actions.UnityString
{
	[Category ("String")]
	[Tooltip ("Sets the variable string to the value string.")]
	public class SetString : Action
	{
		[Shared]
		public StringVariable variable;
		public StringVariable value;

		public override TaskStatus OnUpdate ()
		{
			variable.Value = value.Value;
			return TaskStatus.Success;
		}
	}
}