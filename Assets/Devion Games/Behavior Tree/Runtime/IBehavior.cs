using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames.BehaviorTrees
{
	public interface IBehavior
	{
		BehaviorTree GetBehaviorTree () ;

		bool IsTemplate ();

		Object GetObject ();

		int GetInstanceID ();
	}
}