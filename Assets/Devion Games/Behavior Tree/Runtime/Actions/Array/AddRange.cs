using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames.BehaviorTrees.Actions.UnityArray
{
	[Category ("Array")]
	[Tooltip ("Adds an array to the array.")]
	public class AddRange : Action
	{
		[Tooltip ("The array.")]
		public ArrayVariable m_Array;
		[Tooltip ("The array to add.")]
		public ArrayVariable m_Other;

		public override TaskStatus OnUpdate ()
		{
			this.m_Array.AddRange (m_Other.Value);
			return TaskStatus.Success;
		}


	}
}
