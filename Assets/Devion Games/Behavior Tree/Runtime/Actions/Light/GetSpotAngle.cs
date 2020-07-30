using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames.BehaviorTrees.Actions.UnityLight
{
	[Category ("UnityEngine/Light")]
	[Tooltip ("The angle of the light's spotlight cone in degrees.")]
	[HelpURL ("https://docs.unity3d.com/ScriptReference/Light-spotAngle.html")]
	public class GetSpotAngle: Action
	{
		[Tooltip ("The game object to operate on.")]
		public GameObjectVariable m_gameObject;
		[Shared]
		public FloatVariable m_SpotAngle;

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
			m_SpotAngle.Value = m_Light.spotAngle;
			return TaskStatus.Success;
		}
	}
}
