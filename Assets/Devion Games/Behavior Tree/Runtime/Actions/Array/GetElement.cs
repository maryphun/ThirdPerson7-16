using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames.BehaviorTrees.Actions.UnityArray
{
	[Category ("Array")]
	[Tooltip ("Gets an array element at index.")]
	public class GetElement : Action
	{
		[Tooltip ("The array.")]
		public ArrayVariable m_Array;
		[Tooltip ("Index of the element.")]
		public IntVariable m_Index;
		[Shared]
		[Tooltip ("The element.")]
		public GenericVariable m_Store;

		public override TaskStatus OnUpdate ()
		{
			if (this.m_Index.Value < 0 || this.m_Index.Value > this.m_Array.Value.Length) {
				return TaskStatus.Failure;
			}
			this.m_Store.Value = this.m_Array.Value [this.m_Index.Value];

			return TaskStatus.Success;
		}


	}
}
