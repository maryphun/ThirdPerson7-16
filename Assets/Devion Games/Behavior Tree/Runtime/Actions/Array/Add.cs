using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames.BehaviorTrees.Actions.UnityArray
{
	[Category ("Array")]
	[Tooltip ("Adds a new element to the array.")]
	public class Add : Action
	{
		[Tooltip ("The array.")]
		public ArrayVariable m_Array;
		[Tooltip ("The element to add.")]
		public GenericVariable m_Element;

		public override TaskStatus OnUpdate ()
		{
			this.m_Array.Add (m_Element.Value);
			return TaskStatus.Success;
		}


	}
}
