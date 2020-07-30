using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames.BehaviorTrees.Actions.UnityArray
{
	[Category ("Array")]
	[Tooltip ("Clears the array.")]
	public class Clear : Action
	{
		[Tooltip ("The array.")]
		public ArrayVariable m_Array;

		public override TaskStatus OnUpdate ()
		{
			System.Array.Clear (this.m_Array.Value, 0, this.m_Array.Value.Length);
			return TaskStatus.Success;
		}


	}
}
