using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames.BehaviorTrees.Actions.UnityLight
{
	[Category ("UnityEngine/Light")]
	[Tooltip ("How to render the light.")]
	[HelpURL ("https://docs.unity3d.com/ScriptReference/Light-renderMode.html")]
	public class SetRenderMode: Action
	{
		[Tooltip ("The game object to operate on.")]
		public GameObjectVariable m_gameObject;
		public LightRenderMode m_RenderMode;

		private GameObject m_PrevGameObject;
		private Light m_Light;

		public override void OnStart ()
		{
			if (m_gameObject.Value != null && m_gameObject.Value != m_PrevGameObject) {
				m_PrevGameObject = m_gameObject.Value;
				m_Light = m_gameObject.Value.GetComponent<Light> ();
			}
		}

		public override TaskStatus OnUpdate ()
		{
			if (m_Light == null) {
				Debug.LogWarning ("Missing Component of type Light!");
				return TaskStatus.Failure;
			}
			m_Light.renderMode = m_RenderMode;
			return TaskStatus.Success;
		}
	}
}
