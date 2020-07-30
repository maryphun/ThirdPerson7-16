using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames.BehaviorTrees.Conditionals.UnityGameObject
{
	[Category ("UnityEngine/GameObject")]
	[Tooltip ("Returns success if the gameobjects name is equal to name.")]
	public class CompareName : Conditional
	{
		[Tooltip ("The game object to operate on.")]
		public GameObjectVariable m_gameObject;
		[Tooltip ("Name to compare with")]
		public StringVariable m_name;


		public override TaskStatus OnUpdate ()
		{
			if (this.m_gameObject.Value == null) {
				return TaskStatus.Failure;
			}
			return this.m_gameObject.Value.name == m_name.Value ? TaskStatus.Success : TaskStatus.Failure;
		}
	}
}