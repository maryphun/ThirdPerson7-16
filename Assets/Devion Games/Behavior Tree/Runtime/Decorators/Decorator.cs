using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames.BehaviorTrees
{
	public abstract class Decorator : Task
	{
		public override int maxChildCount {
			get {
				return 1;
			}
		}
	}
}