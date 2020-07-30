using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames.BehaviorTrees
{
	public interface IDirty
	{
		bool dirty{ get; set; }
	}
}
