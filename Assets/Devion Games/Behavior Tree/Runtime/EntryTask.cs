using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames.BehaviorTrees
{
	[Icon ("Entry")]
	public class EntryTask : Task
	{
		public override int maxChildCount {
			get {
				return 1;
			}
		}

		public override TaskStatus OnUpdate ()
		{
			return children [0].Tick ();
		}
	}
}