using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames.BehaviorTrees.Actions.UnityGameObject
{
	[Category ("UnityEngine/GameObject")]
	[Tooltip ("Makes the game object not be destroyed automatically when loading a new scene.")]
	[HelpURL ("https://docs.unity3d.com/ScriptReference/Object.DontDestroyOnLoad.html")]
	public class DontDestroyOnLoad : Action
	{
		[Tooltip ("The game object.")]
		public GameObjectVariable m_gameObject;

		public override TaskStatus OnUpdate ()
		{
			if (this.m_gameObject.Value == null) {
				return TaskStatus.Failure;
			}
		
			GameObject.DontDestroyOnLoad (this.m_gameObject.Value);

			return TaskStatus.Success;
		}
	}
}