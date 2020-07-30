using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames.BehaviorTrees.Actions.UnityGameObject
{
	[Category ("UnityEngine/GameObject")]
	[Tooltip ("Sets the game objects tag.")]
	[HelpURL ("https://docs.unity3d.com/ScriptReference/GameObject-tag.html")]
	public class SetTag : Action
	{
		[Tooltip ("The game object to operate on.")]
		public GameObjectVariable m_gameObject;
		[Tooltip ("The tag.")]
		public StringVariable m_Tag;

		public override TaskStatus OnUpdate ()
		{
			if (this.m_gameObject.Value == null) {
				return TaskStatus.Failure;
			}
			m_gameObject.Value.tag = m_Tag.Value;
			return TaskStatus.Success;
		}
	}
}