using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames.BehaviorTrees.Conditionals.UnityGameObject
{
	[Category ("UnityEngine/GameObject")]
	[Tooltip ("Returns success if the gameobject is null.")]
	public class IsNull : Conditional
	{
		[Tooltip ("The game object to operate on.")]
		public GameObjectVariable m_gameObject;

		public override TaskStatus OnUpdate ()
		{
			return this.m_gameObject.Value == null ? TaskStatus.Success : TaskStatus.Failure;
		}
	}
}