using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames.BehaviorTrees.Actions.UnityCamera
{
	[Category ("UnityEngine/Camera")]
	[Tooltip ("How the camera clears the background.")]
	[HelpURL ("https://docs.unity3d.com/ScriptReference/Camera-clearFlags.html")]
	public class SetClearFlags: Action
	{
		[Tooltip ("The game object to operate on.")]
		public GameObjectVariable m_gameObject;
		public CameraClearFlags m_ClearFlags;

		private GameObject m_PrevGameObject;
		private Camera m_Camera;

		public override void OnStart ()
		{
			if (m_gameObject.Value != null && m_gameObject.Value != m_PrevGameObject) {
				m_PrevGameObject = m_gameObject.Value;
				m_Camera = m_gameObject.Value.GetComponent<Camera> ();
			}
		}

		public override TaskStatus OnUpdate ()
		{
			if (m_Camera == null) {
				Debug.LogWarning ("Missing Component of type Camera!");
				return TaskStatus.Failure;
			}
			m_Camera.clearFlags = m_ClearFlags;
			return TaskStatus.Success;
		}
	}
}
