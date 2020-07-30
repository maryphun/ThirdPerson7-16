using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames.BehaviorTrees.Actions.UnityCamera
{
	[Category ("UnityEngine/Camera")]
	[Tooltip ("The first enabled camera tagged MainCamera.")]
	[HelpURL ("https://docs.unity3d.com/ScriptReference/Camera-main.html")]
	public class GetMain: Action
	{
		[Tooltip ("The game object to operate on.")]
		public GameObjectVariable m_gameObject;
		[Shared]
		public GameObjectVariable m_Main;

		public override TaskStatus OnUpdate ()
		{
			if (Camera.main == null) {
				Debug.LogWarning ("Main Camera not present in scene!");
				return TaskStatus.Failure;
			}
			m_Main.Value = Camera.main.gameObject;
			return TaskStatus.Success;
		}
	}
}
