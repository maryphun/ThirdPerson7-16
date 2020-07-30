using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames.BehaviorTrees.Actions.UnityGameObject
{
	[Category ("UnityEngine/GameObject")]
	[Tooltip ("Sets the game objects name.")]
	[HelpURL ("https://docs.unity3d.com/ScriptReference/Object-name.html")]
	public class SetName : Action
	{
		[Tooltip ("The game object to operate on.")]
		public GameObjectVariable m_gameObject;
		[Tooltip ("The name.")]
		public StringVariable m_name;

		public override TaskStatus OnUpdate ()
		{
			if (this.m_gameObject.Value == null) {
				return TaskStatus.Failure;
			}
			m_gameObject.Value.name = m_name.Value;
			return TaskStatus.Success;
		}
	}
}