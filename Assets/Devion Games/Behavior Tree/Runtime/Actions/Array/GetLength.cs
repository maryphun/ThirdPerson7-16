using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames.BehaviorTrees.Actions.UnityArray
{
	[Category ("Array")]
	[Tooltip ("Gets the length of the array.")]
	public class GetLength : Action
	{
		[Tooltip ("The array.")]
		public ArrayVariable m_Array;
		[Shared]
		[Tooltip ("The lenght.")]
		public IntVariable m_Store;

		public override TaskStatus OnUpdate ()
		{
			this.m_Store.Value = this.m_Array.Value.Length;
			return TaskStatus.Success;
		}


	}
}
