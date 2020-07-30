using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames.BehaviorTrees.Actions.UnityArray
{
	[Category ("Array")]
	[Tooltip ("Removes an array element at index.")]
	public class RemoveAt : Action
	{
		[Tooltip ("The array.")]
		public ArrayVariable m_Array;
		[Tooltip ("The index.")]
		public IntVariable m_Index;

		public override TaskStatus OnUpdate ()
		{
			this.m_Array.RemoveAt (this.m_Index.Value);
			return TaskStatus.Success;
		}


	}
}
