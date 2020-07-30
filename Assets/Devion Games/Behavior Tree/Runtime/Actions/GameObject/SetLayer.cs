using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames.BehaviorTrees.Actions.UnityGameObject
{
	[Category ("UnityEngine/GameObject")]
	[Tooltip ("Sets the game objects layer.")]
	[HelpURL ("https://docs.unity3d.com/ScriptReference/GameObject-layer.html")]
	public class SetLayer : Action
	{
		[Tooltip ("The game object to operate on.")]
		public GameObjectVariable m_gameObject;
		[Tooltip ("The layer.")]
		public IntVariable m_Layer;

		public override TaskStatus OnUpdate ()
		{
			if (this.m_gameObject.Value == null) {
				return TaskStatus.Failure;
			}
			m_gameObject.Value.layer = m_Layer.Value;
			return TaskStatus.Success;
		}
	}
}