using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames.BehaviorTrees.Conditionals
{
	[Icon ("Conditional")]
	public abstract class Conditional : Task
	{
		public override int maxChildCount {
			get {
				return 0;
			}
		}
	}
}