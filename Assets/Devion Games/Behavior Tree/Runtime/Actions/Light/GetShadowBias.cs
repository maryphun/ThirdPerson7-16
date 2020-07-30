using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames.BehaviorTrees.Actions.UnityLight
{
	[Category ("UnityEngine/Light")]
	[Tooltip ("Shadow mapping constant bias.")]
	[HelpURL ("https://docs.unity3d.com/ScriptReference/Light-shadowBias.html")]
	public class GetShadowBias: Action
	{
		[Tooltip ("The game object to operate on.")]
		public GameObjectVariable m_gameObject;
		[Shared]
		public FloatVariable m_ShadowBias;

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
			m_ShadowBias.Value = m_Light.shadowBias;
			return TaskStatus.Success;
		}
	}
}
