using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames.BehaviorTrees.Actions.UnityGameObject
{
	[Category ("UnityEngine/GameObject")]
	[Tooltip ("Gets the game objects name.")]
	[HelpURL ("https://docs.unity3d.com/ScriptReference/Object-name.html")]
	public class GetName : Action
	{
		[Tooltip ("The game object to operate on.")]
		public GameObjectVariable m_gameObject;
		[Shared]
		[Tooltip ("The name.")]
		public StringVariable m_name;

		public override TaskStatus OnUpdate ()
		{
			if (this.m_gameObject.Value == null) {
				return TaskStatus.Failure;
			}
			m_name.Value = m_gameObject.Value.name;
			return TaskStatus.Success;
		}
	}
}