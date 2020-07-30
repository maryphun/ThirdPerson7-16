using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames.BehaviorTrees.Actions
{
	[Icon ("Action")]
	public abstract class Action : Task
	{
		public override int maxChildCount {
			get {
				return 0;
			}
		}
	}
}