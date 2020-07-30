using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames.BehaviorTrees.Actions.UnityGameObject
{
	[Category ("UnityEngine/GameObject")]
	[Tooltip ("Gets the game objects tag.")]
	[HelpURL ("https://docs.unity3d.com/ScriptReference/GameObject-tag.html")]
	public class GetTag : Action
	{
		[Tooltip ("The game object to operate on.")]
		public GameObjectVariable m_gameObject;
		[Tooltip ("The tag.")]
		[Shared]
		public StringVariable m_Tag;

		public override TaskStatus OnUpdate ()
		{
			if (this.m_gameObject.Value == null) {
				return TaskStatus.Failure;
			}
			m_Tag.Value = m_gameObject.Value.tag;
			return TaskStatus.Success;
		}
	}
}