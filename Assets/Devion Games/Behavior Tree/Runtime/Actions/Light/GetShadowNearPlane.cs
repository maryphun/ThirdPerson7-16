using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames.BehaviorTrees.Actions.UnityLight
{
	[Category ("UnityEngine/Light")]
	[Tooltip ("Near plane value to use for shadow frustums.")]
	[HelpURL ("https://docs.unity3d.com/ScriptReference/Light-shadowNearPlane.html")]
	public class GetShadowNearPlane: Action
	{
		[Tooltip ("The game object to operate on.")]
		public GameObjectVariable m_gameObject;
		[Shared]
		public FloatVariable m_ShadowNearPlane;

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
			m_ShadowNearPlane.Value = m_Light.shadowNearPlane;
			return TaskStatus.Success;
		}
	}
}
