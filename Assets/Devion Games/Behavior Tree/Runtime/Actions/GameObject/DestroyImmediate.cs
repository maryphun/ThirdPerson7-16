using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames.BehaviorTrees.Actions.UnityGameObject
{
	[Category ("UnityEngine/GameObject")]
	[Tooltip ("Destroys the game object immediately.")]
	[HelpURL ("https://docs.unity3d.com/ScriptReference/Object.DestroyImmediate.html")]
	public class DestroyImmediate : Action
	{
		[Tooltip ("The game object to destroy.")]
		public GameObjectVariable m_gameObject;

		public override TaskStatus OnUpdate ()
		{
			if (this.m_gameObject.Value == null) {
				return TaskStatus.Failure;
			}

			GameObject.DestroyImmediate (this.m_gameObject.Value);

			return TaskStatus.Success;
		}
	}
}