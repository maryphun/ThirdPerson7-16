using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames.BehaviorTrees
{
	public abstract class Composite : Task
	{
		public AbortType abortType = AbortType.None;

	}

	public enum AbortType { 
		None,
		Self, 
		LowerPriority,
		Both
	}
}