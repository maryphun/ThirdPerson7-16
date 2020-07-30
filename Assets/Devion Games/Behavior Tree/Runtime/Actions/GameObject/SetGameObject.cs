using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames.BehaviorTrees.Actions.UnityGameObject
{
	[Category ("UnityEngine/GameObject")]
	[Tooltip ("Sets the game object variable.")]
	public class SetGameObject : Action
	{
		[Tooltip ("The game object to operate on.")]
		public GameObjectVariable m_gameObject;
		[NotRequired]
		[Tooltip ("The new gameobject to set.")]
		public GameObjectVariable m_NewGameObject;

		public override TaskStatus OnUpdate ()
		{
			m_gameObject.Value = m_NewGameObject.Value;
			return TaskStatus.Success;
		}
	}
}